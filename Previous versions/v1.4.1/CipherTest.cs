using System;
using System.Collections.Generic;
using static System.Console;

namespace CipherTest
{
    internal class Program
    {
        static void Main()
        {
            Title = "CipherTest chamber - v1.4.1";

            Write("\n\n\n\t\t\t   Добро пожаловать в CipherTest!");
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


        static public Random gRandom = new Random();
        static public string gEncoded, gDecoded, gAlphabet;
        static public int  gShift;
        static public bool gSimplified = true, gAdvInfo = true, gClearUsed = false;
        static public byte gVersion = 4; //  1=RE10    3=RE13    4=RE14    5=RE15


        static public string En_RE10(string Decoded, string Alphabet, int Shift)
        {
            string Encoded = "";

            //Encode the first character
            Encoded += Alphabet[(Alphabet.IndexOf(Decoded[0]) + Shift) % Alphabet.Length];

            for (int i = 1; i < Decoded.Length; i++) //Encode the rest of the message
            {
                Encoded += Alphabet[(Alphabet.IndexOf(Decoded[i]) + 1 + Alphabet.IndexOf(Encoded[i - 1])) % Alphabet.Length];
            }
            return Encoded;
        }
        static public string De_RE10(string Encoded, string Alphabet, int Shift)
        {
            string Decoded = "";

            //Decode the first character
            Decoded += Alphabet[Alphabet.IndexOf(Alphabet[(Alphabet.IndexOf(Encoded[0]) - Shift + Alphabet.Length * 3) % Alphabet.Length])];
            
            for (int i = 1; i < Encoded.Length; i++) //Decode the rest of the message
            {
                Decoded += Alphabet[Alphabet.IndexOf(Alphabet[(Alphabet.IndexOf(Encoded[i]) - 1 - Alphabet.IndexOf(Encoded[i - 1]) + Alphabet.Length * (i + 2)) % Alphabet.Length])];
            }
            return Decoded;
        }

        static public string En_RE13(string Decoded, string Alphabet, int Shift)
        {
            string Encoded = "";

            //Encode the first character
            Encoded += Alphabet[(Alphabet.IndexOf(Decoded[0]) + Shift) % Alphabet.Length];
            
            for (int i = 1; i < Decoded.Length; i++) //Encode the rest of the message
            {
                Encoded += Alphabet[(Alphabet.IndexOf(Decoded[i]) + Alphabet.IndexOf(Encoded[i - 1])) % Alphabet.Length];
            }
            return Encoded;
        }
        static public string De_RE13(string Encoded, string Alphabet, int Shift)
        {
            string Decoded = "";

            //Decode the first character
            Decoded += Alphabet[Alphabet.IndexOf(Alphabet[(Alphabet.IndexOf(Encoded[0]) - Shift + Alphabet.Length * 2) % Alphabet.Length])];
            
            for (int i = 1; i < Encoded.Length; i++) //Decode the rest of the message
            {
                Decoded += Alphabet[Alphabet.IndexOf(Alphabet[(Alphabet.IndexOf(Encoded[i]) - Alphabet.IndexOf(Encoded[i - 1]) + Alphabet.Length * (i + 2)) % Alphabet.Length])];
            }
            return Decoded;
        }

        static public string En_RE14(string Decoded, string Alphabet, int Shift)
        {
            string Encoded = "";
            int[] EnID = new int[Decoded.Length];

            EnID[0] = Alphabet.IndexOf(Decoded[0]); //Encode the first character
            Encoded += Alphabet[(EnID[0] + Shift) % Alphabet.Length];

            for (int i = 1; i < Decoded.Length; i++) //Encode the rest of the message
            {
                EnID[i] = Alphabet.IndexOf(Decoded[i]) + EnID[i - 1];
                Encoded += Alphabet[(EnID[i] + Shift * (i % 2)) % Alphabet.Length];
            }
            return Encoded;
        }
        static public string De_RE14(string Encoded, string Alphabet, int Shift)
        {
            string Decoded = "";
            int[] DeID = new int[Encoded.Length];

            DeID[0] += Alphabet.IndexOf(Encoded[0]); //Decode the first character
            Decoded += Alphabet[(DeID[0] - Shift + Alphabet.Length) % Alphabet.Length];

            DeID[1] = Alphabet.IndexOf(Encoded[1]) - Alphabet.IndexOf(Encoded[0]); //Decode the second 
            Decoded += Alphabet[((DeID[1] + Alphabet.Length) % Alphabet.Length)]; //     character

            for (int i = 2; i < Encoded.Length; i++) //Decode the rest of the message
            {
                DeID[i] = Alphabet.IndexOf(Encoded[i]) - Alphabet.IndexOf(Encoded[i - 1]) + Shift - Shift * 2 * (i % 2);
                Decoded += Alphabet[((DeID[i] + Alphabet.Length * 2) % Alphabet.Length)];
            }
            return Decoded;
        }

        //-------------------------------------------------------------------------------//
        static public short GetUserTask()
        {
            short ChosenTask = -1;
            string UserInput;

            Write("\n\n\n\t\t[?]  - Что вы хотите сделать?\n");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("\t\t   > 1 <    - О программе\n");
            ForegroundColor = ConsoleColor.White;
            Write("\t\t   > 2 <    - Закодировать сообщение\n");
            Write("\t\t   > 3 <    - Декодировать сообщение\n");
            Write("\t\t   > 4 <    - Настройки\n");
            Write("\t\t   > 0 <    - Выход\n");

            while (ChosenTask < 0 || ChosenTask > 4)
            {
                Write("\n\t\t[->] - Ваш выбор: ");
                UserInput = ReadLine().Trim();
                if (!short.TryParse(UserInput, out ChosenTask))
                {
                    Write("\t\tНе удалось распознать выбор. Пожалуйста, повторите ввод\n");
                    ChosenTask = -1;
                }
                else
                {
                    ChosenTask = short.Parse(UserInput);
                    if (ChosenTask < 0 || ChosenTask > 4) Write("\t\tЧисло не попадает в допустимый диапозон. Пожалуйста, повторите ввод\n");
                }
            }
            if (gClearUsed)
            {
                Clear();
                Write("\n\n\n\t\t\t   Добро пожаловать в CipherTest!");
            }
            return ChosenTask;
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
                    Write("\n\n\n\t\t\t   Добро пожаловать в CipherTest!\n\n\n");
                    Write("\t\t\t--->  Изменение настроек  <---\n\n");
                }

                Write("\t\t[?]  - Изменить версию используемого шифра:\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 1) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 1 <    - РЕ 1.0, Защита: 3/5\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 3) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 3 <    - РЕ 1.3, Защита: 3/5\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 4) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 4 <    - РЕ 1.4, Защита: 4/5    (По умолчанию)\n");
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

                Write("\n\t\t  > 0 <    - Назад");

                Write("\n\n\t\tВаш выбор: ");
                UserInput = ReadLine().Trim();
                switch (UserInput)
                {
                    case "1":
                        gVersion = 1;
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
                }
            }
            if (gClearUsed)
            {
                Clear();
                Write("\n\n\n\t\t\t   Добро пожаловать в Jabr!");
            }
        }

