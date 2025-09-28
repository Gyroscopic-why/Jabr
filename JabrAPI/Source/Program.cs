using System;
using System.Text;
using System.Collections.Generic;

using static System.Console;


using AVcontrol;
using JabrAPI.Source;



namespace JabrAPI
{
    internal class Program
    {
        static void Main()
        {
            //string aboba = "aboba";
            string exitFlag = "";

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