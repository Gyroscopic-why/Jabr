using System;
using System.Collections.Generic;


using JabrAPI.Template;
using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class Noise
    {
        static public partial class Add
        {
            static public string Text(string message, IEncryptionKey reKey,
                    out Exception? exception)
            {
                if (IsMessageAndReKeyAndNoisifierValid(message, reKey, out exception) &&
                    reKey.Noisifier.IsValid.ForMessageAndReKey(reKey, message, out exception))
                {
                    try
                    {
                        return FastText(message, reKey.Noisifier);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static public string Text(string message, IEncryptionKey reKey,
                bool throwExceptions = false)
            {
                string result = Text(message, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }

            static public string Text(string message, Noisifier noisifier,
                out Exception? exception)
            {
                if (IsMessageAndNoisifierValid(message, noisifier, out exception) &&
                    noisifier.IsValid.ForMessage(message, out exception))
                {
                    try
                    {
                        return FastText(message, noisifier);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static public string Text(string message, Noisifier noisifier,
                bool throwExceptions = false)
            {
                string result = Text(message, noisifier, out Exception? exception);
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
            static public void FastTextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, Noisifier noisifier)
            {
                Internal.AddFastTextFile
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