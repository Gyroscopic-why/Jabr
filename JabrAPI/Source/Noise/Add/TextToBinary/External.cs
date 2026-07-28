using System;
using System.Collections.Generic;


using AVcontrol;
using JabrAPI.Template;



namespace JabrAPI
{
    static public partial class Noise
    {
        static public partial class Add
        {
            static public List<Byte> TextToBinary_ASCII(string message, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.ASCII, out exception);
            static public List<Byte> TextToBinary_ASCII(string message, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.ASCII, throwExceptions);
            static public List<Byte> FastTextToBinary_ASCII(string message, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinary(message, reKey, ToBinary.ASCII);



            static public List<Byte> TextToBinary_Utf8(string message, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.Utf8, out exception);
            static public List<Byte> TextToBinary_Utf8(string message, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.Utf8, throwExceptions);
            static public List<Byte> FastTextToBinary_Utf8(string message, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinary(message, reKey, ToBinary.Utf8);



            static public List<Byte> TextToBinary_Utf16(string message, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.Utf16, out exception);
            static public List<Byte> TextToBinary_Utf16(string message, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.Utf16, throwExceptions);
            static public List<Byte> FastTextToBinary_Utf16(string message, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinary(message, reKey, ToBinary.Utf16);



            static public List<Byte> TextToBinaryBE_Utf16(string message, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.BigEndianUtf16, out exception);
            static public List<Byte> TextToBinaryBE_Utf16(string message, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.BigEndianUtf16, throwExceptions);
            static public List<Byte> FastTextToBinaryBE_Utf16(string message, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinary(message, reKey, ToBinary.BigEndianUtf16);



            static public List<Byte> TextToBinary_Utf32(string message, IEncryptionKey reKey, out Exception? exception)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.Utf32, out exception);
            static public List<Byte> TextToBinary_Utf32(string message, IEncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.AddNoiseTextToBinaryValidator(message, reKey, ToBinary.Utf32, throwExceptions);
            static public List<Byte> FastTextToBinary_Utf32(string message, IEncryptionKey reKey)
                => InternalLink.AddNoiseFastTextToBinary(message, reKey, ToBinary.Utf32);
        }
    }
}