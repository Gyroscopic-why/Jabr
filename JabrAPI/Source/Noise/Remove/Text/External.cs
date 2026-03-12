using System;


using JabrAPI.Template;
using static JabrAPI.Noise.Miscellaneous;



namespace JabrAPI.Noise
{
    static public partial class Remove
    {
        static public string Text(string noised, IEncryptionKey reKey,
            out Exception? exception)
        {
            if (IsMessageAndReKeyAndNoisifierValid(noised, reKey, out exception))
            {
                try
                {
                    reKey.Noisifier.IsValid.ForRemoving(reKey, noised, true);

                    return FastText(noised, reKey.Noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string Text(string noised, IEncryptionKey reKey,
            bool throwExceptions = false)
        {
            string result  = Text(noised, reKey, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }
        


        static public string Text(string noised, Noisifier noisifier,
            out Exception? exception)
        {
            if (IsMessageAndNoisifierValid(noised, noisifier, out exception))
            {
                try
                {
                    return FastText(noised, noisifier);
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string Text(string noised, Noisifier noisifier,
            bool throwExceptions = false)
        {
            string result = Text(noised, noisifier, out Exception? exception);
            if (exception != null && throwExceptions) throw exception;
            return result;
        }



        static public string FastText(string noised, Noisifier noisifier)
        {
            return Internal.RemoveFastText
            (
                noised,
                noisifier
            );
        }
    }
}