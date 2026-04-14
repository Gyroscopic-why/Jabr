using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    static public partial class RE5
    {
        static public partial class Decrypt
        {
            static public string TextFromBinary_ASCII(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.ASCII, out exception);
            static public string TextFromBinary_ASCII(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.ASCII, throwExceptions);
            static public string FastTextFromBinary_ASCII(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.ASCII);



            static public string TextFromBinary_Utf8(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.Utf8, out exception);
            static public string TextFromBinary_Utf8(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.Utf8, throwExceptions);
            static public string FastTextFromBinary_Utf8(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.Utf8);



            static public string TextFromBinary_Utf16(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.Utf16, out exception);
            static public string TextFromBinary_Utf16(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.Utf16, throwExceptions);
            static public string FastTextFromBinary_Utf16(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.Utf16);



            static public string TextFromBinaryBE_Utf16(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.BigEndianUtf16, out exception);
            static public string TextFromBinaryBE_Utf16(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.BigEndianUtf16, throwExceptions);
            static public string FastTextFromBinaryBE_Utf16(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.BigEndianUtf16);



            static public string TextFromBinary_Utf32(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.Utf32, out exception);
            static public string TextFromBinary_Utf32(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextValidator(message, reKey, FromBinary.Utf32, throwExceptions);
            static public string FastTextFromBinary_Utf32(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.Utf32);
        }
    }
}