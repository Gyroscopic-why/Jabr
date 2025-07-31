using System;


namespace JabrAPI
{
    public class RE4
    {
        static public string Encrypt(string decrypted, string alphabet, Int32 shift)
        {
            string encrypted = "";
            Int32 length = alphabet.Length, messageLength = decrypted.Length;
            Int32[] eID = new Int32[messageLength];

            eID[0] = alphabet.IndexOf(decrypted[0]);  //Encrypt the first character
            encrypted += alphabet[(eID[0] + shift) % length];

            for (Int32 i = 1; i < messageLength; i++) //Encrypt the rest of the message
            {
                eID[i] = alphabet.IndexOf(decrypted[i]) + eID[i - 1];
                encrypted += alphabet[(eID[i] + shift * (i % 2)) % length];
            }
            return encrypted;
        }
        static public string Decrypt(string encrypted, string alphabet, Int32 shift)
        {
            string decrypted = "";
            Int32 length = alphabet.Length, messageLength = encrypted.Length;
            Int32[] DeID = new Int32[messageLength];

            DeID[0] += alphabet.IndexOf(encrypted[0]); //Decrypt the first character
            decrypted += alphabet[(DeID[0] - shift + length) % length];

            DeID[1] = alphabet.IndexOf(encrypted[1]) - alphabet.IndexOf(encrypted[0]); //Decrypt the
            decrypted += alphabet[((DeID[1] + length) % length)];                // second character

            for (Int32 i = 2; i < messageLength; i++) //Decrypt the rest of the message
            {
                DeID[i] = alphabet.IndexOf(encrypted[i]) - alphabet.IndexOf(encrypted[i - 1]) + shift - shift * 2 * (i % 2);
                decrypted += alphabet[(DeID[i] + length * 2) % length];
            }
            return decrypted;
        }

    }
}