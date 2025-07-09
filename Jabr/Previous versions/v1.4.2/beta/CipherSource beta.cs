//using System;
//using static Jabr.CustomFunctions;
using static System.Console;

namespace Jabr
{
    internal class CipherSource
    {
        static public string E_RE1(string _decrypted, string _alphabet, int _shift)
        {
            string _encrypted = "";

            //Encrypt the first character
            _encrypted += _alphabet[(_alphabet.IndexOf(_decrypted[0]) + _shift) % _alphabet.Length];

            for (int i = 1; i < _decrypted.Length; i++) //Encrypt the rest of the message
            {
                _encrypted += _alphabet[(_alphabet.IndexOf(_decrypted[i]) + 1 + _alphabet.IndexOf(_encrypted[i - 1])) % _alphabet.Length];
            }
            return _encrypted;
        }
        static public string D_RE1(string _encrypted, string _alphabet, int _shift)
        {
            string Decoded = "";

            //Decrypt the first character
            Decoded += _alphabet[_alphabet.IndexOf(_alphabet[(_alphabet.IndexOf(_encrypted[0]) - _shift + _alphabet.Length * 3) % _alphabet.Length])];

            for (int i = 1; i < _encrypted.Length; i++) //Decrypt the rest of the message
            {
                Decoded += _alphabet[_alphabet.IndexOf(_alphabet[(_alphabet.IndexOf(_encrypted[i]) - 1 - _alphabet.IndexOf(_encrypted[i - 1]) + _alphabet.Length * (i + 2)) % _alphabet.Length])];
            }
            return Decoded;
        }

        static public string E_RE2(string _decrypted, string _alphabet, int Shift)
        {
            string _encrypted = "";

            //Encrypt the first character
            _encrypted += _alphabet[(_alphabet.IndexOf(_decrypted[0]) + Shift) % _alphabet.Length];

            for (int i = 1; i < _decrypted.Length; i++) //Encrypt the rest of the message
            {
                _encrypted += _alphabet[(_alphabet.IndexOf(_decrypted[i]) + _alphabet.IndexOf(_encrypted[i - 1])) % _alphabet.Length];
            }
            return _encrypted;
        } //Almost legacy code, pls just use RE3
        static public string D_RE2(string _encrypted, string _alphabet, int _shift)
        {
            string _decrypted = "";

            //Decrypt the first character
            _decrypted += _alphabet[_alphabet.IndexOf(_alphabet[(_alphabet.IndexOf(_encrypted[0]) - _shift + _alphabet.Length * 2) % _alphabet.Length])];

            for (int i = 1; i < _encrypted.Length; i++) //Decrypt the rest of the message
            {
                _decrypted += _alphabet[_alphabet.IndexOf(_alphabet[(_alphabet.IndexOf(_encrypted[i]) - _alphabet.IndexOf(_encrypted[i - 1]) + _alphabet.Length * (i + 2)) % _alphabet.Length])];
            }
            return _decrypted;
        } //RE3 is better than RE2 in every way

        static public string E_RE3(string _decrypted, string _alphabet, int _shift)
        {
            string _encrypted = "";
            int _length = _alphabet.Length, _messageLength = _decrypted.Length;

            //Encrypt the first character
            _encrypted += _alphabet[(_alphabet.IndexOf(_decrypted[0]) + _shift) % _length];

            for (int i = 1; i < _messageLength; i++) //Encrypt the rest of the message
            {
                _encrypted += _alphabet[(_alphabet.IndexOf(_decrypted[i]) + _alphabet.IndexOf(_encrypted[i - 1])) % _length];
            }
            return _encrypted;
        }
        static public string D_RE3(string _encrypted, string _alphabet, int _shift)
        {
            string _decrypted = "";
            int _length = _alphabet.Length, _messageLength = _encrypted.Length;

            //Decrypt the first character
            _decrypted += _alphabet[(_alphabet.IndexOf(_encrypted[0]) - _shift + _length * 2) % _length];

            for (int i = 1; i < _messageLength; i++) //Decrypt the rest of the message
            {
                _decrypted += _alphabet[(_alphabet.IndexOf(_encrypted[i]) - _alphabet.IndexOf(_encrypted[i - 1]) + _length * (i + 2)) % _length];
            }
            return _decrypted;
        }

