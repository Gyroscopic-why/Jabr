using System;
using static System.Console;

using JabrAPI.Source;



namespace JabrAPI
{
    internal class Program
    {
        static void Main()
        {
            //string aboba = "aboba";
            //RE5.EncryptionKey reKey = new RE5.EncryptionKey();

            RE5.EncryptionKey reKey = new RE5.EncryptionKey("abcqwertyuiop1234567", "abolk123", 3);
            string msg = "aboba";
            Write("\n\tMessage:   (" + msg.Length     + ") " + msg);
            Write("\n\tPrimary:   (" + reKey.PrLength + ") " + reKey.PrimaryAlphabet);
            Write("\n\tExternal:  (" + reKey.ExLength + ") " + reKey.ExternalAlphabet);
            //Write("\n\tShifts count: " + reKey.Shifts.Length);

            /*Write("\n\tShifts:   ");
            for (Int32 curShift = 0; curShift < reKey.Shifts.Length; curShift++) 
                Write(reKey.Shifts[curShift] + " ");*/

            string enc  = RE5.Encrypt(msg, reKey, true);
            string test = RE5.EncryptWithConsoleInfo(msg, reKey);
            
            Demo.AlgorithmTest(enc, test);

            string dec  = RE5.Decrypt(test, reKey, true);
            string tset = RE5.DecryptWithConsoleInfo(test, reKey);
            
            Demo.AlgorithmTest(dec, tset);
            ReadKey();

        }
    }
}
