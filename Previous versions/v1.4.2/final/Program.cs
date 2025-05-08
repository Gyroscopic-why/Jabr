

using static System.Console;


using static Jabr.CryptingLogic;
using static Jabr.GlobalSettings;
using static Jabr.GlobalVariables;
using static Jabr.ParametersLogicIO;

namespace Jabr 
{
    internal class Program
    {
        
        static void Main()
        {
            OutputEncoding = System.Text.Encoding.UTF8;
            Title = "Jabr cryptor - " + gProgramVersion;

            Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!");
            short OurTask = GetUserTask(gUseShortcuts, gClearUsed);

            while (OurTask != 0)
            {
                switch (OurTask)
                {
                    case 1:
                        // Write info about the program, show a tutorial to the ciphers
                        break; //Currently in development

                    case 2:
                        GetInfo(0); // 0 = encoding   1 = decoding
                        Encrypt(gVersion, gShowInfo, gDecrypted, gEncrypted, gAlphabet, gShift);
                        break;

                    case 3:
                        GetInfo(1); // 0 = encoding   1 = decoding
                        Decrypt(gVersion, gShowInfo, gDecrypted, gEncrypted, gAlphabet, gShift);
                        break;

                    case 4:
                        ChangeSettings();
                        break;

                    default:
                        break;
                }

                //  Let the user chose the next task
                OurTask = GetUserTask(gUseShortcuts, gClearUsed);
            }
        }
    }
}
