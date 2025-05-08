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