        //-------------------------------------------------------------------------------//
        static public void Encode(byte Version, bool AdvInfo)
        {
            var Encode = new Dictionary<byte, Action>()  {
            { 1, () => gEncoded = En_RE10(gDecoded, gAlphabet, gShift) },
            { 3, () => gEncoded = En_RE13(gDecoded, gAlphabet, gShift) },
            { 4, () => gEncoded = En_RE14(gDecoded, gAlphabet, gShift) }   };

            var EnInfo = new Dictionary<byte, Action>()  {
            { 1, () => En_RE10_Details(gEncoded, gDecoded, gAlphabet, gShift) },
            { 3, () => En_RE13_Details(gEncoded, gDecoded, gAlphabet, gShift) },
            { 4, () => En_RE14_Details(gEncoded, gDecoded, gAlphabet, gShift) }   };

            Encode[Version]();
            if(AdvInfo) EnInfo[Version]();
            ShowResult(gEncoded, "За");
        }
        static public void Decode(byte Version, bool AdvInfo)
        {
            var Decode = new Dictionary<byte, Action>()  {
            { 1, () => gDecoded = De_RE10(gEncoded, gAlphabet, gShift) },
            { 3, () => gDecoded = De_RE13(gEncoded, gAlphabet, gShift) },
            { 4, () => gDecoded = De_RE14(gEncoded, gAlphabet, gShift) }   };

            var DeInfo = new Dictionary<byte, Action>()  {
            { 1, () => De_RE10_Details(gEncoded, gDecoded, gAlphabet, gShift) },
            { 3, () => De_RE13_Details(gEncoded, gDecoded, gAlphabet, gShift) },
            { 4, () => De_RE14_Details(gEncoded, gDecoded, gAlphabet, gShift) }   };

            Decode[Version]();
            if(AdvInfo) DeInfo[Version]();
            ShowResult(gDecoded, "Де");
        }

