using System;
using System.Collections.Generic;
using System.Linq;

using static System.Console;


using AVcontrol;



namespace JabrAPI.Source
{
    public class Legacy
    {
        static public  string RE5_EncryptWithConsoleInfo(string message, RE5.EncryptionKey reKey, bool displayInfo = true)
        {
            try { return RE5_UnsafeEncryptWithConsoleInfo(message, reKey, displayInfo); }
            catch { return ""; }
        }
        static public  string RE5_EncryptWithConsoleInfo(string message, RE5.EncryptionKey reKey, out Exception? exception)
        {
            try
            {
                string result = RE5_UnsafeEncryptWithConsoleInfo(message, reKey);
                exception = null;

                return result;
            }
            catch (Exception innerException)
            {
                exception = innerException;
                return "";
            }
        }
        static private string RE5_UnsafeEncryptWithConsoleInfo(string message, RE5.EncryptionKey reKey, bool displayInfo = true)
        {
            Int32 exLength = reKey.ExLength, messageLength = message.Length, shCount = reKey.ShCount;
            Int32[] buffer = new Int32[messageLength], ids = new Int32[messageLength];
            List<Int16> shifts = reKey.Shifts; string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;


            Int32 maxEncodingLength = Numsys.AutoAsList
            (
                Math.Ceiling
                (
                    (float)
                    (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                        reKey.PrLength * 2 + shifts.Max() - 4
                    ) / exLength
                ).ToString(),
                10,
                exLength
            ).Count;


            ids[0] = prAlphabet.IndexOf(message[0]);
            buffer[0] = ids[0] + shifts[0];
            string encoding = Numsys.ToCustomAsString
            (
                (buffer[0] / exLength).ToString(),
                10,
                exLength,
                exAlphabet,
                maxEncodingLength
            );


            string encrypted = exAlphabet[buffer[0] % exLength].ToString() + encoding;

            for (var curId = 1; curId < messageLength; curId++)
            {
                ids[curId] = prAlphabet.IndexOf(message[curId]);
                buffer[curId] = ids[curId] + shifts[curId % shCount] + ids[curId - 1];

                encoding = Numsys.ToCustomAsString
                (
                    (buffer[curId] / exLength).ToString(),
                    10,
                    exLength,
                    exAlphabet,
                    maxEncodingLength
                );

                encrypted += exAlphabet[buffer[curId] % exLength] + encoding;
            }

            if (displayInfo) RE5_EncryptingInfo(buffer, exLength, maxEncodingLength, [.. shifts], shCount, ids, encrypted, message, messageLength);
            return encrypted;
        }



        static public  string RE5_DecryptWithConsoleInfo(string encrypted, RE5.EncryptionKey reKey, bool displayInfo = true)
        {
            try { return RE5_UnsafeDecryptWithConsoleInfo(encrypted, reKey, displayInfo); }
            catch { return ""; }
        }
        static public  string RE5_DecryptWithConsoleInfo(string encrypted, RE5.EncryptionKey reKey, out Exception? exception, bool displayInfo = true)
        {
            try
            {
                string result = RE5_UnsafeDecryptWithConsoleInfo(encrypted, reKey, displayInfo);
                exception = null;

                return result;
            }
            catch (Exception innerException)
            {
                exception = innerException;
                return "";
            }
        }
        static private string RE5_UnsafeDecryptWithConsoleInfo(string encrypted, RE5.EncryptionKey reKey, bool displayInfo = true)
        {
            Int32 exLength = reKey.ExLength, encryptedLength = encrypted.Length, shCount = reKey.ShCount, encCurId = 0;
            Int32[] buffer = new Int32[encryptedLength], ids = new Int32[encryptedLength];
            List<Int16> shifts = reKey.Shifts; string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;

            for (var curChar = 0; curChar < encryptedLength; curChar++)
                ids[curChar] = exAlphabet.IndexOf(encrypted[curChar]);


            Int32 helper = (Int32)Math.Ceiling
                (
                    (double)
                    (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                        reKey.PrLength * 2 + shifts.Max() - 4
                    ) / exLength
                );
            Int32 maxEncodingLength = exLength == 10 ?
                DigitsInPositive(helper)  // Optimisation for base 10 encoding
              : Numsys.AsList
              (
                    helper.ToString(),
                    10,
                    exLength
                ).Count;

            Int32 realMessageLength = encryptedLength / (maxEncodingLength + 1);
            Int32[] decodedIds = new Int32[realMessageLength];
            Int32 parsedEncoding = (Int32)Numsys.ToDecimalFromCustom
            (
                Utils.Interval
                (
                    encrypted,
                    1,
                    1 + maxEncodingLength
                ),
                exLength,
                exAlphabet
            );


            buffer[0] = ids[0] - shifts[0];
            decodedIds[0] = buffer[0] + parsedEncoding * exLength;
            string decrypted = prAlphabet[decodedIds[0]].ToString();


            for (var curId = 1; curId < realMessageLength; curId++)
            {
                encCurId += maxEncodingLength + 1;
                buffer[curId] = ids[encCurId] - decodedIds[curId - 1] - shifts[curId % shCount];

                parsedEncoding = (Int32)Numsys.ToDecimalFromCustom
                (
                    Utils.Interval
                    (
                        encrypted,
                        encCurId + 1,
                        encCurId + 1 + maxEncodingLength
                    ),
                    exLength,
                    exAlphabet
                );

                decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                decrypted += prAlphabet[decodedIds[curId]];
            }

            if (displayInfo) RE5_DecryptingInfo(buffer, exLength, maxEncodingLength, [.. shifts], shCount, ids, decodedIds, encrypted, decrypted, realMessageLength);
            return decrypted;
        }



