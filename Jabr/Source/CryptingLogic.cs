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
        
        static public void GetInfo(Byte type)
        {
            GetMessage  (gSimplified, type);
            GetAlphabet (gSimplified, type);
            GetShifts   (gSimplified, type);
        }
             //  Get parameters for the encryption/decryption process


        //---------------  Encryption / Decryption  management logic  -----------------------------------//
        static public void Encrypt(Byte cipherVersion, bool showAdvInfo, string decrypted, string alphabet, List<Int32> shifts)
        {
            string encRE3 = "", encRE4 = "";
            bool showRE3 = false, showRE4 = false;

            if (cipherVersion == 3 || cipherVersion == 255 && gUseRE3)
            {
                encRE3 = ERE3(decrypted, alphabet, shifts);
                if (showAdvInfo) ERE3Info(encRE3, decrypted, alphabet, shifts);
                showRE3 = true;
            }
            if (cipherVersion == 4 || cipherVersion == 255 && gUseRE4)
            {
                encRE4 = ERE4(decrypted, alphabet, shifts);
                if (showAdvInfo) ERE4Info(encRE4, decrypted, alphabet, shifts);
                showRE4 = true;
            }

            if (showRE3) ShowResult(encRE3, "За", 3);
            if (showRE4) ShowResult(encRE4, "За", 4);
        }
        static public void Decrypt(Byte cipherVersion, bool showAdvInfo, string encrypted, string alphabet, List<Int32> shifts)
        {
            string decRE3 = "", decRE4 = "";
            bool showRE3 = false, showRE4 = false;

            if (cipherVersion == 3 || cipherVersion == 255 && gUseRE3)
            {
                decRE3 = DRE3(encrypted, alphabet, shifts);
                if (showAdvInfo) DRE3Info(decRE3, encrypted, alphabet, shifts);
                showRE3 = true;
            }
            if (cipherVersion == 4 || cipherVersion == 255 && gUseRE4)
            {
                decRE4 = DRE4(encrypted, alphabet, shifts);
                if (showAdvInfo) DRE4Info(decRE4, encrypted, alphabet, shifts);
                showRE4 = true;
            }

            if (showRE3) ShowResult(decRE3, "Де", 3);
            if (showRE4) ShowResult(decRE4, "Де", 4);
        }



        //---------------------------  Process result  --------------------------------------------------//
        static public void ShowResult(string result, string type, Int32 cipherVersion)
        {
            Write("\n\t\t[=]  - " + type + "шифрованное с помощью РЕ" + cipherVersion + " сообщение: ");
            BackgroundColor = ConsoleColor.DarkGreen;
            Write(result);  //  Write the result
            BackgroundColor = ConsoleColor.Black;
            Write("\n");
        }
    }
}
