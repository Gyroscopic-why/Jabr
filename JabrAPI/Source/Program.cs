using AVcontrol;
using System;
using System.Collections.Generic;
using static System.Console;



namespace JabrAPI
{
    internal class Program
    {
        static void Main()
        {
            RE5.EncryptionKey initial = new(true);
            RE5.EncryptionKey copy = new(false);

            Write("\n\tExported:\n");

            List<Byte> iniExp = initial.ExportAsBinary();
            for (var i = 0; i < iniExp.Count; i++) Write(iniExp[i] + " ");
            Write("\n\n");



            copy.ImportFromBinary(initial.ExportAsBinary(), true);

            Write("\n\tImport (COPY) - Exported:\n");

            List<Byte> copExp = initial.ExportAsBinary();
            for (var i = 0; i < copExp.Count; i++) Write(copExp[i] + " ");
            Write("\n\n");
            ReadKey();


            //TestBenchmarker.DecryptBenchmark();
            //Write("\n\n\n\t\t\tDecrypt benchmark finished. Press any key to launch full benchmark suite");
            //ReadKey();

            //TestBenchmarker.Run();

            //RE4.EncryptionKey reKey1 = new RE3.EncryptionKey(2), reKey2 = new RE3.EncryptionKey(2);
            //reKey1.Next();
            //reKey1.GenerateRandomShifts(0);
            //reKey2.Next();
            //reKey2.GenerateRandomShifts(0);

            //Write("\n\treKey1: " + reKey1.ExportAsString() + "\n");
            //Write("\n\treKey2: " + reKey2.ExportAsString() + "\n");

            //string initial = "aboba aboba aboba";
            //Write("\nInitial message:      " + initial);
            
            //string enc1 = RE4.Encrypt(initial, reKey1);
            //Write("\nEncrypted with key 1: " + enc1);

            //string enc2 = RE4.Encrypt(enc1, reKey2);
            //Write("\nEncrypted with 1 & 2: " + enc2);

            //string dec1 = RE4.Decrypt(enc2, reKey1);
            //Write("\nEnc12 decrypted w/ 1: " + dec1);

            //string dec2 = RE4.Decrypt(dec1, reKey2);
            //Write("\nFully decrypted w1,2: " + dec2);

            //string enc21 = RE4.Decrypt(enc2, reKey2);
            //Write("\n\nCorrect decrypted enc2 w2: " + enc21);

            //string dec21 = RE4.Decrypt(enc21, reKey1);
            //Write("\nCorrect decrypted fully: " + dec21);

            //ReadKey();



            //Write("\n\n\t\t\tStarting benchmark...");
            //for (var i = 1; i < 100_000; i++)
            //{
            //    reKey.Next();

            //    string dec3 = RE3.FastDecrypt(RE3.FastEncrypt(initial, reKey), reKey);
            //    if (dec3 != initial)
            //    {
            //        Write("\n\n\t\t\tRE3: Something went wrong at iteration " + i);
            //        Write("\n\tExpected: " + initial);
            //        Write("\n\tGot: " + dec3);
            //        Write("\n\tReKey: " + reKey);
            //        ReadKey();
            //        return;
            //    }

            //    string dec4 = RE4.FastDecrypt(RE4.FastEncrypt(initial, reKey), reKey);
            //    if (dec4 != initial)
            //    {
            //        Write("\n\n\t\t\tRE4: Something went wrong at iteration " + i);
            //        Write("\n\tExpected: " + initial);
            //        Write("\n\tGot: " + dec4);
            //        Write("\n\tReKey: " + reKey);
            //        ReadKey();
            //        return;
            //    }

            //    if (i % 1000 == 0)
            //    {
            //        Write("\n\t\tCompleted iteration " + i + ", no problems so far!");
            //    }
            //}

            //Write("\n\n\t\tBenchmark finished, no problems :) ");
            ReadKey();
        }
    }
}