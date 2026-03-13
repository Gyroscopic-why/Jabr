using System;



namespace JabrAPI.Noise
{
    static internal partial class Internal
    {
        static public string RemoveFastText(string message, Noisifier noisifier)
        {
            Int32 chunkSize  = noisifier.settings.ChunkSizeForSplitting,
                  chunkCount = (Int32)Math.Ceiling((double)message.Length / chunkSize);

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
                        message.Substring
                        (
                            chunk * chunkSize,
                            Math.Min
                            (
                                chunkSize,
                                message.Length - chunk * chunkSize
                            )
                        ),
                        ref ignoringIsActive,
                        primary,
                        complex
                    );

                Console.Write($"\n\t{chunk + 1})       ");
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.Write("".PadRight(finalisedChunks[chunk].Length, ' '));
                Console.BackgroundColor = ConsoleColor.Black;
            }

            return string.Concat(finalisedChunks);
        }
    }
}