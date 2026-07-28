using System;


using AVcontrol;
using JabrAPI.Template;



namespace JabrAPI
{
    static public partial class Noise
    {
        static public partial class Add
        {
            static public bool TextToBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.ASCII, out exception);
            static public bool TextToBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.ASCII, out exception);

            static public bool TextToBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.ASCII, throwExceptions);
            static public bool TextToBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, bool throwExceptions)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.ASCII, throwExceptions);

            static public void FastTextToBinaryFile_ASCII(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.ASCII);
            static public void FastTextToBinaryFile_ASCII(string absoluteInputDirectory, string fileName, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.ASCII);



            static public bool TextToBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.Utf8, out exception);
            static public bool TextToBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.Utf8, out exception);

            static public bool TextToBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.Utf8, throwExceptions);
            static public bool TextToBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, bool throwExceptions)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.Utf8, throwExceptions);

            static public void FastTextToBinaryFile_Utf8(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.Utf8);
            static public void FastTextToBinaryFile_Utf8(string absoluteInputDirectory, string fileName, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.Utf8);



            static public bool TextToBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.Utf16, out exception);
            static public bool TextToBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.Utf16, out exception);

            static public bool TextToBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.Utf16, throwExceptions);
            static public bool TextToBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, bool throwExceptions)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.Utf16, throwExceptions);

            static public void FastTextToBinaryFile_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.Utf16);
            static public void FastTextToBinaryFile_Utf16(string absoluteInputDirectory, string fileName, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.Utf16);



            static public bool TextToBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.BigEndianUtf16, out exception);
            static public bool TextToBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.BigEndianUtf16, out exception);

            static public bool TextToBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.BigEndianUtf16, throwExceptions);
            static public bool TextToBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, bool throwExceptions)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.BigEndianUtf16, throwExceptions);

            static public void FastTextToBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.BigEndianUtf16);
            static public void FastTextToBinaryFileBE_Utf16(string absoluteInputDirectory, string fileName, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.BigEndianUtf16);



            static public bool TextToBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.Utf32, out exception);
            static public bool TextToBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.Utf32, out exception);

            static public bool TextToBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.Utf32, throwExceptions);
            static public bool TextToBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                IEncryptionKey reKey, bool throwExceptions)
                => InternalLink.AddNoiseTextToBinaryFileValidator(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.Utf32, throwExceptions);

            static public void FastTextToBinaryFile_Utf32(string absoluteInputDirectory, string fileName,
                string absoluteOutputDirectory, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteOutputDirectory, reKey, ToBinary.Utf32);
            static public void FastTextToBinaryFile_Utf32(string absoluteInputDirectory, string fileName, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinaryFile(absoluteInputDirectory, fileName,
                    absoluteInputDirectory, reKey, ToBinary.Utf32);
        }
    }
}