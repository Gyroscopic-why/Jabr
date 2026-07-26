using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class Internal
        {
            static public List<Byte> EncryptFastTextToBinary(string message, EncryptionKey reKey, Func<string, Byte[]> convertRule)
            {
                string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;
                Int32 exLength = reKey.ExLength, messageLength = message.Length, shCount = reKey.ShCount;
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
                Int32 thisRoundLength, shDelta, shiftStartId = 0, prevId = 0;

                List<Byte> result = new(messageLength * (maxEncodingLength + 1));


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


                    result.AddRange
                    (
                        convertRule
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
                        )
                    );
                }

                return result;
            }



            static public void EncryptFastTextToBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, Func<string, Byte[]> convertRule)
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

                using FileStream outputStream = new(Path.Combine(absoluteOutputDirectory, finalFileName), FileMode.Create, FileAccess.Write);

                using StreamReader reader = new(Path.Combine(absoluteInputDirectory, fileName));
                using BinaryWriter writer = new(outputStream);


                char[] messageChunk = new char[chunkSize];
                Int32 offset = 0, prevId = 0, shiftStartId = 0, shDelta, charsRead;

                while ((charsRead = reader.ReadBlock(messageChunk, 0, chunkSize)) > 0)
                {
                    shDelta = shiftStartId + charsRead;

                    shifts  = shDelta > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, Math.Min(shiftStartId, shDelta - shCount))]
                          : allShifts.GetRange(shiftStartId, charsRead);
                    shiftStartId = shDelta % shCount;


                    writer.Write
                    (
                        convertRule
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
                        )
                    );

                    offset += charsRead;
                }
            }
        }
    }
}