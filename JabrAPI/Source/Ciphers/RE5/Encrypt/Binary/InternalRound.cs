using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class Internal
        {
            static public List<Byte> EncryptionRound(
                List<Byte> messageChunk,
                List<Byte> prAlphabet, List<Byte> exAlphabet,
                List<Byte> shifts,
                Int32 exLength, Int32 maxEncodingLength,
                ref Int32 prevId)
            {
                Int32 messageLength = messageChunk.Count, shCount = shifts.Count;
                Int32 buffer = prAlphabet.IndexOf(messageChunk[0]), curFinal = prevId + buffer + shifts[0];
                prevId = buffer;

                List<Byte> encoding = Numsys.ToCustomAsBinary
                (
                    Split.BigEndian<Int32, Byte>(curFinal / exLength, 10),
                    10,
                    exLength,
                    exAlphabet,
                    maxEncodingLength
                );

                #pragma warning disable IDE0028
                List<Byte> encrypted = new(messageLength * (maxEncodingLength + 1));
                encrypted.AddRange([exAlphabet[curFinal % exLength], .. encoding]);
                #pragma warning restore IDE0028


                for (var curId = 1; curId < messageLength; curId++)
                {
                    buffer   = prAlphabet.IndexOf(messageChunk[curId]);
                    curFinal = buffer + prevId + shifts[curId % shCount];
                    prevId   = buffer;

                    encoding = Numsys.ToCustomAsBinary
                    (
                        Split.BigEndian<Int32, Byte>(curFinal / exLength, 10),
                        10,
                        exLength,
                        exAlphabet,
                        maxEncodingLength
                    );

                    encrypted.AddRange([exAlphabet[curFinal % exLength], .. encoding]);
                }

                return encrypted;
            }
        }
    }
}