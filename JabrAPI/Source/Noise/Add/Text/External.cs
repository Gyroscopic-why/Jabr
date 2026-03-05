using System;
using System.Collections.Generic;
using JabrAPI.Template;



namespace JabrAPI.Noise
{
    static public partial class Add
    {
        static public string Text(string message, IEncryptionKey reKey,
                out Exception? exception)
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
            else if (reKey.Noisifier == null)
            {
                exception = new ArgumentException
                (
                    "Noisifier is undefined (null or empty)",
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

                    string result = FastText(message, reKey);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string Text(string message, IEncryptionKey reKey,
            bool throwException = false)
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
            else if (reKey.Noisifier == null)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Noisifier is undefined (null or empty)",
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
                    return FastText(message, reKey);
                }
                catch { if (throwException) throw; }
            }
            return "";
        }
        static public string FastText(string message, IEncryptionKey reKey)
        {
            return Internal.OLD_FastText
            (
                message,
                reKey.Noisifier,
                string.Concat
                (
                    new HashSet<char>
                    (reKey.FinalAlphabet)
                )
            );
        }



        static public string Text(string message, Noisifier noisifier,
            out Exception? exception)
        {
            if (message == null || message == "" || message.Length < 1)
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
                    "Noisifier is undefined (null or empty)",
                    nameof(noisifier)
                );
            }
            else
            {
                try
                {
                    noisifier.IsComplexNoiseValidForMessage(message, true);
                    noisifier.IsPrimaryNoiseValidForMessage(message, true);

                    string result = FastText(message, noisifier);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string Text(string message, Noisifier noisifier,
            bool throwException = false)
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
            else if (noisifier == null)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Noisifier is undefined (null or empty)",
                        nameof(noisifier)
                    );
                }
            }
            else if (noisifier.IsComplexNoiseValidForMessage(message, throwException)
                  && noisifier.IsPrimaryNoiseValidForMessage(message, throwException))
            {
                try
                {
                    return FastText(message, noisifier);
                }
                catch { if (throwException) throw; }
            }
            return "";
        }
        static public string FastText(string message, Noisifier noisifier)
        {
            return Internal.OLD_FastText
            (
                message,
                noisifier,
                string.Concat
                (
                    new HashSet<char>
                    (message)
                )
            );
        }
    }
}