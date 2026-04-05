using System;
using System.Collections.Generic;


using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class RE5
    {
        static public partial class Decrypt
        {
            static public List<Byte> Bytes(List<Byte> encrypted, BinaryKey reKey, out Exception? exception)
            {
                if (IsMessageAndReKeyValid(encrypted, reKey, out exception) &&
                    reKey.IsValid.ForDecryption(encrypted, out exception))
                {
                    try
                    {
                        return FastBytes(encrypted, reKey);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return [];
            }
            static public List<Byte> Bytes(List<Byte> encrypted, BinaryKey reKey, bool throwExceptions = false)
            {
                List<Byte> result = Bytes(encrypted, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }


            static public bool BytesFile(string inputPath, string outputPath,
                BinaryKey reKey, out Exception? exception)
            {
                if (IsReKeyValid(reKey, out exception) &&
                    IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        FastBytesFile(inputPath, outputPath, reKey);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static public bool BytesFile(string inputPath, string outputPath,
                BinaryKey reKey, bool throwExceptions = false)
            {
                bool result = BytesFile(inputPath, outputPath, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static public List<Byte> FastBytes(List<Byte> message, BinaryKey reKey)
                => Internal.DecryptFastBytes(message, reKey);
            static public void FastBytesFile(string inputPath, string outputPath, BinaryKey reKey)
                => Internal.DecryptFastBytesFile(inputPath, outputPath, reKey);
        }
    }
}