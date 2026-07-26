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
            static public List<Byte> DecryptFastBinary(List<Byte> encrypted, BinaryKey reKey)
            {
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount, encLength = encrypted.Count;
                List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet, allShifts = reKey.Shifts, shifts;


                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + allShifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    Utils.DigitCount(helper) + 1  // Optimisation for base 10 encoding
                    : Numsys.AsList128<Int32>
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count + 1;  //  + 1 is to account for EncodingLength and the character it belongs to


                Int32 chunkSize = (Int32)reKey.ChunkSize / maxEncodingLength * maxEncodingLength;
                if   (chunkSize < maxEncodingLength) chunkSize = maxEncodingLength;

                Int32 chunkCount = (Int32)Math.Ceiling((double)encLength / chunkSize);
                Int32 shiftStartId = 0, decodedId = 0;
                Int32 realMessageLength, thisRoundLength, shDelta;


                List<Byte> result = new(encLength / maxEncodingLength);  //  Real message length


                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    thisRoundLength =
                        Math.Min
                        (
                            encLength - chunk * chunkSize,
                            chunkSize
                        );

                    realMessageLength = thisRoundLength / maxEncodingLength;
                    shDelta = shiftStartId + realMessageLength;

                    shifts  = shDelta > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, Math.Min(shiftStartId, shDelta - shCount))]
                          : allShifts.GetRange(shiftStartId, realMessageLength);
                    shiftStartId = shDelta % shCount;


                    result.AddRange
                    (
                        DecryptionRound
                        (
                            encrypted.GetRange
                            (
                                chunk * chunkSize,
                                thisRoundLength
                            ),
                            prAlphabet,
                            exAlphabet,
                            shifts,
                            exLength,
                            maxEncodingLength,
                            realMessageLength,
                            ref decodedId
                        )
                    );
                }

                return result;
            }


            static public void DecryptFastBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, BinaryKey reKey)
            {
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount;
                List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet, allShifts = reKey.Shifts, shifts;


                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + allShifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    Utils.DigitCount(helper) + 1  // Optimisation for base 10 encoding
                    : Numsys.AsList128<Int32>
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count + 1;  //  + 1 is to account for EncodingLength and the character it belongs to


                Int32 chunkSize = (Int32)reKey.ChunkSize / maxEncodingLength * maxEncodingLength;
                if   (chunkSize < maxEncodingLength) chunkSize = maxEncodingLength;


                string finalFileName;
                if (!reKey.KeepOriginalFileExtension)
                {
                    finalFileName = Path.ChangeExtension(fileName, "dec-re5");
                    for (var i = 1; File.Exists(Path.Combine(absoluteOutputDirectory, finalFileName)); i++)
                        finalFileName = Path.ChangeExtension(fileName, $"dec{i}-re5");
                }
                else finalFileName = Path.ChangeExtension(fileName, null);

                using FileStream inputStream  = new(Path.Combine(absoluteInputDirectory, fileName), FileMode.Open, FileAccess.Read);
                using FileStream outputStream = new(Path.Combine(absoluteOutputDirectory, finalFileName), FileMode.Create, FileAccess.Write);

                using BinaryReader reader = new(inputStream);
                using BinaryWriter writer = new(outputStream);


                Byte[] messageChunk = new Byte[chunkSize];
                Int32 offset = 0, decodedId = 0, shiftStartId = 0;
                Int32 realMessageLength, shDelta, bytesRead;

                while ((bytesRead = reader.Read(messageChunk, 0, chunkSize)) > 0)
                {
                    realMessageLength = bytesRead / maxEncodingLength;
                    shDelta = shiftStartId + realMessageLength;

                    shifts  = shDelta > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, Math.Min(shiftStartId, shDelta - shCount))]
                          : allShifts.GetRange(shiftStartId, realMessageLength);
                    shiftStartId = shDelta % shCount;


                    writer.Write
                    (
                        [..
                            DecryptionRound
                            (
                                new List<Byte>(messageChunk).GetRange(0, bytesRead),
                                prAlphabet,
                                exAlphabet,
                                shifts,
                                exLength,
                                maxEncodingLength,
                                realMessageLength,
                                ref decodedId
                            )
                        ]
                    );

                    offset += bytesRead;
                }
            }
        }
    }
}