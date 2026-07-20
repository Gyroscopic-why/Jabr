using System;
using System.Linq;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    public partial class OutputInterval(double probability, Int32 minLength, Int32 maxLength)
    {
        public double Probability { get; set; } = probability;

        public Int32 MinOutLength { get; set; } = minLength;
        public Int32 MaxOutLength { get; set; } = maxLength;



        static public Int32 OutputLength(
            Int32 curLength,
            OutputInterval[]? options,
            IntervalFilters intervalSetting,
            LengthChoiceSetting lengthSetting,
            SecureRandom? randomRef)
        {
            if (options == null || options.Length == 0) return 0;

            randomRef ??= new SecureRandom();
            OutputInterval? outputInterval = options.Length == 1
                ? options[0] : ChooseInterval(curLength, options, randomRef, intervalSetting);

            return outputInterval == null ? 0 : Math.Max
            (
                curLength,
                lengthSetting switch
                {
                    LengthChoiceSetting.ALWAYS_PICK_SMALLEST_VALID
                        => outputInterval.MinOutLength,

                    LengthChoiceSetting.ALWAYS_PICK_LARGEEST_VALID
                        => outputInterval.MaxOutLength,

                    LengthChoiceSetting.CHOOSE_RANDOM_FROM_VALID or _
                        => randomRef.Next(outputInterval.MinOutLength, outputInterval.MaxOutLength + 1)
                }
            );
        }

        static private OutputInterval? ChooseInterval(Int32 curLength,
            OutputInterval[] options, SecureRandom randomRef,
            IntervalFilters intervalFilters)
        {
            double allChance = 0.0;
            Int32 optionsCount = options.Length;

            ParameterCounting paramCounting = new();
            List<MiniInterval> validIntervals = new(optionsCount);

            for (var id = 0; id < optionsCount; id++)
            {
                Int32 difToMin = Math.Abs(curLength - options[id].MinOutLength);
                Int32 difToMax = options[id].MaxOutLength - curLength;

                if (difToMax > 0)
                {
                    var absDif = Math.Min(difToMin, difToMax);
                    var minLength = options[id].MinOutLength;
                    var maxLength = options[id].MaxOutLength;
                    var probability = options[id].Probability;

                    allChance += probability;

                    validIntervals.Add
                    (
                        new MiniInterval
                        (
                            minLength,
                            maxLength,
                            absDif,
                            difToMin,
                            difToMax,
                            id,
                            probability
                        )
                    );

                    paramCounting.Update
                    (
                        minLength,
                        maxLength,
                        absDif,
                        difToMin,
                        difToMax
                    );
                }
            }

            var chosenIntervalId =
                ChooseIntervalId
                (
                    FilterOutValidIntervals
                    (
                        validIntervals,
                        intervalFilters,
                        paramCounting,
                        ref allChance
                    ),
                    allChance,
                    randomRef
                );

            if (chosenIntervalId < 0 || chosenIntervalId >= optionsCount) return null;
            else return options[chosenIntervalId];
        }

        static private List<MiniInterval> FilterOutValidIntervals(
            List<MiniInterval> valid,
            IntervalFilters intervalFilters,
            ParameterCounting paramCounting,
            ref double allChance)
        {
            foreach (var filter in intervalFilters.FiltersPriorities)
            {
                if (valid.Count <= 1) break;

                valid = filter switch
                {
                    IntervalFilters.FilterType.OUT_LENGTH_RANGE
                        => ApplyFilter_OUT_LENGTH_RANGE
                            (
                                valid,
                                intervalFilters.OUT_LENGTH_RANGE,
                                ref paramCounting,
                                ref allChance
                            ),
                    IntervalFilters.FilterType.MIN_OUT_LENGTH
                        => ApplyFilter_MIN_OUT_LENGTH
                            (
                                valid,
                                intervalFilters.MIN_OUT_LENGTH,
                                ref paramCounting,
                                ref allChance
                            ),
                    IntervalFilters.FilterType.MAX_OUT_LENGTH
                        => ApplyFilter_MAX_OUT_LENGTH
                            (
                                valid,
                                intervalFilters.MAX_OUT_LENGTH,
                                ref paramCounting,
                                ref allChance
                            ),
                    IntervalFilters.FilterType.ABSOLUTE_DIFFERENCE
                        => ApplyFilter_ABSOLUTE_DIFFERENCE
                            (
                                valid,
                                intervalFilters.ABSOLUTE_DIFFERENCE,
                                ref paramCounting,
                                ref allChance
                            ),
                    IntervalFilters.FilterType.DIFFERENCE_TO_MIN
                        => ApplyFilter_DIFFERENCE_TO_MIN
                            (
                                valid,
                                intervalFilters.DIFFERENCE_TO_MIN,
                                ref paramCounting,
                                ref allChance
                            ),
                    IntervalFilters.FilterType.DIFFERENCE_TO_MAX
                        => ApplyFilter_DIFFERENCE_TO_MAX
                            (
                                valid,
                                intervalFilters.DIFFERENCE_TO_MAX,
                                ref paramCounting,
                                ref allChance
                            ),
                    _ => valid
                };
            }
            return valid;
        }
        static private Int32 ChooseIntervalId(List<MiniInterval> validIntervals, double allChance, SecureRandom randomRef)
        {
            if (validIntervals.Count == 0) return -1;
            if (validIntervals.Count == 1) return validIntervals[0].Id;

            if (allChance == 0.0) allChance = validIntervals.Sum(p => p.Probability);
            double randomChoice = randomRef.NextDouble(allChance);

            foreach (MiniInterval interval in validIntervals)
            {
                randomChoice -= interval.Probability;
                if (randomChoice < 0) return interval.Id;
            }
            return -1;
        }
    }
}