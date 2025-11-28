using System;
using System.Collections.Generic;

using static System.Console;



namespace Jabr
{
    internal class CipherSource
    {
        static public string ERE3(string message,   string alphabet, List<Int32> shifts)
        {
            Int32 aLength = alphabet.Length, messageLength = message.Length, shCount = shifts.Count;
            Int32[] eID = new Int32[messageLength];

            Int32 buffer = alphabet.IndexOf(message[0]);
            eID[0] = (buffer + shifts[0]) % aLength;
            string encrypted = alphabet[eID[0]].ToString();

            for (var i = 1; i < messageLength; i++)
            {
                buffer = alphabet.IndexOf(message[i]);
                eID[i] = (buffer + eID[i - 1] + shifts[i % shCount]) % aLength;
                encrypted += alphabet[eID[i]];
            }

            return encrypted;
        }
        static public string DRE3(string encrypted, string alphabet, List<Int32> shifts)
        {
            Int32 aLength = alphabet.Length, messageLength = encrypted.Length, shCount = shifts.Count;
            Int32[] eID = new Int32[messageLength];

            eID[0] = alphabet.IndexOf(encrypted[0]);
            string decrypted = alphabet[(eID[0] - shifts[0] + aLength) % aLength].ToString();

            for (var i = 1; i < messageLength; i++)
            {
                eID[i] = alphabet.IndexOf(encrypted[i]);
                decrypted += alphabet[(eID[i] - eID[i - 1] - shifts[i % shCount] + 4 * aLength) % aLength];
            }
            return decrypted;
        }


        static public string ERE4(string message,   string alphabet, List<Int32> shifts)
        {
            Int32 aLength = alphabet.Length, messageLength = message.Length, shCount = shifts.Count;
            Int32[] eID = new Int32[messageLength];

            eID[0] = alphabet.IndexOf(message[0]);
            string encrypted = alphabet[(eID[0] + shifts[0]) % aLength].ToString();

            for (var i = 1; i < messageLength; i++)
            {
                eID[i] = (alphabet.IndexOf(message[i]) + eID[i - 1]) % aLength;
                encrypted += alphabet[(eID[i] + shifts[i % shCount] * (i % 2 + 1)) % aLength];
            }
            return encrypted;
        }
        static public string DRE4(string encrypted, string alphabet, List<Int32> shifts)
        {
            Int32 aLength = alphabet.Length, messageLength = encrypted.Length, shCount = shifts.Count;
            Int32[] dID = new Int32[messageLength];

            dID[0] = alphabet.IndexOf(encrypted[0]) - shifts[0];
            string decrypted = alphabet[(dID[0] + aLength) % aLength].ToString();

            for (var i = 1; i < messageLength; i++)
            {
                dID[i] = alphabet.IndexOf(encrypted[i]) - shifts[i % shCount] * (i % 2 + 1);
                decrypted += alphabet[(dID[i] - dID[i - 1] + 4 * aLength) % aLength];
            }
            return decrypted;
        }



        //----------------------  Info output about the process  ----------------------------------//


        static public void ERE3Info(string encrypted, string message, string alphabet, List<Int32> shifts)
        {
            Int32 aLength = alphabet.Length, messageLength = message.Length, shCount = shifts.Count;
            Int32[] eID = new Int32[messageLength];

            Int32 buffer = alphabet.IndexOf(message[0]);
            eID[0] = (buffer + shifts[0]) % aLength;


            Write("\n\n\t\t[i]  - Дополнительная информация о процессе шифрования РЕ3:");

            if (encrypted[0] != ' ') Write($"\n\t\t         > 1) {encrypted[0]}");
            else                     Write($"\n\t\t         > 1) SPACE");
            Write($"({eID[0]}) = (");

            if (message[0] != ' ') Write(message[0]);
            else                   Write("SPACE");
            Write($".ID){buffer} + (сдвиг[1]){shifts[0]} | мод({aLength})");


            for (var i = 1; i < messageLength; i++)
            {
                buffer = alphabet.IndexOf(message[i]);
                eID[i] = (buffer + eID[i - 1] + shifts[i % shCount]) % aLength;

                if (encrypted[i] != ' ') Write($"\n\t\t         > {i + 1}) {encrypted[i]}");
                else                     Write($"\n\t\t         > {i + 1}) SPACE");

                Write($"({eID[i]}) = (");
                if (message[0] != ' ') Write(message[0]);
                else                   Write("SPACE");

                Write($".ID){buffer} + (заш[{i}]){eID[i - 1]}" +
                    $" + (сдвиг[{i % shCount + 1}]){shifts[i % shCount]} | мод({aLength})");
            }
        }
        static public void DRE3Info(string encrypted, string message, string alphabet, List<Int32> shifts)
        {
            Int32 aLength = alphabet.Length, messageLength = encrypted.Length, shCount = shifts.Count;
            Int32[] eID = new Int32[messageLength];
            eID[0] = alphabet.IndexOf(encrypted[0]);


            Write("\n\n\t\t[i]  - Дополнительная информация о процессе:");

            if (message[0] != ' ') Write($"\n\t\t         > 1) {message[0]}");
            else                   Write($"\n\t\t         > 1) SPACE");
            Write($"({alphabet.IndexOf(message[0])}) = (");

            if (encrypted[0] != ' ') Write(encrypted[0]);
            else                     Write("SPACE");
            Write($".ID){eID[0]} - (сдвиг[1]){shifts[0]} + (мод){aLength} | мод({aLength})");


            for (var i = 1; i < messageLength; i++)
            {
                eID[i] = alphabet.IndexOf(encrypted[i]);

                if (message[i] != ' ') Write($"\n\t\t         > {i + 1}) {message[i]}");
                else                   Write($"\n\t\t         > {i + 1}) SPACE");
                Write($"({alphabet.IndexOf(message[i])}) = (");

                if (encrypted[i] != ' ') Write(encrypted[i]);
                else                     Write("SPACE");
                Write($".ID){eID[i]} - ");

                if (encrypted[i - 1] != ' ') Write(encrypted[i - 1]);
                else                         Write("SPACE");
                Write($".ID){eID[i]} - (сдвиг[{i % shCount + 1}]){shifts[i % shCount]} +  4 * {aLength}(мод) | мод({aLength})");
            }
        }


