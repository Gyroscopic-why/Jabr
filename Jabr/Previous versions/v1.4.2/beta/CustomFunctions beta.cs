using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static Jabr.CipherSource;
using static System.Console;

namespace Jabr
{
    internal class CustomFunctions
    {
        static public Random   gRandom = new Random();
        static public string   gEncrypted = "", gDecrypted = "";
        static public string   gAlphabet; //Preparation for Gen5 ciphers
        static public int      gShift;
        static public bool     gAdvInfo = false, gSimplified = false, gClearUsed = true, gUseShortcuts = true;
        static public byte     gShortcutError = 1,  gVersion = 4; 
        //For the gVersion (cipher version):   1=RE1   2=RE2   3=RE3   4=RE4   5=RE5  :D
        //Shortcut errors:     0 - shortcut was succesfull
        //                     1 - no signs of shortcuts was found in the user input
        //                     2 - error while reading shortcut
        //
        //I will add more errors in the future split the "2" option into multiple more detailed exceptions


        //-------------------------------------------------------------------------------//
        static public void GetInfo(byte _Type)
        {
            switch (gVersion)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    GetMessage(gSimplified, _Type, gVersion);
                    GetAlphabet(gSimplified, _Type);
                    GetShift(gSimplified);
                    break;
                case 5:         // REserved for later, get the joke? REserved? XD
                    break;
                default:
                    break;
            }
        }


