using System;
using System.Collections.Generic;


using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class RE5
    {
        static public partial class Encrypt
        {
            static public List<Byte> Binary(List<Byte> message, BinaryKey reKey, out Exception? exception)
            {
                if (IsMessageAndReKeyValid(message, reKey, out exception) &&
                    reKey.IsValid.ForEncryption(message, out exception))
                {
                    try
                    {
                        return FastBinary(message, reKey);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return [];
            }
            static public List<Byte> Binary(List<Byte> message, BinaryKey reKey, bool throwExceptions = false)
            {
                List<Byte> result = Binary(message, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }


            static public bool BinaryFile(string inputPath, string outputPath,
                BinaryKey reKey, out Exception? exception)
            {
                if (IsReKeyValid(reKey, out exception) &&
                    IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        FastBinaryFile(inputPath, outputPath, reKey);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static public bool BinaryFile(string inputPath, string outputPath,
                BinaryKey reKey, bool throwExceptions = false)
            {
                bool result = BinaryFile(inputPath, outputPath, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static public List<Byte> FastBinary(List<Byte> message, BinaryKey reKey)
                => Internal.EncryptFastBinary(message, reKey);
            static public void FastBinaryFile(string inputPath, string outputPath, BinaryKey reKey)
                => Internal.EncryptFastBinaryFile(inputPath, outputPath, reKey);
        }
    }
}