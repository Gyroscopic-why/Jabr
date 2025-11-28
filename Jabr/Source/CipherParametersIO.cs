using System;

using static System.Console;


using static Jabr.ShortcutLogic;
using static Jabr.GlobalVariables;
using System.Collections.Generic;



namespace Jabr
{
    internal class ParametersLogicIO
    {
        static public bool CheckUserConfirmation(string userInput = "", bool processTheInputFirst = true)
        {
            if (processTheInputFirst) userInput = ReadLine().Trim().ToLower();

            return userInput == "да"  || userInput == "д"
                || userInput == "yes" || userInput == "ye" || userInput == "y";
        }
        static public void SimpleRepeatOldInformation(bool simplified, Byte type,
            string message = "", string alphabet = "", bool displayShifts = false)
        {
            Clear();
            ForegroundColor = ConsoleColor.Gray;
            Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!");

            Write("\n\n\n");
            if (!simplified)
            {
                Write("\t\t\t--->  ");
                if (type == 0) Write("За");
                else Write("Де");
                Write("шифровка сообщений  <---\n\n");
            }


            if (message != "")
            {
                Write("\n\t\t[i]  - Исходное сообщение: ");
                BackgroundColor = ConsoleColor.DarkBlue;
                if (type == 0) Write(gDecrypted);
                else Write(gEncrypted);
                    BackgroundColor = ConsoleColor.Black;
                Write("\n");
            }
            if (alphabet != "")
            {
                Write("\t\t       Выбранный алфавит:  ");
                BackgroundColor = ConsoleColor.DarkMagenta;
                Write(alphabet);
                BackgroundColor = ConsoleColor.Black;
                Write("\n");
            }
            if (displayShifts && gShifts.Count > 0)
            {
                Write("\t\t       Выбранные сдвиги:   ");
                for (var i = 0; i < gShifts.Count - 1; i++) Write(gShifts[i] + ", ");
                Write(gShifts[gShifts.Count - 1] + "\n");
            }
        }


        static public Int16 GetUserTask(bool useShortcuts, bool clearUsed)
        {
            Int16 chosenTask = -1;
            string userInput;

            Write("\n\n\n\t\t[?]  - Что вы хотите сделать?\n");
            Write("\t\t   > 1 <    - О программе\n");
            Write("\t\t   > 2 <    - Зашифровать сообщение\n");
            Write("\t\t   > 3 <    - Дешифровать сообщение\n");
            Write("\t\t   > 4 <    - Настройки\n");
            Write("\t\t   > 0 <    - Выход\n");
            if (useShortcuts) Write("\t\t[i]  - Или можете ввести быструю команду с помощью ключевых символов '+' или '-'\n");

            while (chosenTask < 0 || chosenTask > 4)
            {
                Write("\n\t\t[->] - Ваш выбор: ");
                userInput = ReadLine().Trim();
                if (useShortcuts) CheckForShortcut(userInput);

                ForegroundColor = ConsoleColor.Red;
                switch(gShortcutError)
                {
                    case 0:
                        ForegroundColor = ConsoleColor.Green;
                        Write("\t\t[!]  - Команда быстрого ввода была успешно распознана\n");
                        break;
                    case 2:
                        Write("\t\t[!]  - Команда быстрого ввода не распознана\n");
                        break;
                    case 3:
                        Write("\t\t[!]  - Не удалось получить позицию первого разделителя быстрой команды\n");
                        break;
                    case 4:
                        Write("\t\t[!]  - Не удалось получить позицию второго разделителя быстрой команды\n");
                        break;
                    case 5:
                        Write("\t\t[!]  - Не удалось распознать все или часть сдвигов быстрой команды\n");
                        break;
                    case 6:
                        Write("\t\t[!]  - Один или несколько из распознанных сдвигов вне допустимого диапозона\n");
                        break;
                    case 7:
                        Write("\t\t[!]  - Выбранная версия шифра в быстрой команде не поддерживается текущим клиентом\n");
                        break;
                }
                gShortcutError = 1;  //  reset to no shortcut was found
                ForegroundColor = ConsoleColor.Gray;


                if (!Int16.TryParse(userInput, out chosenTask))
                {
                    Write("\t\tНе удалось распознать выбор. Пожалуйста, повторите ввод\n");
                    chosenTask = -1;
                }
                else if (chosenTask < 0 || chosenTask > 4) Write("\t\tЧисло не попадает в допустимый диапозон. Пожалуйста, повторите ввод\n");
            }
            if (clearUsed)
            {
                Clear();
                Write("\n\n\n\t\t\t   Добро пожаловать в Jabr " + gProgramVersion + "!");
            }
            return chosenTask;
        }