        static public void GetMessage(bool _simplified, byte _type, int _cipherVersion)
        {
            string UserInput;
            bool ConfirmMessage = false;

            Write("\n\n\n");
            if (!_simplified)
            {
                Write("\t\t\t--->  ");
                if (_type == 0) Write("За");
                else Write("Де");
                Write("шифровка сообщений с помощью РЕ" + _cipherVersion + "  <---\n\n");
            }

            while (!ConfirmMessage && !_simplified || _type < 2)
            {
                if (_simplified) Write("\n\t\t[->] - Т");
                else Write("\n\t\t[->] - Введите сообщение которое т");
                if (_type == 0) Write("ребуется закодировать: ");
                else Write("ребуется декодировать: ");
                BackgroundColor = ConsoleColor.Blue;

                //0 = encoding   1 = decoding
                if (_type == 0) gDecrypted = ReadLine(); //Got the message
                else gEncrypted = ReadLine();

                BackgroundColor = ConsoleColor.Black;
                if (!_simplified)
                {
                    Write("\t\t[?]  - Сообщение верно? (Да/Нет | Yes/No): ");
                    UserInput = ReadLine().Trim().ToLower();
                    ConfirmMessage = (UserInput == "да" || UserInput == "yes");
                    if (ConfirmMessage) _type += 2;
                }
                else _type += 2;
            }
            Write("\n");
        }
        static public void GetAlphabet(bool _simplified, byte _type)
        {   //     Type = 0   -> Encrypting        Type = 1   -> Decrypting
            string _message = _type == 0 ? gDecrypted : gEncrypted;
            bool _valid = false;

            while (!_valid) // Parsing the alphabet for encrypting/decrypting
            {
                if (_simplified) Write("\n\t\t[->] - Алфавит: ");                              //
                else                                                                           //
                {                                                                              //
                    Write("\n\t\t[->] - Введите алфавит с помощью которого будет ");           //
                    if (_type == 0) Write("за");                                               //
                    else Write("де");                                                          //
                    Write("шифрованно сообщение: ");                                           //
                }   //                                           Ask the user for the alphabet //
                BackgroundColor = ConsoleColor.Magenta; //
                gAlphabet = ReadLine();                 //
                BackgroundColor = ConsoleColor.Black;   //Got the alphabet

                _valid = CheckAlphabet(gAlphabet, _message, _simplified); // Check the alphabet
            }   //Try again if errors ocured
            Write("\n");
        }
          static public bool CheckAlphabet(string _alphabet, string _message, bool _simplified)
        {
            bool _valid = true;
            string _missing = "", _duplicates = "";


            for (int i = 0; i < _alphabet.Length; i++) //Check for duplicate letters in the alphabet
            {
                if (_alphabet.IndexOf(_alphabet[i]) != i //Check if there is another copy of our char
                    && _duplicates.IndexOf(_alphabet[i]) == -1) _duplicates += _alphabet[i]; //and we havent counted it yet
            }
            if (_duplicates != "") // There is duplicates
            {   // Then write error messages and duplicates info
                if (_simplified) Write("\t\t[!]  - Повторяющиеся буквы: ");
                else Write("\t\t[!]  - Алфавит содержит повторяющиеся буквы: ");
                for (int i = 0; i < _duplicates.Length - 1; i++) Write(_duplicates[i] + ", ");
                Write(_duplicates[_duplicates.Length - 1] + "\n");

                _valid = false; // the alphabet is no longer valid
            }

            if (_simplified)
            {
                for (int i = 0; i < _message.Length; i++) //Search for missing letters
                {
                    if (_alphabet.IndexOf(_message[i]) == -1 //If the char doesnt exist in our alphabet
                        && _missing.IndexOf(_message[i]) == -1) _missing += _message[i]; //And we havent counted it yet
                }
            }
            else
            {
                Write("\t\t[i]  - Номера букв сообщения в алфавите: "); //Write extra info
                for (int i = 0; i < _message.Length; i++) //Search for missing letters
                {
                    Write(_alphabet.IndexOf(_message[i]) + " ");
                    if (_alphabet.IndexOf(_message[i]) == -1 //If the char doesnt exist in our alphabet
                        && _missing.IndexOf(_message[i]) == -1) _missing += _message[i]; //And we havent counted it yet
                }
                Write("\n");
            }
            if (_missing == " ") // If the alphabet is missing the "space" char
            {
                if (_duplicates == "") // If there arent any duplicates
                {
                    _alphabet += " "; // Then we add the "space" char to the alphabet

                    if (_simplified) Write("\t\t       Добавлен пробел");                   //
                    else Write("\t\t       Алфавит будет изменён, для содержания пробела"); //
                    Write("\n\t\t       Новый алфавит: ");                                  //
                                                        // Inform the user about the change //
                    BackgroundColor = ConsoleColor.Magenta;                                 //
                    Write(_alphabet);                                                       //
                    BackgroundColor = ConsoleColor.Black;                                   //
                    Write("\n");                                                            //
                }
                _missing = ""; // All is good, the "space" cahr is no longer missing
            }
            else if (_missing != "") // Check if we are missing any letters/cahr
            {   // if we are missing something
                if (_simplified) Write("\t\t[!]  - Нет необходимых букв: ");                 //
                else Write("\t\t[!]  - В алфавите нет необходимых букв из сообщения: ");     //
                for (int i = 0; i < _missing.Length - 1; i++) Write(_missing[i] + ", ");     //
                Write(_missing[_missing.Length - 1] + "\n"); // Then write the error message //

                _valid = false; // The alphabet is no longer valid
            }
            if (!_simplified && _missing == "" && _duplicates == "")
            {
                Write("\t\t[?]  - Алфавит введён верно? (Да/Нет | Yes/No): "); //
                _message = ReadLine().Trim().ToLower();                        // Ask the user if he agrees to the parsed alphabet
                _valid = (_message == "да" || _message == "yes");              //
            }   // If the user accepted the alphabet or not                    //
            return _valid;
        }
        static public void GetShift(bool _simplified)
        {
            bool Confirm = false, SuccessfulParse = false;
            string UserInput;

            while (!SuccessfulParse || !Confirm && !_simplified)
            {
                SuccessfulParse = false;
                if (_simplified) Write("\n\t\t[->] - Сдвиг от 0 до " + gAlphabet.Length + ": ");
                else Write("\n\t\t[->] - Введите сдвиг для алфавита (число от 0 до " + gAlphabet.Length + "): ");
                UserInput = ReadLine().Replace(" ", "");
                if (int.TryParse(UserInput, out gShift)) //Checking for errors
                {
                    gShift = int.Parse(UserInput);
                    if (gShift >= 0 && gShift <= gAlphabet.Length) SuccessfulParse = true;
                    else
                    {
                        if (_simplified) Write("\t\t[!]  - Вне диапозона\n");
                        else Write("\t\t[!]  - Введённой число не попадает в допустимый диапозон. Повторите ввод\n");
                    }
                }
                else
                {
                    if (_simplified) Write("[!]  - Некоректный ввод\n");
                    else Write("\t\t[!]  - Некоректный ввод, попробуйте ещё раз\n");
                }
                if (SuccessfulParse && !_simplified)
                {
                    Write("\t\t[?]  - Сдвиг введён верно? (Да/Нет | Yes/No): ");
                    UserInput = ReadLine().Trim().ToLower();
                    Confirm = (UserInput == "да" || UserInput == "yes");
                }
            } //Try again if errors ocured
            Write("\n");
        }


