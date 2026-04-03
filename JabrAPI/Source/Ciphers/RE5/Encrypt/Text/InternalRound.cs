using System;
using System.Text;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI.RE5
{
    static internal partial class Internal
    {
        static public string EncryptionRound(
            string messageChunk,
            string prAlphabet, string exAlphabet,
            List<Int16> shifts,
            Int32 exLength, Int32 maxEncodingLength,
            ref Int32[] ids)
        {
            Int32 messageLength = messageChunk.Length, shCount = shifts.Count, buffer = ids[1] + ids[0] + shifts[0];

            string encoding = Numsys.ToCustomAsString
            (
                (buffer / exLength).ToString(),
                10,
                exLength,
                exAlphabet,
                maxEncodingLength
            );

            StringBuilder encrypted = new(messageLength * (maxEncodingLength + 1));
            encrypted.Append(exAlphabet[buffer % exLength] + encoding);


            for (var curId = 1; curId < messageLength; curId++)
            {
                ids[1] = prAlphabet.IndexOf(messageChunk[curId]);
                buffer = ids[1] + ids[0] + shifts[curId % shCount];
                ids[0] = ids[1];

                encoding = Numsys.ToCustomAsString
                (
                    (buffer / exLength).ToString(),
                    10,
                    exLength,
                    exAlphabet,
                    maxEncodingLength
                );

                encrypted.Append(exAlphabet[buffer % exLength] + encoding);
            }

            return encrypted.ToString();
        }
    }
}