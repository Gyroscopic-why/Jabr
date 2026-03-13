using System;
using System.Collections.Generic;



namespace JabrAPI.RE5
{
    static public partial class Decrypt
    {
        static public partial class WithNoise
        {
            static public List<Byte> Bytes(List<Byte> encrypted, BinaryKey reKey, out Exception? exception)
            {
                List<Byte> denoised = Noise.Remove.Bytes(encrypted, reKey, out exception);
                return denoised == null || denoised.Count < 1 ? []
                     : Decrypt.Bytes(denoised, reKey, out exception);
            }
            static public List<Byte> Bytes(List<Byte> encrypted, BinaryKey reKey, bool throwExceptions = false)
            {
                List<Byte> denoised = Noise.Remove.Bytes(encrypted, reKey, throwExceptions);
                return denoised == null || denoised.Count < 1 ? []
                     : Decrypt.Bytes(denoised, reKey, throwExceptions);
            }


            static public List<Byte> FastBytes(List<Byte> encrypted, BinaryKey reKey)
            {
                List<Byte> denoised = Noise.Remove.FastBytes(encrypted, reKey.Noisifier);
                return denoised == null || denoised.Count < 1 ? []
                     : Decrypt.FastBytes(denoised, reKey);
            }
        }
    }
}