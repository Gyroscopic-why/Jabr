using System;
using System.Collections.Generic;



namespace JabrAPI
{
    static public partial class RE5
    {
        static public partial class Decrypt
        {
            static public partial class WithNoise
            {
                static public List<Byte> Binary(List<Byte> encrypted, BinaryKey reKey, out Exception? exception)
                {
                    List<Byte> denoised = Noise.Remove.Binary(encrypted, reKey, out exception);
                    return denoised == null || denoised.Count < 1 ? []
                         : Decrypt.Binary(denoised, reKey, out exception);
                }
                static public List<Byte> Binary(List<Byte> encrypted, BinaryKey reKey, bool throwExceptions = false)
                {
                    List<Byte> denoised = Noise.Remove.Binary(encrypted, reKey, throwExceptions);
                    return denoised == null || denoised.Count < 1 ? []
                         : Decrypt.Binary(denoised, reKey, throwExceptions);
                }


                static public List<Byte> FastBinary(List<Byte> encrypted, BinaryKey reKey)
                {
                    List<Byte> denoised = Noise.Remove.FastBinary(encrypted, reKey.Noisifier);
                    return denoised == null || denoised.Count < 1 ? []
                         : Decrypt.FastBinary(denoised, reKey);
                }
            }
        }
    }
}