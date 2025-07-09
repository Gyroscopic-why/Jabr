using System;

using static System.Console;


using static Jabr.ShortcutLogic;
using static Jabr.GlobalVariables;


namespace Jabr
{
    internal class ParametersLogicIO
    {

        static public short GetUserTask(bool useShortcuts, bool clearUsed)
        {
            short chosenTask = -1;
            string userInput;

            Write("\n\n\n\t\t[?]  - Что вы хотите сделать?\n");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("\t\t   > 1 <    - О программе\n");
            ForegroundColor = ConsoleColor.White;
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
                if (gShortcutError == 0)
                {
                    gShortcutError = 1; //Return the input state to "no shortcut was found"
                    Write("\t\tКоманда быстрого ввода была успешно распознана\n"); // Inform the user of the succesful parse of the shortcut
                }
                else if (gShortcutError == 2)
                {
                    gShortcutError = 1; //Return the input state to "no shortcut was found"
                    Write("\t\tКоманда быстрого ввода не распознана\n"); // Inform the user of the UNsuccesful parse of the shortcut
                }
                else if (!short.TryParse(userInput, out chosenTask))
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
             //  Get the current program task


        static public void GetMessage(bool simplified, byte type, int cipherVersion)
        {
            string UserInput;
            bool ConfirmMessage = false;

            Write("\n\n\n");
            if (!simplified)
            {
                Write("\t\t\t--->  ");
                if (type == 0) Write("За");
                else Write("Де");
                Write("шифровка сообщений с помощью РЕ" + cipherVersion + "  <---\n\n");
            }

            while (!ConfirmMessage && !simplified || type < 2)
            {
                if (simplified) Write("\n\t\t[->] - Т");
                else Write("\n\t\t[->] - Введите сообщение которое т");
                if (type == 0) Write("ребуется закодировать: ");
                else Write("ребуется декодировать: ");
                BackgroundColor = ConsoleColor.Blue;

                //0 = encoding   1 = decoding
                if (type == 0) gDecrypted = ReadLine(); //Got the message
                else gEncrypted = ReadLine();

                BackgroundColor = ConsoleColor.Black;
                if (!simplified)
                {
                    Write("\t\t[?]  - Сообщение верно? (Да/Нет | Yes/No): ");
                    UserInput = ReadLine().Trim().ToLower();
                    ConfirmMessage = (UserInput == "да" || UserInput == "yes");
                    if (ConfirmMessage) type += 2;
                }
                else type += 2;
            }
            Write("\n");
        }
             //  Getting the original message
             //  For the later encrypting/decrypting of it
             //
             //  Includes an optional "confirmation"
             //  Of the parsed message (if the user made a mistake and wants to change it)


        static public void GetAlphabet(bool simplified, byte type)
        {   //     Type = 0   -> Encrypting        Type = 1   -> Decrypting
            string message = type == 0 ? gDecrypted : gEncrypted;
            bool valid = false;

            while (!valid) // Parsing the alphabet for encrypting/decrypting
            {
                if (simplified) Write("\n\t\t[->] - Алфавит: ");                              //
                else                                                                           //
                {                                                                              //
                    Write("\n\t\t[->] - Введите алфавит с помощью которого будет ");           //
                    if (type == 0) Write("за");                                               //
                    else Write("де");                                                          //
                    Write("шифрованно сообщение: ");                                           //
                }   //                                           Ask the user for the alphabet //
                BackgroundColor = ConsoleColor.Magenta; //
                gAlphabet = ReadLine();                 //
                BackgroundColor = ConsoleColor.Black;   //Got the alphabet

                valid = CheckAlphabet(gAlphabet, message, simplified); // Check the alphabet
            }   //Try again if errors ocured
            Write("\n");
        }
             //  Getting the alphabet/alphabets  (multiple will be used in Gen5 ciphers)
             //  for the encryption/decryption processes
             

        static public bool CheckAlphabet(string alphabet, string message, bool simplified)
        {
            bool valid = true;
            string missing = "", duplicates = "";


            for (int i = 0; i < alphabet.Length; i++) //Check for duplicate letters in the alphabet
            {
                if (alphabet.IndexOf(alphabet[i]) != i //Check if there is another copy of our char
                    && duplicates.IndexOf(alphabet[i]) == -1) duplicates += alphabet[i]; //and we havent counted it yet
            }
            if (duplicates != "") // There is duplicates
            {   // Then write error messages and duplicates info
                if (simplified) Write("\t\t[!]  - Повторяющиеся буквы: ");
                else Write("\t\t[!]  - Алфавит содержит повторяющиеся буквы: ");
                for (int i = 0; i < duplicates.Length - 1; i++) Write(duplicates[i] + ", ");
                Write(duplicates[duplicates.Length - 1] + "\n");

                valid = false; // the alphabet is no longer valid
            }

            if (simplified)
            {
                for (int i = 0; i < message.Length; i++) //Search for missing letters
                {
                    if (alphabet.IndexOf(message[i]) == -1 //If the char doesnt exist in our alphabet
                        && missing.IndexOf(message[i]) == -1) missing += message[i]; //And we havent counted it yet
                }
            }
            else
            {
                Write("\t\t[i]  - Номера букв сообщения в алфавите: "); //Write extra info
                for (int i = 0; i < message.Length; i++) //Search for missing letters
                {
                    Write(alphabet.IndexOf(message[i]) + " ");
                    if (alphabet.IndexOf(message[i]) == -1 //If the char doesnt exist in our alphabet
                        && missing.IndexOf(message[i]) == -1) missing += message[i]; //And we havent counted it yet
                }
                Write("\n");
            }
            if (missing == " ") // If the alphabet is missing the "space" char
            {
                if (duplicates == "") // If there arent any duplicates
                {
                    alphabet += " "; // Then we add the "space" char to the alphabet

                    if (simplified) Write("\t\t       Добавлен пробел");                   //
                    else Write("\t\t       Алфавит будет изменён, для содержания пробела"); //
                    Write("\n\t\t       Новый алфавит: ");                                  //
                                                                                            // Inform the user about the change //
                    BackgroundColor = ConsoleColor.Magenta;                                 //
                    Write(alphabet);                                                       //
                    BackgroundColor = ConsoleColor.Black;                                   //
                    Write("\n");                                                            //
                }
                missing = ""; // All is good, the "space" cahr is no longer missing
            }
            else if (missing != "") // Check if we are missing any letters/cahr
            {   // if we are missing something
                if (simplified) Write("\t\t[!]  - Нет необходимых букв: ");                 //
                else Write("\t\t[!]  - В алфавите нет необходимых букв из сообщения: ");     //
                for (int i = 0; i < missing.Length - 1; i++) Write(missing[i] + ", ");     //
                Write(missing[missing.Length - 1] + "\n"); // Then write the error message //

                valid = false; // The alphabet is no longer valid
            }
            if (!simplified && missing == "" && duplicates == "")
            {
                Write("\t\t[?]  - Алфавит введён верно? (Да/Нет | Yes/No): "); //
                message = ReadLine().Trim().ToLower();                        // Ask the user if he agrees to the parsed alphabet
                valid = (message == "да" || message == "yes");              //
            }   // If the user accepted the alphabet or not                    //
            return valid;
        }
             //  Validate the chosen alphabet for the current message
             //
             //  A valid alphabet:
             //    - Must      include all chosen characters from the original message
             //    - Must      include every chosen character only one time
             //    - Can       include additional character not present in the original message
             //
             //  Returns the validatin state of a chosen alphabet
             //    - False: not valid
             //    - True:   is valid


        static public void GetShift(bool simplified)
        {
            bool Confirm = false, SuccessfulParse = false;
            string UserInput;

            while (!SuccessfulParse || !Confirm && !simplified)
            {
                SuccessfulParse = false;
                if (simplified) Write("\n\t\t[->] - Сдвиг от 0 до " + gAlphabet.Length + ": ");
                else Write("\n\t\t[->] - Введите сдвиг для алфавита (число от 0 до " + gAlphabet.Length + "): ");
                UserInput = ReadLine().Replace(" ", "");
                if (int.TryParse(UserInput, out gShift)) //Checking for errors
                {
                    gShift = int.Parse(UserInput);
                    if (gShift >= 0 && gShift <= gAlphabet.Length) SuccessfulParse = true;
                    else
                    {
                        if (simplified) Write("\t\t[!]  - Вне диапозона\n");
                        else Write("\t\t[!]  - Введённой число не попадает в допустимый диапозон. Повторите ввод\n");
                    }
                }
                else
                {
                    if (simplified) Write("[!]  - Некоректный ввод\n");
                    else Write("\t\t[!]  - Некоректный ввод, попробуйте ещё раз\n");
                }
                if (SuccessfulParse && !simplified)
                {
                    Write("\t\t[?]  - Сдвиг введён верно? (Да/Нет | Yes/No): ");
                    UserInput = ReadLine().Trim().ToLower();
                    Confirm = (UserInput == "да" || UserInput == "yes");
                }
            } //Try again if errors ocured
            Write("\n");
        }
             //  Getting a valid shift for the message from the alphabet
             //    - Must     be a number
             //    - Must     be   > 0
             //    - Must     be   < Alphabet length

    }
}