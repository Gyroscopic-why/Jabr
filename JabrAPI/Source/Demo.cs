using System;
using System.Text;
using System.Collections.Generic;

using static System.Console;


using AVcontrol;



namespace JabrAPI.Source
{
    internal class Demo
    {
        static public bool AlgorithmTest(string fast, string debug, string fromBin, bool writeDebug = false)
        {
            bool result = fast == debug && fast == fromBin;
            if (!writeDebug) return result;



            Write("\n\n\tAlgorithm test: ");
            if (result)
            {
                ForegroundColor = ConsoleColor.Green;
                Write("Passed");
                ForegroundColor = ConsoleColor.Gray;
            }
            else
            {
                ForegroundColor = ConsoleColor.Red;
                Write("Failed");
                ForegroundColor = ConsoleColor.Gray;
            }

            Write("\n\tFast:      ");
            if (fast.Length < debug.Length)
            {
                for (var curId = 0; curId < fast.Length; curId++)
                {
                    if (fast[curId] == debug[curId]) ForegroundColor = ConsoleColor.DarkGreen;
                    else ForegroundColor = ConsoleColor.DarkRed;
                    Write(fast[curId]);
                }

                ForegroundColor = ConsoleColor.Gray;
                Write("\n\tDebug:     ");

                Write(debug.Substring(0, fast.Length));
                ForegroundColor = ConsoleColor.DarkCyan;
                Write(debug.Substring(fast.Length));
            }
            else
            {
                Write(fast.Substring(0, debug.Length));
                ForegroundColor = ConsoleColor.DarkCyan;
                Write(fast.Substring(debug.Length));

                ForegroundColor = ConsoleColor.Gray;
                Write("\n\tDebug:     ");

                for (var curId = 0; curId < debug.Length; curId++)
                {
                    if (debug[curId] == fast[curId]) ForegroundColor = ConsoleColor.DarkGreen;
                    else ForegroundColor = ConsoleColor.DarkRed;
                    Write(debug[curId]);
                }
            }

            ForegroundColor = ConsoleColor.Gray;
            Write("\n\tFromBin:   ");
            for (var curId = 0; curId < Math.Min(fromBin.Length, debug.Length); curId++)
            {
                if (fromBin[curId] == debug[curId]) ForegroundColor = ConsoleColor.Green;
                else ForegroundColor = ConsoleColor.Red;
                Write(fromBin[curId]);
            }
            

            ForegroundColor = ConsoleColor.Gray;
            Write("\n\n");

            return result;
        }