        static public void ShowResult(string Result, string Type)
        {
            Write("\n\t\t[=]  - " + Type + "кодированное сообщение: ");
            BackgroundColor = ConsoleColor.DarkGreen;
            Write(Result); //Write the result
            BackgroundColor = ConsoleColor.Black;
            Write("\n");
        }

        //-------------------------------------------------------------------------------//
        static public void GetInfo(byte _Type)
        {
            switch (gVersion)
            {
                case 1:
                case 3:
                case 4:
                    GetMessage(gSimplified, _Type);
                    GetAlphabet(gSimplified, _Type);
                    GetShift(gSimplified);
                    break;
                case 5:
                    break;
                default:
                    break;
            }
        }

        static public void GetMessage(bool Simplified, byte _Type) 
        {
            string UserInput;
            bool ConfirmMessage = false;

            Write("\n\n\n");
            if (!Simplified)
            {
                Write("\t\t\t--->  ");
                if (_Type == 0) Write("За");
                else Write("Де");
                Write("шифровка сообщений с помощью РЕ ");
                switch (gVersion)
                {
                    case 1:
                        Write("1.0");
                        break;
                    case 3:
                        Write("1.3");
                        break;
                    case 4:
                        Write("1.4");
                        break;
                    case 5:
                        Write("1.5");
                        break;
                }
                Write("  <---\n\n");
            }

            while (!ConfirmMessage && !Simplified || _Type < 2) 
            { 
                if (Simplified) Write("\n\t\t[->] - Т");
                else Write("\n\t\t[->] - Введите сообщение которое т");
                if (_Type == 0) Write("ребуется закодировать: ");
                else Write("ребуется декодировать: ");
                BackgroundColor = ConsoleColor.Blue;

                //0 = encoding   1 = decoding
                if (_Type == 0) gDecoded = ReadLine(); //Got the message
                else gEncoded = ReadLine();
                
                BackgroundColor = ConsoleColor.Black;
                if (!Simplified)
                {
                    Write("\t\t[?]  - Сообщение верно? (Да/Нет | Yes/No): ");
                    UserInput = ReadLine().Trim().ToLower();
                    ConfirmMessage = (UserInput == "да" || UserInput == "yes");
                    if (ConfirmMessage) _Type += 2;
                }
                else _Type += 2;
            }
            Write("\n");
        }
        static public void GetAlphabet(bool Simplified, byte _Type)
        {   //     Coding = 0 -> Encoding    Coding = 1 -> Decoding
            string Missing = "☺", Duplicates = "☺";
            string Message;
            bool Confirm = false;

            while (Missing != "" || Duplicates != "" || !Confirm && !Simplified)
            {
                Message = _Type == 0 ? gDecoded : gEncoded;
                Duplicates = "";
                Missing = "";

                if (Simplified) Write("\n\t\t[->] - Алфавит: ");
                else
                {
                    Write("\n\t\t[->] - Введите алфавит с помощью которого будет ");
                    if (_Type == 0) Write("за");
                    else Write("де");
                    Write("кодированно сообщение: ");
                }
                BackgroundColor = ConsoleColor.Magenta;
                gAlphabet = ReadLine(); //Got the alphabet
                BackgroundColor = ConsoleColor.Black;

                for (int i = 0; i < gAlphabet.Length; i++) //Check for duplicate letters in the alphabet
                {
                    if (gAlphabet.IndexOf(gAlphabet[i]) != i 
                        && Duplicates.IndexOf(gAlphabet[i]) == -1) Duplicates += gAlphabet[i];
                }
                if (Duplicates != "")
                {
                    if(Simplified) Write("\t\t[!]  - Повторяющиеся буквы: ");
                    else Write("\t\t[!]  - Алфавит содержит повторяющиеся буквы: ");
                    for (int i = 0; i < Duplicates.Length - 1; i++) Write(Duplicates[i] + ", ");
                    Write(Duplicates[Duplicates.Length - 1] + "\n");
                }

                if (Simplified)
                {
                    for (int i = 0; i < Message.Length; i++) //Search for missing letters
                    {
                        if (gAlphabet.IndexOf(Message[i]) == -1
                            && Missing.IndexOf(Message[i]) == -1) Missing += Message[i];
                    }
                }
                else
                {
                    Write("\t\t[i]  - Номера букв сообщения в алфавите: "); //Write extra info
                    for (int i = 0; i < Message.Length; i++) //Search for missing letters
                    {
                        Write(gAlphabet.IndexOf(Message[i]) + " ");
                        if (gAlphabet.IndexOf(Message[i]) == -1
                            && Missing.IndexOf(Message[i]) == -1) Missing += Message[i];
                    }
                    Write("\n");
                }
                if (Missing == " ")
                {
                    if (Duplicates == "")
                    {
                        gAlphabet += " ";

                        if (Simplified) Write("\t\t       Добавлен пробел");
                        else Write("\t\t       Алфавит будет изменён, для содержания пробела");
                        Write("\n\t\t       Новый алфавит: ");

                        BackgroundColor = ConsoleColor.Magenta;
                        Write(gAlphabet);
                        BackgroundColor = ConsoleColor.Black;
                        Write("\n");
                    }
                    Missing = "";  
                }
                else if (Missing != "") //Write error message
                {
                    Missing = Missing.Replace(" ", "");
                    if(Simplified) Write("\t\t[!]  - Нет необходимых букв: ");
                    else Write("\t\t[!]  - В алфавите нет необходимых букв из сообщения: ");
                    for (int i = 0; i < Missing.Length - 1; i++) Write(Missing[i] + ", ");
                    Write(Missing[Missing.Length - 1] + "\n");
                }
                if(!Simplified && Missing == "" && Duplicates == "")
                {
                    Write("\t\t[?]  - Алфавит введён верно? (Да/Нет | Yes/No): ");
                    Message = ReadLine().Trim().ToLower();
                    Confirm = (Message == "да" || Message == "yes");
                }
            }//Try again if errors ocured
            Write("\n");
        }
        static public void GetShift(bool Simplified)
        {
            bool Confirm = false, SuccessfulParse = false;
            string UserInput;

            while (!SuccessfulParse || !Confirm && !Simplified)
            {
                SuccessfulParse = false;
                if (Simplified) Write("\n\t\t[->] - Сдвиг от 0 до " + gAlphabet.Length + ": ");
                else Write("\n\t\t[->] - Введите сдвиг для алфавита (число от 0 до " + gAlphabet.Length + "): ");
                UserInput = ReadLine().Replace(" ", "");
                if (int.TryParse(UserInput, out gShift)) //Checking for errors
                {
                    gShift = int.Parse(UserInput);
                    if (gShift >= 0 && gShift <= gAlphabet.Length) SuccessfulParse = true;
                    else
                    {
                        if (Simplified) Write("\t\t[!]  - Вне диапозона\n");
                        else Write("\t\t[!]  - Введённой число не попадает в допустимый диапозон. Повторите ввод\n");
                    }
                }
                else
                {
                    if (Simplified) Write("[!]  - Некоректный ввод\n");
                    else Write("\t\t[!]  - Некоректный ввод, попробуйте ещё раз\n");
                }
                if (SuccessfulParse && !Simplified)
                {
                    Write("\t\t[?]  - Сдвиг введён верно? (Да/Нет | Yes/No): ");
                    UserInput = ReadLine().Trim().ToLower();
                    Confirm = (UserInput == "да" || UserInput == "yes");
                }
            } //Try again if errors ocured
            Write("\n");
        }

