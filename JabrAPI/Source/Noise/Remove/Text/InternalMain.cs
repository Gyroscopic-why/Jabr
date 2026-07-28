using System;
using System.IO;



namespace JabrAPI
{
    static public partial class Noise
    {
        static internal partial class Internal
        {
            static public string RemoveFastText(string noised, Noisifier noisifier)
            {
                Int32 chunkSize  = (Int32)noisifier.settings.ChunkSize,
                      chunkCount = (Int32)Math.Ceiling((double)noised.Length / chunkSize);

                if (chunkSize < 1)
                    throw new ArgumentException
                    (
                        $"Impossible to split data into chunks of size: {chunkSize}",
                        nameof(noisifier.settings)
                    );

                string[] finalisedChunks = new string[chunkCount];
                bool ignoringIsActive = false;
                string primary = noisifier.PrimaryNoise, complex = noisifier.ComplexNoise;

                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    finalisedChunks[chunk] =
                        RemovalRound
                        (
                            noised.Substring
                            (
                                chunk * chunkSize,
                                Math.Min
                                (
                                    chunkSize,
                                    noised.Length - chunk * chunkSize
                                )
                            ),
                            ref ignoringIsActive,
                            primary,
                            complex
                        );
                }

                return string.Concat(finalisedChunks);
            }



            static public void RemoveFastTextFile(string absoluteInputDirectory, string fileName, string absoluteOutputDirectory, Noisifier noisifier)
            {
                string primary = noisifier.PrimaryNoise, complex = noisifier.ComplexNoise, finalFileName;
                Int32 chunkSize = (Int32)noisifier.settings.ChunkSize;
                if   (chunkSize < 2) chunkSize = 2;


                if (!noisifier.settings.KeepOriginalFileExtension)
                {
                    finalFileName = Path.ChangeExtension(fileName, "dnoisev5");
                    for (var i = 1; File.Exists(Path.Combine(absoluteOutputDirectory, finalFileName)); i++)
                        finalFileName = Path.ChangeExtension(fileName, $"dnoisev5-{i}");
                }
                else finalFileName = fileName + ".dnoisev5";

                using StreamReader reader = new(Path.Combine(absoluteInputDirectory, fileName));
                using StreamWriter writer = new(Path.Combine(absoluteOutputDirectory, finalFileName));


                Int32 charsRead;
                bool ignoringIsActive = false;
                char[] noisedChunk = new char[chunkSize];

                while ((charsRead = reader.ReadBlock(noisedChunk, 0, chunkSize)) > 0)
                {
                    writer.Write
                    (
                        RemovalRound
                        (
                            new string(noisedChunk[0..charsRead]),
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