using System;



namespace JabrAPI.RE5
{
    static public partial class Encrypt
    {
        static public partial class WithNoise
        {
            static public string Text(string message, EncryptionKey reKey, out Exception? exception)
            {
                string result = Encrypt.Text(message, reKey, out exception);
                return result == null || result.Length < 1 ? ""
                     : Noise.Add.Text(result, reKey, out exception);
            }
            static public string Text(string message, EncryptionKey reKey, bool throwExceptions = false)
            {
                string result = Encrypt.Text(message, reKey, throwExceptions);
                return result == null || result.Length < 1 ? ""
                     : Noise.Add.Text(result, reKey, throwExceptions);
            }


            static public string FastText(string message, EncryptionKey reKey)
            {
                string result = Encrypt.FastText(message, reKey);
                return result == null || result.Length < 1 ? ""
                     : Noise.Add.FastText(result, reKey.Noisifier);
            }
        }
    }
}