using System;
using System.Collections.Generic;


using JabrAPI.Template;
using static JabrAPI.Noise.Miscellaneous;



namespace JabrAPI.Noise
{
    static public partial class Add
    {
        static public string Text(string message, IEncryptionKey reKey,
                out Exception? exception)
        {
            if (IsMessageAndReKeyAndNoisifierValid(message, reKey, out exception))
            {
                try
                {
                    reKey.Noisifier.IsValid.ForAdding(reKey, message, true);

                    return FastText(message, reKey.Noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string Text(string message, IEncryptionKey reKey,
            bool throwExceptions = false)
        {
            string result  = Text(message, reKey, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }



        static public string Text(string message, Noisifier noisifier,
            out Exception? exception)
        {
            if (IsMessageAndNoisifierValid(message, noisifier, out exception))
            {
                try
                {
                    noisifier.IsValid.ForAdding(message, true);

                    return FastText(message, noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string Text(string message, Noisifier noisifier,
            bool throwExceptions = false)
        {
            string result  = Text(message, noisifier, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }
        static public string FastText(string message, Noisifier noisifier)
        {
            return Internal.AddFastText
            (
                message,
                noisifier,
                string.Concat
                (
                    new HashSet<char>
                    (message)
                )
            );
        }
    }
}