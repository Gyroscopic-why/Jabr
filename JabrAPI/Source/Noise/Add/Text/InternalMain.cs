using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    static public partial class Noise
    {
        static internal partial class Internal
        {
            static public string AddFastText(string message, Noisifier noisifier, string fakeSelection)
            {
                Int32 chunkSize = (Int32)noisifier.settings.ChunkSize;

                if (chunkSize < 2) chunkSize = 2;


                Int32 outputLength = noisifier.settings.OutputLength,
                      curLength    = message.Length;

                if (outputLength  == 0)
                    outputLength  = (Int32)Math.Pow
                    (
                        2,
                        noisifier.settings.MinimizeOutputLengthIfDynamic ?
                            Math.Min
                            (
                                (Int32)noisifier.settings.BoundaryAlignment,
                                (Int32)Math.Ceiling(Math.Log2(curLength))
                            )
                        : (Int32)noisifier.settings.BoundaryAlignment
                    );
                
                if (curLength > outputLength)
                {
                    if (noisifier.settings.UseDynamicOutputAlignment)
                        outputLength *= 1 + curLength / outputLength;
                    else return message;
                }


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

                #pragma warning disable IDE0028
                SecureRandom random = new(128);
                List<char> almostResult = new(outputLength);
                #pragma warning restore IDE0028

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
                            maxAvgNoiseCount,
                            ref prevFinalUnnoised
                        )
                    );

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"\n\t{chunk / chunkSize + 1})       ");
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("".PadRight(almostResult.Count, ' '));
                    Console.ForegroundColor = ConsoleColor.Gray;
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
}