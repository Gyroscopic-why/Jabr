using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI.Noise
{
    static internal partial class Internal
    {
        static public List<Byte> AddFastBytes(List<Byte> message, BinaryNoisifier noisifier, List<Byte> fakeSelection)
        {
            Int32 chunkSize = noisifier.settings.ChunkSizeForSplitting;

            if (chunkSize < 1)
                throw new ArgumentException
                (
                    $"Impossible to split data into chunks of size: {chunkSize}",
                    nameof(noisifier.settings)
                );


            Int32 outputLength = noisifier.settings.OutputLength,
                  curLength    = message.Count;

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
            if (message.Count >= outputLength) return message;


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
            List<Byte> almostResult = new(outputLength);
            fakeSelection = fakeSelection.Count < 1 ? noisifier.PrimaryNoise : fakeSelection;
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
                        [.. message.GetRange
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

                Console.Write($"\n\t{chunk / chunkSize})       ");
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
                Console.Write("".PadRight(chunkSize, ' '));
                Console.BackgroundColor = ConsoleColor.Black;
            }

            return almostResult;
        }
    }
}