        static public void CheckForShortcut(string _userInput)
        {
            if (_userInput.Length > 8 && (_userInput[0] == '+' || _userInput[0] == '-'))
            {   // Check for crypting type ("+" encryption,  "-" decryption) for the initial shortcut recognition

                gShortcutError = 2; 
                // !!! Only needed for error output in my GetTask function,
                // you can easily delete this in your code if you dont have such a system
                //
                // gShortcutError = 2  means that we found an attempt to use a shortcut
                // So we asume the shortcut recognition failed, so we prepare the error message
                // If the shortcut parse is succesfull, we will reasign the gShortcutError value to 0
                //
                // I do the asigning here for optimisation to the error output
                // However, in the future I will split it to get more details why the parsing failed
                
                if (_userInput[1] == '1' || _userInput[1] == '2' || _userInput[1] == '3' || _userInput[1] == '4')
                {   // Check for cipher version, currently supported: RE1, RE2, RE3, RE4

                    int[] _curJointPos = new int[2]; // Find the posID of the joints "::" used in shortcuts

                    _curJointPos[0] = GetJointPosition(_userInput, _userInput.Length - 2); 
                    //Try to find the first joint pos
                    
                    if (_curJointPos[0] != -1) 
                    {   // If the joint was found
                        
                        _curJointPos[1] = GetJointPosition(_userInput, _curJointPos[0] - 1); 
                        //Try to find the second joint

                        if (_curJointPos[1] != -1)
                        {   // If the second joint was found
                            ReadShortcut(_userInput, _curJointPos); // Try to find the shortcut
                        }
                    }
                }
            }
        }
        static public void ReadShortcut(string _shortcut, int[] _jointPos)
        {   // Note that the joints are stored backwards int the array (_jp[0] = joint 2, _jp[1] = joint 1)
            string _expAlphabet = "", _temp = ""; //_temp will temporary hold the expected shift, then the message
            int _expShift;

            for(int i = _jointPos[1] + 2; i < _jointPos[0]; i++)
                _expAlphabet += _shortcut[i]; // Get the alphabet between joint 1 && 2

            for(int i = _jointPos[0] + 2; i < _shortcut.Length; i++)
                _temp += _shortcut[i]; // Get the shift after the second joint
            
            if(int.TryParse(_temp, out _expShift)) // Check if the shift is valid (is an integer)
            {
                if(_expShift < _expAlphabet.Length && _expShift > 0) // Check that the shift is valid
                {   // (RE validation is when the shift for the alphabet is more than 0, and less than the alphabet length)
                    _temp = ""; // The message will be stored in "str _temp" rather than another string for optimisation
                    
                    for (int i = 2; i < _jointPos[1]; i++)
                    {
                        _temp += _shortcut[i]; // Get the message
                    }

                    if (CheckAlphabet(_expAlphabet, _temp, true)) //true = simplified (without extra questions to the user)
                    {   //If the alphabet is valid, then use the shortcut
                        
                        if (_shortcut[0] == '+') Encrypt(byte.Parse(_shortcut[1].ToString()), gAdvInfo, _temp, gEncrypted, _expAlphabet, _expShift);
                        else Decrypt(byte.Parse(_shortcut[1].ToString()), gAdvInfo, gDecrypted, _temp, _expAlphabet, _expShift);
                        
                        gShortcutError = 0;
                        // !!!!! Only needed for error output in my GetTask function,
                        // you can easily delete this in your code if you dont have such a system
                        //
                        // gShortcutError = 0, means that the parsing of a shortcut was succesfull,
                        // therefore no errors was detected
                    }
                }
            }
        }
          static public int GetJointPosition(string _tempString, int _startID)
        {
            int _position = -1; // Propose the joint doesn't exist

            for (int i = _startID; i > 1 && _position == -1; i--)
            {   // start searching for the joint "::" from the end
                if (_tempString[i] == ':' && _tempString[i - 1] == ':') _position = i - 1;
            }   // if the joint "::" was found, return the posID of the first ':' char

            return _position;
        }

