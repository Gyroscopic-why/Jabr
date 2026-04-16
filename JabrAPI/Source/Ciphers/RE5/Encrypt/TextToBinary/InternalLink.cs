using System;
using System.Collections.Generic;


using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class InternalLink
        {
            static internal List<Byte> EncryptTextToBinaryValidator(string message, EncryptionKey reKey,
                Func<string, Byte[]> convertRule, out Exception? exception)
            {
                if (IsMessageAndReKeyAndNoisifierValid(message, reKey, out exception) &&
                    reKey.IsValid.ForEncryption(message, out exception))
                {
                    try
                    {
                        return EncryptFastTextToBinary(message, reKey, convertRule);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return [];
            }
            static internal List<Byte> EncryptTextToBinaryValidator(string message, EncryptionKey reKey,
                Func<string, Byte[]> convertRule, bool throwExceptions)
            {
                List<Byte> result = EncryptTextToBinaryValidator(message, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static internal bool EncryptTextToBinaryFileValidator(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, Func<string, Byte[]> convertRule, out Exception? exception)
            {
                if (IsReKeyValid(reKey, out exception) &&
                    IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        EncryptFastTextToBinaryFile(absoluteInputDirectory, fileName,
                            absoluteOutputDirectory, reKey, convertRule);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static internal bool EncryptTextToBinaryFileValidator(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, Func<string, Byte[]> convertRule, bool throwExceptions)
            {
                bool result = EncryptTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                                absoluteOutputDirectory, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static internal List<Byte> EncryptFastTextToBinary(string message, EncryptionKey reKey, Func<string, Byte[]> convertRule)
                => Internal.EncryptFastTextToBinary(message, reKey, convertRule);
            static internal void EncryptFastTextToBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, Func<string, Byte[]> convertRule)
                => Internal.EncryptFastTextToBinaryFile(absoluteInputDirectory, fileName, absoluteOutputDirectory, reKey, convertRule);
        }
    }
}