        static public void GetMessage(bool simplified, Byte type)
        {

            while (type < 2)
            {
                if (GlobalSettings.gClearUsed) SimpleRepeatOldInformation(simplified, type);

                if (simplified) Write("\n\t\t[->] - Т");
                else Write("\n\t\t[->] - Введите сообщение которое требуется ");

                if (type == 0) Write("зашифровать: ");
                else Write("дешифровать: ");
                BackgroundColor = ConsoleColor.DarkBlue;

                //  0 = encrypting   1 = decrypting
                if (type == 0) gDecrypted = ReadLine();
                else gEncrypted = ReadLine();
                type += 2;

                BackgroundColor = ConsoleColor.Black;
                if (!simplified)
                {
                    Write("\t\t[?]  - Сообщение верно? ");
                    ForegroundColor = ConsoleColor.DarkGray;
                    Write("(Да/Нет | Yes/No): ");
                    ForegroundColor = ConsoleColor.Gray;
                    if (!CheckUserConfirmation()) type -= 2;
                }
            }
            Write("\n");
        }


        static public void GetAlphabet(bool simplified, Byte type)
        {   //     Type = 0   -> Encrypting        Type = 1   -> Decrypting
            string message = type == 0 ? gDecrypted : gEncrypted;
            bool valid = false;

            if (GlobalSettings.gClearUsed) SimpleRepeatOldInformation(simplified, type, gDecrypted);

            while (!valid)
            {
                if (simplified) Write("\n\t\t[->] - Алфавит: ");
                else
                {
                    Write("\n\t\t[->] - Введите алфавит с помощью которого будет ");
                    if (type == 0) Write("за");
                    else Write("де");
                    Write("шифрованно сообщение: ");
                }

                BackgroundColor = ConsoleColor.DarkMagenta;
                gAlphabet = ReadLine();
                BackgroundColor = ConsoleColor.Black;

                if (GlobalSettings.gClearUsed) SimpleRepeatOldInformation(simplified, type, gDecrypted, gAlphabet);

                Write("\n");
                valid = CheckAlphabet(gAlphabet, message, simplified);

            }
            Write("\n");
        }
             

        static public bool CheckAlphabet(string alphabet, string message, bool simplified)
        {
            string missing = "", duplicates = "";
            bool valid = true;


            if (simplified)
            {
                for (var i = 0; i < message.Length; i++)  //  Search for missing
                {
                    if (i < message.Length)
                    {
                        if (alphabet.IndexOf(message[i]) == -1
                            && missing.IndexOf(message[i]) == -1)
                            missing += message[i];
                    }
                    if (i < alphabet.Length
                        && alphabet.IndexOf(alphabet[i]) != i
                        && duplicates.IndexOf(alphabet[i]) == -1)
                    {
                        duplicates += alphabet[i];
                    }
                }
            }
            else
            {
                ForegroundColor = ConsoleColor.DarkGray;
                Write("\t\t[i]  - Номера символов сообщения в алфавите: ");

                var errorCheckingLength = Math.Max(message.Length, alphabet.Length);
                for (var i = 0; i < errorCheckingLength; i++)  //  Search for missing
                {
                    if (i < message.Length)
                    {
                        var charId = alphabet.IndexOf(message[i]);
                        Write(charId + " ");

                        if (charId == -1
                            && missing.IndexOf(message[i]) == -1)
                            missing += message[i];
                    }
                    if ( i < alphabet.Length
                        &&   alphabet.IndexOf(alphabet[i]) != i
                        && duplicates.IndexOf(alphabet[i]) == -1)
                    {
                        duplicates += alphabet[i];
                    }
                }
                ForegroundColor = ConsoleColor.Gray;
                Write("\n");
            }


            if (duplicates != "")
            {
                ForegroundColor = ConsoleColor.Red;
                if (simplified) Write("\t\t[!]  - Повторяющиеся символов: ");
                else Write("\t\t[!]  - Алфавит содержит повторяющиеся символов: ");

                ForegroundColor = ConsoleColor.Gray;
                for (var i = 0; i < duplicates.Length - 1; i++)
                {
                    BackgroundColor = ConsoleColor.DarkMagenta;
                    Write(duplicates[i]);
                    BackgroundColor = ConsoleColor.Black;
                    Write(", ");
                }
                BackgroundColor = ConsoleColor.DarkMagenta;
                Write(duplicates[duplicates.Length - 1]);
                BackgroundColor = ConsoleColor.Black;
                Write("\n");

                valid = false;
            }

            if (missing == " ")  //  only missing 'space'
            {
                if (duplicates == "")
                {
                    alphabet += " ";

                    ForegroundColor = ConsoleColor.Magenta;
                    if (simplified) Write("\t\t       Добавлен пробел");
                    else Write("\t\t       Алфавит будет изменён, для содержания пробела");

                    ForegroundColor = ConsoleColor.Gray;
                    Write("\n\t\t       Новый алфавит: ");
                    BackgroundColor = ConsoleColor.DarkMagenta;
                    Write(alphabet);
                    BackgroundColor = ConsoleColor.Black;
                    Write("\n");
                }
            }
            else if (missing != "")
            {
                ForegroundColor = ConsoleColor.Red;
                if (simplified) Write("\t\t[!]  - Нет необходимых символов: ");
                else Write("\t\t[!]  - В алфавите нет необходимых символов из сообщения: ");

                ForegroundColor = ConsoleColor.Gray;
                for (var i = 0; i < missing.Length - 1; i++)
                {
                    BackgroundColor = ConsoleColor.DarkMagenta;
                    Write(missing[i]);
                    BackgroundColor = ConsoleColor.Black;
                    Write(", ");
                }
                BackgroundColor = ConsoleColor.DarkMagenta;
                Write(missing[missing.Length - 1]);
                BackgroundColor = ConsoleColor.Black;
                Write("\n");

                valid = false;
            }

            if (valid && !simplified)
            {
                Write("\t\t[?]  - Алфавит введён верно? ");
                ForegroundColor = ConsoleColor.DarkGray;
                Write("(Да/Нет | Yes/No): ");
                ForegroundColor = ConsoleColor.Gray;
                return CheckUserConfirmation();
            }

            return valid;
        }
             //  A valid alphabet:
             //    - Must      include all chosen characters from the original message
             //    - Must      include every chosen character only one time
             //    - Can       include additional character not present in the original message


