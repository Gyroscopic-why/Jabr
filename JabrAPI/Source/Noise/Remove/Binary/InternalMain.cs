using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;



namespace JabrAPI
{
    static public partial class Noise
    {
        static internal partial class Internal
        {
            static public Byte[] RemoveFastBinary(List<Byte> message, BinaryNoisifier noisifier)
            {
                Int32 chunkSize  = (Int32)noisifier.settings.ChunkSize,
                      chunkCount = (Int32)Math.Ceiling((double)message.Count / chunkSize);

                if (chunkSize < 1)
                    throw new ArgumentException
                    (
                        $"Impossible to split data into chunks of size: {chunkSize}",
                        nameof(noisifier.settings)
                    );

                Byte[][] finalisedChunks = new Byte[chunkCount][];
                bool ignoringIsActive = false;
                List<Byte> primary = noisifier.PrimaryNoise, complex = noisifier.ComplexNoise;

                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    finalisedChunks[chunk] =
                        RemovalRound
                        (
                            message.GetRange
                            (
                                chunk * chunkSize,
                                Math.Min
                                (
                                    chunkSize,
                                    message.Count - chunk * chunkSize
                                )
                            ),
                            ref ignoringIsActive,
                            primary,
                            complex
                        );
                }


                Byte[] result = new byte[finalisedChunks.Sum(c => c.Length)];
                Int32 offset = 0;
                var span = result.AsSpan();

                foreach (var chunk in finalisedChunks)
                {
                    chunk.CopyTo(span[offset..]);
                    offset += chunk.Length;
                }

                return result;
            }



            static public void RemoveFastBinaryFile(string absoluteInputDirectory,
                string fileName, string absoluteOutputDirectory, BinaryNoisifier noisifier)
            {
                List<Byte> primary = noisifier.PrimaryNoise, complex = noisifier.ComplexNoise;
                string finalFileName;

                Int32 chunkSize = (Int32)noisifier.settings.ChunkSize;
                if   (chunkSize < 2) chunkSize = 2;


                if (!noisifier.settings.KeepOriginalFileExtension)
                {
                    finalFileName = Path.ChangeExtension(fileName, "dnoisev5");
                    for (var i = 1; File.Exists(Path.Combine(absoluteOutputDirectory, finalFileName)); i++)
                        finalFileName = Path.ChangeExtension(fileName, $"dnoisev5-{i}");
                }
                else finalFileName = fileName + ".dnoisev5";


                using FileStream inputStream  = new(Path.Combine(absoluteInputDirectory,  fileName),      FileMode.Open,   FileAccess.Read);
                using FileStream outputStream = new(Path.Combine(absoluteOutputDirectory, finalFileName), FileMode.Create, FileAccess.Write);

                using BinaryReader reader = new(inputStream);
                using BinaryWriter writer = new(outputStream);


                Int32 bytesRead;
                bool ignoringIsActive = false;
                Byte[] noisedChunk = new Byte[chunkSize];

                while ((bytesRead = reader.Read(noisedChunk, 0, chunkSize)) > 0)
                {
                    writer.Write
                    (
                        RemovalRound
                        (
                            [.. noisedChunk[0..bytesRead]],
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