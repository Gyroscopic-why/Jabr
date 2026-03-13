using System;
using System.Linq;
using System.Collections.Generic;


using JabrAPI.Template;
using static JabrAPI.Miscellaneous;



namespace JabrAPI.Noise
{
    static public partial class Add
    {
        static public List<Byte> Bytes(List<Byte> message, IBinaryKey reKey,
            out Exception? exception)
        {
            if (IsMessageAndReKeyAndNoisifierValid(message, reKey, out exception) &&
                reKey.Noisifier.IsValid.ForMessageAndReKey(reKey, message, out exception))
            {
                try
                {
                    return FastBytes(message, reKey.Noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return [];
        }
        static public List<Byte> Bytes(List<Byte> message, IBinaryKey reKey,
            bool throwExceptions = false)
        {
            List<Byte> result = Bytes(message, reKey, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }



        static public List<Byte> Bytes(List<Byte> message, BinaryNoisifier noisifier,
            out Exception? exception)
        {
            if (IsMessageAndNoisifierValid(message, noisifier, out exception) &&
                    noisifier.IsValid.ForMessage(message, out exception))
            {
                try
                {
                    return FastBytes(message, noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return [];
        }
        static public List<Byte> Bytes(List<Byte> message, BinaryNoisifier noisifier,
            bool throwExceptions = false)
        {
            List<Byte> result = Bytes(message, noisifier, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }
        static public List<Byte> FastBytes(List<Byte> message, BinaryNoisifier noisifier)
        {
            return Internal.AddFastBytes
            (
                message,
                noisifier,
                [.. message.Distinct()]
            );
        }
    }
}