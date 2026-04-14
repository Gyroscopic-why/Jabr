using System;
using System.Collections.Generic;


using static JabrAPI.Miscellaneous;
using static JabrAPI.RE5;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class InternalLink
        {
            static internal string DecryptTextValidator(List<Byte> encrypted, EncryptionKey reKey,
                Func<List<Byte>, string> convertRule, out Exception? exception)
            {
                var msgForValidation = convertRule(encrypted);

                if (IsMessageAndReKeyAndNoisifierValid(msgForValidation, reKey, out exception) &&
                    reKey.IsValid.ForDecryption(msgForValidation, out exception))
                {
                    try
                    {
                        return DecryptFastTextFromBinary(encrypted, reKey, convertRule);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static internal string DecryptTextValidator(List<Byte> encrypted, EncryptionKey reKey,
                Func<List<Byte>, string> convertRule, bool throwExceptions)
            {
                string result = DecryptTextValidator(encrypted, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static internal string DecryptFastTextFromBinary(List<Byte> encrypted, EncryptionKey reKey, Func<List<Byte>, string> convertRule)
                => Internal.DecryptFastTextFromBinary(encrypted, reKey, convertRule);
            static internal void DecryptFastTextFileFromBinary(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, Func<List<Byte>, string> convertRule)
                => Internal.DecryptFastTextFileFromBinary(absoluteInputDirectory, fileName, absoluteInputDirectory, reKey, convertRule);
        }
    }
}