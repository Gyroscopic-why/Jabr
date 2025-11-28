using System;
using static Jabr.CryptingLogic;
using static Jabr.GlobalSettings;
using static Jabr.GlobalVariables;
using static Jabr.ParametersLogicIO;
using static Jabr.ProgramInfo;
using static System.Console;



namespace Jabr 
{
    internal class Program
    {
        
        static void Main()
        {
            OutputEncoding = System.Text.Encoding.UTF8;
            Title = "Jabr cryptor - " + gProgramVersion;

            ForegroundColor = ConsoleColor.Gray;
            Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!");
            short OurTask = GetUserTask(gUseShortcuts, gClearUsed);

            while (OurTask != 0)
            {
                switch (OurTask)
                {
                    case 1:
                        ShowProgramInfo();
                        break;

                    case 2:
                        GetInfo(0);    //  0 = encrypting   1 = decrypting
                        Encrypt(255, gShowInfo, gDecrypted, gAlphabet, gShifts);
                        break;

                    case 3:
                        GetInfo(1);    //  0 = encrypting   1 = decrypting
                        Decrypt(255, gShowInfo, gEncrypted, gAlphabet, gShifts);
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
