//using System;
using static System.Console;
using static Jabr.CustomFunctions;
//using static Jabr.CipherSource;

namespace Jabr {
    internal class Program
    {
        
        static void Main(string[] args)
        {
            OutputEncoding = System.Text.Encoding.UTF8;
            Title = "Jabr - encoder/decoder - v1.4.2 beta";

            Write("\n\n\n\t\t\t   Добро пожаловать в Jabr v1.4.2 beta!");
            short OurTask = GetUserTask(gUseShortcuts, gClearUsed);

            while (OurTask != 0)
            {
                switch (OurTask)
                {
                    case 1:
                        // Write info about the program, show a tutorial to the ciphers
                        break; //Currently in development

                    case 2:
                        GetInfo(0); //0 = encoding   1 = decoding
                        Encrypt(gVersion, gAdvInfo, gDecrypted, gEncrypted, gAlphabet, gShift);
                        break;

                    case 3:
                        GetInfo(1); //0 = encoding   1 = decoding
                        Decrypt(gVersion, gAdvInfo, gDecrypted, gEncrypted, gAlphabet, gShift);
                        break;

                    case 4:
                        ChangeSettings();
                        break;

                    default:
                        break;
                }
                OurTask = GetUserTask(gUseShortcuts, gClearUsed);
            }
        }
    }
}
