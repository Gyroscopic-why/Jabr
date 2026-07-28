using System;
using System.Collections.Generic;


using static JabrAPI.Miscellaneous;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class InternalLink
        {
            internal delegate string FromBinaryDelegate(List<Byte> input, out List<Byte> output);


            static internal string DecryptTextFromBinaryValidator(List<Byte> encrypted, EncryptionKey reKey,
                FromBinaryDelegate convertRule, out Exception? exception)
            {
                var msgForValidation = convertRule(encrypted, out List<Byte>? leftover);

                if (IsMessageAndReKeyAndNoisifierValid(msgForValidation, reKey, out exception) &&
                    reKey.IsValid.ForDecryption(msgForValidation, out exception) &&
                    (leftover == null || leftover.Count == 0))
                {
                    try
                    {
                        return DecryptFastTextFromBinary(encrypted, reKey, convertRule);
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static internal string DecryptTextFromBinaryValidator(List<Byte> encrypted, EncryptionKey reKey,
                FromBinaryDelegate convertRule, bool throwExceptions)
            {
                string result = DecryptTextFromBinaryValidator(encrypted, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }


            static internal bool DecryptTextFromBinaryFileValidator(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, FromBinaryDelegate convertRule, out Exception? exception)
            {
                if (IsReKeyValid(reKey, out exception) &&
                    IsNoisifierValid(reKey.Noisifier, out exception))
                {
                    try
                    {
                        DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                            absoluteOutputDirectory, reKey, convertRule);
                        return true;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return false;
            }
            static internal bool DecryptTextFromBinaryFileValidator(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, FromBinaryDelegate convertRule, bool throwExceptions)
            {
                bool result = DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                                absoluteOutputDirectory, reKey, convertRule, out Exception? exception);
                if (exception != null && throwExceptions) throw exception;
                return result;
            }



            static internal string DecryptFastTextFromBinary(List<Byte> encrypted, EncryptionKey reKey,
                FromBinaryDelegate convertRule)
                => Internal.DecryptFastTextFromBinary(encrypted, reKey, convertRule);
            static internal void DecryptFastTextFromBinaryFile(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, FromBinaryDelegate convertRule)
                => Internal.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, convertRule);
        }
    }
}