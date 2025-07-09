using System;

using static System.Console;


using static Jabr.GlobalVariables;



namespace Jabr
{
    internal class GlobalSettings
    {

            //  Flag that enables additional info output
        static public bool gShowInfo = false;


            //  Flag that enables a simplified user interface
        static public bool gSimplified = false;


            //  Flag that enables console buffer cleaning
            //  (of the already used information)
        static public bool gClearUsed = true;


            //  Flag that enables shortcut parsing from the menu
        static public bool gUseShortcuts = true;


            //  Current selected cipher version
            //     > 1 <    = RE1
            //     > 2 <    = RE2
            //     > 3 <    = RE3
            //     > 4 <    = RE4
            //
            //  !  > 5 <    = RE5  :D
        static public byte gVersion = 4;



        static public void ChangeSettings()
        {
            string UserInput = "";

            while (UserInput != "0")
            {
                Write("\n\n\n");
                if (gClearUsed)
                {
                    Clear();
                    Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!\n\n\n");
                    Write("\t\t\t--->  Изменение настроек  <---\n\n");
                }

                Write("\t\t[?]  - Изменить версию используемого шифра:\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 1) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 1 <    - РЕ1, Защита: 3/5, скорость: 1/5\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 2) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 2 <    - РЕ2, Защита: 3/5, скорость: 2/5\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 3) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 3 <    - РЕ3, Защита: 3/5, скорость: 5/5\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 4) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 4 <    - РЕ4, Защита: 5/5, скорость: 4/5   (По умолчанию)\n");
                ForegroundColor = ConsoleColor.White;

                Write("\n\t\t  > 6 <    - Выводить дополнительную информацию о процессе шифрования: ");
                if (gShowInfo)
                {
                    ForegroundColor = ConsoleColor.Green;
                    Write("Да");
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("Нет");
                }
                ForegroundColor = ConsoleColor.White;

                Write("\n\t\t  > 7 <    - Стирать использованную информацию: ");
                if (gClearUsed)
                {
                    ForegroundColor = ConsoleColor.Green;
                    Write("Да");
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("Нет");
                }
                ForegroundColor = ConsoleColor.White;

                Write("\n\t\t  > 8 <    - Упростить ввод данных: ");
                if (gSimplified)
                {
                    ForegroundColor = ConsoleColor.Green;
                    Write("Да");
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("Нет");
                }
                ForegroundColor = ConsoleColor.White;

                Write("\n\t\t  > 9 <    - Использовать быстрые команды для шифровки: ");
                if (gUseShortcuts)
                {
                    ForegroundColor = ConsoleColor.Green;
                    Write("Да");
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("Нет");
                }
                ForegroundColor = ConsoleColor.White;

                Write("\n\t\t  > 0 <    - Назад");

                Write("\n\n\t\tВаш выбор: ");
                UserInput = ReadLine().Trim();
                switch (UserInput)
                {
                    case "1":
                        gVersion = 1;
                        break;
                    case "2":
                        gVersion = 2;
                        break;
                    case "3":
                        gVersion = 3;
                        break;
                    case "4":
                        gVersion = 4;
                        break;

                    //case "5":
                    //gVersion = 5;
                    //break;

                    case "6":
                        gShowInfo = !gShowInfo;
                        break;
                    case "7":
                        gClearUsed = !gClearUsed;
                        break;
                    case "8":
                        gSimplified = !gSimplified;
                        break;
                    case "9":
                        gUseShortcuts = !gUseShortcuts;
                        break;
                }
            }
            if (gClearUsed)
            {
                Clear();
                Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!");
            }
        }
    }
}