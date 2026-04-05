using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class Internal
        {
            static public string DecryptFastText(string encrypted, EncryptionKey reKey)
            {
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount, encLength = encrypted.Length;
                string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;
                List<Int16> allShifts = reKey.Shifts;


                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + allShifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    Utils.DigitCount(helper) + 1  // Optimisation for base 10 encoding
                    : Numsys.AsList
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count + 1;  //  + 1 is to account for EncodingLength and the character it belongs to


                Int32 chunkSize = (Int32)reKey.ChunkSize, decodedId = 0;
                chunkSize -= chunkSize % maxEncodingLength;

                if (chunkSize < maxEncodingLength) chunkSize = maxEncodingLength;
                Int32 chunkCount = (Int32)Math.Ceiling((double)encLength / chunkSize);
                Int32 shPerChunk = chunkSize / maxEncodingLength;


                StringBuilder result = new(encrypted.Length / maxEncodingLength);  //  Real message length

                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    Int32 thisRoundLength =
                        Math.Min
                        (
                            encLength - chunk * chunkSize,
                            chunkSize
                        );

                    var shiftStartId = (chunk * shPerChunk) % shCount;
                    List<Int16> shifts = shiftStartId + thisRoundLength > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                     .. allShifts.GetRange(0, shiftStartId)]
                          : allShifts.GetRange(shiftStartId, thisRoundLength);


                    result.Append
                    (
                        DecryptionRound
                        (
                            encrypted.Substring
                            (
                                chunk * chunkSize,
                                thisRoundLength
                            ),
                            prAlphabet,
                            exAlphabet,
                            shifts,
                            exLength,
                            maxEncodingLength,
                            thisRoundLength / maxEncodingLength,
                            ref decodedId
                        )
                    );


                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"\n\t{chunk + 1})       ");
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("".PadRight(result.Length, ' '));
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                return result.ToString();
            }


            static public void DecryptFastTextFile(string inputPath, string outputPath, EncryptionKey reKey)
            {
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount;
                string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;
                List<Int16> allShifts = reKey.Shifts;


                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + allShifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    Utils.DigitCount(helper) + 1  // Optimisation for base 10 encoding
                    : Numsys.AsList
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count + 1;  //  + 1 is to account for EncodingLength and the character it belongs to


                Int32 chunkSize = (Int32)reKey.ChunkSize, decodedId = 0;
                chunkSize -= chunkSize % maxEncodingLength;
                if (chunkSize < maxEncodingLength) chunkSize = maxEncodingLength;



                using StreamReader reader = new(inputPath);
                using StreamWriter writer = new(outputPath);

                char[] messageChunk = new char[chunkSize];
                Int32 offset = 0, thisRoundLength = reader.ReadBlock(messageChunk, offset, chunkSize);

                while (thisRoundLength > 0)
                {
                    var shiftStartId = (offset / maxEncodingLength) % shCount;
                    List<Int16> shifts = shiftStartId + thisRoundLength > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                     .. allShifts.GetRange(0, shiftStartId)]
                          : allShifts.GetRange(shiftStartId, thisRoundLength);

                    writer.Write
                    (
                        DecryptionRound
                        (
                            messageChunk.ToList().GetRange
                            (
                                offset,
                                thisRoundLength
                            ).ToString()!,
                            prAlphabet,
                            exAlphabet,
                            shifts,
                            exLength,
                            maxEncodingLength,
                            thisRoundLength / maxEncodingLength,
                            ref decodedId
                        )
                    );

                    offset += thisRoundLength;
                    thisRoundLength = reader.ReadBlock(messageChunk, offset, chunkSize);
                }
            }
        }
    }
}