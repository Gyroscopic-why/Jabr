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
                List<Int16> allShifts = reKey.Shifts, shifts;


                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + allShifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    Utils.DigitCount(helper)  //  Optimisation for base 10 encoding
                  : Numsys.AsList128<Int32>
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count;

                Int32 chunkSize  = (Int32)reKey.ChunkSize / (maxEncodingLength + 1);
                if   (chunkSize <= maxEncodingLength) chunkSize = maxEncodingLength + 1;

                Int32 chunkCount = (Int32)Math.Ceiling((double)messageLength / chunkSize);
                Int32 shiftStartId = 0, prevId = 0, thisRoundLength, shDelta;


                StringBuilder result = new(messageLength * (maxEncodingLength + 1));

                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    thisRoundLength =
                        Math.Min
                        (
                            messageLength - chunk * chunkSize,
                            chunkSize
                        );

                    shDelta = shiftStartId + thisRoundLength;

                    shifts  = shDelta > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, Math.Min(shiftStartId, shDelta - shCount))]
                          : allShifts.GetRange(shiftStartId, thisRoundLength);
                    shiftStartId = shDelta % shCount;


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
                }

                return result.ToString();
            }


            static public void EncryptFastTextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey)
            {
                string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount;
                List<Int16> allShifts = reKey.Shifts, shifts;


                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + reKey.Shifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    Utils.DigitCount(helper)  //  Optimisation for base 10 encoding
                  : Numsys.AsList128<Int32>
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count;

                Int32 chunkSize  = (Int32)reKey.ChunkSize / (maxEncodingLength + 1);
                if   (chunkSize <= maxEncodingLength) chunkSize = maxEncodingLength + 1;


                string finalFileName;
                if (!reKey.KeepOriginalFileExtension)
                {
                    finalFileName = Path.ChangeExtension(fileName, "enc-re5");
                    for (var i = 1; File.Exists(Path.Combine(absoluteOutputDirectory, finalFileName)); i++)
                        finalFileName = Path.ChangeExtension(fileName, $"enc{i}-re5");
                }
                else finalFileName = fileName + ".re5";

                using StreamReader reader = new(Path.Combine(absoluteInputDirectory,  fileName));
                using StreamWriter writer = new(Path.Combine(absoluteOutputDirectory, finalFileName));
                
                
                char[] messageChunk = new char[chunkSize];
                Int32 offset = 0, prevId = 0, shiftStartId = 0, shDelta, charsRead;

                while ((charsRead = reader.ReadBlock(messageChunk, 0, chunkSize)) > 0)
                {
                    shDelta = shiftStartId + charsRead;

                    shifts = shDelta > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, Math.Min(shiftStartId, shDelta - shCount))]
                          : allShifts.GetRange(shiftStartId, charsRead);
                    shiftStartId = shDelta % shCount;

                    writer.Write
                    (
                        EncryptionRound
                        (
                            new string(messageChunk, 0, charsRead),
                            prAlphabet,
                            exAlphabet,
                            shifts,
                            exLength,
                            maxEncodingLength,
                            ref prevId
                        )
                    );

                    offset += charsRead;
                }
            }
        }
    }
}