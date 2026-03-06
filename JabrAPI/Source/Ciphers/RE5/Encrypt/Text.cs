using System;
using System.Linq;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI.RE5
{
    static public partial class Encrypt
    {
        static public string Text(string message, EncryptionKey reKey, bool throwException = false)
        {
            if (message == null || message == "" || message.Length < 1)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Message is invalid - cannot be null or empty",
                        nameof(message)
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
            else if (reKey.IsValid.ForEncryption(message, throwException))
            {
                try
                {
                    return FastText(message, reKey);
                }
                catch (Exception) { if (throwException) throw; }
            }
            return "";
        }
        static public string Text(string message, EncryptionKey reKey, out Exception? exception)
        {
            if (message == null || message == "" || message.Length < 1)
            {
                exception = new ArgumentException
                (
                    "Message is invalid - cannot be null or empty",
                    nameof(message)
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
                    reKey.IsValid.ForEncryption(message, true);

                    string result = FastText(message, reKey);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string FastText(string message, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, messageLength = message.Length, shCount = reKey.ShCount, buffer;
            List<Int16> shifts = reKey.Shifts; string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;


            Int32 helper = (Int32)Math.Ceiling
                (
                    (double)
                    (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                        reKey.PrLength * 2 + shifts.Max() - 4
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

            Int32[] ids = new Int32[messageLength];
            ids[0] = prAlphabet.IndexOf(message[0]);
            buffer = ids[0] + shifts[0];

            string encoding = Numsys.ToCustomAsString
            (
                (buffer / exLength).ToString(),
                10,
                exLength,
                exAlphabet,
                maxEncodingLength
            );
            string encrypted = exAlphabet[buffer % exLength] + encoding;


            for (var curId = 1; curId < messageLength; curId++)
            {
                ids[curId] = prAlphabet.IndexOf(message[curId]);
                buffer = ids[curId] + shifts[curId % shCount] + ids[curId - 1];

                encoding = Numsys.ToCustomAsString
                (
                    (buffer / exLength).ToString(),
                    10,
                    exLength,
                    exAlphabet,
                    maxEncodingLength
                );

                encrypted += exAlphabet[buffer % exLength] + encoding;
            }

            return encrypted;
        }
    }
}