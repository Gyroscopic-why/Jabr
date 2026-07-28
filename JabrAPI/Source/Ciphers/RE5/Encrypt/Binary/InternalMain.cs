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
            static public List<Byte> EncryptFastBinary(List<Byte> message, BinaryKey reKey)
            {
                List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet, allShifts = reKey.Shifts, shifts;
                Int32 exLength = reKey.ExLength, messageLength = message.Count, shCount = reKey.ShCount;


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
                        EncryptionRound
                        (
                            message.GetRange
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

                return result;
            }


            static public void EncryptFastBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, BinaryKey reKey)
            {
                List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet, allShifts = reKey.Shifts, shifts;
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

                using FileStream inputStream  = new(Path.Combine(absoluteInputDirectory, fileName),       FileMode.Open,   FileAccess.Read);
                using FileStream outputStream = new(Path.Combine(absoluteOutputDirectory, finalFileName), FileMode.Create, FileAccess.Write);
                
                using BinaryReader reader = new(inputStream);
                using BinaryWriter writer = new(outputStream);


                Byte[] messageChunk = new Byte[chunkSize];
                Int32 offset = 0, prevId = 0, shiftStartId = 0, shDelta, bytesRead;

                while ((bytesRead = reader.Read(messageChunk, 0, chunkSize)) > 0)
                {
                    shDelta = shiftStartId + bytesRead;

                    shifts = shDelta > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, Math.Min(shiftStartId, shDelta - shCount))]
                          : allShifts.GetRange(shiftStartId, bytesRead);
                    shiftStartId = shDelta % shCount;

                    writer.Write
                    (
                        [..
                            EncryptionRound
                            (
                                new List<Byte>(messageChunk).GetRange(0, bytesRead),
                                prAlphabet,
                                exAlphabet,
                                shifts,
                                exLength,
                                maxEncodingLength,
                                ref prevId
                            )
                        ]
                    );

                    offset += bytesRead;
                }
            }
        }
    }
}