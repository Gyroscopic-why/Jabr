using System;

using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class RE5
    {
        static public partial class Decrypt
        {
            static public string Text(string encrypted, EncryptionKey reKey, out Exception? exception)
            {
                if (IsMessageAndReKeyAndNoisifierValid(encrypted, reKey, out exception) &&
                    reKey.IsValid.ForDecryption(encrypted, out exception))
                {
                    try
                    {
                        return FastText(encrypted, reKey);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static public string Text(string encrypted, EncryptionKey reKey, bool throwExceptions = false)
            {
                string result = Text(encrypted, reKey, out Exception? exception);
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
                        FastTextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey);
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
                bool result = TextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }
            static public bool TextFile(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, bool throwExceptions = false)
                => TextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, throwExceptions);



            static public string FastText(string message, EncryptionKey reKey)
                => Internal.DecryptFastText(message, reKey);
            static public void FastTextFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey)
                => Internal.DecryptFastTextFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey);
        }
    }
}