        static public void Run()
        {
            //string aboba = "aboba";
            string? exitFlag = "";

            while (exitFlag != "0")
            {
                Clear();


                RE5.EncryptionKey reKey = new RE5.EncryptionKey();
                //RE5.EncryptionKey reKey = new RE5.EncryptionKey("qwertyuabo", "01");
                reKey.Next();
                //reKey.GenerateRandomShifts(0);

                //RE5.EncryptionKey reKey = new RE5.EncryptionKey("abcqwertyuiop1234567", "abolk123", 3);
                string msg = "aboba";
                Int32 prlen = reKey.PrLength, msglen = msg.Length;
                string pral = reKey.PrAlphabet;
                Write("\n\tMessage:   (" + msglen + ") " + msg);
                Write("\n\tPrimary:   (" + prlen + ") ");
                for (var i = 0; i < prlen; i++)
                {
                    for (var j = 0; j < msglen; j++)
                    {
                        if (pral[i] == msg[j])
                        {
                            ForegroundColor = ConsoleColor.Green;
                            j += msglen;
                        }
                    }
                    Write(pral[i]);
                    ForegroundColor = ConsoleColor.Gray;
                }
                Write("\n\tExternal:  (" + reKey.ExLength + ") " + reKey.ExternalAlphabet);
                //Write("\n\tShifts count: " + reKey.Shifts.Length);

                /*Write("\n\tShifts:   ");
                for (Int32 curShift = 0; curShift < reKey.Shifts.Length; curShift++) 
                    Write(reKey.Shifts[curShift] + " ");*/

                string enc = RE5.Encrypt(msg, reKey, true);
                string test = RE5.EncryptWithConsoleInfo(msg, reKey);
                List<Int16> bin = RE5.EncryptToBinaryUtf16(msg, reKey);


                Demo.AlgorithmTest(enc, test, Encoding.Unicode.GetString(ToBinary.LittleEndian(bin.ToArray())));
                Write("\n\n\n");

                bool flag = false;
                string dec, tset, fromBin;
                dec = RE5.FastDecrypt(test, reKey);
                try { dec = RE5.FastDecrypt(test, reKey); }
                catch
                {
                    dec = "";
                    ForegroundColor = ConsoleColor.Red;
                    Write("\n\n\tFast Decrypt: Failed");
                    ForegroundColor = ConsoleColor.Gray;
                }
                try { tset = RE5.DecryptWithConsoleInfo(test, reKey); }
                catch
                {
                    tset = "";
                    ForegroundColor = ConsoleColor.Red;
                    Write("\n\tDebug Decrypt: Failed");
                    ForegroundColor = ConsoleColor.Gray;
                }
                if (tset == null)
                {
                    tset = "";
                    ForegroundColor = ConsoleColor.Red;
                    Write("\n\tDebug Decrypt: Failed");
                    ForegroundColor = ConsoleColor.Gray;
                }
                try
                {
                    fromBin = RE5.DecryptFromBinaryUtf16(bin, reKey);
                    flag = true;
                }
                catch
                {
                    fromBin = "";
                    ForegroundColor = ConsoleColor.Red;
                    Write("\n\tFrom Binary Decrypt: Failed");
                    ForegroundColor = ConsoleColor.Gray;
                }

                Demo.AlgorithmTest(dec, tset, fromBin);
                while (flag && false)
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("\n\n\n\n\t\tCATASTROPHIC FAILURE");
                    Write("\n\t\tCATASTROPHIC FAILURE");
                    Write("\n\t\tCATASTROPHIC FAILURE");

                    ForegroundColor = ConsoleColor.Gray;
                    Write("\n\n\tPress any key to REFRESH DEBUG...");


                    ReadKey();
                    Clear();



                    prlen = reKey.PrLength;
                    msglen = msg.Length;
                    pral = reKey.PrAlphabet;
                    Write("\n\tMessage:   (" + msglen + ") " + msg);
                    Write("\n\tPrimary:   (" + prlen + ") ");
                    for (var i = 0; i < prlen; i++)
                    {
                        for (var j = 0; j < msglen; j++)
                        {
                            if (pral[i] == msg[j])
                            {
                                ForegroundColor = ConsoleColor.Green;
                                j += msglen;
                            }
                        }
                        for (var j = 0; j < fromBin.Length; j++)
                        {
                            if (pral[i] == fromBin[j])
                            {
                                ForegroundColor = ConsoleColor.Red;
                                j += fromBin.Length;
                            }
                        }
                        Write(pral[i]);
                        ForegroundColor = ConsoleColor.Gray;
                    }


                    Write("\n\tExternal:  (" + reKey.ExLength + ") " + reKey.ExternalAlphabet);
                    RE5.EncryptWithConsoleInfo(msg, reKey);
                    Write("\n\n\n");

                    try { tset = RE5.DecryptWithConsoleInfo(test, reKey); }
                    catch
                    {
                        tset = "";
                        ForegroundColor = ConsoleColor.Red;
                        Write("\n\tDebug Decrypt: Failed");
                        ForegroundColor = ConsoleColor.Gray;
                    }
                    if (tset == null)
                    {
                        tset = "";
                        ForegroundColor = ConsoleColor.Red;
                        Write("\n\tDebug Decrypt: Failed");
                        ForegroundColor = ConsoleColor.Gray;
                    }
                    try
                    {
                        fromBin = RE5.DecryptFromBinaryUtf16(bin, reKey);
                        flag = true;
                    }
                    catch
                    {
                        fromBin = "";
                        ForegroundColor = ConsoleColor.Red;
                        Write("\n\tFrom Binary Decrypt: Failed");
                        ForegroundColor = ConsoleColor.Gray;
                    }

                    Demo.AlgorithmTest(dec, tset, fromBin);
                }

                //if (!Demo.AlgorithmTest(dec, tset, fromBin))
                //{
                //    while (true)
                //    {
                //        ForegroundColor = ConsoleColor.Red;
                //        Write("\n\n\n\n\t\tCATASTROPHIC FAILURE");
                //        Write("\n\t\tCATASTROPHIC FAILURE");
                //        Write("\n\t\tCATASTROPHIC FAILURE");
                //        Write("\n\t\tCATASTROPHIC FAILURE");
                //        Write("\n\t\tCATASTROPHIC FAILURE");
                //        Write("\n\t\tCATASTROPHIC FAILURE");
                //        Write("\n\t\tCATASTROPHIC FAILURE");

                //        ForegroundColor = ConsoleColor.Gray;
                //        Write("\n\n\tPress any key to REFRESH DEBUG...");


                //        ReadKey();
                //        Clear();


                //        RE5.EncryptWithConsoleInfo(msg, reKey);

                //        try { tset = RE5.DecryptWithConsoleInfo(test, reKey); }
                //        catch
                //        {
                //            tset = "";
                //            ForegroundColor = ConsoleColor.Red;
                //            Write("\n\tDebug Decrypt: Failed");
                //            ForegroundColor = ConsoleColor.Gray;
                //        }
                //        if (tset == null)
                //        {
                //            tset = "";
                //            ForegroundColor = ConsoleColor.Red;
                //            Write("\n\tDebug Decrypt: Failed");
                //            ForegroundColor = ConsoleColor.Gray;
                //        }

                //        Demo.AlgorithmTest(dec, tset, fromBin);
                //    }
                //}


                Write("\n\n\n\t\tEnter '0' to exit ");
                exitFlag = ReadLine();
            }
        }
    }
}
