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
        }
    }
}