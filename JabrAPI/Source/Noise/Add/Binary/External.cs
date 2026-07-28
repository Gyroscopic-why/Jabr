using System;
using System.Linq;
using System.Collections.Generic;


using JabrAPI.Template;
using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class Noise
    {
        static public partial class Add
        {
            static public List<Byte> Binary(List<Byte> message, IBinaryKey reKey,
                out Exception? exception)
            {
                if (IsMessageAndReKeyAndNoisifierValid(message, reKey, out exception) &&
                    reKey.Noisifier.IsValid.ForMessageAndReKey(reKey, message, out exception))
                {
                    try
                    {
                        return FastBinary(message, reKey.Noisifier);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return [];
            }
            static public List<Byte> Binary(List<Byte> message, IBinaryKey reKey,
                bool throwExceptions = false)
            {
                List<Byte> result = Binary(message, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }

            static public List<Byte> Binary(List<Byte> message, BinaryNoisifier noisifier,
                out Exception? exception)
            {
                if (IsMessageAndNoisifierValid(message, noisifier, out exception) &&
                        noisifier.IsValid.ForMessage(message, out exception))
                {
                    try
                    {
                        return FastBinary(message, noisifier);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return [];
            }
            static public List<Byte> Binary(List<Byte> message, BinaryNoisifier noisifier,
                bool throwExceptions = false)
            {
                List<Byte> result = Binary(message, noisifier, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }


            static public bool BinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IBinaryKey reKey,
                    out Exception? exception)
            {
                if (IsReKeyValid(reKey, out exception) &&
                    IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        FastBinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey.Noisifier);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static public bool BinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IBinaryKey reKey,
                bool throwExceptions = false)
            {
                bool result = BinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey, out Exception? exception);
                if (!result && throwExceptions) throw exception!;
                return result;
            }
            static public bool BinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, BinaryNoisifier noisifier,
                    out Exception? exception)
            {
                if (IsNoisifierValid(noisifier, out exception))
                {
                    try
                    {
                        FastBinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, noisifier);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static public bool BinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, BinaryNoisifier noisifier,
                bool throwExceptions = false)
            {
                bool result = BinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, noisifier, out Exception? exception);
                if (!result && throwExceptions) throw exception!;
                return result;
            }

            static public bool BinaryFile(string absoluteInputDirectory, string fileName, IBinaryKey reKey, out Exception? exception)
                => BinaryFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, out exception);
            static public bool BinaryFile(string absoluteInputDirectory, string fileName, IBinaryKey reKey, bool throwExceptions = false)
                => BinaryFile(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, throwExceptions);
            static public bool BinaryFile(string absoluteInputDirectory, string fileName, BinaryNoisifier noisifier, out Exception? exception)
                => BinaryFile(absoluteInputDirectory, fileName, absoluteInputDirectory, noisifier, out exception);
            static public bool BinaryFile(string absoluteInputDirectory, string fileName, BinaryNoisifier noisifier, bool throwExceptions = false)
                => BinaryFile(absoluteInputDirectory, fileName, absoluteInputDirectory, noisifier, throwExceptions);



            static public List<Byte> FastBinary(List<Byte> message, BinaryNoisifier noisifier)
            {
                return Internal.AddFastBinary
                (
                    message,
                    noisifier,
                    [.. message.Distinct()]
                );
            }
            static public void FastBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, BinaryNoisifier noisifier)
            {
                Internal.AddFastBinaryFile
                (
                    absoluteInputDirectory,
                    fileName,
                    absoluteOutputDirectory,
                    noisifier
                );
            }
        }
    }
}