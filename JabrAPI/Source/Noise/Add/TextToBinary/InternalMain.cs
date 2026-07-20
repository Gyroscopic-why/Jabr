using System;
using System.IO;
using System.Collections.Generic;


using AVcontrol;
using JabrAPI.Template;



namespace JabrAPI
{
    static public partial class Noise
    {
        static internal partial class Internal
        {
            static public List<Byte> AddNoiseFastTextToBinary(string message, IEncryptionKey reKey, string fakeSelection, Func<string, Byte[]> convertRule)
            {
                Noisifier noisifierRef = reKey.Noisifier;
                SecureRandom random = new(noisifierRef.RandomReseedInterval);
                Int32 chunkSize = (Int32) noisifierRef.settings.ChunkSize, initialLength = message.Length,
                  hardChunkSize = (Int32)(chunkSize * noisifierRef.settings.HardChunkSizeToSoftCoefficient);
                if (chunkSize < 2) chunkSize = 2;
                if (hardChunkSize < chunkSize) hardChunkSize = chunkSize;


                Int32 outputLength = OutputInterval.OutputLength
                    (
                        initialLength,
                        noisifierRef.settings.DynamicOutputIntervals,
                        noisifierRef.settings.IntervalChoiceSetting,
                        noisifierRef.settings.LengthChoiceSetting,
                        random
                    );
                if (initialLength >= outputLength) return [.. convertRule(message)];


                Int32 maxSyntropy = Miscellaneous.CalculateMaxNonEntropy
                    (
                        noisifierRef.settings.ExpectedEntropy,
                        initialLength,
                        outputLength
                    );
                double maxAvgNoiseCount = 2.0 * (outputLength - initialLength) / (initialLength + 1);
                double avgNoisePerCharInRound = (double)initialLength / outputLength;

                List<Byte> result = new(outputLength);
                fakeSelection = fakeSelection == "" ? noisifierRef.PrimaryNoise : fakeSelection;
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
                                (Int32)(result.Count * avgNoisePerCharInRound
                                    + 0.75 - result.Count / outputLength) - offset  // 0.75 = ((outP / outP) + 0.5) / 2
                            )
                        );

                    result.AddRange
                    (
                        convertRule
                        (
                            new string
                            (
                                [..
                                    AdditionRound
                                    (
                                        [.. message.Substring
                                            (
                                                offset,
                                                messageChunk
                                            )
                                        ],
                                        fakeSelection,
                                        noisifierRef,
                                        random,
                                        maxRoundLength,
                                        maxSyntropy,
                                        maxAvgNoiseCount,
                                        ref prevFinalUnnoised
                                    )
                                ]
                    )   )   );

                    offset += messageChunk;
                }

                return result;
            }



            static public void AddNoiseFastTextToBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, Func<string, Byte[]> convertRule)
            {
                Noisifier noisifierRef = reKey.Noisifier;
                SecureRandom random = new(noisifierRef.RandomReseedInterval);
                Int32 chunkSize = (Int32)noisifierRef.settings.ChunkSize, initialLength = 0,
                  hardChunkSize = (Int32)(chunkSize * noisifierRef.settings.HardChunkSizeToSoftCoefficient);
                if (chunkSize < 2) chunkSize = 2;
                if (hardChunkSize < chunkSize) hardChunkSize = chunkSize;

                string finalFileName;
                if (!noisifierRef.settings.KeepOriginalFileExtension)
                {
                    finalFileName = Path.ChangeExtension(fileName, "noisedv5");
                    for (var i = 1; File.Exists(Path.Combine(absoluteOutputDirectory, finalFileName)); i++)
                        finalFileName = Path.ChangeExtension(fileName, $"noisedv5-{i}");
                }
                else finalFileName = fileName + ".noisedv5";


                using FileStream outputStream = new(Path.Combine(absoluteOutputDirectory, finalFileName), FileMode.Create, FileAccess.Write);
                using StreamReader reader = new(Path.Combine(absoluteInputDirectory, fileName));
                using BinaryWriter writer = new(outputStream);

                using (StreamReader lengthReader = new(Path.Combine(absoluteInputDirectory, fileName)))
                {
                    while (lengthReader.Read() != -1) initialLength++;

                    lengthReader.Close();
                    lengthReader.Dispose();
                }

                Int32 outputLength = OutputInterval.OutputLength
                    (
                        initialLength,
                        noisifierRef.settings.DynamicOutputIntervals,
                        noisifierRef.settings.IntervalChoiceSetting,
                        noisifierRef.settings.LengthChoiceSetting,
                        random
                    );
                if (initialLength >= outputLength)
                {
                    Int32 charsRead;
                    char[] readBuffer = new char[chunkSize];

                    while ((charsRead = reader.ReadBlock(readBuffer, 0, chunkSize)) > 0)
                    {
                        writer.Write
                        (
                            convertRule(new string (readBuffer[0..charsRead]))
                        );
                    }
                    return;
                }


                Int32 maxSyntropy = Miscellaneous.CalculateMaxNonEntropy
                    (
                        noisifierRef.settings.ExpectedEntropy,
                        initialLength,
                        outputLength
                    );
                double maxAvgNoiseCount = 2.0 * (outputLength - initialLength) / (initialLength + 1);
                double avgNoisePerCharInRound = (double)initialLength / outputLength;


                List<char> parsedChars = [];
                Byte[] roundResult;
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
                            char[] readBuffer = new char[messageChunk];
                            var actuallyRead = reader.ReadBlock(readBuffer, 0, messageChunk);

                            parsedChars = new List<char>(readBuffer).GetRange(0, actuallyRead);
                            isFileEnd = actuallyRead == 0;
                        }
                    }
                    else
                    {
                        parsedChars = [];
                        maxRoundLength = outputLength - processedCount;
                    }


                    roundResult = 
                        convertRule
                        (
                            new string
                            (
                                [..
                                    AdditionRound
                                    (
                                        parsedChars,
                                        noisifierRef.PrimaryNoise,  //  Fake selection is not supported in file noise.Addition
                                        noisifierRef,
                                        random,
                                        maxRoundLength,
                                        maxSyntropy,
                                        maxAvgNoiseCount,
                                        ref prevFinalUnnoised
                                    )
                                ]
                        )   );

                    offset += messageChunk;
                    processedCount += roundResult.Length;
                    writer.Write(roundResult);
                }
            }
        }
    }
}