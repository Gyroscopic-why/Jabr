using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    static public partial class Noise
    {
        static internal partial class Internal
        {
            static private List<Byte> AdditionRound(
                List<Byte> message, List<Byte> fakeSelection,
                BinaryNoisifier noisifier, SecureRandom random,
                Int32 maxRoundLength, Int32 maxSyntropy,
                double maxAvgNoiseCount,
                ref Int32 prevFinalUnnoised)
            {
                Int32 initialLength = message.Count, chosenOffset;

                if (initialLength >= maxRoundLength)
                {
                    prevFinalUnnoised = initialLength;
                    return message;
                }

                Int32 minNoiseCount = noisifier.settings.ForceOptimalEntropy
                        && prevFinalUnnoised >= maxSyntropy ? 1 : 0;

                double overflow = random.NextDouble
                (
                    minNoiseCount,
                    Math.Max
                    (
                        Math.Min
                        (
                            maxAvgNoiseCount,
                            maxRoundLength - message.Count + 1
                                - initialLength / maxSyntropy
                        ),
                        1
                    )
                );
                chosenOffset = (Int32)Math.Floor(overflow);
                overflow -= chosenOffset;


                if (chosenOffset > 0)
                {
                    prevFinalUnnoised = 1;

                    if (chosenOffset >= 2 && random.NextBoolChance(
                        noisifier.settings.ComplexNoiseIntervalBiasPercents))
                    {
                        message.InsertRange
                        (
                            0,
                            noisifier.RandomComplexSequence(2)
                        );

                        for (var i = 1; i < chosenOffset - 1; i++)
                        {
                            if (i < chosenOffset - 2 &&
                                random.NextBoolChance(
                                    noisifier.settings.ComplexNoisePairBiasPercents))
                            {
                                message.InsertRange
                                (
                                    1,
                                    noisifier.RandomComplexSequence(2)
                                );

                                i++;
                            }
                            else if (random.NextBoolChance(
                                     noisifier.settings.PrimaryNoiseBiasPercents))
                                message.Insert
                                (
                                    1,
                                    noisifier.RandomPrimaryByte
                                );
                            else message.Insert
                                (
                                    1,
                                    fakeSelection
                                    [
                                        random.Next(fakeSelection.Count)
                                    ]
                                );
                        }
                    }
                    else message.InsertRange
                        (
                            0,
                            noisifier.RandomPrimarySequence(chosenOffset)
                        );
                }
                else prevFinalUnnoised += 2;


                if (message.Count >= maxRoundLength)
                {
                    prevFinalUnnoised = message.Count - chosenOffset + 1;
                    return message;
                }


                const Int32 minOffsetStep = 1;
                Int32 totalOffset = minOffsetStep + chosenOffset;


                for (var i = 1; i <= initialLength; i++)
                {
                    minNoiseCount = noisifier.settings.ForceOptimalEntropy
                            && prevFinalUnnoised >= maxSyntropy
                            && i < initialLength ? 1 : 0;

                    overflow += random.NextDouble
                    (
                        minNoiseCount,
                        Math.Max
                        (
                            Math.Min
                            (
                                maxAvgNoiseCount,
                                maxRoundLength - message.Count
                                    - (initialLength - i) / maxSyntropy
                            ),
                            minNoiseCount
                        )
                    );
                    chosenOffset = (Int32)Math.Floor(overflow);
                    overflow -= chosenOffset;


                    if (chosenOffset > 0)
                    {
                        prevFinalUnnoised = 0;

                        if (chosenOffset >= 2 && random.NextBoolChance(
                            noisifier.settings.ComplexNoiseIntervalBiasPercents))
                        {
                            message.InsertRange
                            (
                                totalOffset,
                                noisifier.RandomComplexSequence(2)
                            );

                            for (var j = 1; j < chosenOffset - 1; j++)
                            {
                                if (j < chosenOffset - 2 &&
                                random.NextBoolChance(
                                    noisifier.settings.ComplexNoisePairBiasPercents))
                                {
                                    message.InsertRange
                                    (
                                        totalOffset + minOffsetStep,
                                        noisifier.RandomComplexSequence(2)
                                    );

                                    j++;
                                }
                                else if (random.NextBoolChance(
                                    noisifier.settings.PrimaryNoiseBiasPercents))
                                    message.Insert
                                    (
                                        totalOffset + minOffsetStep,
                                        noisifier.RandomPrimaryByte
                                    );
                                else message.Insert
                                    (
                                        totalOffset + minOffsetStep,
                                        fakeSelection
                                        [
                                            random.Next(fakeSelection.Count)
                                        ]
                                    );
                            }
                        }
                        else message.InsertRange
                            (
                                totalOffset,
                                noisifier.RandomPrimarySequence(chosenOffset)
                            );
                    }

                    totalOffset += chosenOffset + minOffsetStep;

                    if (message.Count >= maxRoundLength)
                    {
                        prevFinalUnnoised = message.Count - totalOffset + 1;
                        return message;
                    }

                    prevFinalUnnoised++;
                }

                prevFinalUnnoised++;
                return message;
            }
        }
    }
}