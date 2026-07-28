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


            //  Legacy selected cipher version
            //  !  > 1 <    = RE1    Unsupported because of legacy compatibility issues
            //  !  > 2 <    = RE2    Unsupported because of legacy compatibility issues
            //     > 3 <    = RE3
            //     > 4 <    = RE4
            //  !  > 5 <    = RE5    Unsupported because of client version (v1.4.3)
        static public bool gUseRE3 = false;
        static public bool gUseRE4 = true;



        static public void ChangeSettings()
        {
            string userInput = "";

            while (userInput != "0" || !gUseRE3 && !gUseRE4)
            {
                Write("\n\n\n");
                if (gClearUsed)
                {
                    Clear();
                    ForegroundColor = ConsoleColor.Gray;
                    Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!\n\n\n");
                    Write("\t\t\t--->  Изменение настроек  <---\n\n");
                }

                Write("\t\t[?]  - Изменить версию используемого шифра:\n");
                ForegroundColor = ConsoleColor.DarkGray;
                Write("\t\t   -1-     - РЕ1, Больше не поддерживается\n");
                Write("\t\t   -2-     - РЕ2, Больше не поддерживается\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gUseRE3) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 3 <    - РЕ3, Защита:  4/10, скорость: 10/10\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gUseRE4) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 4 <    - РЕ4, Защита:  5/10, скорость: 9/10   (По умолчанию)\n");
                ForegroundColor = ConsoleColor.DarkGray;
                Write("\t\t   -5-     - РЕ5, Защита: 10/10, скорость: 8/10\n");
                Write("\t\t                  (Не поддерживается клиентом версии 4)\n");
                ForegroundColor = ConsoleColor.Gray;

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
                ForegroundColor = ConsoleColor.Gray;

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
                ForegroundColor = ConsoleColor.Gray;

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
                ForegroundColor = ConsoleColor.Gray;

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

                Write("\n\t\t  > 0 <    - Назад\n\n");

                if (userInput == "0")
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("\t\t[!]  - Необходимо выбрать хотя бы 1 протокол шифрования");
                    ForegroundColor = ConsoleColor.White;
                }


                Write("\n\t\tВаш выбор: ");
                userInput = ReadLine().Trim();
                switch (userInput)
                {
                    case "3":
                        gUseRE3 = !gUseRE3;
                        break;
                    case "4":
                        gUseRE4 = !gUseRE4;
                        break;


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
                ForegroundColor = ConsoleColor.Gray;
                Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!");
            }
        }
    }
}