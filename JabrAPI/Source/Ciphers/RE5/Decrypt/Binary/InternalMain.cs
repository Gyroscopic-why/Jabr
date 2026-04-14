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
                List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet, allShifts = reKey.Shifts;


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


                #pragma warning disable IDE0028
                List<Byte> result = new(encLength / maxEncodingLength);  //  Real message length
                #pragma warning restore IDE0028


                for (var chunk = 0; chunk < chunkCount; chunk++)
                {
                    Int32 thisRoundLength =
                        Math.Min
                        (
                            encLength - chunk * chunkSize,
                            chunkSize
                        );

                    var shiftStartId = (chunk * shPerChunk) % shCount;
                    List<Byte> shifts = shiftStartId + thisRoundLength > shCount ?
                        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                         .. allShifts.GetRange(0, shiftStartId)]
                          : allShifts.GetRange(shiftStartId, thisRoundLength);


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
                            thisRoundLength / maxEncodingLength,
                            ref decodedId
                        )
                    );
                }

                return result;
            }


            static public void DecryptFastBinaryFile(string inputPath, string outputPath, BinaryKey reKey)
            {
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount;
                List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet, allShifts = reKey.Shifts;


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



                //using BinaryReader reader = new(inputPath);
                //using BinaryWriter writer = new(outputPath);

                //Byte[] messageChunk = new Byte[chunkSize];
                //Int32 offset = 0, thisRoundLength = reader.ReadBlock(messageChunk, offset, chunkSize);

                //while (thisRoundLength > 0)
                //{
                //    var shiftStartId = (offset / maxEncodingLength) % shCount;
                //    List<Byte> shifts = shiftStartId + thisRoundLength > shCount ?
                //        [.. allShifts.GetRange(shiftStartId, shCount - shiftStartId),
                //         .. allShifts.GetRange(0, shiftStartId)]
                //          : allShifts.GetRange(shiftStartId, thisRoundLength);

                //    writer.Write
                //    (
                //        DecryptionRound
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
                //            thisRoundLength / maxEncodingLength,
                //            ref decodedId
                //        )
                //    );

                //    offset += thisRoundLength;
                //    thisRoundLength = reader.ReadBlock(messageChunk, offset, chunkSize);
                //}
            }
        }
    }
}