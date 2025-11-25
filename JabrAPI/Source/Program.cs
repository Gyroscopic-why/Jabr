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

            RE4.EncryptionKey reKey = new();
            string initial = "aboba aboba aboba";

            Write("\n\n\t\t\tStarting benchmark...");
            for (var i = 1; i < 10_000; i++)
            {
                reKey.Next();

                string dec3 = RE3.FastDecrypt(RE3.FastEncrypt(initial, reKey), reKey);
                if (dec3 != initial)
                {
                    Write("\n\n\t\t\tRE3: Something went wrong at iteration " + i);
                    Write("\n\tExpected: " + initial);
                    Write("\n\tGot: " + dec3);
                    Write("\n\tReKey: " + reKey);
                    ReadKey();
                    return;
                }

                string dec4 = RE4.FastDecrypt(RE4.FastEncrypt(initial, reKey), reKey);
                if (dec4 != initial)
                {
                    Write("\n\n\t\t\tRE4: Something went wrong at iteration " + i);
                    Write("\n\tExpected: " + initial);
                    Write("\n\tGot: " + dec4);
                    Write("\n\tReKey: " + reKey);
                    ReadKey();
                    return;
                }

                if (i % 1000 == 0)
                {
                    Write("\n\t\tCompleted iteration " + i + ", no problems so far!");
                }
            }

            Write("\n\n\t\tBenchmark finished, no problems :) ");
            ReadKey();
        }
    }
}