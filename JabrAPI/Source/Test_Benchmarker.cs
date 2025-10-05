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

            List<Int64> ms = new List<Int64>();
            const Int32 totalAttempts = 100;
            

            for (var attempt = 0; attempt < totalAttempts; attempt++)
            {
                string enc = RE5.FastEncrypt("Aboba", reKey);
                Stopwatch timer = new Stopwatch();

                if (attempt % 2 == 0)
                {
                    Write("\n\t\t\tFAST   - ");
                    timer.Start();
                    for (var i = 0; i < 1_000_000; i++) _ = RE5.FastDecrypt(enc, reKey);
                }
                else
                {
                    Write("\n\t\t\tINFO   - ");
                    timer.Start();
                    for (var i = 0; i < 1_000_000; i++) _ = RE5.DecryptWithConsoleInfo(enc, reKey, false);
                }

                timer.Stop();
                ms.Add(timer.ElapsedMilliseconds);

                Write($"\tAttempt {attempt + 1})\t DECRYPT     1m: {timer.ElapsedMilliseconds / 1000}.{timer.ElapsedMilliseconds % 1000}");

                if (attempt % 10 == 9)
                {
                    Write("\n\t\t\tRefreshing key...");
                    reKey.Next();
                }
            }

            Int64 min = ms.Min(), max = ms.Max(), sum = ms.Sum();
            double avg = ms.Average();

            Write($"\n\n\t\t\tBenchmark finished, Interval: {min/1000}.{min%1000}-{max/1000}.{max%1000}, average: {(Int64)avg/1000}.{(Int64)avg%1000}");
            Write("\n\t\t\tTotal time elapsed for " + totalAttempts + "m operations: " + sum / 1000 + "." + sum % 1000);
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
                        if (num % 16384UL == 0UL)
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
