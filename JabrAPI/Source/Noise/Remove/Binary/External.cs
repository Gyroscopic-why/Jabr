using System;
using System.Collections.Generic;


using JabrAPI.Template;
using static JabrAPI.Noise.Miscellaneous;



namespace JabrAPI.Noise
{
    static public partial class Remove
    {
        static public List<Byte> Bytes(List<Byte> noised, IBinaryKey reKey,
            out Exception? exception)
        {
            if(IsMessageAndReKeyAndNoisifierValid(noised, reKey, out exception))
            {
                try
                {
                    reKey.Noisifier.IsValid.ForRemoving(reKey, noised, true);

                    return FastBytes(noised, reKey.Noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return [];
        }
        static public List<Byte> Bytes(List<Byte> noised, IBinaryKey reKey,
            bool throwExceptions = false)
        {
            List<Byte> result = Bytes(noised, reKey, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }


        static public List<Byte> Bytes(List<Byte> noised, BinaryNoisifier noisifier,
            out Exception? exception)
        {
            if (IsMessageAndNoisifierValid(noised, noisifier, out exception))
            {
                try
                {
                    return FastBytes(noised, noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return [];
        }
        static public List<Byte> Bytes(List<Byte> noised, BinaryNoisifier noisifier,
            bool throwExceptions = false)
        {
            List<Byte> result = Bytes(noised, noisifier, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }



        static public List<Byte> FastBytes(List<Byte> noised, BinaryNoisifier noisifier)
        {
            return
            [.. Internal.RemoveFastBytes
                (
                    noised,
                    noisifier
                )
            ];
        }
    }
}