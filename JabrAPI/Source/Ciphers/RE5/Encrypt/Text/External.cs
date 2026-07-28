using System;
using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class RE5
    {
        static public partial class Encrypt
        {
            static public string Text(string message, EncryptionKey reKey, out Exception? exception)
            {
                if (IsMessageAndReKeyAndNoisifierValid(message, reKey, out exception) &&
                    reKey.IsValid.ForEncryption(message, out exception))
                {
                    try
                    {
                        return FastText(message, reKey);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static public string Text(string message, EncryptionKey reKey, bool throwExceptions = false)
            {
                string result = Text(message, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }


            static public bool TextFile(string absoluteInputDirectory, string fileName, string absoluteOutputDirectory,
                EncryptionKey reKey, out Exception? exception)
            {
                if (IsReKeyValid(reKey, out exception) &&
                    IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        FastTextFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static public bool TextFile(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, out Exception? exception)
                => TextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, out exception);
            static public bool TextFile(string absoluteInputDirectory, string fileName, string absoluteOutputDirectory,
                EncryptionKey reKey, bool throwExceptions = false)
            {
                bool result = TextFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }
            static public bool TextFile(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, bool throwExceptions = false)
                => TextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, throwExceptions);



            static public string FastText(string message, EncryptionKey reKey)
                => Internal.EncryptFastText(message, reKey);
            static public void FastTextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey)
                => Internal.EncryptFastTextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey);
        }
    }
}