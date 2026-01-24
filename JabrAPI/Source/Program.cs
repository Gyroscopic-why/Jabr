using AVcontrol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Console;



namespace JabrAPI
{
    internal class Program
    {
        static void Main()
        {
            RE5.EncryptionKey initial = new(true);
            RE5.EncryptionKey copy = new(false);
            Stopwatch timer = new();

            string exportBuffer = "";

            for (var hide = 0; hide < 1; hide++)
            {
                List<Int64> ms1 = [], ms2 = [];
                const Int64 totalAttempts = 10, iterationsPerAttempt = 1_000_000;
                Write($"\n\n\n\t\t[i]  - Starting benchmark of {totalAttempts * iterationsPerAttempt / 1_000_000}m Key Export & Import");

                for (var attempt = 0; attempt < totalAttempts; attempt++)
                {
                    if (attempt % 2 == 0)
                    {
                        initial.Next();

                        Write("\n\t\t\tEXPORT     - ");
                        timer.Start();

                        for (var i = 0; i < iterationsPerAttempt; i++)
                            exportBuffer = initial.ExportAsString();

                        timer.Stop();
                        ms1.Add(timer.ElapsedMilliseconds);
                    }
                    else
                    {
                        Write("\n\t\t\tIMPORT     - ");
                        timer.Start();

                        for (var i = 0; i < iterationsPerAttempt; i++) copy.ImportFromString(exportBuffer);

                        timer.Stop();
                        ms2.Add(timer.ElapsedMilliseconds);
                    }

                    string elapsed = ((double)timer.ElapsedMilliseconds / 1000).ToString().Replace(",", ".");
                    Write($"\tAttempt {attempt + 1})\t Exp & Imp     {iterationsPerAttempt / 1_000}k: {elapsed}");
                    timer.Reset();


                    if (attempt % 2 == 1)
                    {
                        Write("\n\t\t\tVALIDATING - ");

                        if (copy.ExportAsString() == exportBuffer)
                        {
                            ForegroundColor = ConsoleColor.Green;
                            Write("\tSUCCESS! Import matches export");
                            ForegroundColor = ConsoleColor.Gray;
                        }
                        else
                        {
                            ForegroundColor = ConsoleColor.Green;
                            Write("\tFAILURE! See differences:");
                            ForegroundColor = ConsoleColor.Gray;
                            Write("\n\t\t\tInitial: " + exportBuffer);
                            Write("\n\t\t\tImport:  " + copy.ExportAsString());
                        }
                    }
                }

                double min1 = (double)ms1.Min() / 1000, max1 = (double)ms1.Max() / 1000;
                double min2 = (double)ms2.Min() / 1000, max2 = (double)ms2.Max() / 1000;
                double sum1 = (double)ms1.Sum() / 1000, sum2 = (double)ms2.Sum() / 1000;
                double avg1 = ms1.Average() / 1000, avg2 = ms2.Average() / 1000, avgGlobal = (avg1 + avg2) / 2;

                bool isFirst1 = min1 < min2, isFirst2 = max1 > max2;

                string i1 = min1.ToString().Replace(",", "."), a1 = max1.ToString().Replace(",", ".");
                string i2 = min2.ToString().Replace(",", "."), a2 = max2.ToString().Replace(",", ".");
                string v1 = avg1.ToString().Replace(",", "."), v2 = avg2.ToString().Replace(",", "."), v3 = avgGlobal.ToString().Replace(",", ".");
                string s1 = sum1.ToString().Replace(",", "."), s2 = sum2.ToString().Replace(",", "."), s3 = (sum1 + sum2).ToString().Replace(",", ".");

                Write("\n\n\t\t\tBenchmark finished");
                Write($"\n\t\tEXPORT   - {iterationsPerAttempt / 1_000_000}m operations   interval: {i1}-{a1}, average: {v1}");
                Write($"\n\t\tIMPORT   - {iterationsPerAttempt / 1_000_000}m operations   interval: {i2}-{a2}, average: {v2}");
                Write($"\n\t\tGLOBAL   - {iterationsPerAttempt / 1_000_000}m operations   interval: ");

                if (isFirst1) Write($"{i1}-");
                else Write($"{i2}-");
                if (isFirst2) Write($"{a1}, average: {v3}");
                else Write($"{a2}, average: {v3}");

                Write("\n\n\n\t\t\tTotal time elapsed for");
                Write($"\n\t\tEXPORT    - {totalAttempts * iterationsPerAttempt / 1_000_000}m operations: {s1}");
                Write($"\n\t\tIMPORT    - {totalAttempts * iterationsPerAttempt / 1_000_000}m operations: {s2}");
                Write($"\n\t\tGLOBAL    - {totalAttempts * iterationsPerAttempt / 1_000_000}m operations: {s3}");

                ReadKey();
            }






            //Write("\n\tExported:\n");

            //List<Byte> iniExp = initial.ExportAsBinary();
            //for (var i = 0; i < iniExp.Count; i++) Write(iniExp[i] + " ");
            //Write("\n\n");



            //copy.ImportFromBinary(iniExp, true);

            //Write("\n\tImport (COPY) - Exported:\n");

            //List<Byte> copExp = copy.ExportAsBinary();
            //for (var i = 0; i < copExp.Count; i++) Write(copExp[i] + " ");
            //Write("\n\n");





            //Write("\n\tString Exported:\n" + initial.ExportAsString());

            //copy.ImportFromString(initial.ExportAsString(), true);

            //Write("\n\tImport (COPY) - String Exported:\n" + copy.ExportAsString());

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