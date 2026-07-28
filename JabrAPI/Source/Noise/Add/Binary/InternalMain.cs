using System;
using System.IO;
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
                SecureRandom random = new(noisifier.RandomReseedInterval);
                Int32 chunkSize = (Int32)noisifier.settings.ChunkSize, initialLength = message.Count,
                  hardChunkSize = (Int32)(chunkSize * noisifier.settings.HardChunkSizeToSoftCoefficient);
                if (chunkSize < 2) chunkSize = 2;
                if (hardChunkSize < chunkSize) hardChunkSize = chunkSize;


                Int32 outputLength = OutputInterval.OutputLength
                    (
                        initialLength,
                        noisifier.settings.DynamicOutputIntervals,
                        noisifier.settings.IntervalChoiceSetting,
                        noisifier.settings.LengthChoiceSetting,
                        random
                    );
                if (initialLength >= outputLength) return message;


                Int32 maxSyntropy = Miscellaneous.CalculateMaxNonEntropy
                    (
                        noisifier.settings.ExpectedEntropy,
                        initialLength,
                        outputLength
                    );
                double maxAvgNoiseCount = 2.0 * (outputLength - initialLength) / (initialLength + 1);
                double avgNoisePerCharInRound = (double)initialLength / outputLength;

                List<Byte> result = new(outputLength);
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



            static public void AddFastBinaryFile(string absoluteInputDirectory,
                string fileName, string absoluteOutputDirectory, BinaryNoisifier noisifier)
            {
                SecureRandom random = new(noisifier.RandomReseedInterval);
                Int32 chunkSize = (Int32)noisifier.settings.ChunkSize, initialLength = 0,
                  hardChunkSize = (Int32)(chunkSize * noisifier.settings.HardChunkSizeToSoftCoefficient);
                if (chunkSize   < 2) chunkSize = 2;
                if (hardChunkSize <  chunkSize) hardChunkSize = chunkSize;

                string finalFileName;
                if (!noisifier.settings.KeepOriginalFileExtension)
                {
                    finalFileName = Path.ChangeExtension(fileName, "noisedv5");
                    for (var i = 1; File.Exists(Path.Combine(absoluteOutputDirectory, finalFileName)); i++)
                        finalFileName = Path.ChangeExtension(fileName, $"noisedv5-{i}");
                }
                else finalFileName = fileName + ".noisedv5";


                using FileStream lengthStream = new(Path.Combine(absoluteInputDirectory, fileName),       FileMode.Open,   FileAccess.Read);
                using FileStream inputStream  = new(Path.Combine(absoluteInputDirectory, fileName),       FileMode.Open,   FileAccess.Read);
                using FileStream outputStream = new(Path.Combine(absoluteOutputDirectory, finalFileName), FileMode.Create, FileAccess.Write);

                using (BinaryReader lengthReader = new(lengthStream))
                {
                    while (lengthReader.Read() != -1) initialLength++;

                    lengthReader.Close();
                    lengthReader.Dispose();
                }
                Int32 outputLength = OutputInterval.OutputLength
                (
                    initialLength,
                    noisifier.settings.DynamicOutputIntervals,
                    noisifier.settings.IntervalChoiceSetting,
                    noisifier.settings.LengthChoiceSetting,
                    random
                );


                if (initialLength >= outputLength)
                {
                    File.Copy
                    (
                        Path.Combine(absoluteInputDirectory, fileName),
                        Path.Combine(absoluteOutputDirectory, finalFileName),
                        false  //  overwrite
                    );
                    return;
                }


                Int32 maxSyntropy = Miscellaneous.CalculateMaxNonEntropy
                    (
                        noisifier.settings.ExpectedEntropy,
                        initialLength,
                        outputLength
                    );
                double maxAvgNoiseCount = 2.0 * (outputLength - initialLength) / (initialLength + 1);
                double avgNoisePerCharInRound = (double)initialLength / outputLength;

                using BinaryReader reader = new(inputStream);
                using BinaryWriter writer = new(outputStream);

                List<Byte> parsedChars = [];
                bool isFileEnd = false;
                Int32 prevFinalUnnoised = 0, offset = 0, processedCount = 0, maxRoundLength, messageChunk;

                for (var curOptimalSize = chunkSize; processedCount + initialLength - offset < outputLength; curOptimalSize += chunkSize)
                {
                    random.Reseed();

                    maxRoundLength = Math.Min
                        (
                            hardChunkSize,
                            Math.Min
                            (
                                outputLength,
                                curOptimalSize
                            ) - processedCount
                        );

                    messageChunk = processedCount - outputLength + maxRoundLength >= 0
                        ? initialLength - offset
                        : Math.Min
                        (
                            initialLength - offset,
                            Math.Max
                            (
                                (Int32)(maxRoundLength * avgNoisePerCharInRound),
                                (Int32)(processedCount * avgNoisePerCharInRound
                                    + 0.75 - processedCount / outputLength) - offset  // 0.75 = ((outP / outP) + 0.5) / 2
                            )
                        );

                    if (!isFileEnd)
                    {
                        if (messageChunk == 0)
                        {
                            isFileEnd = true;
                            parsedChars = [];
                            maxRoundLength = outputLength - processedCount;
                        }
                        else
                        {
                            Byte[] readBuffer = new Byte[messageChunk];
                            var actuallyRead = reader.Read(readBuffer, 0, messageChunk);

                            parsedChars = new List<Byte>(readBuffer).GetRange(0, actuallyRead);
                            isFileEnd = actuallyRead == 0;
                        }
                    }
                    else
                    {
                        parsedChars = [];
                        maxRoundLength = outputLength - processedCount;
                    }


                    parsedChars = AdditionRound
                    (
                        parsedChars,
                        noisifier.PrimaryNoise,  //  Fake selection is not supported in file noise.Addition
                        noisifier,
                        random,
                        maxRoundLength,
                        maxSyntropy,
                        maxAvgNoiseCount,
                        ref prevFinalUnnoised
                    );

                    offset += messageChunk;
                    processedCount += parsedChars.Count;
                    writer.Write([.. parsedChars]);
                }
            }
        }
    }
}