using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

using static System.Console;


using JabrAPI.Source;
using AVcontrol;
using System.Linq;



namespace JabrAPI
{
    public class TestBenchmarker
    {
        static public void DecryptBenchmark()
        {
            Write("\n\n\n\t\t\tDecrypt benchmark running!");
            RE5.EncryptionKey reKey = new RE5.EncryptionKey(256);
            reKey.Next();
            Write($"\n\t\t\tEncryption key: ex={reKey.ExAlphabet}, sh={reKey.ShAmount}, pr={reKey.PrAlphabet}\n");


            for (var hide = 0; hide < 1; hide++)
            {
                List<Int64> ms1 = new List<Int64>(), ms2 = new List<Int64>();
                Stopwatch timer = new Stopwatch();
                string enc = RE5.FastEncrypt("Aboba", reKey);
                const Int64 totalAttempts = 100, iterationsPerAttempt = 1_000_000;
                Write($"\n\n\n\t\t[i]  - Starting benchmark of {totalAttempts * iterationsPerAttempt / 1_000_000}m message decrypt");

                for (var warmup = 0;  warmup  < iterationsPerAttempt; warmup++) _ = RE5.FastDecrypt(enc, reKey);
                for (var attempt = 0; attempt < totalAttempts; attempt++)
                {
                    if (attempt % 2 == 0)
                    {
                        Write("\n\t\t\tFAST   - ");
                        timer.Start();
                        for (var i = 0; i < iterationsPerAttempt; i++) _ = RE5.FastDecrypt(enc, reKey);

                        timer.Stop();
                        ms1.Add(timer.ElapsedMilliseconds);
                    }
                    else
                    {
                        Write("\n\t\t\tINFO   - ");
                        timer.Start();
                        for (var i = 0; i < iterationsPerAttempt; i++) _ = RE5.DecryptWithConsoleInfo(enc, reKey, false);

                        timer.Stop();
                        ms2.Add(timer.ElapsedMilliseconds);
                    }

                    string elapsed = ((double)timer.ElapsedMilliseconds / 1000).ToString().Replace(",", ".");
                    Write($"\tAttempt {attempt + 1})\t       operations: {iterationsPerAttempt / 1_000_000}m: {elapsed}");
                    timer.Reset();

                    if (attempt % 10 == 9)
                    {
                        Write("\n\t\t\tRefreshing key...");
                        reKey.Next();
                        enc = RE5.FastEncrypt("aboba", reKey);
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
                Write($"\n\t\tFAST       - {iterationsPerAttempt / 1_000_000}m operations   interval: {i1}-{a1}, average: {v1}");
                Write($"\n\t\tINFO       - {iterationsPerAttempt / 1_000_000}m operations   interval: {i2}-{a2}, average: {v2}");
                Write($"\n\t\tGLOBAL     - {iterationsPerAttempt / 1_000_000}m operations   interval: ");

                if (isFirst1) Write($"{i1}-");
                else Write($"{i2}-");
                if (isFirst2) Write($"{a1}, average: {v3}");
                else Write($"{a2}, average: {v3}");

                Write("\n\n\n\t\t\tTotal time elapsed for");
                Write($"\n\t\tFAST       - {totalAttempts * iterationsPerAttempt / 1_000_000}m operations: {s1}");
                Write($"\n\t\tINFO       - {totalAttempts * iterationsPerAttempt / 1_000_000}m operations: {s2}");
                Write($"\n\t\tGLOBAL     - {totalAttempts * iterationsPerAttempt / 1_000_000}m operations: {s3}");
                ReadKey();
            }
        }



        static public void Run()
        {
            new Thread(new ThreadStart(CheckKeys))
            {
                IsBackground = true
            }.Start();
            Write("\n\n\t\t\t\tRunning!");
            ulong num = 0UL;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (_isRunning)
            {
                RE5.EncryptionKey encryptionKey = new RE5.EncryptionKey();
                encryptionKey.Next();
                string text = "aboba";
                string text2 = RE5.Encrypt(text, encryptionKey, true);
                List<short> list = RE5.EncryptToBinaryUtf16(text, encryptionKey);
                if (!Demo.AlgorithmTest(text2, text2, Encoding.Unicode.GetString(ToBinary.LittleEndian<short>(list.ToArray())), false))
                {
                    stopwatch.Stop();
                    for (; ; )
                    {
                        Clear();
                        int prLength = encryptionKey.PrLength;
                        int length = text.Length;
                        string prAlphabet = encryptionKey.PrAlphabet;
                        Write("\n\tMessage:   (" + length.ToString() + ") " + text);
                        Write("\n\tPrimary:   (" + prLength.ToString() + ") ");
                        for (int i = 0; i < prLength; i++)
                        {
                            for (int j = 0; j < length; j++)
                            {
                                if (prAlphabet[i] == text[j])
                                {
                                    ForegroundColor = ConsoleColor.Green;
                                    j += length;
                                }
                            }
                            Write(prAlphabet[i]);
                            ForegroundColor = ConsoleColor.Gray;
                        }
                        Write("\n\tExternal:  (" + encryptionKey.ExLength.ToString() + ") " + encryptionKey.ExternalAlphabet);
                        RE5.EncryptWithConsoleInfo(text, encryptionKey);
                        try
                        {
                            text2 = RE5.Encrypt(text, encryptionKey, true);
                        }
                        catch
                        {
                            text2 = "";
                            ForegroundColor = ConsoleColor.Red;
                            Write("\n\n\tFast Encrypt: Failed");
                            ForegroundColor = ConsoleColor.Gray;
                        }
                        try
                        {
                            list = RE5.EncryptToBinaryUtf16(text, encryptionKey);
                        }
                        catch
                        {
                            list = new List<short>();
                            ForegroundColor = ConsoleColor.Red;
                            Write("\n\tFrom Binary Decrypt: Failed");
                            ForegroundColor = ConsoleColor.Gray;
                        }
                        Demo.AlgorithmTest(text2, text2, Encoding.Unicode.GetString(ToBinary.LittleEndian<short>(list.ToArray())), true);
                        Write("\n\n\n\n\tDONE INFO! Try refreshing ");
                        Write("\n\tIteratation: " + num.ToString());
                        Write("\n\tElapsed time: " + stopwatch.ElapsedMilliseconds.ToString());
                        ReadLine();
                    }
                }
                else
                {
                    string text3;
                    try
                    {
                        text3 = RE5.FastDecrypt(text2, encryptionKey);
                    }
                    catch
                    {
                        text3 = "";
                        ForegroundColor = ConsoleColor.Red;
                        Write("\n\n\tFast Decrypt: Failed");
                        ForegroundColor = ConsoleColor.Gray;
                    }
                    string fromBin;
                    try
                    {
                        fromBin = RE5.DecryptFromBinaryUtf16(list, encryptionKey);
                    }
                    catch
                    {
                        fromBin = "";
                        ForegroundColor = ConsoleColor.Red;
                        Write("\n\tFrom Binary Decrypt: Failed");
                        ForegroundColor = ConsoleColor.Gray;
                    }
                    if (!Demo.AlgorithmTest(text3, text3, fromBin, false))
                    {
                        stopwatch.Stop();
                        for (; ; )
                        {
                            Clear();
                            int prLength2 = encryptionKey.PrLength;
                            int length2 = text.Length;
                            string prAlphabet2 = encryptionKey.PrAlphabet;
                            Write("\n\tMessage:   (" + length2.ToString() + ") " + text);
                            Write("\n\tPrimary:   (" + prLength2.ToString() + ") ");
                            for (int k = 0; k < prLength2; k++)
                            {
                                for (int l = 0; l < length2; l++)
                                {
                                    if (prAlphabet2[k] == text[l])
                                    {
                                        ForegroundColor = ConsoleColor.Green;
                                        l += length2;
                                    }
                                }
                                Write(prAlphabet2[k]);
                                ForegroundColor = ConsoleColor.Gray;
                            }
                            Write("\n\tExternal:  (" + encryptionKey.ExLength.ToString() + ") " + encryptionKey.ExternalAlphabet);
                            string encMessage = RE5.EncryptWithConsoleInfo(text, encryptionKey);
                            try
                            {
                                text3 = RE5.FastDecrypt(text2, encryptionKey);
                            }
                            catch
                            {
                                text3 = "";
                                ForegroundColor = ConsoleColor.Red;
                                Write("\n\n\tFast Decrypt: Failed");
                                ForegroundColor = ConsoleColor.Gray;
                            }
                            string text4;
                            try
                            {
                                text4 = RE5.DecryptWithConsoleInfo(encMessage, encryptionKey);
                            }
                            catch
                            {
                                text4 = "";
                                ForegroundColor = ConsoleColor.Red;
                                Write("\n\tDebug Decrypt: Failed");
                                ForegroundColor = ConsoleColor.Gray;
                            }
                            if (text4 == null)
                            {
                                text4 = "";
                                ForegroundColor = ConsoleColor.Red;
                                Write("\n\tDebug Decrypt: Failed");
                                ForegroundColor = ConsoleColor.Gray;
                            }
                            try
                            {
                                fromBin = RE5.DecryptFromBinaryUtf16(list, encryptionKey);
                            }
                            catch
                            {
                                fromBin = "";
                                ForegroundColor = ConsoleColor.Red;
                                Write("\n\tFrom Binary Decrypt: Failed");
                                ForegroundColor = ConsoleColor.Gray;
                            }
                            Demo.AlgorithmTest(text3, text4, fromBin, true);
                            Write("\n\n\n\n\tDONE INFO! Try refreshing ");
                            Write("\n\tIteratation: " + num.ToString());
                            Write("\n\tElapsed time: " + stopwatch.ElapsedMilliseconds.ToString());
                            ReadLine();
                        }
                    }
                    else
                    {
                        num += 1UL;
                        if (num % 8192UL == 0UL)
                        {
                            long num2 = stopwatch.ElapsedMilliseconds;
                            long num3 = num2 / 1000L;
                            long num4 = num3 / 60L;
                            long num5 = num4 / 60L;
                            num2 %= 1000L;
                            num3 %= 60L;
                            num4 %= 60L;
                            Write(string.Format("\n\t\tIteration {0}) Time: {1}h {2}m {3}s {4}ms", new object[]
                            {
                                num,
                                num5,
                                num4,
                                num3,
                                num2
                            }));
                        }
                    }
                }
            }
            stopwatch.Stop();
            Write("\n\n\n\t\t\tTesting terminated");
            Write("\n\t\tTotal iterations: " + num.ToString());
            Write("\n\t\tElapsed time: " + stopwatch.ElapsedMilliseconds.ToString());
            Write("\n\n\t\tPress ENTER to exit the demo ");
            ForegroundColor = ConsoleColor.Black;
            ReadLine();
        }

        private static void CheckKeys()
        {
            while (_isRunning)
            {
                if (KeyAvailable)
                {
                    HandleKeyPress(ReadKey(true).Key);
                }
                Thread.Sleep(10);
            }
        }



        private static void HandleKeyPress(ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
            {
                _isRunning = false;
                WriteLine("\n\n\tExiting...\n\n");
                return;
            }
            if (key != ConsoleKey.S)
            {
                return;
            }
            WriteLine("\nСтатус: тесты выполняются...");
        }



        private static bool _isRunning = true;
    }
}
