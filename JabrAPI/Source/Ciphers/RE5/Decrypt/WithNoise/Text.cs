using System;



namespace JabrAPI.RE5
{
    static public partial class Decrypt
    {
        static public partial class WithNoise
        {
            static public string Text(string encrypted, EncryptionKey reKey, out Exception? exception)
            {
                string denoised = Noise.Remove.Text(encrypted, reKey, out exception);
                return denoised == null || denoised.Length < 1 ? ""
                     : Decrypt.Text(denoised, reKey, out exception);
            }
            static public string Text(string encrypted, EncryptionKey reKey, bool throwExceptions = false)
            {
                string denoised = Noise.Remove.Text(encrypted, reKey, throwExceptions);
                return denoised == null || denoised.Length < 1 ? ""
                     : Decrypt.Text(denoised, reKey, throwExceptions);
            }


            static public string FastText(string encrypted, EncryptionKey reKey)
            {
                string denoised = Noise.Remove.FastText(encrypted, reKey.Noisifier);
                return denoised == null || denoised.Length < 1 ? ""
                     : Decrypt.FastText(denoised, reKey);
            }
        }
    }
}