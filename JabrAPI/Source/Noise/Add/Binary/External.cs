using System;
using System.Linq;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.Noise
{
    static public partial class Add
    {
        static public List<Byte> Bytes(List<Byte> message, IBinaryKey reKey,
            out Exception? exception)
        {
            if (message == null || message.Count < 1)
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
                    "Encryption key is undefined (null)",
                    nameof(reKey)
                );
            }
            else if (reKey.Noisifier == null)
            {
                exception = new ArgumentException
                (
                    "Noisifier is undefined (null)",
                    nameof(reKey.Noisifier)
                );
            }
            else
            {
                try
                {
                    reKey.Noisifier.IsComplexNoiseValidForKey(reKey, true);
                    reKey.Noisifier.IsPrimaryNoiseValidForKey(reKey, true);

                    reKey.Noisifier.IsComplexNoiseValidForMessage(message, true);
                    reKey.Noisifier.IsPrimaryNoiseValidForMessage(message, true);

                    List<Byte> result = FastBytes(message, reKey.Noisifier);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return [];
        }
        static public List<Byte> Bytes(List<Byte> message, IBinaryKey reKey,
            bool throwException = false)
        {
            if (message == null || message.Count < 1)
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
                        "Encryption key is undefined (null)",
                        nameof(reKey)
                    );
                }
            }
            else if (reKey.Noisifier == null)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Noisifier is undefined (null)",
                        nameof(reKey.Noisifier)
                    );
                }
            }
            else if (reKey.Noisifier.IsPrimaryNoiseValidForKey(reKey, throwException)
                  && reKey.Noisifier.IsComplexNoiseValidForKey(reKey, throwException)
                  && reKey.Noisifier.IsPrimaryNoiseValidForMessage(message, throwException)
                  && reKey.Noisifier.IsComplexNoiseValidForMessage(message, throwException))
            {
                try
                {
                    return FastBytes(message, reKey.Noisifier);
                }
                catch { if (throwException) throw; }
            }
            return [];
        }



        static public List<Byte> Bytes(List<Byte> message, BinaryNoisifier noisifier,
            out Exception? exception)
        {
            if (message == null || message.Count < 1)
            {
                exception = new ArgumentException
                (
                    "Message is invalid - cannot be null or empty",
                    nameof(message)
                );
            }
            else if (noisifier == null)
            {
                exception = new ArgumentException
                (
                    "Noisifier is undefined (null)",
                    nameof(noisifier)
                );
            }
            else
            {
                try
                {
                    noisifier.IsComplexNoiseValidForMessage(message, true);
                    noisifier.IsPrimaryNoiseValidForMessage(message, true);

                    List<Byte> result = FastBytes(message, noisifier);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return [];
        }
        static public List<Byte> Bytes(List<Byte> message, BinaryNoisifier noisifier,
            bool throwException = false)
        {
            if (message == null || message.Count < 1)
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
            else if (noisifier == null)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Noisifier is undefined (null)",
                        nameof(noisifier)
                    );
                }
            }
            else if (noisifier.IsComplexNoiseValidForMessage(message, throwException)
                  && noisifier.IsPrimaryNoiseValidForMessage(message, throwException))
            {
                try
                {
                    return FastBytes(message, noisifier);
                }
                catch { if (throwException) throw; }
            }
            return [];
        }
        static public List<Byte> FastBytes(List<Byte> message, BinaryNoisifier noisifier)
        {
            return Internal.AddFastBytes
            (
                message,
                noisifier,
                [.. message.Distinct()]
            );
        }
    }
}