using System;
using System.Linq;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI.RE5
{
    static public partial class Decrypt
    {
        static public List<Byte> Bytes(List<Byte> encrypted, BinaryKey reKey, bool throwException = false)
        {
            if (encrypted == null || encrypted.Count < 1)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Encrypted message is invalid - cannot be null or empty",
                        nameof(encrypted)
                    );
                }
            }
            else if (reKey == null)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Encryption key is undefined (null or empty)",
                        nameof(reKey)
                    );
                }
            }
            else if (reKey.IsPrimaryPartiallyValid(throwException)
                    && reKey.IsExternalValid(encrypted, throwException))
            {
                try
                {
                    return FastBytes(encrypted, reKey);
                }
                catch (Exception) { if (throwException) throw; }
            }
            return [];
        }
        static public List<Byte> Bytes(List<Byte> encrypted, BinaryKey reKey, out Exception? exception)
        {
            if (encrypted == null || encrypted.Count < 1)
            {
                exception = new ArgumentException
                (
                    "Encrypted message is invalid - cannot be null or empty",
                    nameof(encrypted)
                );
            }
            else if (reKey == null)
            {
                exception = new ArgumentException
                (
                    "Encryption key is undefined (null or empty)",
                    nameof(reKey)
                );
            }
            else
            {
                try
                {
                    reKey.IsPrimaryPartiallyValid(true);
                    reKey.IsExternalValid(encrypted, true);

                    List<Byte> result = FastBytes(encrypted, reKey);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return [];
        }
        static public List<Byte> FastBytes(List<Byte> encrypted, BinaryKey reKey)
        {
            Int32 exLength = reKey.ExLength, shCount = reKey.ShCount, encCurId = 0, buffer;
            List<Byte> shifts = reKey.Shifts; List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;

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

            Int32 realMessageLength = encrypted.Count / (maxEncodingLength + 1);
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
            List<Byte> decrypted = [prAlphabet[decodedIds[0]]];


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
                decrypted.Add(prAlphabet[decodedIds[curId]]);
            }

            return decrypted;
        }
    }
}