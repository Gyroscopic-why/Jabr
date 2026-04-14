using System;
using System.Collections.Generic;


using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class InternalLink
        {
            static internal List<Byte> EncryptTextValidator(string message, EncryptionKey reKey,
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
            static internal List<Byte> EncryptTextValidator(string message, EncryptionKey reKey,
                Func<string, Byte[]> convertRule, bool throwExceptions)
            {
                List<Byte> result = EncryptTextValidator(message, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static internal List<Byte> EncryptFastTextToBinary(string message, EncryptionKey reKey, Func<string, Byte[]> convertRule)
                => Internal.EncryptFastTextToBinary(message, reKey, convertRule);
            static internal void EncryptFastTextFileToBinary(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, Func<string, Byte[]> convertRule)
                => Internal.EncryptFastTextFileToBinary(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, convertRule);
        }
    }
}