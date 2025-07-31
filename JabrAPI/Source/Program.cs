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

            RE5.EncryptionKey reKey = new RE5.EncryptionKey("abcqwertyuiop1234567", "aboba123", 3);
            string msg = "aboba";
            Write("\n\tMessage:   (" + msg.Length     + ") " + msg);
            Write("\n\tPrimary:   (" + reKey.PrimaryAlphabet.Length  + ") " + reKey.PrimaryAlphabet);
            Write("\n\tExternal:  (" + reKey.ExternalAlphabet.Length + ") " + reKey.ExternalAlphabet);
            //Write("\n\tShifts count: " + reKey.Shifts.Length);

            /*Write("\n\tShifts:   ");
            for (Int32 curShift = 0; curShift < reKey.Shifts.Length; curShift++) 
                Write(reKey.Shifts[curShift] + " ");*/

            //string enc  = RE5.Encrypt(msg, reKey);

            //string dec  = RE5.Decrypt(enc, reKey);

            ReadKey();

        }
    }
}
