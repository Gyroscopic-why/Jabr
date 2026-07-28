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
                Int32 decodedId  = 0, shiftStartId = 0;
                Int32 thisRoundLength, realMessageLength, shDelta;


                StringBuilder result = new(encLength / maxEncodingLength);  //  Real message length

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
                            realMessageLength,
                            ref decodedId
                        )
                    );
                }

                return result.ToString();
            }


            static public void DecryptFastTextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey)
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

                using StreamReader reader = new(Path.Combine(absoluteInputDirectory,  fileName));
                using StreamWriter writer = new(Path.Combine(absoluteOutputDirectory, finalFileName));


                char[] messageChunk = new char[chunkSize];
                Int32 offset = 0, shiftStartId = 0, decodedId = 0; 
                Int32 realMessageLength, shDelta, charsRead;

                while ((charsRead = reader.ReadBlock(messageChunk, 0, chunkSize)) > 0)
                {
                    realMessageLength = charsRead / maxEncodingLength;
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
                            new string(messageChunk, 0, charsRead),
                            prAlphabet,
                            exAlphabet,
                            shifts,
                            exLength,
                            maxEncodingLength,
                            realMessageLength,
                            ref decodedId
                        )
                    );

                    offset += charsRead;
                }
            }
        }
    }
}