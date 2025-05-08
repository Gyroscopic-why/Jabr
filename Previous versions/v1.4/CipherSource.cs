using System;
using static Jabr.CustomFunctions;
using static System.Console;

namespace Jabr
{
    internal class CipherSource
    {
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