        static public void ERE4Info(string encrypted, string message, string alphabet, List<Int32> shifts)
        {
            Int32 aLength = alphabet.Length, messageLength = message.Length, shCount = shifts.Count;
            Int32[] eID = new Int32[messageLength];
            eID[0] = alphabet.IndexOf(message[0]);


            Write("\n\n\t\t[i]  - Дополнительная информация о процессе:");

            if (encrypted[0] != ' ') Write($"\n\t\t         > 1) {encrypted[0]}");
            else                     Write($"\n\t\t         > 1) SPACE");
            Write($"({alphabet.IndexOf(encrypted[0])}) = (");

            if (message[0] != ' ') Write(message[0]);
            else                   Write("SPACE");
            Write($".ID){eID[0]} + (сдвиг[1]){shifts[0]} | мод({aLength})");


            for (var i = 1; i < messageLength; i++)
            {
                eID[i] = (alphabet.IndexOf(message[i]) + eID[i - 1]) % aLength;

                if (encrypted[i] != ' ') Write($"\n\t\t         > {i + 1}) {encrypted[i]}");
                else                     Write($"\n\t\t         > {i + 1}) SPACE");

                Write($"({alphabet.IndexOf(encrypted[i])}) = (");
                if (message[i] != ' ') Write(message[i]);
                else                   Write("SPACE");

                Write($".ID + заш[{i}]){eID[i]} + (сдвиг[{i % shCount + 1}]){shifts[i % shCount]} * {i % 2 + 1} | мод({aLength})");
            }
        }
        static public void DRE4Info(string encrypted, string message, string alphabet, List<Int32> shifts)
        {
            Int32 aLength = alphabet.Length, messageLength = encrypted.Length, shCount = shifts.Count;
            Int32[] dID = new Int32[messageLength];

            dID[0] = alphabet.IndexOf(encrypted[0]) - shifts[0];


            Write("\n\n\t\t[i]  - Дополнительная информация о процессе:");

            if (message[0] != ' ') Write($"\n\t\t         > 1) {message[0]}");
            else                   Write($"\n\t\t         > 1) SPACE");
            Write($"({alphabet.IndexOf(message[0])}) = (");

            if (encrypted[0] != ' ') Write(encrypted[0]);
            else                     Write("SPACE");
            Write($".ID){alphabet.IndexOf(encrypted[0])} - (сдвиг[1]){shifts[0]} + {aLength}(мод) | мод({aLength})");


            for (var i = 1; i < messageLength; i++)
            {
                dID[i] = alphabet.IndexOf(encrypted[i]) - shifts[i % shCount] * (i % 2 + 1);
                message += alphabet[(dID[i] - dID[i - 1] + 4 * aLength) % aLength];


                if (message[i] != ' ') Write($"\n\t\t         > {i + 1}) {message[i]}");
                else                   Write($"\n\t\t         > {i + 1}) SPACE");
                Write($"({alphabet.IndexOf(message[i])}) = (");

                if (encrypted[i] != ' ') Write(encrypted[0]);
                else                     Write("SPACE");

                Write($".ID){alphabet.IndexOf(encrypted[0])}" +
                    $" - (сдвиг[{i % shCount + 1}]){shifts[i % shCount]} * {i % 2 + 1}" +
                    $" - (заш[{i}]){dID[i - 1]} + 4 * {aLength}(мод) | мод({aLength})");
            }
        }
    }
}
