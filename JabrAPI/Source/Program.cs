using System;

using static System.Console;


using JabrAPI.Source;
using AVcontrol;
using System.Collections.Generic;



namespace JabrAPI
{
    internal class Program
    {
        static void Main()
        {
            //TestBenchmarker.DecryptBenchmark();
            //Write("\n\n\n\t\t\tDecrypt benchmark finished. Press any key to launch full benchmark suite");
            //ReadKey();

            //TestBenchmarker.Run();


            RE4.EncryptionKey reKey = new("abcdefghijklmno", 1);
            //reKey.Next();

            Write("\n\tGenerated RE4 Key: " + reKey.Alphabet);

            string aboba = "aboba";
            string enc = RE4.Encrypt(aboba, reKey);

            Write("\n\tEncrypted: " + enc);

            string dec = RE4.Decrypt(enc, reKey);
            Write("\n\tDecrypted: " + dec);

            ReadKey();
        }
    }
}