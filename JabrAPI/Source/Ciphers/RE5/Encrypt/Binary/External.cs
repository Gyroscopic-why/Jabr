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


            static public bool BinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, BinaryKey reKey, out Exception? exception)
            {
                if (IsReKeyValid(reKey, out exception) &&
                    IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        FastBinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static public bool BinaryFile(string absoluteInputDirectory, string fileName,
                BinaryKey reKey, out Exception? exception)
                => BinaryFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, out exception);
            static public bool BinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, BinaryKey reKey, bool throwExceptions = false)
            {
                bool result = BinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }
            static public bool BinaryFile(string absoluteInputDirectory, string fileName,
                BinaryKey reKey, bool throwExceptions = false)
                => BinaryFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, throwExceptions);



            static public List<Byte> FastBinary(List<Byte> message, BinaryKey reKey)
                => Internal.EncryptFastBinary(message, reKey);
            static public void FastBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, BinaryKey reKey)
                => Internal.EncryptFastBinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey);
        }
    }
}