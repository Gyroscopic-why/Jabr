using System;


using JabrAPI.Template;



namespace JabrAPI.Noise
{
    static public partial class Remove
    {
        static public string Text(string noised, IEncryptionKey reKey,
            out Exception? exception)
        {
            if (noised == null || noised == "" || noised.Length < 1)
            {
                exception = new ArgumentException
                (
                    "Noised message is invalid - cannot be null or empty",
                    nameof(noised)
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
                    reKey.Noisifier.IsValid.ForRemoving(reKey, noised, true);

                    string result = FastText(noised, reKey.Noisifier);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string Text(string noised, IEncryptionKey reKey,
            bool throwException = false)
        {
            if (noised == null || noised == "" || noised.Length < 1)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Noised is invalid - cannot be null or empty",
                        nameof(noised)
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
            else if (reKey.Noisifier.IsValid.ForAdding(noised, throwException))
            {
                try
                {
                    return FastText(noised, reKey.Noisifier);
                }
                catch { if (throwException) throw; }
            }
            return "";
        }
        


        static public string Text(string noised, Noisifier noisifier,
            out Exception? exception)
        {
            if (noised == null || noised == "" || noised.Length < 1)
            {
                exception = new ArgumentException
                (
                    "Noised message is invalid - cannot be null or empty",
                    nameof(noised)
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
                    noisifier.IsValid.ForAdding(noised, true);

                    string result = FastText(noised, noisifier);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string Text(string noised, Noisifier noisifier,
            bool throwException = false)
        {
            if (noised == null || noised == "" || noised.Length < 1)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Noised message is invalid - cannot be null or empty",
                        nameof(noised)
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
            else if (noisifier.IsValid.ForAdding(noised, throwException))
            {
                try
                {
                    return FastText(noised, noisifier);
                }
                catch { if (throwException) throw; }
            }
            return "";
        }



        static public string FastText(string noised, Noisifier noisifier)
        {
            return Internal.RemoveFastText
            (
                noised,
                noisifier
            );
        }
    }
}