using System;


using AVcontrol;



namespace JabrAPI.RE5
{
    static public partial class Decrypt
    {
        static public string TextFromBinary_ASCII(Byte[] message, EncryptionKey reKey, bool throwException = false)
            => Text(FromBinary.ASCII(message), reKey, throwException);
        static public string TextFromBinary_ASCII(Byte[] message, EncryptionKey reKey, out Exception? exception)
            => Text(FromBinary.ASCII(message), reKey, out exception);
        static public string FastTextFromBinary_ASCII(Byte[] message, EncryptionKey reKey)
            => FastText(FromBinary.ASCII(message), reKey);



        static public string TextFromBinary_Utf8(Byte[] message, EncryptionKey reKey, bool throwException = false)
            => Text(FromBinary.Utf8(message), reKey, throwException);
        static public string TextFromBinary_Utf8(Byte[] message, EncryptionKey reKey, out Exception? exception)
            => Text(FromBinary.Utf8(message), reKey, out exception);
        static public string FastTextFromBinary_Utf8(Byte[] message, EncryptionKey reKey)
            => FastText(FromBinary.Utf8(message), reKey);



        static public string TextFromBinary_Utf16(Byte[] message, EncryptionKey reKey, bool throwException = false)
            => Text(FromBinary.Utf16(message), reKey, throwException);
        static public string TextFromBinary_Utf16(Byte[] message, EncryptionKey reKey, out Exception? exception)
            => Text(FromBinary.Utf16(message), reKey, out exception);
        static public string FastTextFromBinary_Utf16(Byte[] message, EncryptionKey reKey)
            => FastText(FromBinary.Utf16(message), reKey);



        static public string TextFromBinaryBE_Utf16(Byte[] message, EncryptionKey reKey, bool throwException = false)
            => Text(FromBinary.BigEndianUtf16(message), reKey, throwException);
        static public string TextFromBinaryBE_Utf16(Byte[] message, EncryptionKey reKey, out Exception? exception)
            => Text(FromBinary.BigEndianUtf16(message), reKey, out exception);
        static public string FastTextFromBinaryBE_Utf16(Byte[] message, EncryptionKey reKey)
            => FastText(FromBinary.BigEndianUtf16(message), reKey);



        static public string TextFromBinary_Utf32(Byte[] message, EncryptionKey reKey, bool throwException = false)
            => Text(FromBinary.Utf32(message), reKey, throwException);
        static public string TextFromBinary_Utf32(Byte[] message, EncryptionKey reKey, out Exception? exception)
            => Text(FromBinary.Utf32(message), reKey, out exception);
        static public string FastTextFromBinary_Utf32(Byte[] message, EncryptionKey reKey)
            => FastText(FromBinary.Utf32(message), reKey);
    }
}