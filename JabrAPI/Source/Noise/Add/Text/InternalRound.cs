using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI.Noise
{
    static internal partial class Internal
    {
        static private List<char> AdditionRound(
            List<char> message, string fakeSelection,
            Noisifier noisifier, SecureRandom random,
            Int32 maxRoundLength, Int32 maxSyntropy,
            Int32 minAvgNoiseCount, Int32 maxAvgNoiseCount,
            ref Int32 prevFinalUnnoised)
        {
            Int32 initialLength = message.Count, chosenOffset;
            if (initialLength >= maxRoundLength)
            {
                prevFinalUnnoised = initialLength;
                return message;
            }


            chosenOffset = random.Next
            (
                noisifier.settings.forceOptimalEntropy
                    && prevFinalUnnoised >= maxSyntropy
                    && minAvgNoiseCount <= 0 ?
                       minAvgNoiseCount + 1 : minAvgNoiseCount,
                Math.Min
                (
                    maxAvgNoiseCount,
                    maxRoundLength - message.Count + 1
                        - initialLength / maxSyntropy
                )
            );


            if (chosenOffset > 0)
            {
                prevFinalUnnoised = 1;

                if (chosenOffset >= 2 && random.NextBoolChance(
                    noisifier.settings.complexNoiseIntervalBiasPercents))
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
                                noisifier.settings.complexNoisePairBiasPercents))
                        {
                            message.InsertRange
                            (
                                1,
                                noisifier.RandomComplexSequence(2)
                            );

                            i++;
                        }
                        else if (random.NextBoolChance(
                                 noisifier.settings.primaryNoiseBiasPercents))
                            message.Insert
                            (
                                1,
                                noisifier.RandomPrimaryChar
                            );
                        else message.Insert
                            (
                                1,
                                fakeSelection
                                [
                                    random.Next(fakeSelection.Length)
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
                chosenOffset = random.Next
                (
                    noisifier.settings.forceOptimalEntropy
                        && prevFinalUnnoised >= maxSyntropy
                        && i < initialLength
                        && minAvgNoiseCount <= 0 ?
                           minAvgNoiseCount + 1 : minAvgNoiseCount,
                    Math.Min
                    (
                        maxAvgNoiseCount,
                        maxRoundLength - message.Count + 1
                            - (initialLength - i) / maxSyntropy
                    )
                );


                if (chosenOffset > 0)
                {
                    prevFinalUnnoised = 0;

                    if (chosenOffset >= 2 && random.NextBoolChance(
                        noisifier.settings.complexNoiseIntervalBiasPercents))
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
                                noisifier.settings.complexNoisePairBiasPercents))
                            {
                                message.InsertRange
                                (
                                    totalOffset + minOffsetStep,
                                    noisifier.RandomComplexSequence(2)
                                );

                                j++;
                            }
                            else if (random.NextBoolChance(
                                noisifier.settings.primaryNoiseBiasPercents))
                                message.Insert
                                (
                                    totalOffset + minOffsetStep,
                                    noisifier.RandomPrimaryChar
                                );
                            else message.Insert
                                (
                                    totalOffset + minOffsetStep,
                                    fakeSelection
                                    [
                                        random.Next(fakeSelection.Length)
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