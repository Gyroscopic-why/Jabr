using System;
using System.Collections.Generic;



namespace JabrAPI
{
    static public partial class RE5
    {
        static public partial class Encrypt
        {
            static public partial class WithNoise
            {
                static public List<Byte> Binary(List<Byte> message, BinaryKey reKey, out Exception? exception)
                {
                    List<Byte> result = Encrypt.Binary(message, reKey, out exception);
                    return result == null || result.Count < 1 ? []
                         : Noise.Add.Binary(result, reKey, out exception);
                }
                static public List<Byte> Binary(List<Byte> message, BinaryKey reKey, bool throwExceptions = false)
                {
                    List<Byte> result = Encrypt.Binary(message, reKey, throwExceptions);
                    return result == null || result.Count < 1 ? []
                         : Noise.Add.Binary(result, reKey, throwExceptions);
                }


                static public List<Byte> FastBinary(List<Byte> message, BinaryKey reKey)
                {
                    List<Byte> result = Encrypt.FastBinary(message, reKey);
                    return result == null || result.Count < 1 ? []
                         : Noise.Add.FastBinary(result, reKey.Noisifier);
                }
            }
        }
    }
}