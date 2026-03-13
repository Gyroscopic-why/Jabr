using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI.Noise
{
    static internal partial class Internal
    {
        static public string OLD_AddFastText(string message, Noisifier noisifier, string fakeSelection)
        {
            Int32 staticLength = fakeSelection.Length, curLength = message.Length,
                outputLength = noisifier.settings.OutputLength;

            if (outputLength == 0)
                outputLength = curLength +
                    (Int32)Math.Pow
                    (
                        2,
                        Math.Min
                        (
                            10,
                            Math.Ceiling
                            (
                                Math.Log2(curLength)
                            )
                        )
                    );

            if (message.Length >= outputLength) return message;

            SecureRandom random = new(128);
            List<char> withNoise = [.. message];
            Int32 placementId, noiseCount;

            while (curLength < outputLength)
            {
                if (outputLength - curLength > 2 && random.Next(2) == 0)
                {
                    placementId = random.Next(curLength);
                    noiseCount = Math.Min
                    (
                        curLength - placementId - 1,
                        random.Next
                        (
                            2,
                            (Int32)Math.Ceiling
                            (
                                Math.Sqrt(outputLength - curLength)
                            )
                        )
                    );

                    for (var count = 0; count < noiseCount - 2; count++)
                        withNoise.Insert
                        (
                            placementId,
                            fakeSelection[random.Next(staticLength)]
                        );

                    withNoise.Insert(placementId, noisifier.RandomComplexChar);
                    withNoise.Insert(placementId + noiseCount - 1, noisifier.RandomComplexChar);
                }
                else
                {
                    noiseCount = random.Next
                    (
                        1,
                        (Int32)Math.Ceiling
                        (
                            Math.Sqrt(outputLength - curLength)
                        )
                    );

                    for (var count = 0; count < noiseCount; count++)
                        withNoise.Insert
                        (
                            random.Next(withNoise.Count),
                            noisifier.RandomPrimaryChar
                        );
                }
                curLength = withNoise.Count;
            }

            return new string([.. withNoise]) ?? "";
        }
        static public string AddFastText(string message, Noisifier noisifier, string fakeSelection)
        {
            Int32 chunkSize = noisifier.settings.ChunkSizeForSplitting;

            if (chunkSize < 1)
                throw new ArgumentException
                (
                    $"Impossible to split data into chunks of size: {chunkSize}",
                    nameof(noisifier.settings)
                );


            Int32 outputLength = noisifier.settings.OutputLength,
                  curLength = message.Length;

            if (outputLength == 0)
            {
                outputLength = (Int32)Math.Pow
                (
                    2,
                    Math.Min
                    (
                        (Int32)noisifier.settings.BoundaryAlignment,
                        Math.Ceiling
                        (
                            Math.Log2(curLength)
                        )
                    )
                );
                if (curLength > outputLength &&
                    noisifier.settings.UseDynamicOutputAlignment)
                    outputLength *= (1 + curLength / outputLength);
            }
            if (message.Length >= outputLength) return message;


            Int32 maxAvgNoiseCount =
                Math.Max
                (
                    1,
                    (outputLength - curLength)
                    / (curLength + 1)
                ) * 2 + 1;
            Int32 maxSyntropy = Miscellaneous.CalculateMaxNonEntropy
                (
                    noisifier.settings.ExpectedEntropy,
                    curLength,
                    outputLength
                );

            SecureRandom random = new(128);
            List<char> almostResult = new(outputLength);
            fakeSelection = fakeSelection == "" ? noisifier.PrimaryNoise : fakeSelection;
            Int32 prevFinalUnnoised = 0;


            for (var chunk = 0; chunk <= curLength; chunk += chunkSize)
            {
                random.Reseed();

                Int32 maxRoundLength =
                    Math.Min
                    (
                        chunkSize + chunk,
                        curLength
                    )
                        * outputLength
                        / curLength
                        - almostResult.Count;

                almostResult.AddRange
                (
                    AdditionRound
                    (
                        [.. message.Substring
                        (
                            chunk,
                            Math.Min
                            (
                                curLength - chunk,
                                chunkSize
                            )
                        )],
                        fakeSelection,
                        noisifier,
                        random,
                        maxRoundLength,
                        maxSyntropy,
                        0,  //  minAvgNoiseCount
                        maxAvgNoiseCount,
                        ref prevFinalUnnoised
                    )
                );

                Console.Write($"\n\t{chunk / chunkSize + 1})       ");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("".PadRight(almostResult.Count, ' '));
                Console.BackgroundColor = ConsoleColor.Black;
            }


            fakeSelection = noisifier.PrimaryNoise;
            while (almostResult.Count < outputLength)
            {
                random.Reseed();
                curLength = almostResult.Count;
                chunkSize = random.Next(1, outputLength - curLength);
                Int32 randPosition = random.Next(0, curLength - chunkSize - 1);

                almostResult.InsertRange
                (
                    randPosition,
                    AdditionRound
                    (
                        [],
                        fakeSelection,
                        noisifier,
                        random,
                        chunkSize,
                        maxSyntropy,
                        0,  //  minAvgNoiseCount
                        maxAvgNoiseCount,
                        ref prevFinalUnnoised
                    )
                );

                Console.Write("\n\t         ");
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("".PadRight(almostResult.Count - curLength, ' '));
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("".PadRight(10 - almostResult.Count + curLength, ' '));
                Console.BackgroundColor = ConsoleColor.Cyan;
                Console.Write("".PadRight(chunkSize, ' '));
                Console.BackgroundColor = ConsoleColor.Black;
            }

            return new string([.. almostResult]);
        }
    }
}