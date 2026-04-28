using System;
using System.Collections.Generic;


using JabrAPI.Template;
using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class Noise
    {
        static public partial class Remove
        {
            static public List<Byte> Binary(List<Byte> noised, IBinaryKey reKey,
                out Exception? exception)
            {
                if (IsMessageAndReKeyAndNoisifierValid(noised, reKey, out exception) &&
                   reKey.Noisifier.IsValid.ForReKey(reKey, out exception))
                {
                    try
                    {
                        return FastBinary(noised, reKey.Noisifier);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return [];
            }
            static public List<Byte> Binary(List<Byte> noised, IBinaryKey reKey,
                bool throwExceptions = false)
            {
                List<Byte> result = Binary(noised, reKey, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }

            static public List<Byte> Binary(List<Byte> noised, BinaryNoisifier noisifier,
                out Exception? exception)
            {
                if (IsMessageAndNoisifierValid(noised, noisifier, out exception))
                {
                    try
                    {
                        return FastBinary(noised, noisifier);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return [];
            }
            static public List<Byte> Binary(List<Byte> noised, BinaryNoisifier noisifier,
                bool throwExceptions = false)
            {
                List<Byte> result = Binary(noised, noisifier, out Exception? exception);
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
                    //try
                    //{
                        FastBinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey.Noisifier);
                        return true;
                    //}
                    //catch (Exception innerException) { exception = innerException; }
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



            static public List<Byte> FastBinary(List<Byte> noised, BinaryNoisifier noisifier)
            {
                return
                [.. Internal.RemoveFastBinary
                    (
                        noised,
                        noisifier
                    )
                ];
            }
            static public void FastBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, BinaryNoisifier noisifier)
            {
                Internal.RemoveFastBinaryFile
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