        static public string E_RE4(string _decrypted, string _alphabet, int _shift)
        {
            string _encrypted = "";
            int _length = _alphabet.Length, _messageLength = _decrypted.Length;
            int[] _eID = new int[_messageLength];

            _eID[0] = _alphabet.IndexOf(_decrypted[0]);  //Encrypt the first character
            _encrypted += _alphabet[(_eID[0] + _shift) % _length];

            for (int i = 1; i < _messageLength; i++) //Encrypt the rest of the message
            {
                _eID[i] = _alphabet.IndexOf(_decrypted[i]) + _eID[i - 1];
                _encrypted += _alphabet[(_eID[i] + _shift * (i % 2)) % _length];
            }
            return _encrypted;
        }
        static public string D_RE4(string _encrypted, string _alphabet, int _shift)
        {
            string _decrypted = "";
            int _length = _alphabet.Length, _messageLength = _encrypted.Length;
            int[] DeID = new int[_messageLength];

            DeID[0] += _alphabet.IndexOf(_encrypted[0]); //Decrypt the first character
            _decrypted += _alphabet[(DeID[0] - _shift + _length) % _length];

            DeID[1] = _alphabet.IndexOf(_encrypted[1]) - _alphabet.IndexOf(_encrypted[0]); //Decrypt the
            _decrypted += _alphabet[((DeID[1] + _length) % _length)];                // second character

            for (int i = 2; i < _messageLength; i++) //Decrypt the rest of the message
            {
                DeID[i] = _alphabet.IndexOf(_encrypted[i]) - _alphabet.IndexOf(_encrypted[i - 1]) + _shift - _shift * 2 * (i % 2);
                _decrypted += _alphabet[(DeID[i] + _length * 2) % _length];
            }
            return _decrypted;
        }

