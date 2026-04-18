using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    static public partial class Noise
    {
        static internal partial class Internal
        {
            static public List<Byte> AddFastBinary(List<Byte> message, BinaryNoisifier noisifier, List<Byte> fakeSelection)
            {
                Int32 chunkSize = (Int32)noisifier.settings.ChunkSize,
                  hardChunkSize = (Int32)(chunkSize * noisifier.settings.HardChunkSizeToSoftCoefficient);
                if (chunkSize < 2) chunkSize = 2;
                if (hardChunkSize < chunkSize) hardChunkSize = chunkSize;


                Int32 outputLength = noisifier.settings.OutputLength, initialLength = message.Count;

                if (outputLength == 0)
                    outputLength = (Int32)Math.Pow
                    (
                        2,
                        noisifier.settings.MinimizeOutputLengthIfDynamic ?
                            Math.Min
                            (
                                (Int32)noisifier.settings.BoundaryAlignment,
                                (Int32)Math.Ceiling(Math.Log2(initialLength))
                            )
                        : (Int32)noisifier.settings.BoundaryAlignment
                    );

                if (initialLength > outputLength)
                {
                    if (noisifier.settings.UseDynamicOutputAlignment)
                        outputLength *= 1 + initialLength / outputLength;
                    else return message;
                }


                Int32 maxSyntropy = Miscellaneous.CalculateMaxNonEntropy
                    (
                        noisifier.settings.ExpectedEntropy,
                        initialLength,
                        outputLength
                    );
                double maxAvgNoiseCount = 2.0 * (outputLength - initialLength) / (initialLength + 1);
                double avgNoisePerCharInRound = (double)initialLength / outputLength;

                SecureRandom random = new(128);
                List <Byte>  result = new(outputLength);

                fakeSelection = fakeSelection.Count < 1 ? noisifier.PrimaryNoise : fakeSelection;
                Int32 prevFinalUnnoised = 0, maxRoundLength, offset = 0, messageChunk;

                for (var curOptimalSize = chunkSize; result.Count + initialLength - offset < outputLength; curOptimalSize += chunkSize)
                {
                    random.Reseed();

                    maxRoundLength = Math.Min
                        (
                            hardChunkSize,
                            Math.Min
                            (
                                outputLength,
                                curOptimalSize
                            ) - result.Count
                        );

                    messageChunk = result.Count - outputLength + maxRoundLength >= 0
                        ? initialLength - offset
                        : Math.Min
                        (
                            initialLength - offset,
                            Math.Max
                            (
                                (Int32)(maxRoundLength * avgNoisePerCharInRound),
                                (Int32)(result.Count   * avgNoisePerCharInRound
                                    + 0.75 - result.Count / outputLength) - offset  // 0.75 = ((outP / outP) + 0.5) / 2
                            )
                        );

                    result.AddRange
                    (
                        AdditionRound
                        (
                             [.. message.GetRange
                                (
                                    offset,
                                    messageChunk
                                )
                            ],
                            fakeSelection,
                            noisifier,
                            random,
                            maxRoundLength,
                            maxSyntropy,
                            maxAvgNoiseCount,
                            ref prevFinalUnnoised
                        )
                    );

                    offset += messageChunk;
                }

                return result;
            }
        }
    }
}