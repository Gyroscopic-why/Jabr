using System;
using System.Collections.Generic;

using static System.Console;


using static Jabr.CipherSource;
using static Jabr.GlobalSettings;
using static Jabr.ParametersLogicIO;


namespace Jabr
{
    internal class CryptingLogic
    {
        
        static public void GetInfo(byte Type)
        {
            switch (gVersion)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    GetMessage  (gSimplified, Type, gVersion);
                    GetAlphabet (gSimplified, Type);
                    GetShift    (gSimplified);
                    break;

                case 5:         // REserved for later, get the joke? REserved? XD
                    break;

                default:
                    break;
            }
        }
             //  Get parameters for the encryption/decryption process


        //---------------  Encryption / Decryption  management logic  -----------------------------------//
        static public void Encrypt(byte cipherVersion, bool showAdvInfo, string decrypted, string encrypted, string alphabet, int shift)
        {
            var encrypt = new Dictionary<byte, Action>()  {
            { 1, () => encrypted = ERE1(decrypted, alphabet, shift) },
            { 2, () => encrypted = ERE2(decrypted, alphabet, shift) },
            { 3, () => encrypted = ERE3(decrypted, alphabet, shift) },
            { 4, () => encrypted = ERE4(decrypted, alphabet, shift) }   };

            var enInfo = new Dictionary<byte, Action>()  {
            { 1, () => ERE1Info(encrypted, decrypted, alphabet, shift) },
            { 2, () => ERE2Info(encrypted, decrypted, alphabet, shift) },
            { 3, () => ERE3Info(encrypted, decrypted, alphabet, shift) },
            { 4, () => ERE4Info(encrypted, decrypted, alphabet, shift) }   };

            encrypt[cipherVersion]();
            if (showAdvInfo) enInfo[cipherVersion]();
            ShowResult(encrypted, "За", cipherVersion); // Clean version through var
        }
        static public void Decrypt(byte cipherVersion, bool showAdvInfo, string decrypted, string encrypted, string alphabet, int shift)
        {
            var decrypt = new Dictionary<byte, Action>()  {
            { 1, () => decrypted = DRE1(encrypted, alphabet, shift) },
            { 2, () => decrypted = DRE2(encrypted, alphabet, shift) },
            { 3, () => decrypted = DRE3(encrypted, alphabet, shift) },
            { 4, () => decrypted = DRE4(encrypted, alphabet, shift) }   };

            var deInfo = new Dictionary<byte, Action>()  {
            { 1, () => DRE1Info(encrypted, decrypted, alphabet, shift) },
            { 2, () => DRE2Info(encrypted, decrypted, alphabet, shift) },
            { 3, () => DRE3Info(encrypted, decrypted, alphabet, shift) },
            { 4, () => DRE4Info(encrypted, decrypted, alphabet, shift) }   };

            decrypt[cipherVersion]();
            if (showAdvInfo) deInfo[cipherVersion]();
            ShowResult(decrypted, "Де", cipherVersion); // Clean version through var
        }



        //---------------------------  Process result  --------------------------------------------------//
        static public void ShowResult(string result, string type, int cipherVersion)
        {
            Write("\n\t\t[=]  - " + type + "кодированное с помощью РЕ" + cipherVersion + " сообщение: ");
            BackgroundColor = ConsoleColor.DarkGreen;
            Write(result); //Write the result
            BackgroundColor = ConsoleColor.Black;
            Write("\n");
        }
    }
}
