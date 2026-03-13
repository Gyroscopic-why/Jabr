using System;
using System.Linq;
using System.Collections.Generic;


using AVcontrol;
using static JabrAPI.Miscellaneous;



namespace JabrAPI.RE5
{
    static public partial class Decrypt
    {
        static public string Text(string encrypted, EncryptionKey reKey, out Exception? exception)
        {
            if (IsMessageAndReKeyAndNoisifierValid(encrypted, reKey, out exception))
            {
                try
                {
                    reKey.IsValid.ForDecryption(encrypted, true);

                    return FastText(encrypted, reKey);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string Text(string encrypted, EncryptionKey reKey, bool throwExceptions = false)
        {
            string result = Text(encrypted, reKey, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }


        static public string FastText(string encrypted, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, shCount = reKey.ShCount, encCurId = 0, buffer;
            List<Int16> shifts = reKey.Shifts; string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;

            Int32 helper = (Int32)Math.Ceiling
                (
                    (double)
                    (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                        reKey.PrLength * 2 + shifts.Max() - 4
                    ) / exLength
                );
            Int32 maxEncodingLength = exLength == 10 ?
                Utils.DigitCount(helper)  // Optimisation for base 10 encoding
                : Numsys.AsList
                (
                    helper.ToString(),
                    10,
                    exLength
                ).Count;

            Int32 realMessageLength = encrypted.Length / (maxEncodingLength + 1);
            Int32 parsedEncoding = (Int32)Numsys.ToDecimalFromCustom
            (
                Utils.Interval
                (
                    encrypted,
                    1,
                    1 + maxEncodingLength
                ),
                exLength,
                exAlphabet
            );

            Int32[] decodedIds = new Int32[realMessageLength];
            decodedIds[0] = exAlphabet.IndexOf(encrypted[0]) - shifts[0] + parsedEncoding * exLength;
            string decrypted = prAlphabet[decodedIds[0]].ToString();


            for (var curId = 1; curId < realMessageLength; curId++)
            {
                encCurId += maxEncodingLength + 1;
                buffer = exAlphabet.IndexOf(encrypted[encCurId])
                    - decodedIds[curId - 1]
                    - shifts[curId % shCount];

                parsedEncoding = (Int32)Numsys.ToDecimalFromCustom
                (
                    Utils.Interval
                    (
                        encrypted,
                        encCurId + 1,
                        encCurId + 1 + maxEncodingLength
                    ),
                    exLength,
                    exAlphabet
                );

                decodedIds[curId] = buffer + parsedEncoding * exLength;
                decrypted += prAlphabet[decodedIds[curId]];
            }

            return decrypted;
        }
    }
}