        //-------------------------------------------------------------------------------//
        static public void E_RE1_Info(string _encrypted, string _decrypted, string _alphabet, int _shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first encrypted character
            Write((_alphabet.IndexOf(_decrypted[0]) + 1) + "+" + _shift + "=" + (_alphabet.IndexOf(_encrypted[0]) + 1) + "/");
            if (_encrypted[0] != ' ') Write(_encrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < _decrypted.Length; i++)
            {   //Write info on the rest of the encrypted message
                Write((_alphabet.IndexOf(_decrypted[i]) + 1) + "+" + (_alphabet.IndexOf(_encrypted[i - 1]) + 1) + "=" + (_alphabet.IndexOf(_encrypted[i]) + 1) + "/");
                if (_encrypted[i] != ' ') Write(_encrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + _alphabet.Length + ")");
        }
        static public void D_RE1_Info(string _encrypted, string _decrypted, string _alphabet, int _shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first decrypted character
            Write((_alphabet.IndexOf(_encrypted[0]) + 1) + "-" + _shift + "=" + (_alphabet.IndexOf(_decrypted[0]) + 1) + "/");
            if (_decrypted[0] != ' ') Write(_decrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < _encrypted.Length; i++)
            {   //Write info about the rest of the decrypted message
                Write((_alphabet.IndexOf(_encrypted[i]) + 1) + "-" + (_alphabet.IndexOf(_encrypted[i - 1]) + 1) + "=" + (_alphabet.IndexOf(_decrypted[i]) + 1) + "/");
                if (_decrypted[i] != ' ') Write(_decrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + _alphabet.Length + ")");
        }

        static public void E_RE2_Info(string _encrypted, string _decrypted, string _alphabet, int _shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first encrypted character
            Write(_alphabet.IndexOf(_decrypted[0]) + "+" + _shift + "=" + _alphabet.IndexOf(_encrypted[0]) + "/");
            if (_encrypted[0] != ' ') Write(_encrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < _decrypted.Length; i++)
            {   //Write info on the rest of the encrypted message
                Write((_alphabet.IndexOf(_decrypted[i]) + "+" + _alphabet.IndexOf(_encrypted[i - 1])) + "=" + _alphabet.IndexOf(_encrypted[i]) + "/");
                if (_encrypted[i] != ' ') Write(_encrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + _alphabet.Length + ")");
        }
        static public void D_RE2_Info(string _encrypted, string _decrypted, string _alphabet, int _shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first decrypted character
            Write(_alphabet.IndexOf(_encrypted[0]) + "-" + _shift + "=" + _alphabet.IndexOf(_decrypted[0]) + "/");
            if (_decrypted[0] != ' ') Write(_decrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < _encrypted.Length; i++)
            {   //Write info about the rest of the decrypted message
                Write(_alphabet.IndexOf(_encrypted[i]) + "-" + _alphabet.IndexOf(_encrypted[i - 1]) + "=" + _alphabet.IndexOf(_decrypted[i]) + "/");
                if (_decrypted[i] != ' ') Write(_decrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + _alphabet.Length + ")");
        }

        static public void E_RE3_Info(string _encrypted, string _decrypted, string _alphabet, int _shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first encrypted character
            Write(_alphabet.IndexOf(_decrypted[0]) + "+" + _shift + "=" + _alphabet.IndexOf(_encrypted[0]) + "/");
            if (_encrypted[0] != ' ') Write(_encrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < _decrypted.Length; i++)
            {   //Write info on the rest of the encrypted message
                Write((_alphabet.IndexOf(_decrypted[i]) + "+" + _alphabet.IndexOf(_encrypted[i - 1])) + "=" + _alphabet.IndexOf(_encrypted[i]) + "/");
                if (_encrypted[i] != ' ') Write(_encrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + _alphabet.Length + ")");
        }
        static public void D_RE3_Info(string _encrypted, string _decrypted, string _alphabet, int _shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first decrypted character
            Write(_alphabet.IndexOf(_encrypted[0]) + "-" + _shift + "=" + _alphabet.IndexOf(_decrypted[0]) + "/");
            if (_decrypted[0] != ' ') Write(_decrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < _encrypted.Length; i++)
            {   //Write info about the rest of the decrypted message
                Write(_alphabet.IndexOf(_encrypted[i]) + "-" + _alphabet.IndexOf(_encrypted[i - 1]) + "=" + _alphabet.IndexOf(_decrypted[i]) + "/");
                if (_decrypted[i] != ' ') Write(_decrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + _alphabet.Length + ")");
        }

        static public void E_RE4_Info(string _encrypted, string _decrypted, string _alphabet, int _shift)
        {
            Write("\n\t\t[i]  - "); //Write info about the first encrypted character
            Write((_alphabet.IndexOf(_decrypted[0]) - _shift) + "+" + _shift + "=" + _alphabet.IndexOf(_decrypted[0]) + "/");
            if (_encrypted[0] != ' ') Write(_encrypted[0] + "  ");
            else Write("Space  ");

            for (int i = 1; i < _decrypted.Length; i++) //Write info about the rest of the encrypted message
            {
                Write((_alphabet.IndexOf(_decrypted[i]) - _shift * (i % 2)) + "+" + _shift * (i % 2) + "=" + _alphabet.IndexOf(_decrypted[i]) + "/");
                if (_encrypted[i] != ' ') Write(_encrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + _alphabet.Length + ")");
        }
        static public void D_RE4_Info(string _encrypted, string _decrypted, string Alphabet, int Shift)
        {
            Write("\n\t\t[i]  - "); //Write info on the first decrypted character
            Write(Alphabet.IndexOf(_encrypted[0]) + "-" + Shift + "=" + Alphabet.IndexOf(_decrypted[0]) + "/");
            if (_decrypted[0] != ' ') Write(_decrypted[0] + "  ");
            else Write("Space  ");

            Write(Alphabet.IndexOf(_encrypted[1]) + "-" + Alphabet.IndexOf(_encrypted[0]) + "=" + Alphabet.IndexOf(_decrypted[1]) + "/");
            if (_decrypted[1] != ' ') Write(_decrypted[1] + "  ");
            else Write("Space  "); //Write info on the second decrypted character of the message

            for (int i = 2; i < _encrypted.Length; i++)
            { //Write info on the rest of the message
                Write(Alphabet.IndexOf(_encrypted[i]) + "-" + Alphabet.IndexOf(_encrypted[i - 1]) + "+" + Shift + "-" + (Shift * 2 * (i % 2)) + "=" + Alphabet.IndexOf(_decrypted[i]) + "/");
                if (_decrypted[i] != ' ') Write(_decrypted[i] + "  ");
                else Write("Space  ");
            }
            Write("(mod " + Alphabet.Length + ")");
        }
    }
}
