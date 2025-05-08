using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using static System.Console;

namespace CipherTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Title = "CipherTest chamber - v1.4";

            Write("\n\n\n\t\t\t   Добро пожаловать в CipherTest!");
            short OurTask = GetUserTask();

            while (OurTask != 0)
            {
                switch (OurTask)
                {
                    case 2:
                        SimpleEnInfo();
                        Encode();
                        break;

                    case 3:
                        SimpleDeInfo();
                        Decode();
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
        static public int gShift;
        static public bool gAdvInfo = false, gClearUsed = false;
        static public byte gVersion = 3;//1=J10 //2=J13 //3=J14


        static public string En_J10(string Decoded, string Alphabet, int Shift)
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

        static public string De_J10(string Encoded, string Alphabet, int Shift)
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

        static public string En_J13(string Decoded, string Alphabet, int Shift)
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

        static public string De_J13(string Encoded, string Alphabet, int Shift)
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

        static public string En_J14(string Decoded, string Alphabet, int Shift)
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
        
        static public string De_J14(string Encoded, string Alphabet, int Shift)
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

            while (UserInput != "0") {
                if (gClearUsed) 
                {
                    Clear();
                    Write("\n\n\n\t\t\t   Добро пожаловать в CipherTest!\n\n\n");
                    Write("\t\t\t--->  Изменение настроек  <---\n\n");
                }
                else Write("\n\n");

                Write("\t\t[?]  - Изменить версию используемого шифра:\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 1) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 1 <    - РЕ 1.0, Защита: 3/5\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 2) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 2 <    - РЕ 1.3, Защита: 3/5\n");
                ForegroundColor = ConsoleColor.DarkGray;
                if (gVersion == 3) ForegroundColor = ConsoleColor.Green;
                Write("\t\t  > 3 <    - РЕ 1.4, Защита: 4/5    (По умолчанию)\n");
                ForegroundColor = ConsoleColor.White;

                Write("\n\t\t  > 4 <    - Выводить дополнительную информацию о процессе шифрования: ");
                if (gAdvInfo) {
                    ForegroundColor = ConsoleColor.Green;
                    Write("Да");
                } else { 
                    ForegroundColor = ConsoleColor.Red;
                    Write("Нет");
                }
                ForegroundColor = ConsoleColor.White;

                Write("\n\t\t  > 5 <    - Стирать использованную информацию: ");
                if (gClearUsed) {
                    ForegroundColor = ConsoleColor.Green;
                    Write("Да");
                } else { 
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
                        gAdvInfo = !gAdvInfo;
                        break;
                    case "5":
                        gClearUsed = !gClearUsed;
                        break;
                }
            }
            if (gClearUsed)
            {
                Clear();
                Write("\n\n\n\t\t\t   Добро пожаловать в CipherTest!");
            }
        }

        static public void SimpleEnInfo() //Getting encoding info
        {
            string UserInput, AlphabetErrors = "☺", AlphabetDuplicates = "☺";
            bool SuccessfulParse = false;
            gEncoded = "";

            //--------------------------------------------------------------------------------------//
            Write("\n\n\n\t\t\t\t--->  Encoding  <---\n\n");


            Write("\n\t\t[->] - Требуется закодировать: ");
            BackgroundColor = ConsoleColor.Blue;
            gDecoded = ReadLine(); //Got the message
            BackgroundColor = ConsoleColor.Black;

            while (AlphabetDuplicates != "" || AlphabetErrors != "")
            {
                AlphabetDuplicates = "";
                AlphabetErrors = "";
                Write("\n\n\n\t\t[->] - Алфавит: ");
                BackgroundColor = ConsoleColor.Magenta;
                gAlphabet = ReadLine(); //Got the alphabet
                BackgroundColor = ConsoleColor.Black;

                for (int i = 0; i < gAlphabet.Length; i++) //Check for duplicate letters in the alphabet
                {
                    if (gAlphabet.IndexOf(gAlphabet[i]) != i && AlphabetDuplicates.IndexOf(gAlphabet[i]) == -1) AlphabetDuplicates += gAlphabet[i];
                }
                if (AlphabetDuplicates != "") Write("\t\t[!]  - Повторяющиеся буквы\n");


                Write("\t\t[i]  - Номера букв: "); //Write extra info
                for (int i = 0; i < gDecoded.Length; i++) //Search for missing letters in tha alphabet
                {
                    Write(gAlphabet.IndexOf(gDecoded[i]) + " ");
                    if (gAlphabet.IndexOf(gDecoded[i]) == -1 && AlphabetErrors.IndexOf(gDecoded[i]) == -1) AlphabetErrors += gDecoded[i];
                }
                if (AlphabetErrors == " ")
                {
                    if (AlphabetDuplicates == "")
                    {
                        gAlphabet += " ";
                        Write("\n\t\t       Добавлен пробел\n\t\t       Новый алфавит: ");
                        BackgroundColor = ConsoleColor.Magenta;
                        Write(gAlphabet);
                        BackgroundColor = ConsoleColor.Black;
                        Write(" ");
                    }
                    AlphabetErrors = "";
                }
                else if (AlphabetErrors != "") //Write error message
                {
                    AlphabetErrors = AlphabetErrors.Replace(" ", "");
                    Write("\n\t\t[!]  - Нет необходимых букв: ");
                    for (int i = 0; i < AlphabetErrors.Length - 1; i++) Write(AlphabetErrors[i] + ", ");
                    Write(AlphabetErrors[AlphabetErrors.Length - 1]);
                }
            }//Try again if errors ocured
            //--------------------------------------------------------------------------------------//


            //Get the open key
            while (!SuccessfulParse)
            {
                Write("\n\n\n\t\t[->] - Сдвиг от 0 до " + gAlphabet.Length + ": ");
                UserInput = ReadLine().Replace(" ", "");
                if (int.TryParse(UserInput, out gShift)) //Checking for errors
                {
                    gShift = int.Parse(UserInput);
                    if (gShift >= 0 && gShift <= gAlphabet.Length) SuccessfulParse = true;
                    else Write("\t\t[!]  - Вне диапозона. Повторите ввод");
                }
                else Write("\t\t[!]  - Повторите ввод");
            } //Try again if errors ocured
            //--------------------------------------------------------------------------------------//
        }

        static public void SimpleDeInfo() //Getting decoding info
        {
            string UserInput, AlphabetErrors = "☺", AlphabetDuplicates = "☺";
            bool SuccessfulParse = false;
            gDecoded = "";

            //--------------------------------------------------------------------------------------//
            Write("\n\n\n\t\t\t\t--->  Decoding  <---\n\n");
            Write("\n\t\t[->] - Требуется декодировать: ");
            BackgroundColor = ConsoleColor.Blue;
            gEncoded = ReadLine(); //Got the message
            BackgroundColor = ConsoleColor.Black;

            while (AlphabetDuplicates != "" || AlphabetErrors != "")
            {
                AlphabetDuplicates = "";
                AlphabetErrors = "";
                Write("\n\n\n\t\t[->] - Алфавит: ");
                BackgroundColor = ConsoleColor.Magenta;
                gAlphabet = ReadLine(); //Got the alphabet
                BackgroundColor = ConsoleColor.Black;

                for (int i = 0; i < gAlphabet.Length; i++) //Check for duplicate letters in the alphabet
                {
                    if (gAlphabet.IndexOf(gAlphabet[i]) != i && AlphabetDuplicates.IndexOf(gAlphabet[i]) == -1) AlphabetDuplicates += gAlphabet[i];
                }
                if (AlphabetDuplicates != "") Write("\t\t[!]  - Повторяющиеся буквы\n");


                Write("\t\t[i]  - Номера букв: "); //Write extra info
                for (int i = 0; i < gEncoded.Length; i++) //Search for missing letters in tha alphabet
                {
                    Write(gAlphabet.IndexOf(gEncoded[i]) + " ");
                    if (gAlphabet.IndexOf(gEncoded[i]) == -1 && AlphabetErrors.IndexOf(gEncoded[i]) == -1) AlphabetErrors += gEncoded[i];
                }
                if (AlphabetErrors == " ")
                {
                    if (AlphabetDuplicates == "")
                    {
                        gAlphabet += " ";
                        Write("\n\t\t       Добавлен пробел\n\t\t       Новый алфавит: ");
                        BackgroundColor = ConsoleColor.Magenta;
                        Write(gAlphabet);
                        BackgroundColor = ConsoleColor.Black;
                        Write(" ");
                    }
                    AlphabetErrors = "";
                }
                else if (AlphabetErrors != "") //Write error message
                {
                    AlphabetErrors = AlphabetErrors.Replace(" ", "");
                    Write("\n\t\t[!]  - Нет необходимых букв: ");
                    for (int i = 0; i < AlphabetErrors.Length - 1; i++) Write(AlphabetErrors[i] + ", ");
                    Write(AlphabetErrors[AlphabetErrors.Length - 1]);
                }
            }//Try again if errors ocured
            //--------------------------------------------------------------------------------------//

            //Get the open key
            while (!SuccessfulParse)
            {
                Write("\n\n\n\t\t[->] - Сдвиг от 0 до " + gAlphabet.Length + ": ");
                UserInput = ReadLine().Replace(" ", "");
                if (int.TryParse(UserInput, out gShift)) //Checking for errors
                {
                    gShift = int.Parse(UserInput);
                    if (gShift >= 0 && gShift <= gAlphabet.Length) SuccessfulParse = true;
                    else Write("\t\t[!]  - Вне диапозона. Повторите ввод");
                }
                else Write("\t\t[!]  - Повторите ввод");
            } //Try again if errors ocured
            //--------------------------------------------------------------------------------------//
        }

        static public void Encode()
        {
            var Encode = new Dictionary<byte, Action>()  {
            { 1, () => gEncoded = En_J10(gDecoded, gAlphabet, gShift) },
            { 2, () => gEncoded = En_J13(gDecoded, gAlphabet, gShift) },
            { 3, () => gEncoded = En_J14(gDecoded, gAlphabet, gShift) }   };

            var EnInfo = new Dictionary<byte, Action>()  {
            { 1, () => En_J10_Details(gEncoded, gDecoded, gAlphabet, gShift) },
            { 2, () => En_J13_Details(gEncoded, gDecoded, gAlphabet, gShift) },
            { 3, () => En_J14_Details(gEncoded, gDecoded, gAlphabet, gShift) }   };

            Encode[gVersion]();
            if(gAdvInfo) EnInfo[gVersion]();
            ShowResult(gEncoded, "За");
        }

        static public void Decode()
        {
            var Decode = new Dictionary<byte, Action>()  {
            { 1, () => gDecoded = De_J10(gEncoded, gAlphabet, gShift) },
            { 2, () => gDecoded = De_J13(gEncoded, gAlphabet, gShift) },
            { 3, () => gDecoded = De_J14(gEncoded, gAlphabet, gShift) }   };

            var DeInfo = new Dictionary<byte, Action>()  {
            { 1, () => De_J10_Details(gEncoded, gDecoded, gAlphabet, gShift) },
            { 2, () => De_J13_Details(gEncoded, gDecoded, gAlphabet, gShift) },
            { 3, () => De_J14_Details(gEncoded, gDecoded, gAlphabet, gShift) }   };

            Decode[gVersion]();
            if(gAdvInfo) DeInfo[gVersion]();
            ShowResult(gDecoded, "Де");
        }

        static public void ShowResult(string Result, string Type)
        {
            Write("\n\t\t[=]  - " + Type + "кодированное сообщение: ");
            BackgroundColor = ConsoleColor.DarkGreen;
            Write(Result); //Write the result
            BackgroundColor = ConsoleColor.Black;
        }

        static public void En_J10_Details(string Encoded, string Decoded, string Alphabet, int Shift)
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

        static public void De_J10_Details(string Encoded, string Decoded, string Alphabet, int Shift)
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
        
        static public void En_J13_Details(string Encoded, string Decoded, string Alphabet, int Shift)
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

        static public void De_J13_Details(string Encoded, string Decoded, string Alphabet, int Shift)
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

        static public void En_J14_Details(string Encoded, string Decoded, string Alphabet, int Shift)
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

        static public void De_J14_Details(string Encoded, string Decoded, string Alphabet, int Shift)
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