        //-------------------------------------------------------------------------------//
        static public void En_RE10_Details(string Encoded, string Decoded, string Alphabet, int Shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first encoded character
            Write((Alphabet.IndexOf(Decoded[0]) + 1) + "+" + Shift + "=" + (Alphabet.IndexOf(Encoded[0]) + 1) + "/" + Encoded[0] + "  ");

            for (int i = 1; i < Decoded.Length; i++)
            {   //Write info on the rest of the encoded message
                Write(((Alphabet.IndexOf(Decoded[i]) + 1) + "+" + Alphabet.IndexOf(Encoded[i - 1])) + "=" + (Alphabet.IndexOf(Encoded[i]) + 1) + "/");
                if (Encoded[i] != ' ') Write(Encoded[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + Alphabet.Length + ")");
        }
        static public void De_RE10_Details(string Encoded, string Decoded, string Alphabet, int Shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first decoded character
            Write(Alphabet.IndexOf(Encoded[0]) + "-" + Shift + "=" + (Alphabet.IndexOf(Decoded[0]) + 1) + "/");
            if (Decoded[0] != ' ') Write(Decoded[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < Encoded.Length; i++)
            {   //Write info about the rest of the decoded message
                Write(Alphabet.IndexOf(Encoded[i]) + "-" + Alphabet.IndexOf(Encoded[i - 1]) + "=" + (Alphabet.IndexOf(Decoded[i]) + 1) + "/");
                if (Decoded[i] != ' ') Write(Decoded[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + Alphabet.Length + ")");
        }

        static public void En_RE13_Details(string Encoded, string Decoded, string Alphabet, int Shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first encoded character
            Write(Alphabet.IndexOf(Decoded[0]) + "+" + Shift + "=" + Alphabet.IndexOf(Encoded[0]) + "/" + Encoded[0] + "  ");

            for (int i = 1; i < Decoded.Length; i++)
            {   //Write info on the rest of the encoded message
                Write((Alphabet.IndexOf(Decoded[i]) + "+" + Alphabet.IndexOf(Encoded[i - 1])) + "=");
                Write((Alphabet.IndexOf(Decoded[i]) + Alphabet.IndexOf(Encoded[i - 1])) % Alphabet.Length + "/");
                if (Encoded[i] != ' ') Write(Encoded[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + Alphabet.Length + ")");
        }
        static public void De_RE13_Details(string Encoded, string Decoded, string Alphabet, int Shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first decoded character
            Write(Alphabet.IndexOf(Encoded[0]) + "-" + Shift + "=" + Alphabet.IndexOf(Decoded[0]) + "/");
            if (Decoded[0] != ' ') Write(Decoded[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < Encoded.Length; i++)
            {   //Write info about the rest of the decoded message
                Write(Alphabet.IndexOf(Encoded[i]) + "-" + Alphabet.IndexOf(Encoded[i - 1]) + "=" + Alphabet.IndexOf(Decoded[i]) + "/");
                if (Decoded[i] != ' ') Write(Decoded[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + Alphabet.Length + ")");
        }

        static public void En_RE14_Details(string Encoded, string Decoded, string Alphabet, int Shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first decoded character
            Write((Alphabet.IndexOf(Decoded[0]) - Shift) + "+" + Shift + "=" + Alphabet.IndexOf(Decoded[0]) + "/");
            if (Encoded[0] != ' ') Write(Encoded[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < Decoded.Length; i++) //Write info about the rest of the decoded message
            {
                Write((Alphabet.IndexOf(Decoded[i]) - Shift * (i % 2)) + "+" + Shift * (i % 2) + "=" + Alphabet.IndexOf(Decoded[i]) + "/");
                if (Encoded[i] != ' ') Write(Encoded[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + Alphabet.Length + ")");
        }
        static public void De_RE14_Details(string Encoded, string Decoded, string Alphabet, int Shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first encoded character
            Write(Alphabet.IndexOf(Encoded[0]) + "-" + Shift + "=" + Alphabet.IndexOf(Decoded[0]) + "/");
            if (Decoded[0] != ' ') Write(Decoded[0] + "  ");
            else Write("Space  ");

            Write(Alphabet.IndexOf(Encoded[1]) + "-" + Alphabet.IndexOf(Encoded[0]) + "=" + Alphabet.IndexOf(Decoded[1]) + "/");
            if (Decoded[1] != ' ') Write(Decoded[1] + "  ");
            else Write("Space  "); //Write info on the second encoded character of the message

            for (int i = 2; i < Encoded.Length; i++)
            { //Write info on the rest of the message
                Write(Alphabet.IndexOf(Encoded[i]) + "-" + Alphabet.IndexOf(Encoded[i - 1]) + "+" + Shift + "-" + (Shift * 2 * (i % 2)) + "=" + Alphabet.IndexOf(Decoded[i]) + "/");
                if (Decoded[i] != ' ') Write(Decoded[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + Alphabet.Length + ")");
        }
    }
}
