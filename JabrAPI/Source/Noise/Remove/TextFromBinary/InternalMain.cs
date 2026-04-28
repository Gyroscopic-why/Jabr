using System;
using System.IO;
using System.Collections.Generic;


using static JabrAPI.Noise.InternalLink;



namespace JabrAPI
{
    static public partial class Noise
    {
        static internal partial class Internal
        {
            static public string RemoveNoiseFastTextFromBinary(List<Byte> noised, Noisifier noisifier, FromBinaryDelegate convertRule)
            {
                Int32 chunkSize  = (Int32)noisifier.settings.ChunkSize,
                      chunkCount = (Int32)Math.Ceiling((double)noised.Count / chunkSize);

                if (chunkSize < 1)
                    throw new ArgumentException
                    (
                        $"Impossible to split data into chunks of size: {chunkSize}",
                        nameof(noisifier.settings)
                    );

                List<Byte> leftoverRaw = [];

                string[] finalisedChunks = new string[chunkCount];
                bool ignoringIsActive = false;
                string primary = noisifier.PrimaryNoise, complex = noisifier.ComplexNoise;

                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    finalisedChunks[chunk] =
                        RemovalRound
                        (
                            convertRule
                            (
                                [
                                    .. leftoverRaw,
                                    .. noised.GetRange
                                    (
                                        chunk * chunkSize,
                                        Math.Min
                                        (
                                            chunkSize,
                                            noised.Count - chunk * chunkSize
                                        )
                                    )
                                ],
                                out leftoverRaw
                            ),
                            ref ignoringIsActive,
                            primary,
                            complex
                        );
                }

                return string.Concat(finalisedChunks);
            }



            static public void RemoveNoiseFastTextFromBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, Noisifier noisifier, FromBinaryDelegate convertRule)
            {
                string primary = noisifier.PrimaryNoise, complex = noisifier.ComplexNoise, finalFileName;
                Int32 chunkSize = (Int32)noisifier.settings.ChunkSize;
                if (chunkSize < 2) chunkSize = 2;


                if (!noisifier.settings.KeepOriginalFileExtension)
                {
                    finalFileName = Path.ChangeExtension(fileName, "dnoisev5");
                    for (var i = 1; File.Exists(Path.Combine(absoluteOutputDirectory, finalFileName)); i++)
                        finalFileName = Path.ChangeExtension(fileName, $"dnoisev5-{i}");
                }
                else finalFileName = fileName + ".dnoisev5";

                using FileStream inputStream = new(Path.Combine(absoluteInputDirectory, fileName), FileMode.Open, FileAccess.Read);

                using BinaryReader reader = new(inputStream);
                using StreamWriter writer = new(Path.Combine(absoluteOutputDirectory, finalFileName));


                Int32 charsRead;
                List<Byte> leftoverRaw = [];
                bool ignoringIsActive = false;
                Byte[] noisedChunk = new Byte[chunkSize];

                while ((charsRead = reader.Read(noisedChunk, 0, chunkSize)) > 0)
                {
                    writer.Write
                    (
                        RemovalRound
                        (
                            convertRule
                            (
                                [
                                    .. leftoverRaw,
                                    .. noisedChunk[0..charsRead]
                                ],
                                out leftoverRaw
                            ),
                            ref ignoringIsActive,
                            primary,
                            complex
                        )
                    );
                }
            }
        }
    }
}