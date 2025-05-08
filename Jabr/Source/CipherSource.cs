

using static System.Console;


namespace Jabr
{
    internal class CipherSource
    {
        static public string ERE1(string decrypted, string alphabet, int shift)
        {
            string encrypted = "";

            //Encrypt the first character
            encrypted += alphabet[(alphabet.IndexOf(decrypted[0]) + shift) % alphabet.Length];

            for (int i = 1; i < decrypted.Length; i++) //Encrypt the rest of the message
            {
                encrypted += alphabet[(alphabet.IndexOf(decrypted[i]) + 1 + alphabet.IndexOf(encrypted[i - 1])) % alphabet.Length];
            }
            return encrypted;
        }
        static public string DRE1(string encrypted, string alphabet, int shift)
        {
            string Decoded = "";

            //Decrypt the first character
            Decoded += alphabet[alphabet.IndexOf(alphabet[(alphabet.IndexOf(encrypted[0]) - shift + alphabet.Length * 3) % alphabet.Length])];

            for (int i = 1; i < encrypted.Length; i++) //Decrypt the rest of the message
            {
                Decoded += alphabet[alphabet.IndexOf(alphabet[(alphabet.IndexOf(encrypted[i]) - 1 - alphabet.IndexOf(encrypted[i - 1]) + alphabet.Length * (i + 2)) % alphabet.Length])];
            }
            return Decoded;
        }


        static public string ERE2(string decrypted, string alphabet, int Shift)
        {
            string encrypted = "";

            //Encrypt the first character
            encrypted += alphabet[(alphabet.IndexOf(decrypted[0]) + Shift) % alphabet.Length];

            for (int i = 1; i < decrypted.Length; i++) //Encrypt the rest of the message
            {
                encrypted += alphabet[(alphabet.IndexOf(decrypted[i]) + alphabet.IndexOf(encrypted[i - 1])) % alphabet.Length];
            }
            return encrypted;
        } //Almost legacy code, pls just use RE3
        static public string DRE2(string encrypted, string alphabet, int shift)
        {
            string decrypted = "";

            //Decrypt the first character
            decrypted += alphabet[alphabet.IndexOf(alphabet[(alphabet.IndexOf(encrypted[0]) - shift + alphabet.Length * 2) % alphabet.Length])];

            for (int i = 1; i < encrypted.Length; i++) //Decrypt the rest of the message
            {
                decrypted += alphabet[alphabet.IndexOf(alphabet[(alphabet.IndexOf(encrypted[i]) - alphabet.IndexOf(encrypted[i - 1]) + alphabet.Length * (i + 2)) % alphabet.Length])];
            }
            return decrypted;
        } //RE3 is better than RE2 in every way


        static public string ERE3(string decrypted, string alphabet, int shift)
        {
            string encrypted = "";
            int length = alphabet.Length, messageLength = decrypted.Length;

            //Encrypt the first character
            encrypted += alphabet[(alphabet.IndexOf(decrypted[0]) + shift) % length];

            for (int i = 1; i < messageLength; i++) //Encrypt the rest of the message
            {
                encrypted += alphabet[(alphabet.IndexOf(decrypted[i]) + alphabet.IndexOf(encrypted[i - 1])) % length];
            }
            return encrypted;
        }
        static public string DRE3(string encrypted, string alphabet, int shift)
        {
            string decrypted = "";
            int length = alphabet.Length, messageLength = encrypted.Length;

            //Decrypt the first character
            decrypted += alphabet[(alphabet.IndexOf(encrypted[0]) - shift + length * 2) % length];

            for (int i = 1; i < messageLength; i++) //Decrypt the rest of the message
            {
                decrypted += alphabet[(alphabet.IndexOf(encrypted[i]) - alphabet.IndexOf(encrypted[i - 1]) + length * (i + 2)) % length];
            }
            return decrypted;
        }


        static public string ERE4(string decrypted, string alphabet, int shift)
        {
            string encrypted = "";
            int length = alphabet.Length, messageLength = decrypted.Length;
            int[] eID = new int[messageLength];

            eID[0] = alphabet.IndexOf(decrypted[0]);  //Encrypt the first character
            encrypted += alphabet[(eID[0] + shift) % length];

            for (int i = 1; i < messageLength; i++) //Encrypt the rest of the message
            {
                eID[i] = alphabet.IndexOf(decrypted[i]) + eID[i - 1];
                encrypted += alphabet[(eID[i] + shift * (i % 2)) % length];
            }
            return encrypted;
        }
        static public string DRE4(string encrypted, string alphabet, int shift)
        {
            string decrypted = "";
            int length = alphabet.Length, messageLength = encrypted.Length;
            int[] DeID = new int[messageLength];

            DeID[0] += alphabet.IndexOf(encrypted[0]); //Decrypt the first character
            decrypted += alphabet[(DeID[0] - shift + length) % length];

            DeID[1] = alphabet.IndexOf(encrypted[1]) - alphabet.IndexOf(encrypted[0]); //Decrypt the
            decrypted += alphabet[((DeID[1] + length) % length)];                // second character

            for (int i = 2; i < messageLength; i++) //Decrypt the rest of the message
            {
                DeID[i] = alphabet.IndexOf(encrypted[i]) - alphabet.IndexOf(encrypted[i - 1]) + shift - shift * 2 * (i % 2);
                decrypted += alphabet[(DeID[i] + length * 2) % length];
            }
            return decrypted;
        }




        //----------------------  Info output about the process  ----------------------------------//


