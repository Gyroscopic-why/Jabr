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
                    reKey.Noisifier.IsValid.ForAdding(reKey, message, true);

                    string result = FastText(message, reKey.Noisifier);
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
            else if (reKey.Noisifier.IsValid.ForAdding(message, throwException))
            {
                try
                {
                    return FastText(message, reKey.Noisifier);
                }
                catch { if (throwException) throw; }
            }
            return "";
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
                    "Noisifier is undefined (null)",
                    nameof(noisifier)
                );
            }
            else
            {
                try
                {
                    noisifier.IsValid.ForAdding(message, true);

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
                        "Noisifier is undefined (null)",
                        nameof(noisifier)
                    );
                }
            }
            else if (noisifier.IsValid.ForAdding(message, throwException))
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
            return Internal.AddFastText
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