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
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_ASCII, out exception);
            static public string TextFromBinary_ASCII(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_ASCII, throwExceptions);
            static public string FastTextFromBinary_ASCII(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.Unsanitized_ASCII);



            static public string TextFromBinary_Utf8(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_Utf8, out exception);
            static public string TextFromBinary_Utf8(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_Utf8, throwExceptions);
            static public string FastTextFromBinary_Utf8(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.Unsanitized_Utf8);



            static public string TextFromBinary_Utf16(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_Utf16, out exception);
            static public string TextFromBinary_Utf16(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_Utf16, throwExceptions);
            static public string FastTextFromBinary_Utf16(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.Unsanitized_Utf16);



            static public string TextFromBinaryBE_Utf16(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_BigEndianUtf16, out exception);
            static public string TextFromBinaryBE_Utf16(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_BigEndianUtf16, throwExceptions);
            static public string FastTextFromBinaryBE_Utf16(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.Unsanitized_BigEndianUtf16);



            static public string TextFromBinary_Utf32(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_Utf32, out exception);
            static public string TextFromBinary_Utf32(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => InternalLink.DecryptTextFromBinaryValidator(message, reKey, FromBinary.Unsanitized_Utf32, throwExceptions);
            static public string FastTextFromBinary_Utf32(List<Byte> message, EncryptionKey reKey)
                => InternalLink.DecryptFastTextFromBinary(message, reKey, FromBinary.Unsanitized_Utf32);
        }
    }
}