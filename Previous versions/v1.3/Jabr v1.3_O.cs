using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Jabr
{
    internal class Program
    {
        static Random RandomNumber = new Random();
        static string OriginalMessageG, EncodedMessageG, DecodedMessageG, AlphabetG;
        static int ShiftCodeG;

        static void Main(string[] args)
        {
            Title = "Jabr - encoder/decoder - v1.3_O";  // Optimised version without dev info

            Write("\n\n\n\t\t\t\tДобро пожаловать в Jabr!");
            short OurTask = GetUserTask();

            while (OurTask != 6)
            {
                switch (OurTask)
                {
                    case 2:
                        GetInfoForEncoding();
                        EncodedMessageG = EncodeWithRE(OriginalMessageG, AlphabetG, ShiftCodeG);
                        //WriteEncodingDetails(OriginalMessageG, EncodedMessageG, AlphabetG, ShiftCodeG);
                        Write("\n\t\t[=]  - Закодированное сообщение: ");
                        BackgroundColor = ConsoleColor.DarkGreen;
                        Write(EncodedMessageG); //Write the result
                        BackgroundColor = ConsoleColor.Black;
                        Write(" ");
                        break;
                    case 3:
                        GetInfoForDecoding();
                        DecodedMessageG = DecodeWithRE(EncodedMessageG, AlphabetG, ShiftCodeG);
                        //WriteDecodingDetails(EncodedMessageG,  DecodedMessageG, AlphabetG, ShiftCodeG);
                        Write("\n\t\t[=]  - Декодированное сообщение: ");
                        BackgroundColor = ConsoleColor.DarkGreen;
                        Write(DecodedMessageG); //Write the result
                        BackgroundColor = ConsoleColor.Black;
                        Write(" ");
                        break;
                    default:
                        break;
                }
                OurTask = GetUserTask();
            }
        }

        static short GetUserTask()
        {
            short ChoosedTask = 0;
            string UserInput;

            Write("\n\n\n\t\t[?]  - Что вы хотите сделать?\n");
            Write("\t\t   > 1 <    - О программе\n");
            Write("\t\t   > 2 <    - Закодировать сообщение\n");
            Write("\t\t   > 3 <    - Декодировать сообщение\n");
            ForegroundColor = ConsoleColor.DarkGray;
            Write("\t\t   > 4 <    - Дополнительные параметры кодирования\n");
            Write("\t\t   > 5 <    - Настройки\n");
            ForegroundColor = ConsoleColor.White;
            Write("\t\t   > 6 <    - Выход\n");

            while (ChoosedTask < 1 || ChoosedTask > 6)
            {
                Write("\n\t\t[->] - Ваш выбор: ");
                UserInput = ReadLine().Trim();
                if (!short.TryParse(UserInput, out ChoosedTask)) Write("\t\tНе удалось распознать выбор. Пожалуйста, повторите ввод\n");
                else
                {
                    ChoosedTask = short.Parse(UserInput);
                    if (ChoosedTask < 1 || ChoosedTask > 6) Write("\t\tЧисло не попадает в допустимый диапозон. Пожалуйста, повторите ввод\n");
                }
            }

            Clear();
            Write("\n\n\n\t\t\t\tДобро пожаловать в Jabr!\n\n\n");

            return ChoosedTask;
        }

        static void GetInfoForEncoding() //Getting encoding info
        {
            string UserInput, AlphabetErrors = "☺", AlphabetDuplicates = "☺";
            bool SuccessfulParse = false, ConfirmMessage = false;

            OriginalMessageG = "";
            AlphabetG = "";
            EncodedMessageG = "";
            ShiftCodeG = 0;
            
            //--------------------------------------------------------------------------------------//
            Write("\t\t\t--->  Encoding a message using RE methode  <---\n\n");
            

            while(!ConfirmMessage)
            { //Try to get the message and the alphabet
                Write("\n\t\t[->] - Введите сообщение которое требуется закодировать: ");
                BackgroundColor = ConsoleColor.Blue;
                OriginalMessageG = ReadLine(); //Got the message
                BackgroundColor = ConsoleColor.Black;

                Write("\t\t[?]  - Сообщение верно? (Да/Нет | Yes/No): ");
                UserInput = ReadLine().Trim().ToLower();
                ConfirmMessage = (UserInput == "да" || UserInput == "yes");
            }

            while (AlphabetDuplicates != "" || AlphabetErrors != "")
            {
                AlphabetDuplicates = "";
                AlphabetErrors = "";
                Write("\n\n\n\t\t[->] - Введите алфавит с помощью которого будет закодированно сообщение: ");
                BackgroundColor = ConsoleColor.Magenta;
                AlphabetG = ReadLine(); //Got the alphabet
                BackgroundColor = ConsoleColor.Black;

                for (int i = 0; i < AlphabetG.Length; i++) //Check for duplicate letters in the alphabet
                {
                    if (AlphabetG.IndexOf(AlphabetG[i]) != i && AlphabetDuplicates.IndexOf(AlphabetG[i]) == -1) AlphabetDuplicates += AlphabetG[i];
                }
                if (AlphabetDuplicates != "") Write("\t\t[!]  - Алфавит содержит повторяющиеся буквы, повторите ввод.\n");


                Write("\t\t[i]  - Номера букв сообщения в алфавите: "); //Write extra info
                for (int i = 0; i < OriginalMessageG.Length; i++) //Search for missing letters in tha alphabet
                {
                    Write(AlphabetG.IndexOf(OriginalMessageG[i]) + " ");
                    if (AlphabetG.IndexOf(OriginalMessageG[i]) == -1 && AlphabetErrors.IndexOf(OriginalMessageG[i]) == -1) AlphabetErrors += OriginalMessageG[i];
                }
                if (AlphabetErrors == " ")
                {
                    if (AlphabetDuplicates == "")
                    {
                        AlphabetG += " ";
                        Write("\n\t\t       Алфавит будет изменён, для содержания пробела\n\t\t       Новый алфавит: ");
                        BackgroundColor = ConsoleColor.Magenta;
                        Write(AlphabetG);
                        BackgroundColor = ConsoleColor.Black;
                        Write(" ");
                    }
                    AlphabetErrors = "";
                }
                else if (AlphabetErrors != "") //Write error message
                {
                    AlphabetErrors = AlphabetErrors.Replace(" ", "");
                    Write("\n\t\t[!]  - В алфавите нет необходимых букв из сообщения: ");
                    for (int i = 0; i < AlphabetErrors.Length - 1; i++) Write(AlphabetErrors[i] + ", ");
                    Write(AlphabetErrors[AlphabetErrors.Length - 1]);
                }
            }//Try again if errors ocured
            //--------------------------------------------------------------------------------------//


            //Get the open key
            while (!SuccessfulParse)
            {
                Write("\n\n\n\t\t[->] - Введите сдвиг для алфавита (число от 0 до " + AlphabetG.Length + "): ");
                UserInput = ReadLine().Replace(" ", "");
                if (int.TryParse(UserInput, out ShiftCodeG)) //Checking for errors
                {
                    ShiftCodeG = int.Parse(UserInput);
                    if (ShiftCodeG >= 0 && ShiftCodeG <= AlphabetG.Length) SuccessfulParse = true;
                    else Write("\t\t[!]  - Введённой число не попадает в допустимый диапозон. Повторите вводho");
                }
                else Write("\t\t[!]  - Некорректный ввод, пожалуйста, повторите попытку");
            } //Try again if errors ocured
            //--------------------------------------------------------------------------------------//
        }

        static void GetInfoForDecoding() //Getting decoding info
        {
            string UserInput, AlphabetErrors = "☺", AlphabetDuplicates = "☺";
            bool SuccessfulParse = false, ConfirmMessage = false;

            DecodedMessageG = "";
            AlphabetG = "";
            EncodedMessageG = "";
            ShiftCodeG = 0;
            
            //--------------------------------------------------------------------------------------//
            Write("\t\t\t--->  Decoding a message using RE methode  <---\n\n");
                        
            while (!ConfirmMessage)
            { //Try to get the message and the alphabet
                Write("\n\t\t[->] - Введите сообщение которое требуется декодировать: ");
                BackgroundColor = ConsoleColor.Blue;
                EncodedMessageG = ReadLine(); //Got the message
                BackgroundColor = ConsoleColor.Black;

                Write("\t\t[?]  - Сообщение верно? (Да/Нет | Yes/No): ");
                UserInput = ReadLine().Trim().ToLower();
                ConfirmMessage = (UserInput == "да" || UserInput == "yes");
            }

            while (AlphabetDuplicates != "" || AlphabetErrors != "")
            {
                AlphabetDuplicates = "";
                AlphabetErrors = "";
                Write("\n\n\n\t\t[->] - Введите алфавит с помощью которого будет декодированно сообщение: ");
                BackgroundColor = ConsoleColor.Magenta;
                AlphabetG = ReadLine(); //Got the alphabet
                BackgroundColor = ConsoleColor.Black;

                for (int i = 0; i < AlphabetG.Length; i++) //Check for duplicate letters in the alphabet
                {
                    if (AlphabetG.IndexOf(AlphabetG[i]) != i && AlphabetDuplicates.IndexOf(AlphabetG[i]) == -1) AlphabetDuplicates += AlphabetG[i];
                }
                if (AlphabetDuplicates != "") Write("\t\t[!]  - Алфавит содержит повторяющиеся буквы, повторите ввод.\n");


                Write("\t\t[i]  - Номера букв сообщения в алфавите: "); //Write extra info
                for (int i = 0; i < EncodedMessageG.Length; i++) //Search for missing letters in tha alphabet
                {
                    Write(AlphabetG.IndexOf(EncodedMessageG[i]) + " ");
                    if (AlphabetG.IndexOf(EncodedMessageG[i]) == -1 && AlphabetErrors.IndexOf(EncodedMessageG[i]) == -1) AlphabetErrors += EncodedMessageG[i];
                }
                if (AlphabetErrors == " ")
                {
                    if (AlphabetDuplicates == "")
                    {
                        AlphabetG += " ";
                        Write("\n\t\t       Алфавит будет изменён, для содержания пробела\n\t\t       Новый алфавит: ");
                        BackgroundColor = ConsoleColor.Magenta;
                        Write(AlphabetG);
                        BackgroundColor = ConsoleColor.Black;
                        Write(" ");
                    }
                    AlphabetErrors = "";
                }
                else if (AlphabetErrors != "") //Write error message
                {
                    AlphabetErrors = AlphabetErrors.Replace(" ", "");
                    Write("\n\t\t[!]  - В алфавите нет необходимых букв из сообщения: ");
                    for (int i = 0; i < AlphabetErrors.Length - 1; i++) Write(AlphabetErrors[i] + ", ");
                    Write(AlphabetErrors[AlphabetErrors.Length - 1]);
                }
            }//Try again if errors ocured
            //--------------------------------------------------------------------------------------//

            //Get the open key
            while (!SuccessfulParse)
            {
                Write("\n\n\n\t\t[->] - Введите сдвиг для алфавита (число от 0 до " + AlphabetG.Length + "): ");
                UserInput = ReadLine().Replace(" ", "");
                if (int.TryParse(UserInput, out ShiftCodeG)) //Checking for errors
                {
                    ShiftCodeG = int.Parse(UserInput);
                    if (ShiftCodeG >= 0 && ShiftCodeG <= AlphabetG.Length) SuccessfulParse = true;
                    else Write("\t\t[!]  - Введённой число не попадает в допустимый диапозон. Повторите вводho");
                }
                else Write("\t\t[!]  - Некорректный ввод, пожалуйста, повторите попытку");
            } //Try again if errors ocured
            //--------------------------------------------------------------------------------------//
        }

        /*static void WriteEncodingDetails(string OriginalMessage, string EncodedMessage, string Alphabet, int ShiftCode)
        {
            Write("\t\t[i]  - "); //Write info on the first encoded character of the message
            Write(((Alphabet.IndexOf(OriginalMessage[0]) + ShiftCode) % Alphabet.Length) + "-" + EncodedMessage[0] + "  ");

            for (int i = 1; i < OriginalMessage.Length; i++) 
            {   //Write info on the rest of the encoded message
                Write((Alphabet.IndexOf(OriginalMessage[i]) + "+" + Alphabet.IndexOf(EncodedMessage[i - 1])) + "/");
                Write((Alphabet.IndexOf(OriginalMessage[i]) + Alphabet.IndexOf(EncodedMessage[i - 1])) % Alphabet.Length + "-");
                if (EncodedMessage[i] != ' ') Write(EncodedMessage[i] + "  ");
                else Write("Space  ");
            }

            Write("\n\t\t[=]  - Закодированное сообщение: ");
            BackgroundColor = ConsoleColor.DarkGreen;
            Write(EncodedMessage); //Write the result
            BackgroundColor = ConsoleColor.Black;
            Write(" ");
        }*/

        /*static void WriteDecodingDetails(string EncodedMessage, string DecodedMessage, string Alphabet, int ShiftCode)
        {
            Write("\t\t[i]  - "); //Write info about the first decoded character of the message
            Write(Alphabet.IndexOf(Alphabet[(Alphabet.IndexOf(EncodedMessage[0]) - ShiftCode + Alphabet.Length * 2) % Alphabet.Length]) + "-" + DecodedMessage[0] + "  ");

            for (int i = 1; i < EncodedMessage.Length; i++) 
            {   //Write info about the rest of the decoded message
                Write(Alphabet.IndexOf(Alphabet[(Alphabet.IndexOf(EncodedMessage[i]) - Alphabet.IndexOf(EncodedMessage[i - 1]) + Alphabet.Length * (i + 2)) % Alphabet.Length]) + "-");
                if (EncodedMessage[i] != ' ') Write(EncodedMessage[i] + "  ");
                else Write("Space  ");
            }

            Write("\n\t\t[=]  - Декодированное сообщение: ");
            BackgroundColor = ConsoleColor.DarkGreen;
            Write(DecodedMessage); //Write the result
            BackgroundColor = ConsoleColor.Black;
            Write(" ");
        }*/

        static string EncodeWithRE(string OriginalMessage, string Alphabet, int ShiftCode)
        {
            string EncodedMessage = "" ;

            EncodedMessage += Alphabet[(Alphabet.IndexOf(OriginalMessage[0]) + ShiftCode) % Alphabet.Length];
            for (int i = 1; i < OriginalMessage.Length; i++) //Encode the rest of the message
            {
                EncodedMessage += Alphabet[(Alphabet.IndexOf(OriginalMessage[i]) + Alphabet.IndexOf(EncodedMessage[i - 1])) % Alphabet.Length];
            }
            return EncodedMessage;
        }

        static string DecodeWithRE(string EncodedMessage, string Alphabet, int ShiftCode)
        {
            string DecodedMessage = "";

            DecodedMessage += Alphabet[Alphabet.IndexOf(Alphabet[(Alphabet.IndexOf(EncodedMessage[0]) - ShiftCode + Alphabet.Length * 2) % Alphabet.Length])];
            for (int i = 1; i < EncodedMessage.Length; i++) //Decode the rest of the message
            {
                DecodedMessage += Alphabet[Alphabet.IndexOf(Alphabet[(Alphabet.IndexOf(EncodedMessage[i]) - Alphabet.IndexOf(EncodedMessage[i - 1]) + Alphabet.Length * (i + 2)) % Alphabet.Length])];
            }
            return DecodedMessage;
        }
    }
}
