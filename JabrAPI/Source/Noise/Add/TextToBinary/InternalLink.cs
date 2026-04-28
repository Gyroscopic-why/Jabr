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
            static internal List<Byte> AddNoiseTextToBinaryValidator(string message, IEncryptionKey reKey,
                Func<string, Byte[]> convertRule, out Exception? exception)
            {
                if (IsMessageAndReKeyAndNoisifierValid(message, reKey, out exception) &&
                    reKey.IsValid!.ForEncryption(message, out exception))
                {
                    try
                    {
                        return AddNoiseFastTextToBinary(message, reKey, convertRule);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return [];
            }
            static internal List<Byte> AddNoiseTextToBinaryValidator(string message, IEncryptionKey reKey,
                Func<string, Byte[]> convertRule, bool throwExceptions)
            {
                List<Byte> result = AddNoiseTextToBinaryValidator(message, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static internal bool AddNoiseTextToBinaryFileValidator(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, Func<string, Byte[]> convertRule, out Exception? exception)
            {
                if (IsReKeyValid(reKey, out exception) &&
                    IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                            absoluteOutputDirectory, reKey, convertRule);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static internal bool AddNoiseTextToBinaryFileValidator(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, Func<string, Byte[]> convertRule, bool throwExceptions)
            {
                bool result = AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                                absoluteOutputDirectory, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static internal List<Byte> AddNoiseFastTextToBinary(string message, IEncryptionKey reKey, Func<string, Byte[]> convertRule)
                => Internal.AddNoiseFastTextToBinary
                (
                    message,
                    reKey,
                    string.Concat
                    (
                        new HashSet<char>
                        (message)
                    ),
                    convertRule
                );
            static internal void AddNoiseFastTextToBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, Func<string, Byte[]> convertRule)
                => Internal.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey, convertRule);
        }
    }
}