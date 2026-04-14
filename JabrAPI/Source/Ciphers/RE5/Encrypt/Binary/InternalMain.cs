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
            static public List<Byte> EncryptFastBytes(List<Byte> message, BinaryKey reKey)
            {
                List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet, allShifts = reKey.Shifts;
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
                  : Numsys.AsList
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count;

                Int32 chunkSize = (Int32)reKey.ChunkSize / (maxEncodingLength + 1), prevId = 0;
                if (chunkSize <= maxEncodingLength) chunkSize = maxEncodingLength + 1;
                Int32 chunkCount = (Int32)Math.Ceiling((double)messageLength / chunkSize);


                #pragma warning disable IDE0028
                List<Byte> result = new(messageLength * (maxEncodingLength + 1));
                #pragma warning restore IDE0028


                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    Int32 thisRoundLength =
                        Math.Min
                        (
                            messageLength - chunk * chunkSize,
                            chunkSize
                        );

                    var shiftStartId = (chunk * chunkSize) % shCount;
                    List<Byte> shifts = shiftStartId + thisRoundLength > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, shiftStartId)]
                          : allShifts.GetRange(shiftStartId, thisRoundLength);


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


            static public void EncryptFastBytesFile(string inputPath, string outputPath, BinaryKey reKey)
            {
                List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;
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

                Int32 chunkSize = (Int32)reKey.ChunkSize / maxEncodingLength, prevId = 0;
                if (chunkSize <= maxEncodingLength) chunkSize = maxEncodingLength + 1;


                //using BinaryReader reader = new(inputPath);
                //using BinaryWriter writer = new(outputPath);

                //Byte[] messageChunk = new Byte[chunkSize];
                //Int32 offset = 0, thisRoundLength = reader.ReadBlock(messageChunk, offset, chunkSize);

                //while (thisRoundLength > 0)
                //{
                //    var shiftStartId = offset % shCount;
                //    List<Byte> shifts = shiftStartId + thisRoundLength > shCount ?
                //        [.. reKey.Shifts.GetRange(shiftStartId, shCount - shiftStartId),
                //         .. reKey.Shifts.GetRange(0, shiftStartId)]
                //          : reKey.Shifts.GetRange(shiftStartId, thisRoundLength);

                //    writer.Write
                //    (
                //        EncryptionRound
                //        (
                //            messageChunk.ToList().GetRange
                //            (
                //                offset,
                //                thisRoundLength
                //            ),
                //            prAlphabet,
                //            exAlphabet,
                //            shifts,
                //            exLength,
                //            maxEncodingLength,
                //            ref prevId
                //        )
                //    );

                //    offset += thisRoundLength;
                //    thisRoundLength = reader.ReadBlock(messageChunk, offset, chunkSize);
                //}
            }
        }
    }
}