        //-------------------------------------------------------------------------------//
          static public short GetUserTask(bool _useShortcuts, bool _clearUsed)
        {
            short _chosenTask = -1;
            string _userInput;

            Write("\n\n\n\t\t[?]  - Что вы хотите сделать?\n");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("\t\t   > 1 <    - О программе\n");
            ForegroundColor = ConsoleColor.White;
            Write("\t\t   > 2 <    - Зашифровать сообщение\n");
            Write("\t\t   > 3 <    - Дешифровать сообщение\n");
            Write("\t\t   > 4 <    - Настройки\n");
            Write("\t\t   > 0 <    - Выход\n");
            if (_useShortcuts) Write("\t\t[i]  - Или можете ввести быструю команду с помощью ключевых символов '+' или '-'\n");

            while (_chosenTask < 0 || _chosenTask > 4)
            {
                Write("\n\t\t[->] - Ваш выбор: ");
                _userInput = ReadLine().Trim();
                if (_useShortcuts) CheckForShortcut(_userInput);
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
                else if (!short.TryParse(_userInput, out _chosenTask))
                {
                    Write("\t\tНе удалось распознать выбор. Пожалуйста, повторите ввод\n");
                    _chosenTask = -1;
                }
                else if (_chosenTask < 0 || _chosenTask > 4) Write("\t\tЧисло не попадает в допустимый диапозон. Пожалуйста, повторите ввод\n");
            }
            if (_clearUsed)
            {
                Clear();
                Write("\n\n\n\t\t\t   Добро пожаловать в Jabr v1.4.2 beta!");
            }
            return _chosenTask;
        }

        static public void ChangeSettings()
        {
            string UserInput = "";

            while (UserInput != "0")
            {
                Write("\n\n\n");
                if (gClearUsed)
                {
                    Clear();
                    Write("\n\n\n\t\t\t   Добро пожаловать в Jabr v1.4.2 beta!\n\n\n");
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
                if (gAdvInfo)
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
                        gAdvInfo = !gAdvInfo;
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
                Write("\n\n\n\t\t\t   Добро пожаловать в Jabr v1.4.2 beta!");
            }
        }

        //-------------------------------------------------------------------------------//
        static public void Encrypt(byte _cipherVersion, bool _showAdvInfo, string _decrypted, string _encrypted, string _alphabet, int _shift)
        {
            var encrypt = new Dictionary<byte, Action>()  {
            { 1, () => _encrypted = E_RE1(_decrypted, _alphabet, _shift) },
            { 2, () => _encrypted = E_RE2(_decrypted, _alphabet, _shift) },
            { 3, () => _encrypted = E_RE3(_decrypted, _alphabet, _shift) },
            { 4, () => _encrypted = E_RE4(_decrypted, _alphabet, _shift) }   };

            var enInfo = new Dictionary<byte, Action>()  {
            { 1, () => E_RE1_Info(_encrypted, _decrypted, _alphabet, _shift) },
            { 2, () => E_RE2_Info(_encrypted, _decrypted, _alphabet, _shift) },
            { 3, () => E_RE3_Info(_encrypted, _decrypted, _alphabet, _shift) },
            { 4, () => E_RE4_Info(_encrypted, _decrypted, _alphabet, _shift) }   };

            encrypt[_cipherVersion]();
            if (_showAdvInfo) enInfo[_cipherVersion]();
            ShowResult(_encrypted, "За", _cipherVersion); // Clean version through var
        }
        static public void Decrypt(byte _cipherVersion, bool _showAdvInfo, string _decrypted, string _encrypted, string _alphabet, int _shift)
        {
            var decrypt = new Dictionary<byte, Action>()  {
            { 1, () => _decrypted = D_RE1(_encrypted, _alphabet, _shift) },
            { 2, () => _decrypted = D_RE2(_encrypted, _alphabet, _shift) },
            { 3, () => _decrypted = D_RE3(_encrypted, _alphabet, _shift) },
            { 4, () => _decrypted = D_RE4(_encrypted, _alphabet, _shift) }   };

            var deInfo = new Dictionary<byte, Action>()  {
            { 1, () => D_RE1_Info(_encrypted, _decrypted, _alphabet, _shift) },
            { 2, () => D_RE2_Info(_encrypted, _decrypted, _alphabet, _shift) },
            { 3, () => D_RE3_Info(_encrypted, _decrypted, _alphabet, _shift) },
            { 4, () => D_RE4_Info(_encrypted, _decrypted, _alphabet, _shift) }   };

            decrypt[_cipherVersion]();
            if (_showAdvInfo) deInfo[_cipherVersion]();
            ShowResult(_decrypted, "Де", _cipherVersion); // Clean version through var
        }

        static public void ShowResult(string _result, string _type, int _cipherVersion)
        {
            Write("\n\t\t[=]  - " + _type + "кодированное с помощью РЕ" + _cipherVersion + " сообщение: ");
            BackgroundColor = ConsoleColor.DarkGreen;
            Write(_result); //Write the result
            BackgroundColor = ConsoleColor.Black;
            Write("\n");
        }
    }
}
