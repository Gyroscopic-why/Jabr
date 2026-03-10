using System;
using System.Linq;
using System.Collections.Generic;


using JabrAPI.Template;
using static JabrAPI.Noise.Miscellaneous;



namespace JabrAPI.Noise
{
    static public partial class Add
    {
        static public List<Byte> Bytes(List<Byte> message, IBinaryKey reKey,
            out Exception? exception)
        {
            if (IsMessageAndReKeyAndNoisifierValid(message, reKey, out exception))
            {
                try
                {
                    reKey.Noisifier.IsComplexNoiseValidForKey(reKey, true);
                    reKey.Noisifier.IsPrimaryNoiseValidForKey(reKey, true);

                    reKey.Noisifier.IsComplexNoiseValidForMessage(message, true);
                    reKey.Noisifier.IsPrimaryNoiseValidForMessage(message, true);

                    return FastBytes(message, reKey.Noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return [];
        }
        static public List<Byte> Bytes(List<Byte> message, IBinaryKey reKey,
            bool throwException = false)
        {
            List<Byte> result = Bytes(message, reKey, out Exception? exception);
            if (exception != null && throwException) throw exception;
            return result;
        }



        static public List<Byte> Bytes(List<Byte> message, BinaryNoisifier noisifier,
            out Exception? exception)
        {
            if (IsMessageAndNoisifierValid(message, noisifier, out exception))
            {
                try
                {
                    noisifier.IsComplexNoiseValidForMessage(message, true);
                    noisifier.IsPrimaryNoiseValidForMessage(message, true);

                    return FastBytes(message, noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return [];
        }
        static public List<Byte> Bytes(List<Byte> message, BinaryNoisifier noisifier,
            bool throwException = false)
        {
            List<Byte> result = Bytes(message, noisifier, out Exception? exception);
            if (exception != null && throwException) throw exception;
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