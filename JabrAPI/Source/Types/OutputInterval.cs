using System;
using System.Linq;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    public class OutputInterval(double probability, Int32 minLength, Int32 maxLength)
    {
        public double Probability { get; set; } = probability;

        public Int32 MinLength { get; set; } = minLength;
        public Int32 MaxLength { get; set; } = maxLength;



        static public Int32 OutputLength(
            Int32 curLength,
            OutputInterval[]? options,
            IntervalChoiceSetting intervalSetting,
            LengthChoiceSetting lengthSetting,
            SecureRandom? randomRef)
        {
            if (options == null || options.Length == 0) return 0;

            randomRef ??= new SecureRandom();
            OutputInterval outputInterval = ChooseInterval(curLength, options, randomRef, intervalSetting);

            return Math.Max
            (
                curLength,
                lengthSetting switch
                {
                    LengthChoiceSetting.ALWAYS_PICK_SMALLEST_VALID
                        => outputInterval.MinLength,

                    LengthChoiceSetting.ALWAYS_PICK_LARGEEST_VALID
                        => outputInterval.MaxLength,

                    LengthChoiceSetting.CHOOSE_RANDOM_FROM_VALID or _
                        => randomRef.Next(outputInterval.MinLength, outputInterval.MaxLength + 1)
                }
            );
        }

        static private OutputInterval ChooseInterval(Int32 curLength,
            OutputInterval[] options, SecureRandom randomRef,
            IntervalChoiceSetting choiceSetting = IntervalChoiceSetting.ANY_VALID)
        {
            double allChance = 0.0;
            Int32 optionsCount = options.Length;

            ParameterCounting paramCounting = new();
            List<MiniInterval> validIntervals = new(optionsCount);

            for (var id = 0; id < optionsCount; id++)
            {
                Int32 minDif = Math.Abs(curLength - options[id].MinLength);
                Int32 maxDif = options[id].MaxLength - curLength;

                if (maxDif > 0)
                {
                    var actualMinDif = Math.Min(minDif, maxDif);
                    var actualMaxDif = Math.Max(minDif, maxDif);
                    var probability = options[id].Probability;
                    var minLength   = options[id].MinLength;
                    var maxLength   = options[id].MaxLength;

                    allChance += probability;

                    validIntervals.Add
                    (
                        new MiniInterval
                        (
                            minLength,
                            maxLength,
                            actualMinDif,
                            actualMaxDif,
                            id,
                            probability
                        )
                    );

                    paramCounting.Update
                    (
                        minLength,
                        maxLength,
                        actualMaxDif,
                        actualMaxDif
                    );
                }
            }

            ;

            return choiceSetting switch
            {
                IntervalChoiceSetting.ANY_VALID
                    => options[ChooseIntervalId(validIntervals, allChance, randomRef)],


                IntervalChoiceSetting.ONLY_WITH_LOWEST_MIN_LENGTH
                    => options
                    [
                        ChooseIntervalId
                        (   [..
                                validIntervals.Where(i =>
                                    i.MinVal == paramCounting.MinVal)
                            ],
                            0.0,
                            randomRef
                        )
                    ],
                IntervalChoiceSetting.ONLY_WITH_HIGHEST_VALUE
                    => options
                    [
                        ChooseIntervalId
                        (   [..
                                validIntervals.Where(i =>
                                    i.MaxVal == paramCounting.MaxVal)
                            ],
                            0.0,
                            randomRef
                        )
                    ],


                IntervalChoiceSetting.PICK_SMALLEST_WITH_LOWEST_DIFFERENCE
                    => options
                    [
                        ChooseIntervalId
                        (   [..
                                validIntervals.Where(i =>
                                    i.MinDif == paramCounting.MinDif &&
                                    i.MinVal == paramCounting.MinVal)
                            ],
                            0.0,
                            randomRef
                        )
                    ],
                IntervalChoiceSetting.PICK_LARGEEST_WITH_LOWEST_DIFFERENCE
                    => options
                    [
                        ChooseIntervalId
                        (   [..
                                validIntervals.Where(i =>
                                    i.MinDif == paramCounting.MinDif &&
                                    i.MaxVal == paramCounting.MaxVal)
                            ],
                            0.0,
                            randomRef
                        )
                    ],


                IntervalChoiceSetting.PICK_SMALLEST_WITH_HIGHEST_DIFFERENCE
                    => options
                    [
                        ChooseIntervalId
                        (   [..
                                validIntervals.Where(i =>
                                    i.MaxDif == paramCounting.MaxDif &&
                                    i.MinVal == paramCounting.MinVal)
                            ],
                            0.0,
                            randomRef
                        )
                    ],
                IntervalChoiceSetting.PICK_LARGEEST_WITH_HIGHEST_DIFFERENCE
                    => options
                    [
                        ChooseIntervalId
                        (   [..
                                validIntervals.Where(i =>
                                    i.MaxDif == paramCounting.MaxDif &&
                                    i.MaxVal == paramCounting.MaxVal)
                            ],
                            0.0,
                            randomRef
                        )
                    ],
                _ => options[ChooseIntervalId(validIntervals, allChance, randomRef)],
            };
        }

        static private Int32 ChooseIntervalId(List<MiniInterval> validIntervals, double allChance, SecureRandom randomRef)
        {
            if (allChance == 0.0)
                allChance = validIntervals.Sum(p => p.Probability);
            double randomChoice = randomRef.NextDouble(allChance);

            foreach (MiniInterval interval in validIntervals)
            {
                randomChoice -= interval.Probability;
                if (randomChoice < 0) return interval.Id;
            }
            return Math.Max(0, validIntervals.Count - 1);
        }





        public enum IntervalChoiceSetting
        {
            ANY_VALID,

            ONLY_WITH_LOWEST_MIN_LENGTH,
            ONLY_WITH_HIGHEST_VALUE,

            PICK_SMALLEST_WITH_LOWEST_DIFFERENCE,
            PICK_LARGEEST_WITH_LOWEST_DIFFERENCE,

            PICK_SMALLEST_WITH_HIGHEST_DIFFERENCE,
            PICK_LARGEEST_WITH_HIGHEST_DIFFERENCE,
        }
        public enum LengthChoiceSetting
        {
            CHOOSE_RANDOM_FROM_VALID,
            ALWAYS_PICK_SMALLEST_VALID,
            ALWAYS_PICK_LARGEEST_VALID,
        }



        private class MiniInterval(
            Int32 minVal, Int32 maxVal,
            Int32 minDif, Int32 maxDif,
            Int32 id, double probability)
        {
            public Int32 MinVal = minVal;
            public Int32 MaxVal = maxVal;
            public Int32 Length = maxVal - minVal;

            public Int32 MinDif = minDif;
            public Int32 MaxDif = maxDif;

            public Int32 Id = id;
            public double Probability = probability;
        }
        private class ParameterCounting
        {
            public Int32 MinVal = Int32.MaxValue;
            public Int32 MaxVal = Int32.MinValue;

            public Int32 MinDif = Int32.MaxValue;
            public Int32 MaxDif = Int32.MinValue;

            public Int32 MinLength = Int32.MaxValue;
            public Int32 MaxLength = Int32.MinValue;



            public ParameterCounting() { }
            public ParameterCounting(
                Int32 minVal, Int32 maxVal,
                Int32 minDif, Int32 maxDif)
            {
                MinVal = minVal;
                MaxVal = maxVal;

                MinDif = minDif;
                MaxDif = maxDif;

                var length = maxVal - minVal;
                MinLength = length;
                MaxLength = length;
            }



            public void Update(
                Int32 minVal, Int32 maxVal,
                Int32 minDif, Int32 maxDif)
            {
                Int32 length = maxVal - minVal;

                if (minVal < MinVal) MinVal = minVal;
                if (maxVal > MaxVal) MaxVal = maxVal;

                if (minDif < MinDif) MinDif = minDif;
                if (maxDif > MaxDif) MaxDif = maxDif;

                if (length < MinLength) MinLength = length;
                if (length > MaxLength) MaxLength = length;
            }
        }
    }
}