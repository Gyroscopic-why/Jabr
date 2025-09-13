using System;

using static System.Console;


namespace JabrAPI.Source
{
    internal class Demo
    {
        static public bool AlgorithmTest(string fast, string debug)
        {
            bool result = fast == debug;

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
            Write("\n\n");

            return result;
        }
    }
}
