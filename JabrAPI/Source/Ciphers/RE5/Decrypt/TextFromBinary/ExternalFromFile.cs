using System;


using AVcontrol;



namespace JabrAPI
{
    static public partial class RE5
    {
        static public partial class Decrypt
        {
            static public bool TextFromBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_ASCII, out exception);
            static public bool TextFromBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_ASCII, out exception);

            static public bool TextFromBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_ASCII, throwExceptions);
            static public bool TextFromBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, bool throwExceptions)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_ASCII, throwExceptions);

            static public void FastTextFromBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_ASCII);
            static public void FastTextFromBinaryFile_ASCII(string absoluteInputDirectory, string fileName, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_ASCII);



            static public bool TextFromBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_Utf8, out exception);
            static public bool TextFromBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_Utf8, out exception);

            static public bool TextFromBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_Utf8, throwExceptions);
            static public bool TextFromBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, bool throwExceptions)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_Utf8, throwExceptions);

            static public void FastTextFromBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_Utf8);
            static public void FastTextFromBinaryFile_Utf8(string absoluteInputDirectory, string fileName, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_Utf8);



            static public bool TextFromBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_Utf16, out exception);
            static public bool TextFromBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_Utf16, out exception);

            static public bool TextFromBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_Utf16, throwExceptions);
            static public bool TextFromBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, bool throwExceptions)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_Utf16, throwExceptions);

            static public void FastTextFromBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_Utf16);
            static public void FastTextFromBinaryFile_Utf16(string absoluteInputDirectory, string fileName, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_Utf16);



            static public bool TextFromBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_BigEndianUtf16, out exception);
            static public bool TextFromBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_BigEndianUtf16, out exception);

            static public bool TextFromBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_BigEndianUtf16, throwExceptions);
            static public bool TextFromBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, bool throwExceptions)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_BigEndianUtf16, throwExceptions);

            static public void FastTextFromBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_BigEndianUtf16);
            static public void FastTextFromBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_BigEndianUtf16);



            static public bool TextFromBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_Utf32, out exception);
            static public bool TextFromBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_Utf32, out exception);

            static public bool TextFromBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_Utf32, throwExceptions);
            static public bool TextFromBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                EncryptionKey reKey, bool throwExceptions)
                => InternalLink.DecryptTextFromBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_Utf32, throwExceptions);

            static public void FastTextFromBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, FromBinary.Unsanitized_Utf32);
            static public void FastTextFromBinaryFile_Utf32(string absoluteInputDirectory, string fileName, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, FromBinary.Unsanitized_Utf32);
        }
    }
}