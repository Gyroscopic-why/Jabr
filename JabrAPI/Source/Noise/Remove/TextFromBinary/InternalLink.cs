using System;
using System.Collections.Generic;


using JabrAPI.Template;
using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class Noise
    {
        static internal partial class InternalLink
        {
            internal delegate string FromBinaryDelegate(List<Byte> input, out List<Byte> output);


            static internal string RemoveNoiseTextFromBinaryValidator(List<Byte> encrypted, IEncryptionKey reKey,
                FromBinaryDelegate convertRule, out Exception? exception)
            {
                var msgForValidation = convertRule(encrypted, out List<Byte>? leftover);

                if (IsMessageAndNoisifierValid(msgForValidation, reKey.Noisifier, out exception) &&
                    (leftover == null || leftover.Count == 0))
                {
                    try
                    {
                        return RemoveNoiseFastTextFromBinary(encrypted, reKey, convertRule);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static internal string RemoveNoiseTextFromBinaryValidator(List<Byte> encrypted, IEncryptionKey reKey,
                FromBinaryDelegate convertRule, bool throwExceptions)
            {
                string result = RemoveNoiseTextFromBinaryValidator(encrypted, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }


            static internal bool RemoveNoiseTextFromBinaryFileValidator(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, FromBinaryDelegate convertRule, out Exception? exception)
            {
                if (IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        RemoveNoiseFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                            absoluteOutputDirectory, reKey, convertRule);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static internal bool RemoveNoiseTextFromBinaryFileValidator(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, FromBinaryDelegate convertRule, bool throwExceptions)
            {
                bool result = RemoveNoiseTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                                absoluteOutputDirectory, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static internal string RemoveNoiseFastTextFromBinary(List<Byte> encrypted, IEncryptionKey reKey,
                FromBinaryDelegate convertRule)
                => Internal.RemoveNoiseFastTextFromBinary(encrypted, reKey.Noisifier, convertRule);
            static internal void RemoveNoiseFastTextFromBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, FromBinaryDelegate convertRule)
                => Internal.RemoveNoiseFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey.Noisifier, convertRule);
        }
    }
}