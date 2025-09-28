using System;

using static System.Console;


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
    }
}
