using System;
using static System.Console;
using static Jabr.CustomFunctions;
using static Jabr.CipherSource;

namespace Jabr 
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Title = "Jabr - encoder/decoder - v1.4.1";

            Write("\n\n\n\t\t\t   Добро пожаловать в Jabr!");
            short OurTask = GetUserTask();

            while (OurTask != 0)
            {
                switch (OurTask)
                {
                    case 2:
                        GetInfo(0); //0 = encoding   1 = decoding
                        Encode(gVersion, gAdvInfo);
                        break;

                    case 3:
                        GetInfo(1); //0 = encoding   1 = decoding
                        Decode(gVersion, gAdvInfo);
                        break;

                    case 4:
                        ChangeSettings();
                        break;

                    default:
                        break;
                }
                OurTask = GetUserTask();
            }
        }
    }
}
