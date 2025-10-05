using System;

using static System.Console;


using JabrAPI.Source;
using AVcontrol;



namespace JabrAPI
{
    internal class Program
    {
        static void Main()
        {
            TestBenchmarker.DecryptBenchmark();
            Write("\n\n\n\t\t\tDecrypt benchmark finished. Press any key to launch full benchmark suite");
            ReadKey();

            TestBenchmarker.Run();
        }
    }
}