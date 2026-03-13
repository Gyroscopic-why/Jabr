using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI.RE5
{
    static public partial class Decrypt
    {
        static public partial class WithNoise
        {
            static public string TextFromBinary_ASCII(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => Text(FromBinary.ASCII(message), reKey, out exception);
            static public string TextFromBinary_ASCII(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => Text(FromBinary.ASCII(message), reKey, throwExceptions);
            static public string FastTextFromBinary_ASCII(List<Byte> message, EncryptionKey reKey)
                => FastText(FromBinary.ASCII(message), reKey);



            static public string TextFromBinary_Utf8(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => Text(FromBinary.Utf8(message), reKey, out exception);
            static public string TextFromBinary_Utf8(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => Text(FromBinary.Utf8(message), reKey, throwExceptions);
            static public string FastTextFromBinary_Utf8(List<Byte> message, EncryptionKey reKey)
                => FastText(FromBinary.Utf8(message), reKey);



            static public string TextFromBinary_Utf16(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => Text(FromBinary.Utf16(message), reKey, out exception);
            static public string TextFromBinary_Utf16(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => Text(FromBinary.Utf16(message), reKey, throwExceptions);
            static public string FastTextFromBinary_Utf16(List<Byte> message, EncryptionKey reKey)
                => FastText(FromBinary.Utf16(message), reKey);



            static public string TextFromBinaryBE_Utf16(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => Text(FromBinary.BigEndianUtf16(message), reKey, out exception);
            static public string TextFromBinaryBE_Utf16(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => Text(FromBinary.BigEndianUtf16(message), reKey, throwExceptions);
            static public string FastTextFromBinaryBE_Utf16(List<Byte> message, EncryptionKey reKey)
                => FastText(FromBinary.BigEndianUtf16(message), reKey);



            static public string TextFromBinary_Utf32(List<Byte> message, EncryptionKey reKey, out Exception? exception)
                => Text(FromBinary.Utf32(message), reKey, out exception);
            static public string TextFromBinary_Utf32(List<Byte> message, EncryptionKey reKey, bool throwExceptions = false)
                => Text(FromBinary.Utf32(message), reKey, throwExceptions);
            static public string FastTextFromBinary_Utf32(List<Byte> message, EncryptionKey reKey)
                => FastText(FromBinary.Utf32(message), reKey);
        }
    }
}