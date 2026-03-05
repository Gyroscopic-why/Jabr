using System;


using AVcontrol;



namespace JabrAPI.RE5
{
    static public partial class Encrypt
    {
        static public Byte[] TextToBinary_ASCII(string message, EncryptionKey reKey, bool throwException = false)
            => ToBinary.ASCII(Text(message, reKey, throwException));
        static public Byte[] TextToBinary_ASCII(string message, EncryptionKey reKey, out Exception? exception)
            => ToBinary.ASCII(Text(message, reKey, out exception));
        static public Byte[] FastTextToBinary_ASCII(string message, EncryptionKey reKey)
            => ToBinary.ASCII(FastText(message, reKey));



        static public Byte[] TextToBinary_Utf8(string message, EncryptionKey reKey, bool throwException = false)
            => ToBinary.Utf8(Text(message, reKey, throwException));
        static public Byte[] TextToBinary_Utf8(string message, EncryptionKey reKey, out Exception? exception)
            => ToBinary.Utf8(Text(message, reKey, out exception));
        static public Byte[] FastTextToBinary_Utf8(string message, EncryptionKey reKey)
            => ToBinary.Utf8(FastText(message, reKey));


        
        static public Byte[] TextToBinary_Utf16(string message, EncryptionKey reKey, bool throwException = false)
            => ToBinary.Utf16(Text(message, reKey, throwException));
        static public Byte[] TextToBinary_Utf16(string message, EncryptionKey reKey, out Exception? exception)
            => ToBinary.Utf16(Text(message, reKey, out exception));
        static public Byte[] FastTextToBinary_Utf16(string message, EncryptionKey reKey)
            => ToBinary.Utf16(FastText(message, reKey));



        static public Byte[] TextToBinaryBE_Utf16(string message, EncryptionKey reKey, bool throwException = false)
            => ToBinary.BigEndianUtf16(Text(message, reKey, throwException));
        static public Byte[] TextToBinaryBE_Utf16(string message, EncryptionKey reKey, out Exception? exception)
            => ToBinary.BigEndianUtf16(Text(message, reKey, out exception));
        static public Byte[] FastTextToBinaryBE_Utf16(string message, EncryptionKey reKey)
            => ToBinary.BigEndianUtf16(FastText(message, reKey));



        static public Byte[] TextToBinary_Utf32(string message, EncryptionKey reKey, bool throwException = false)
            => ToBinary.Utf32(Text(message, reKey, throwException));
        static public Byte[] TextToBinary_Utf32(string message, EncryptionKey reKey, out Exception? exception)
            => ToBinary.Utf32(Text(message, reKey, out exception));
        static public Byte[] FastTextToBinary_Utf32(string message, EncryptionKey reKey)
            => ToBinary.Utf32(FastText(message, reKey));
    }
}