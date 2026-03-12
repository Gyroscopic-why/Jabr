using System;
using System.Linq;
using System.Collections.Generic;



namespace JabrAPI.Noise
{
    static internal partial class Internal
    {
        static public Byte[] RemoveFastBytes(List<Byte> message, BinaryNoisifier noisifier)
        {
            Int32 chunkSize = noisifier.settings.ChunkSizeForSplitting,
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

                Console.Write($"\n\t{chunk})       ");
                Console.BackgroundColor = ConsoleColor.Magenta;
                Console.Write("".PadRight(finalisedChunks[chunk].Length, ' '));
                Console.BackgroundColor = ConsoleColor.Black;
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
    }
}