        static public void ERE1Info(string encrypted, string decrypted, string alphabet, int shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first encrypted character
            Write((alphabet.IndexOf(decrypted[0]) + 1) + "+" + shift + "=" + (alphabet.IndexOf(encrypted[0]) + 1) + "/");
            if (encrypted[0] != ' ') Write(encrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < decrypted.Length; i++)
            {   //Write info on the rest of the encrypted message
                Write((alphabet.IndexOf(decrypted[i]) + 1) + "+" + (alphabet.IndexOf(encrypted[i - 1]) + 1) + "=" + (alphabet.IndexOf(encrypted[i]) + 1) + "/");
                if (encrypted[i] != ' ') Write(encrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + alphabet.Length + ")");
        }
        static public void DRE1Info(string encrypted, string decrypted, string alphabet, int shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first decrypted character
            Write((alphabet.IndexOf(encrypted[0]) + 1) + "-" + shift + "=" + (alphabet.IndexOf(decrypted[0]) + 1) + "/");
            if (decrypted[0] != ' ') Write(decrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < encrypted.Length; i++)
            {   //Write info about the rest of the decrypted message
                Write((alphabet.IndexOf(encrypted[i]) + 1) + "-" + (alphabet.IndexOf(encrypted[i - 1]) + 1) + "=" + (alphabet.IndexOf(decrypted[i]) + 1) + "/");
                if (decrypted[i] != ' ') Write(decrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + alphabet.Length + ")");
        }


        static public void ERE2Info(string encrypted, string decrypted, string alphabet, int shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first encrypted character
            Write(alphabet.IndexOf(decrypted[0]) + "+" + shift + "=" + alphabet.IndexOf(encrypted[0]) + "/");
            if (encrypted[0] != ' ') Write(encrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < decrypted.Length; i++)
            {   //Write info on the rest of the encrypted message
                Write((alphabet.IndexOf(decrypted[i]) + "+" + alphabet.IndexOf(encrypted[i - 1])) + "=" + alphabet.IndexOf(encrypted[i]) + "/");
                if (encrypted[i] != ' ') Write(encrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + alphabet.Length + ")");
        }
        static public void DRE2Info(string encrypted, string decrypted, string alphabet, int shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first decrypted character
            Write(alphabet.IndexOf(encrypted[0]) + "-" + shift + "=" + alphabet.IndexOf(decrypted[0]) + "/");
            if (decrypted[0] != ' ') Write(decrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < encrypted.Length; i++)
            {   //Write info about the rest of the decrypted message
                Write(alphabet.IndexOf(encrypted[i]) + "-" + alphabet.IndexOf(encrypted[i - 1]) + "=" + alphabet.IndexOf(decrypted[i]) + "/");
                if (decrypted[i] != ' ') Write(decrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + alphabet.Length + ")");
        }


        static public void ERE3Info(string encrypted, string decrypted, string alphabet, int shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first encrypted character
            Write(alphabet.IndexOf(decrypted[0]) + "+" + shift + "=" + alphabet.IndexOf(encrypted[0]) + "/");
            if (encrypted[0] != ' ') Write(encrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < decrypted.Length; i++)
            {   //Write info on the rest of the encrypted message
                Write((alphabet.IndexOf(decrypted[i]) + "+" + alphabet.IndexOf(encrypted[i - 1])) + "=" + alphabet.IndexOf(encrypted[i]) + "/");
                if (encrypted[i] != ' ') Write(encrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + alphabet.Length + ")");
        }
        static public void DRE3Info(string encrypted, string decrypted, string alphabet, int shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first decrypted character
            Write(alphabet.IndexOf(encrypted[0]) + "-" + shift + "=" + alphabet.IndexOf(decrypted[0]) + "/");
            if (decrypted[0] != ' ') Write(decrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < encrypted.Length; i++)
            {   //Write info about the rest of the decrypted message
                Write(alphabet.IndexOf(encrypted[i]) + "-" + alphabet.IndexOf(encrypted[i - 1]) + "=" + alphabet.IndexOf(decrypted[i]) + "/");
                if (decrypted[i] != ' ') Write(decrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + alphabet.Length + ")");
        }


        static public void ERE4Info(string encrypted, string decrypted, string alphabet, int shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first encrypted character
            Write((alphabet.IndexOf(decrypted[0]) - shift) + "+" + shift + "=" + alphabet.IndexOf(decrypted[0]) + "/");
            if (encrypted[0] != ' ') Write(encrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < decrypted.Length; i++) //Write info about the rest of the encrypted message
            {
                Write((alphabet.IndexOf(decrypted[i]) - shift * (i % 2)) + "+" + shift * (i % 2) + "=" + alphabet.IndexOf(decrypted[i]) + "/");
                if (encrypted[i] != ' ') Write(encrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + alphabet.Length + ")");
        }
        static public void DRE4Info(string encrypted, string decrypted, string Alphabet, int Shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first decrypted character
            Write(Alphabet.IndexOf(encrypted[0]) + "-" + Shift + "=" + Alphabet.IndexOf(decrypted[0]) + "/");
            if (decrypted[0] != ' ') Write(decrypted[0] + "  ");
            else Write("Space  ");

            Write(Alphabet.IndexOf(encrypted[1]) + "-" + Alphabet.IndexOf(encrypted[0]) + "=" + Alphabet.IndexOf(decrypted[1]) + "/");
            if (decrypted[1] != ' ') Write(decrypted[1] + "  ");
            else Write("Space  "); //Write info on the second decrypted character of the message

            for (int i = 2; i < encrypted.Length; i++)
            { //Write info on the rest of the message
                Write(Alphabet.IndexOf(encrypted[i]) + "-" + Alphabet.IndexOf(encrypted[i - 1]) + "+" + Shift + "-" + (Shift * 2 * (i % 2)) + "=" + Alphabet.IndexOf(decrypted[i]) + "/");
                if (decrypted[i] != ' ') Write(decrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + Alphabet.Length + ")");
        }
    }
}