        static public void GetShifts(bool simplified, byte type)
        {
            bool confirm = false;
            string userInput;

            gShifts.Clear();
            if (GlobalSettings.gClearUsed) SimpleRepeatOldInformation(simplified, type, gDecrypted, gAlphabet);

            while (!confirm)
            {
                ForegroundColor = ConsoleColor.DarkGray;
                Write("\n\t\t[i]  - Нажатие ENTER без ввода числа закончит ввод сдвигов");
                ForegroundColor = ConsoleColor.Gray;

                if (simplified) Write($"\n\t\t[->] - Сдвиг от 0 до {gAlphabet.Length - 1}: ");
                else Write($"\n\t\t[->] - Введите {gShifts.Count + 1}-й сдвиг для алфавита ");
                ForegroundColor = ConsoleColor.DarkGray;
                Write($"(число от 0 до {gAlphabet.Length - 1}): ");
                ForegroundColor = ConsoleColor.Gray;
                userInput = ReadLine().Replace(" ", "");

                if (Int32.TryParse(userInput, out Int32 buffer))
                {
                    if (buffer >= 0 && buffer < gAlphabet.Length)
                    {
                        if (!simplified)
                        {
                            Write("\t\t[?]  - Сдвиг введён верно?");
                            ForegroundColor = ConsoleColor.DarkGray;
                            Write(" (Да/Нет | Yes/No): ");
                            ForegroundColor = ConsoleColor.Gray;
                            if (CheckUserConfirmation(userInput)) gShifts.Add(buffer);
                        }
                        else gShifts.Add(buffer);

                        if (GlobalSettings.gClearUsed)
                            SimpleRepeatOldInformation(simplified, type, gDecrypted, gAlphabet, true);
                    }
                    else
                    {
                        if (GlobalSettings.gClearUsed)
                            SimpleRepeatOldInformation(simplified, type, gDecrypted, gAlphabet, true);

                        ForegroundColor = ConsoleColor.Red;
                        if (simplified) Write("\t\t[!]  - Вне диапозона\n");
                        else Write("\t\t[!]  - Введённой число не попадает в допустимый диапозон. Повторите ввод\n");
                        ForegroundColor = ConsoleColor.Gray;
                    }
                }
                else
                {
                    if (GlobalSettings.gClearUsed)
                        SimpleRepeatOldInformation(simplified, type, gDecrypted, gAlphabet, true);

                    if (userInput == "")
                    {
                        if (gShifts.Count == 0) gShifts.Add(0);
                        confirm = true;
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Red;
                        if (simplified) Write("[!]  - Некоректный ввод\n");
                        else Write("\t\t[!]  - Некоректный ввод, попробуйте ещё раз\n");
                        ForegroundColor = ConsoleColor.Gray;
                    }
                }

                if (gShifts.Count >= gDecrypted.Length) confirm = true;
            }

            Write("\n");
        }
    }
}