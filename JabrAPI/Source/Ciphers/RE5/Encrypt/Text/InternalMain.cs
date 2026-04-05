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
            static public string EncryptFastText(string message, EncryptionKey reKey)
            {
                string prAlphabet = reKey.PrAlphabet,  exAlphabet = reKey.ExAlphabet;
                Int32  exLength   = reKey.ExLength, messageLength = message.Length, shCount = reKey.ShCount;
                List<Int16> allShifts = reKey.Shifts;


                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + allShifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    Utils.DigitCount(helper)  //  Optimisation for base 10 encoding
                  : Numsys.AsList
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count;

                Int32 chunkSize  = (Int32)reKey.ChunkSize / (maxEncodingLength + 1), prevId = 0;
                if   (chunkSize <= maxEncodingLength) chunkSize = maxEncodingLength + 1;
                Int32 chunkCount = (Int32)Math.Ceiling((double)messageLength / chunkSize);


                StringBuilder result = new(messageLength * (maxEncodingLength + 1));

                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    Int32 thisRoundLength =
                        Math.Min
                        (
                            messageLength - chunk * chunkSize,
                            chunkSize
                        );

                    var shiftStartId = (chunk * chunkSize) % shCount;
                    List<Int16> shifts = shiftStartId + thisRoundLength > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                     .. allShifts.GetRange(0, shiftStartId)]
                          : allShifts.GetRange(shiftStartId, thisRoundLength);


                    result.Append
                    (
                        EncryptionRound
                        (
                            message.Substring
                            (
                                chunk * chunkSize,
                                thisRoundLength
                            ),
                            prAlphabet,
                            exAlphabet,
                            shifts,
                            exLength,
                            maxEncodingLength,
                            ref prevId
                        )
                    );


                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write($"\n\t{chunk + 1})       ");
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.Write("".PadRight(result.Length, ' '));
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                return result.ToString();
            }


            static public void EncryptFastTextFile(string inputPath, string outputPath, EncryptionKey reKey)
            {
                string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount;


                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + reKey.Shifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    Utils.DigitCount(helper)  //  Optimisation for base 10 encoding
                  : Numsys.AsList
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count;

                Int32 chunkSize  = (Int32)reKey.ChunkSize / maxEncodingLength, prevId = 0;
                if   (chunkSize <= maxEncodingLength) chunkSize = maxEncodingLength + 1;


                using StreamReader reader = new(inputPath);
                using StreamWriter writer = new(outputPath);

                char[] messageChunk = new char[chunkSize];
                Int32 offset = 0, thisRoundLength = reader.ReadBlock(messageChunk, offset, chunkSize);

                while (thisRoundLength > 0)
                {
                    var shiftStartId = offset % shCount;
                    List<Int16> shifts = shiftStartId + thisRoundLength > shCount ?
                        [.. reKey.Shifts.GetRange(shiftStartId, shCount - shiftStartId),
                     .. reKey.Shifts.GetRange(0, shiftStartId)]
                          : reKey.Shifts.GetRange(shiftStartId, thisRoundLength);

                    writer.Write
                    (
                        EncryptionRound
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
                            ref prevId
                        )
                    );

                    offset += thisRoundLength;
                    thisRoundLength = reader.ReadBlock(messageChunk, offset, chunkSize);
                }
            }
        }
    }
}