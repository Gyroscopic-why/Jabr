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

            public EncryptionKey(string alphabet, List<Int32> shifts)
            {
                _alphabet = alphabet;

                _shifts.Clear();
                if (shifts == null || shifts.Count == 0) _shifts.Add(0);
                else _shifts.AddRange(shifts);
            }
            public EncryptionKey(string alphabet, Int32 shift)
            {
                _alphabet = alphabet;

                _shifts.Clear();
                _shifts.Add(shift);
            }
            public EncryptionKey(string alphabet) => _alphabet = alphabet;
            public EncryptionKey(Int32 shiftCount) => _shCount = shiftCount;
            public EncryptionKey(RE4.BinaryKey binKey)
            {
                _alphabet = Encoding.Unicode.GetString(ToBinary.LittleEndian(binKey.Alphabet.ToArray()));

                _shifts.Clear();
                if (binKey.Shifts == null || binKey.Shifts.Count == 0) _shifts.Add(0);
                else _shifts.AddRange(binKey.Shifts);
            }
            public EncryptionKey(bool autoGenerate = true)
            {
                if (autoGenerate) Default();
                else SetDefault();
            }
        }
        public class BinaryKey : RE4.BinaryKey
        {
            public BinaryKey(string alphabet, List<Int32> shifts)
                => Set(alphabet, shifts);
            public BinaryKey(RE4.EncryptionKey reKey) => Set(reKey);
            public BinaryKey() { }
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



        static public string Decrypt(string encMessage, EncryptionKey reKey, bool throwException = false)
        {
            if (encMessage == null || encMessage == "" || encMessage.Length < 1)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Encrypted message is invalid - cannot be null or empty",
                        nameof(encMessage)
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
            else if (reKey.IsAlphabetValid(encMessage, throwException))
            {
                try
                {
                    return FastDecrypt(encMessage, reKey);
                }

                catch (Exception)
                {
                    if (throwException) throw;
                }
            }

            return "";
        }
        static public string Decrypt(string encMessage, EncryptionKey reKey, out Exception? exception)
        {
            if (encMessage == null || encMessage == "" || encMessage.Length < 1)
            {
                exception = new ArgumentException
                (
                    "Encrypted message is invalid - cannot be null or empty",
                    nameof(encMessage)
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
                    reKey.IsAlphabetValid(encMessage, true);

                    string result = FastDecrypt(encMessage, reKey);
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
                decrypted += alphabet[(eID[i] - eID[i - 1] - shifts[i % shCount] + 3 * aLength) % aLength];
            }
            return decrypted;
        }
    }
}