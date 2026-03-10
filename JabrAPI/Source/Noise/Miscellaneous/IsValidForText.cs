using System;


using JabrAPI.Template;



namespace JabrAPI.Noise
{
    static internal partial class Miscellaneous
    {
        static internal bool IsMessageValid(string message, out  Exception? exception)
        {
            if (message  == null || message == "" || message.Length < 1)
            {
                exception = new ArgumentException
                (
                    "Message is invalid - cannot be null or empty",
                    nameof(message)
                );
                return false;
            }
            exception = null;
            return true;
        }
        static internal bool IsMessageValid(string message, bool throwException = false)
        {
            bool isValid = IsMessageValid(message, out Exception? exception);
            if (!isValid && throwException) throw exception!;
            return isValid;
        }

        static internal bool IsNoisifierValid(Noisifier noisifier, out  Exception? exception)
        {
            if (noisifier == null)
            {
                exception = new ArgumentException
                (
                    "Noisifier is undefined (null)",
                    nameof(noisifier)
                );
                return false;
            }
            exception = null;
            return true;
        }
        static internal bool IsNoisifierValid(Noisifier noisifier, bool throwException = false)
        {
            bool isValid = IsNoisifierValid(noisifier, out Exception? exception);
            if (!isValid && throwException) throw exception!;
            return isValid;
        }

        static internal bool IsReKeyValid(IEncryptionKey reKey, out  Exception? exception)
        {
            if (reKey == null)
            {
                exception = new ArgumentException
                (
                    "Encryption key is undefined (null)",
                    nameof(reKey)
                );
                return false;
            }
            exception = null;
            return true;
        }
        static internal bool IsReKeyValid(IEncryptionKey reKey, bool throwException = false)
        {
            bool isValid = IsReKeyValid(reKey, out Exception? exception);
            if (!isValid && throwException) throw exception!;
            return isValid;
        }


        static internal bool IsMessageAndNoisifierValid(
            string message, Noisifier noisifier, out  Exception? exception)
        {
            bool isValid = IsMessageValid(message, out exception);
            if (isValid) isValid = IsNoisifierValid(noisifier, out exception);
            return isValid;
        }
        static internal bool IsMessageAndNoisifierValid(
            string message, Noisifier noisifier, bool throwException = false)
        {
            bool isValid = IsMessageAndNoisifierValid(message, noisifier, out Exception? exception);
            if (!isValid && throwException) throw exception!;
            return isValid;
        }

        static internal bool IsMessageAndReKeyAndNoisifierValid(
            string message, IEncryptionKey reKey, out  Exception? exception)
        {
            bool isValid = IsReKeyValid(reKey, out exception);
            if (isValid) isValid = IsMessageAndNoisifierValid(message, reKey.Noisifier, out exception);
            return isValid;
        }
        static internal bool IsMessageAndReKeyAndNoisifierValid(
            string message, IEncryptionKey reKey, bool throwException = false)
        {
            bool isValid = IsMessageAndReKeyAndNoisifierValid(message, reKey, out Exception? exception);
            if (!isValid && throwException) throw exception!;
            return isValid;
        }
    }
}