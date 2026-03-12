using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.Noise
{
    static internal partial class Miscellaneous
    {
        static internal bool IsMessageValid(List<Byte> message, out  Exception? exception)
        {
            if (message  == null || message.Count < 1)
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
        static internal bool IsMessageValid(List<Byte> message, bool throwExceptions = false)
        {
            bool isValid = IsMessageValid(message, out Exception? exception);
            if (!isValid && throwExceptions) throw exception!;
            return isValid;
        }

        static internal bool IsNoisifierValid(BinaryNoisifier noisifier, out  Exception? exception)
        {
            if (noisifier == null)
            {
                exception = new ArgumentException
                (
                    "BinaryNoisifier is undefined (null)",
                    nameof(noisifier)
                );
                return false;
            }
            exception = null;
            return true;
        }
        static internal bool IsNoisifierValid(BinaryNoisifier noisifier, bool throwExceptions = false)
        {
            bool isValid = IsNoisifierValid(noisifier, out Exception? exception);
            if (!isValid && throwExceptions) throw exception!;
            return isValid;
        }

        static internal bool IsReKeyValid(IBinaryKey reKey, out  Exception? exception)
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
        static internal bool IsReKeyValid(IBinaryKey reKey, bool throwExceptions = false)
        {
            bool isValid = IsReKeyValid(reKey, out Exception? exception);
            if (!isValid && throwExceptions) throw exception!;
            return isValid;
        }


        static internal bool IsMessageAndNoisifierValid(
            List<Byte> message, BinaryNoisifier noisifier, out  Exception? exception)
        {
            bool isValid = IsMessageValid(message, out exception);
            if (isValid) isValid = IsNoisifierValid(noisifier, out exception);
            return isValid;
        }
        static internal bool IsMessageAndNoisifierValid(
            List<Byte> message, BinaryNoisifier noisifier, bool throwExceptions = false)
        {
            bool isValid = IsMessageAndNoisifierValid(message, noisifier, out Exception? exception);
            if (!isValid && throwExceptions) throw exception!;
            return isValid;
        }

        static internal bool IsMessageAndReKeyAndNoisifierValid(
            List<Byte> message, IBinaryKey reKey, out  Exception? exception)
        {
            bool isValid = IsReKeyValid(reKey, out exception);
            if (isValid) isValid = IsMessageAndNoisifierValid(message, reKey.Noisifier, out exception);
            return isValid;
        }
        static internal bool IsMessageAndReKeyAndNoisifierValid(
            List<Byte> message, IBinaryKey reKey, bool throwExceptions = false)
        {
            bool isValid = IsMessageAndReKeyAndNoisifierValid(message, reKey, out Exception? exception);
            if (!isValid && throwExceptions) throw exception!;
            return isValid;
        }
    }
}