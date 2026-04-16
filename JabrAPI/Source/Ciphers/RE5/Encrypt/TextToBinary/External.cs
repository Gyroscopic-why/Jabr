using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    static public partial class RE5
    {
        static public partial class Encrypt
        {
            static public List<Byte> TextToBinary_ASCII(string message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.ASCII, out exception);
            static public List<Byte> TextToBinary_ASCII(string message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.ASCII, throwExceptions);
            static public List<Byte> FastTextToBinary_ASCII(string message, EncryptionKey reKey)
                => InternalLink.EncryptFastTextToBinary(message, reKey, ToBinary.ASCII);



            static public List<Byte> TextToBinary_Utf8(string message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.Utf8, out exception);
            static public List<Byte> TextToBinary_Utf8(string message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.Utf8, throwExceptions);
            static public List<Byte> FastTextToBinary_Utf8(string message, EncryptionKey reKey)
                => InternalLink.EncryptFastTextToBinary(message, reKey, ToBinary.Utf8);



            static public List<Byte> TextToBinary_Utf16(string message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.Utf16, out exception);
            static public List<Byte> TextToBinary_Utf16(string message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.Utf16, throwExceptions);
            static public List<Byte> FastTextToBinary_Utf16(string message, EncryptionKey reKey)
                => InternalLink.EncryptFastTextToBinary(message, reKey, ToBinary.Utf16);



            static public List<Byte> TextToBinaryBE_Utf16(string message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.BigEndianUtf16, out exception);
            static public List<Byte> TextToBinaryBE_Utf16(string message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.BigEndianUtf16, throwExceptions);
            static public List<Byte> FastTextToBinaryBE_Utf16(string message, EncryptionKey reKey)
                => InternalLink.EncryptFastTextToBinary(message, reKey, ToBinary.BigEndianUtf16);



            static public List<Byte> TextToBinary_Utf32(string message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.Utf32, out exception);
            static public List<Byte> TextToBinary_Utf32(string message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.EncryptTextToBinaryValidator(message, reKey, ToBinary.Utf32, throwExceptions);
            static public List<Byte> FastTextToBinary_Utf32(string message, EncryptionKey reKey)
                => InternalLink.EncryptFastTextToBinary(message, reKey, ToBinary.Utf32);
        }
    }
}