        static private void RE5_EncryptingInfo(Int32[] buffer, Int32 exLength, Int32 maxEncodingLength, Int16[] shifts, Int32 shCount, Int32[] ids, string encrypted, string message, Int32 messageLength)
        {
            //  margin = for ids, bf = buffer, sh = shifts, el = externalAlphabet.Length, sz = message length
            Int32 margin   = DigitsAmount(ids.Max());
            Int32 marginBf = DigitsAmount(buffer.Max()),  marginSh = DigitsAmount(shifts.Max());
            Int32 marginSz = DigitsAmount(messageLength), marginEl = DigitsAmount(exLength - 1);


            //---  First character (its encryption is a bit different so we do it manually)
            Write("\n\t");
            for (var ext = 1; ext < marginSz; ext++) Write(" ");
            Write("0] total:  ");

            for (var ext = DigitsAmount(buffer[0]); ext < marginBf; ext++) Write(" ");
            Write(buffer[0] + " (");

            for (var ext = DigitsAmount(buffer[0] % exLength); ext < marginEl; ext++) Write(" ");
            ForegroundColor = ConsoleColor.Magenta;
            Write(buffer[0] % exLength);
            ForegroundColor = ConsoleColor.Gray;
            Write(") = ");

            for (var ext = DigitsAmount(shifts[0]); ext < marginSh; ext++) Write(" ");
            ForegroundColor = shifts[0] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
            Write(shifts[0]);
            ForegroundColor = ConsoleColor.Gray;
            Write(" + ");

            for (var ext = DigitsAmount(ids[0]); ext < margin; ext++) Write(" ");
            ForegroundColor = ConsoleColor.Green;
            Write(ids[0]);
            ForegroundColor = ConsoleColor.Gray;

            for (var ext = 0; ext < margin * 2; ext++) Write(" ");
            Write("   out: ");
            ForegroundColor = ConsoleColor.DarkCyan;
            Write(encrypted[0]);
            ForegroundColor = ConsoleColor.Gray;


            //  Rest of the message
            for (var curId = 1; curId < messageLength; curId++)
            {
                Write("\n\t");
                for (var ext = DigitsAmount(curId); ext < marginSz; ext++) Write(" ");
                Write(curId + "] total:  ");

                for (var ext = DigitsAmount(buffer[curId]); ext < marginBf; ext++) Write(" ");
                Write(buffer[curId] + " (");

                for (var ext = DigitsAmount(buffer[0] % exLength); ext < marginEl; ext++) Write(" ");
                ForegroundColor = ConsoleColor.Magenta;
                Write(buffer[curId] % exLength);
                ForegroundColor = ConsoleColor.Gray;
                Write(") = ");

                for (var ext = DigitsAmount(shifts[curId % shCount]); ext < marginSh; ext++) Write(" ");
                ForegroundColor = shifts[curId % shCount] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
                Write(shifts[curId % shCount]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" + ");

                for (var ext = DigitsAmount(ids[curId]); ext < margin; ext++) Write(" ");
                ForegroundColor = curId % 2 == 0 ? ConsoleColor.Green : ConsoleColor.DarkYellow;
                Write(ids[curId]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" + ");

                for (var ext = DigitsAmount(ids[curId - 1]); ext < margin; ext++) Write(" ");
                ForegroundColor = curId % 2 == 0 ? ConsoleColor.DarkYellow : ConsoleColor.Green;
                Write(ids[curId - 1]);
                ForegroundColor = ConsoleColor.Gray;

                for (var ext = 0; ext < margin; ext++) Write(" ");
                Write("out: ");
                ForegroundColor = ConsoleColor.DarkCyan;
                Write(encrypted[curId * (maxEncodingLength + 1)]);
                ForegroundColor = ConsoleColor.Gray;
            }



            //  Original message
            Write("\n\tOriginal:  ");
            for (var curChar = 0; curChar < message.Length; curChar++)
            {
                ForegroundColor = curChar % 2 == 0 ? ConsoleColor.Green : ConsoleColor.DarkYellow;
                Write(message[curChar]);

                ForegroundColor = ConsoleColor.DarkGray;
                for (var curId = 0; curId < maxEncodingLength; curId++) Write("_");
            }


            //  Algorithm end result
            ForegroundColor = ConsoleColor.Gray;
            Write("\n\tEncrypted: ");
            for (var curChar = 0; curChar < encrypted.Length; curChar++)
            {
                ForegroundColor = curChar % (maxEncodingLength + 1) == 0 ? ConsoleColor.DarkCyan : ConsoleColor.Gray;
                Write(encrypted[curChar]);
            }
            ForegroundColor = ConsoleColor.Gray;
        }
        static private void RE5_DecryptingInfo(Int32[] buffer, Int32 exLength, Int32 maxEncodingLength, Int16[] shifts, Int32 shCount, Int32[] ids, Int32[] decodedIds, string encrypted, string decrypted, Int32 messageLength)
        {
            //  margin = for ids, bf = buffer, sh = shifts, el = externalAlphabet.Length, sz = message length, en = parsed encoding
            Int32 margin   = DigitsAmount(ids.Max()),        marginEn = DigitsAmount(decodedIds.Max() - buffer.Min());
            Int32 marginSz = DigitsAmount(messageLength),    marginEl = DigitsAmount(exLength);
            Int32 marginBf = DigitsAmount(decodedIds.Max()), marginSh = DigitsAmount(shifts.Max());


            //---  First character (its encryption is a bit different so we do it manually)
            Write("\n\t");
            for (var ext = 1; ext < marginSz; ext++) Write(" ");
            Write("0] total:  ");

            for (var ext = DigitsAmount(decodedIds[0]); ext < marginBf; ext++) Write(" ");
            Write(decodedIds[0] + " (");

            for (var ext = DigitsAmount(decodedIds[0] % exLength); ext < marginEl; ext++) Write(" ");
            ForegroundColor = ConsoleColor.Magenta;
            Write(decodedIds[0] % exLength);
            ForegroundColor = ConsoleColor.Gray;
            Write(") = ");

            for (var ext = DigitsAmount(decodedIds[0] - buffer[0]); ext < marginEn; ext++) Write(" ");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(decodedIds[0] - buffer[0]);
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");

            for (var ext = DigitsAmount(shifts[0]); ext < marginSh; ext++) Write(" ");
            ForegroundColor = shifts[0] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
            Write(shifts[0]);
            ForegroundColor = ConsoleColor.Gray;
            Write(" + ");

            for (var ext = DigitsAmount(ids[0]); ext < margin; ext++) Write(" ");
            ForegroundColor = ConsoleColor.Green;
            Write(ids[0]);
            ForegroundColor = ConsoleColor.Gray;

            for (var ext = 0; ext < marginBf + margin * 2; ext++) Write(" ");
            Write("   out: ");
            ForegroundColor = ConsoleColor.DarkCyan;
            Write(decrypted[0]);
            ForegroundColor = ConsoleColor.Gray;



            //---  Rest of the message
            for (var curId = 1; curId < messageLength; curId++)
            {
                Write("\n\t");
                for (var ext = DigitsAmount(curId); ext < marginSz; ext++) Write(" ");
                Write(curId + "] total:  ");

                for (var ext = DigitsAmount(decodedIds[curId]); ext < marginBf; ext++) Write(" ");
                Write(decodedIds[curId] + " (");

                for (var ext = DigitsAmount(decodedIds[curId] % exLength); ext < marginEl; ext++) Write(" ");
                ForegroundColor = ConsoleColor.Magenta;
                Write(decodedIds[curId] % exLength);
                ForegroundColor = ConsoleColor.Gray;
                Write(") = ");

                for (var ext = DigitsAmount(decodedIds[curId] - buffer[curId]); ext < marginEn; ext++) Write(" ");
                ForegroundColor = ConsoleColor.DarkGray;
                Write(decodedIds[curId] - buffer[curId]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" - ");

                for (var ext = DigitsAmount(shifts[curId % shCount]); ext < marginSh; ext++) Write(" ");
                ForegroundColor = shifts[curId % shCount] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
                Write(shifts[curId % shCount]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" + ");

                for (var ext = DigitsAmount(ids[curId * (maxEncodingLength + 1)]); ext < margin; ext++) Write(" ");
                ForegroundColor = curId % 2 == 0 ? ConsoleColor.Green : ConsoleColor.DarkYellow;
                Write(ids[curId * (maxEncodingLength + 1)]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" - ");

                for (var ext = DigitsAmount(decodedIds[curId - 1]); ext < marginBf; ext++) Write(" ");
                ForegroundColor = curId % 2 == 0 ? ConsoleColor.DarkYellow : ConsoleColor.Green;
                Write(decodedIds[curId - 1]);
                ForegroundColor = ConsoleColor.Gray;

                for (var ext = 0; ext < margin * 2; ext++) Write(" ");
                Write("out: ");
                ForegroundColor = ConsoleColor.DarkCyan;
                Write(decrypted[curId]);
                ForegroundColor = ConsoleColor.Gray;
            }


            //  Original message
            Write("\n\tOriginal:  ");
            for (var curChar = 0; curChar < encrypted.Length; curChar++)
            {
                ForegroundColor = curChar % (maxEncodingLength + 1) == 0 ?
                    ForegroundColor = (curChar / (maxEncodingLength + 1)) % 2 == 0 ?
                    ConsoleColor.Green : ConsoleColor.DarkYellow
                    : ConsoleColor.Gray;

                Write(encrypted[curChar]);
            }
            ForegroundColor = ConsoleColor.Gray;


            //  Algorithm result
            Write("\n\tDecrypted: ");
            for (var curChar = 0; curChar < decrypted.Length; curChar++)
            {
                ForegroundColor = ConsoleColor.DarkCyan;
                Write(decrypted[curChar]);

                ForegroundColor = ConsoleColor.DarkGray;
                for (var curId = 0; curId < maxEncodingLength; curId++) Write("_");
            }


            //  Only readable end result
            ForegroundColor = ConsoleColor.Gray;
            Write("\n\tReadable:  " + decrypted);
        }





        static private Int32 DigitsAmount(Int32 number)
        {
            if (number >= 0) return DigitsInPositive(number);
            else return number > -10 ? 1 : number > -100 ? 2
                      : number > -1_000 ? 3 : number > -10_000 ? 4
                      : number > -100_000 ? 5 : number > -1_000_000 ? 6
                      : number > -10_000_000 ? 7 : number > -100_000_000 ? 8
                      : number > -1_000_000_000 ? 9 : 10;
        }
        static private Int32 DigitsInPositive(Int32 posNumber)
            => posNumber < 10 ? 1 : posNumber < 100 ? 2 : posNumber < 1_000 ? 3
            : posNumber < 10_000 ? 4 : posNumber < 100_000 ? 5 : posNumber < 1_000_000 ? 6
            : posNumber < 10_000_000 ? 7 : posNumber < 100_000_000 ? 8 : posNumber < 1_000_000_000 ? 9 : 10;
    }
}