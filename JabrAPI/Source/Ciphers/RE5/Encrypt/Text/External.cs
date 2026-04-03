using System;
using static JabrAPI.Miscellaneous;



namespace JabrAPI.RE5
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


        static public bool TextFile(string inputPath, string outputPath,
            EncryptionKey reKey, out Exception? exception)
        {
            if (IsReKeyValid(reKey, out exception) &&
                IsNoisifierValid(reKey.Noisifier, out exception))
            {
                try
                {
                    FastTextFile(inputPath, outputPath, reKey);
                    return true;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return false;
        }
        static public bool TextFile(string inputPath, string outputPath,
            EncryptionKey reKey, bool throwExceptions = false)
        {
            bool result = TextFile(inputPath, outputPath, reKey, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }



        static public string FastText(string message, EncryptionKey reKey)
            => Internal.FastText(message, reKey);
        static public void FastTextFile(string inputPath, string outputPath, EncryptionKey reKey)
            => Internal.FastTextFile(inputPath, outputPath, reKey);
    }
}