using System;
using System.Text;
using System.Collections.Generic;

using AVcontrol;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class Internal
        {
            static public string EncryptionRound(
                string messageChunk,
                string prAlphabet, string exAlphabet,
                List<Int16> shifts,
                Int32 exLength, Int32 maxEncodingLength,
                ref Int32 prevId)
            {
                Int32 messageLength = messageChunk.Length, shCount = shifts.Count;
                Int32 buffer = prAlphabet.IndexOf(messageChunk[0]), curFinal = prevId + buffer + shifts[0];
                prevId = buffer;

                string encoding = Numsys.ToCustomAsString128
                (
                    (curFinal / exLength).ToString(),
                    10,
                    exLength,
                    exAlphabet,
                    maxEncodingLength
                );

                StringBuilder encrypted = new(messageLength * (maxEncodingLength + 1));
                encrypted.Append(exAlphabet[curFinal % exLength] + encoding);


                for (var curId = 1; curId < messageLength; curId++)
                {
                    buffer = prAlphabet.IndexOf(messageChunk[curId]);
                    curFinal = buffer + prevId + shifts[curId % shCount];
                    prevId = buffer;

                    encoding = Numsys.ToCustomAsString128
                    (
                        (curFinal / exLength).ToString(),
                        10,
                        exLength,
                        exAlphabet,
                        maxEncodingLength
                    );

                    encrypted.Append(exAlphabet[curFinal % exLength] + encoding);
                }

                return encrypted.ToString();
            }
        }
    }
}