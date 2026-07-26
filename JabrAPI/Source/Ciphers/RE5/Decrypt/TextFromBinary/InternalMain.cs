using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;


using AVcontrol;
using static JabrAPI.RE5.InternalLink;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class Internal
        {
            static public string DecryptFastTextFromBinary(List<Byte> encrypted, EncryptionKey reKey, FromBinaryDelegate convertRule)
            {
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount, encLength = encrypted.Count;
                string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;
                List<Int16> allShifts = reKey.Shifts, shifts;


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
                Int32 decodedId  = 0, shiftStartId = 0, realMessageLength, shDelta;

                List<Byte> leftoverRaw = [];
                string  leftoverParsed = "", messageChunk;
                StringBuilder   result = new(encLength / maxEncodingLength);  //  Real message length

                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    messageChunk =
                        leftoverParsed +
                        convertRule
                        (
                            [
                                .. leftoverRaw,
                                .. encrypted.GetRange
                                (
                                    chunk * chunkSize,
                                    Math.Min  //  thisRoundLength
                                    (
                                        encLength - chunk * chunkSize,
                                        chunkSize
                                    )
                                )
                            ],
                            out leftoverRaw
                        );

                    leftoverParsed = messageChunk
                        [
                           ^(
                                messageChunk.Length % maxEncodingLength
                            )..
                        ];
                    messageChunk = messageChunk
                        [..
                            ^leftoverParsed.Length
                        ];

                    realMessageLength = messageChunk.Length / maxEncodingLength;
                    shDelta = shiftStartId + realMessageLength;

                    shifts  = shDelta > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, Math.Min(shiftStartId, shDelta - shCount))]
                          : allShifts.GetRange(shiftStartId, realMessageLength);
                    shiftStartId = shDelta % shCount;


                    result.Append
                    (
                        DecryptionRound
                        (
                            messageChunk,
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

                return result.ToString();
            }



            static public void DecryptFastTextFromBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, FromBinaryDelegate convertRule)
            {
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount;
                string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;
                List<Int16> allShifts = reKey.Shifts, shifts;


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

                using FileStream inputStream = new(Path.Combine(absoluteInputDirectory, fileName), FileMode.Open, FileAccess.Read);

                using BinaryReader reader = new(inputStream);
                using StreamWriter writer = new(Path.Combine(absoluteOutputDirectory, finalFileName));


                List<Byte> leftoverRaw = [];
                string  leftoverParsed = "", messageChunk;
                Byte[] readChunk = new Byte[chunkSize];
                Int32 offset = 0, bytesRead, decodedId = 0, shiftStartId = 0;
                Int32 realMessageLength, shDelta;

                while ((bytesRead = reader.Read(readChunk, 0, chunkSize)) > 0)
                {
                    messageChunk =
                        leftoverParsed +
                        convertRule
                        (
                            [
                                .. leftoverRaw,
                                .. readChunk.AsSpan(0, bytesRead)
                            ],
                            out leftoverRaw
                        );

                    leftoverParsed = messageChunk
                        [
                           ^(
                                messageChunk.Length % maxEncodingLength
                            )..
                        ];
                    messageChunk = messageChunk
                        [..
                            ^leftoverParsed.Length
                        ];


                    realMessageLength = messageChunk.Length / maxEncodingLength;
                    shDelta = shiftStartId + realMessageLength;

                    shifts  = shDelta > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, Math.Min(shiftStartId, shDelta - shCount))]
                          : allShifts.GetRange(shiftStartId, realMessageLength);
                    shiftStartId = shDelta % shCount;


                    writer.Write
                    (
                        DecryptionRound
                        (
                            messageChunk,
                            prAlphabet,
                            exAlphabet,
                            shifts,
                            exLength,
                            maxEncodingLength,
                            messageChunk.Length / maxEncodingLength,
                            ref decodedId
                        )
                    );

                    offset += bytesRead;
                }
            }
        }
    }
}