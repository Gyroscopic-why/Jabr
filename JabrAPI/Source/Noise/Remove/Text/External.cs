using System;


using JabrAPI.Template;
using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class Noise
    {
        static public partial class Remove
        {
            static public string Text(string noised, IEncryptionKey reKey,
                out Exception? exception)
            {
                if (IsMessageAndReKeyAndNoisifierValid(noised, reKey, out exception) &&
                    reKey.Noisifier.IsValid.ForReKey(reKey, out exception))
                {
                    try
                    {
                        return FastText(noised, reKey.Noisifier);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static public string Text(string noised, IEncryptionKey reKey,
                bool throwExceptions = false)
            {
                string result = Text(noised, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }

            static public string Text(string noised, Noisifier noisifier,
                out Exception? exception)
            {
                if (IsMessageAndNoisifierValid(noised, noisifier, out exception))
                {
                    try
                    {
                        return FastText(noised, noisifier);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static public string Text(string noised, Noisifier noisifier,
                bool throwExceptions = false)
            {
                string result = Text(noised, noisifier, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }


            static public bool TextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey,
                    out Exception? exception)
            {
                if (IsReKeyValid(reKey, out exception) &&
                    IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        FastTextFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey.Noisifier);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static public bool TextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey,
                bool throwExceptions = false)
            {
                bool result = TextFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey, out Exception? exception);
                if (!result && throwExceptions) throw exception!;
                return result;
            }
            static public bool TextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, Noisifier noisifier,
                    out Exception? exception)
            {
                if (IsNoisifierValid(noisifier, out exception))
                {
                    try
                    {
                        FastTextFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, noisifier);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static public bool TextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, Noisifier noisifier,
                bool throwExceptions = false)
            {
                bool result = TextFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, noisifier, out Exception? exception);
                if (!result && throwExceptions) throw exception!;
                return result;
            }


            static public bool TextFile(string absoluteInputDirectory, string fileName, IEncryptionKey reKey, out Exception? exception)
                => TextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, out exception);
            static public bool TextFile(string absoluteInputDirectory, string fileName, IEncryptionKey reKey, bool throwExceptions = false)
                => TextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, throwExceptions);
            static public bool TextFile(string absoluteInputDirectory, string fileName, Noisifier noisifier, out Exception? exception)
                => TextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, noisifier, out exception);
            static public bool TextFile(string absoluteInputDirectory, string fileName, Noisifier noisifier, bool throwExceptions = false)
                => TextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, noisifier, throwExceptions);



            static public string FastText(string noised, Noisifier noisifier)
            {
                return Internal.RemoveFastText
                (
                    noised,
                    noisifier
                );
            }
            static public void FastTextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, Noisifier noisifier)
            {
                Internal.RemoveFastTextFile
                (
                    absoluteInputDirectory,
                    fileName,
                    absoluteOutputDirectory,
                    noisifier
                );
            }
        }
    }
}