using System;
using System.Collections.Generic;



namespace JabrAPI.RE5
{
    static public partial class Encrypt
    {
        static public partial class WithNoise
        {
            static public List<Byte> Bytes(List<Byte> message, BinaryKey reKey, out Exception? exception)
            {
                List<Byte> result = Encrypt.Bytes(message, reKey, out exception);
                return result == null || result.Count < 1 ? []
                     : Noise.Add.Bytes(result, reKey, out exception);
            }
            static public List<Byte> Bytes(List<Byte> message, BinaryKey reKey, bool throwExceptions = false)
            {
                List<Byte> result = Encrypt.Bytes(message, reKey, throwExceptions);
                return result == null || result.Count < 1 ? []
                     : Noise.Add.Bytes(result, reKey, throwExceptions);
            }


            static public List<Byte> FastBytes(List<Byte> message, BinaryKey reKey)
            {
                List<Byte> result = Encrypt.FastBytes(message, reKey);
                return result == null || result.Count < 1 ? []
                     : Noise.Add.FastBytes(result, reKey.Noisifier);
            }
        }
    }
}