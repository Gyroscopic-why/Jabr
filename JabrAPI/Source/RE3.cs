using System;


namespace JabrAPI
{
    public class RE3
    {
        static public string Encrypt(string decrypted, string alphabet, Int32 shift)
        {
            string encrypted = "";
            Int32 length = alphabet.Length, messageLength = decrypted.Length;

            //Encrypt the first character
            encrypted += alphabet[(alphabet.IndexOf(decrypted[0]) + shift) % length];

            for (Int32 i = 1; i < messageLength; i++) //Encrypt the rest of the message
            {
                encrypted += alphabet[(alphabet.IndexOf(decrypted[i]) + alphabet.IndexOf(encrypted[i - 1])) % length];
            }
            return encrypted;
        }
        static public string Decrypt(string encrypted, string alphabet, Int32 shift)
        {
            string decrypted = "";
            Int32 length = alphabet.Length, messageLength = encrypted.Length;

            //Decrypt the first character
            decrypted += alphabet[(alphabet.IndexOf(encrypted[0]) - shift + length * 2) % length];

            for (Int32 i = 1; i < messageLength; i++) //Decrypt the rest of the message
            {
                decrypted += alphabet[(alphabet.IndexOf(encrypted[i]) - alphabet.IndexOf(encrypted[i - 1]) + length * (i + 2)) % length];
            }
            return decrypted;
        }



    }
}