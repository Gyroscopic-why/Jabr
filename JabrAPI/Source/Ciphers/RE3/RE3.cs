using System;
using System.Text;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    public class RE3
    {
        public class EncryptionKey : RE4.EncryptionKey
        {
            public EncryptionKey(string alphabet, List<Int32> shifts) : base(alphabet, shifts) { }
            public EncryptionKey(string alphabet, Int32 shift) : base(alphabet, shift) { }
            public EncryptionKey(string alphabet) => _alphabet = alphabet;
            public EncryptionKey(Int32 shiftCount) : base(shiftCount) { }
            public EncryptionKey(RE4.BinaryKey binKey) : base(binKey) { }
            public EncryptionKey(bool autoGenerate = true) : base(autoGenerate) { }
        }

        public class BinaryKey : RE4.BinaryKey
        {
            public BinaryKey(string alphabet, List<Int32> shifts)
                => Set(alphabet, shifts);
            public BinaryKey(RE4.EncryptionKey reKey) => Set(reKey);
            public BinaryKey() : base() { }
        }



        static public string Encrypt(string message, RE4.EncryptionKey reKey, bool throwException = false)
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
            else if (reKey.IsAlphabetValid(message, throwException))
            {
                try
                {
                    return FastEncrypt(message, reKey);
                }

                catch (Exception)
                {
                    if (throwException) throw;
                }
            }

            return "";
        }
        static public string Encrypt(string message, RE4.EncryptionKey reKey, out Exception? exception)
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
                    reKey.IsAlphabetValid(message, true);

                    string result = FastEncrypt(message, reKey);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string FastEncrypt(string message, RE4.EncryptionKey reKey)
        {
            Int32 aLength = reKey.AlphabetLength, messageLength = message.Length, shCount = reKey.ShCount;
            Int32[] eID = new Int32[messageLength];
            List<Int32> shifts = reKey.Shifts;
            string alphabet = reKey.Alphabet;

            Int32 buffer = alphabet.IndexOf(message[0]);
            eID[0] = (buffer + shifts[0]) % aLength;
            string encrypted = alphabet[eID[0]].ToString();

            for (var i = 1; i < messageLength; i++)
            {
                buffer = alphabet.IndexOf(message[i]);
                eID[i] = (buffer + eID[i - 1] + shifts[i % shCount]) % aLength;
                encrypted += alphabet[eID[i]];
            }
            return encrypted;
        }



        static public string Decrypt(string encrypted, EncryptionKey reKey, bool throwException = false)
        {
            if (encrypted == null || encrypted == "" || encrypted.Length < 1)
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
            else if (reKey.IsAlphabetValid(encrypted, throwException))
            {
                try
                {
                    return FastDecrypt(encrypted, reKey);
                }

                catch (Exception)
                {
                    if (throwException) throw;
                }
            }

            return "";
        }
        static public string Decrypt(string encrypted, EncryptionKey reKey, out Exception? exception)
        {
            if (encrypted == null || encrypted == "" || encrypted.Length < 1)
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
                    reKey.IsAlphabetValid(encrypted, true);

                    string result = FastDecrypt(encrypted, reKey);
                    exception = null;

                    return result;
                }
                catch (Exception innerException)
                {
                    exception = innerException;
                }
            }
            return "";
        }
        static public string FastDecrypt(string encrypted, RE4.EncryptionKey reKey)
        {
            Int32 aLength = reKey.AlphabetLength, messageLength = encrypted.Length, shCount = reKey.ShCount;
            Int32[] eID = new Int32[messageLength];
            List<Int32> shifts = reKey.Shifts;
            string alphabet = reKey.Alphabet;

            eID[0] = alphabet.IndexOf(encrypted[0]);
            string decrypted = alphabet[(eID[0] - shifts[0] + aLength) % aLength].ToString();

            for (var i = 1; i < messageLength; i++)
            {
                eID[i] = alphabet.IndexOf(encrypted[i]);
                decrypted += alphabet[(eID[i] - eID[i - 1] - shifts[i % shCount] + 4 * aLength) % aLength];
            }
            return decrypted;
        }
    }
}