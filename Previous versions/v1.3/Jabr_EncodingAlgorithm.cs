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