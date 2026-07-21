using AVcontrol;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Console;



namespace JabrAPI
{
    internal class Program
    {
        static void Main()
        {
            SecureRandom random = new(128);
            RE5.EncryptionKey reKey = new(true);
            RE5.BinaryKey binKey = new(true);
            string aboba = "aboba baobab";
            List<Byte> lolinit = [0, 1, 2, 3, 3, 3, 2, 1, 0];
            Int32 EXTEND = 128, attemptCount = 0;

            reKey.KeepOriginalFileExtension = false;
            binKey.KeepOriginalFileExtension = false;

            //reKey.ChunkSize = TextChunkSize.cTEST;
            //binKey.ChunkSize = BinaryChunkSize.bTEST;



            //Write("\n\tEnc: " + RE5.Encrypt.Text(aboba, reKey, true));
            //Write("\n\tDec: " + RE5.Decrypt.Text(RE5.Encrypt.Text(aboba, reKey, true), reKey, true));

            //Write("\n\tBin-enc: ");
            //List<Byte> bbinenc = RE5.Encrypt.Binary(lolinit, binKey, true);
            //foreach (Byte b in bbinenc) Write(b + " ");

            //Write("\n\tBin-dec: ");
            //List<Byte> bbindec = RE5.Decrypt.Binary(bbinenc, binKey, true);
            //foreach (Byte b in bbindec) Write(b + " ");

            //ReadKey();


            binKey.Noisifier.Set.Default([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]);
            binKey.Noisifier.Next();

            binKey.Noisifier.RandomReseedInterval = 8192;
            binKey.Noisifier.settings.KeepOriginalFileExtension = false;



            //reKey.Noisifier.Set.Default(['A', 'b', 'o', 'a', '\r', '\n', 'h', 'e', 'l', ' ', 'w', 'r', 'd', '2', '8', ',', '.', '?', '!',
            //'i', 'f', 'y', 'u', 'n', 'g', 't', 's', '-', 'T', 'H', 'E', 'c', 'm', 'k', 'N', 'O', 'I', 'S', 'F', 'R']);
            reKey.Noisifier.settings.InitFromPreset(MasqueradePreset.HTTP_2_gRPC);
            reKey.Noisifier.Next();

            reKey.Noisifier.RandomReseedInterval = 8192;
            reKey.Noisifier.settings.KeepOriginalFileExtension = false;



            string fileContent = "Aboba\nhello world\n228 baobab ,.!?";



            //Write("\n\tInitial: " + fileContent);
            //Write("\n\tAdding Noise.TextToBinary: ");
            //List<Byte> result = Noise.Add.TextToBinary_Utf8(fileContent, reKey, true);
            //foreach (byte b in result) Write(b + " ");

            //Write("\n\n\tDecoded noised: " + FromBinary.Utf8(result));


            //Write("\n\tDenoised: " + Noise.Remove.TextFromBinary_Utf8(result, reKey, true));




            //ReadKey();

            //string t1filePath = "";
            //string t1fileName = "Test6.txt";
            ////string encFileName = "Test2.enc-re5";
            ////string decFileName = "Test2.dec-re5";
            //string t1noisedFileName = "Test6.noisedv5";
            //string t1denoisFileName = "Test6.dnoisev5";


            //Write("\n\tNoising file in process: ");
            //Noise.Add.TextToBinaryFile_Utf8(t1filePath, t1fileName, reKey, true);
            //Write("Done!");

            //Write("\n\tDeNoising file in process: ");
            //Noise.Remove.TextFromBinaryFile_Utf8(t1filePath, t1noisedFileName, reKey, true);
            //Write("Done!");

            //ReadKey();





            //string filePath = "";
            //string fileName = "Test5.aboba";
            //string encFileName = "Test2.enc-re5";
            //string decFileName = "Test2.dec-re5";
            //string noisedFileName = "Test5.noisedv5";
            //string denoisFileName = "Test5.dnoisev5";


            //string tfileName = "Test3.txt";
            //string encFileName = "Test2.enc-re5";
            //string decFileName = "Test2.dec-re5";
            //string tnoisedFileName = "Test3.noisedv5";
            //string tdenoisFileName = "Test3.dnoisev5";
            //Write($"\n\tReKey: {reKey.ExAlphabet}, PrNoise: {reKey.Noisifier.PrimaryNoise}, CplxNoise: {reKey.Noisifier.ComplexNoise}");

            //for (var ii = 0; ii < 1000; ii++)
            //{

                //Write($"\n\tDeleting old {encFileName} & {decFileName} file\n");
                //if (File.Exists(Path.Combine(filePath, encFileName)))
                //    File.Delete(Path.Combine(filePath, encFileName));
                //if (File.Exists(Path.Combine(filePath, decFileName)))
                //    File.Delete(Path.Combine(filePath, decFileName));

                //Write($"\n\tDeleting old {noisedFileName} & {denoisFileName} file\n");
                //if (File.Exists(Path.Combine(filePath, noisedFileName)))
                //    File.Delete(Path.Combine(filePath, noisedFileName));
                //if (File.Exists(Path.Combine(filePath, denoisFileName)))
                //    File.Delete(Path.Combine(filePath, denoisFileName));
                //Write($"\n\tDeleting old {tnoisedFileName} & {tdenoisFileName} file\n");
                //if (File.Exists(Path.Combine(filePath, tnoisedFileName)))
                //    File.Delete(Path.Combine(filePath, tnoisedFileName));
                //if (File.Exists(Path.Combine(filePath, tdenoisFileName)))
                //    File.Delete(Path.Combine(filePath, tdenoisFileName));

                //ReadKey();

                //Stopwatch timerN = new();
                //timerN.Start();
                //Write("\n\tNoising file in process: ");
                //Noise.Add.TextFile(filePath, tfileName, reKey, true);
                //Noise.Add.BinaryFile(filePath, fileName, binKey, true);
                //timerN.Stop();

                //Write("Done: " + ii + " (" + timerN.ElapsedMilliseconds + ")\n");


                //timerN.Reset();
                //timerN.Start();
                //Write("\n\tDeNoising file in process: ");
                //Noise.Remove.TextFile(filePath, tnoisedFileName, reKey, true);
                //Noise.Remove.BinaryFile(filePath, noisedFileName, binKey, true);
            //    timerN.Stop();

            //    Write("Done: " + ii + " (" + timerN.ElapsedMilliseconds + ")\n");

            //    ReadKey();
            //    if (ii > 100) ReadKey();
            //}
            //ReadLine();

            //Write(reKey.ExportAsString() + "\n");
            //fileContent = RE5.Encrypt.Text(fileContent, reKey, true);
            //Write("\n\tExpected behaviour: " + fileContent);
            //Write("\n\tExpected behaviour: ");
            //lolinit = RE5.Encrypt.TextToBinary_Utf8(fileContent, reKey, true);
            //foreach (var b in lolinit) Write(b + " ");
            //Write("\n\tEncrypting file in process: " + RE5.Encrypt.TextFile(filePath, fileName, reKey, true));
            //Write("\n\tEncrypting file in process: " + RE5.Encrypt.TextToBinaryFile_Utf8(filePath, fileName, reKey, true));

            //ReadKey();

            //Write("\n\t(Decoded binary): " + FromBinary.Utf8(lolinit));
            //Write("\n\tExpected binary:  " + RE5.Encrypt.Text(fileContent, reKey, true));
            //Write("\n\tExpected decrypt: " + RE5.Decrypt.Text(RE5.Encrypt.Text(fileContent, reKey, true), reKey, true));
            //Write("\n\tExpected behaviour: " + RE5.Decrypt.Text(fileContent, reKey, true));
            //Write("\n\tDecrypting file in process: " + RE5.Decrypt.TextFile(filePath, encFileName, reKey, true));
            //Write("\n\tExpected behaviour: " + RE5.Decrypt.TextFromBinary_Utf8(lolinit, reKey, true));
            //Write("\n\tDecrypting file in process: " + RE5.Decrypt.TextFromBinaryFile_Utf8(filePath, encFileName, reKey, true));


            //Write("\n\tExpected behaviour: ");
            //lolinit = RE5.Encrypt.Binary(lolinit, binKey, true);
            //foreach (var b in lolinit) Write(b + " ");

            //Write("\n\tEncrypting file in process: " + RE5.Encrypt.BinaryFile(filePath, fileName, binKey, true));

            //ReadKey();

            //Write("\n\tExpected behaviour: ");
            //lolinit = RE5.Decrypt.Binary(lolinit, binKey, true);
            //foreach (var b in lolinit) Write(b + " ");

            //Write("\n\tDecrypting file in process: " + RE5.Decrypt.BinaryFile(filePath, encFileName, binKey, true));

            //ReadLine();


            //reKey.ChunkSize = TextChunkSize.cTEST;
            //reKey.Noisifier.settings.ChunkSize = TextChunkSize.cTEST;

            //binKey.ChunkSize = BinaryChunkSize.bTEST;
            //binKey.Noisifier.settings.ChunkSize = BinaryChunkSize.bTEST;

            //lolinit = RE5.Encrypt.TextToBinary_Utf8(fileContent + fileContent, reKey, true);

            //aboba += fileContent + aboba + fileContent;
            double valueBias = 1.4, powerBias = 1.33;

            Int32 maxNonEntropy = 0, extendBuffer;
            for (var i = 0; i < 1_0; i++)
            {
                Write("\n\tAttempt: " + ++attemptCount);
                //reKey.Set.Sensitive.ExAlphabet("Xv+");
                //reKey.Noisifier.SetDefault(['X', 'x', 'V', 'v', 'Х', 'х', '+', ',', '.']);
                //reKey.Set.Default(162, "", 8, ".,");
                //reKey.Noisifier.Set.Default([',', '.']);
                //reKey.Noisifier.Next();


                //reKey.Set.Sensitive.PrAlphabet(DEFAULT.CHARACTERS.WITH_SPACE);
                //reKey.Set.Sensitive.ExAlphabet("ACH");
                //reKey.Noisifier.Set.Sensitive.PrNoise("`");
                //reKey.Noisifier.Set.Sensitive.CplxNoise("+");





                //reKey.Set.Sensitive.Shifts([0]);

                string encrypted = RE5.Encrypt.Text(aboba, reKey, true);
                //List<Byte> bincrypted = RE5.Encrypt.Binary(lolinit, binKey, true);
                //List<Byte> bincrypted = RE5.Encrypt.TextToBinary_Utf16(aboba, reKey, true);

                //EXTEND = random.Next(encrypted.Length + 2, encrypted.Length * 5);
                //reKey.Noisifier.settings.OutputLength = EXTEND;
                //binKey.Noisifier.settings.OutputLength = EXTEND;


                Write($"\n\tReKey: {reKey.ExAlphabet}, PrNoise: {reKey.Noisifier.PrimaryNoise}, CplxNoise: {reKey.Noisifier.ComplexNoise}");
                Write("\n\tInitial: " + encrypted);
                //Write("\n\tInitial: ");
                //for (var j = 0; j < bincrypted.Count; j++)
                //    Write(bincrypted[j] + " ");

                Write("\n\tAdding noise to data..");

                //string noised = Noise.Add.Text(encrypted, reKey, true);
                string noised = Noise.Internal.AddFastText(encrypted, reKey.Noisifier, "");
                //List<Byte> binoised = Noise.Add.Binary(bincrypted, binKey, true);
                //List<Byte> binoised = RE5.Encrypt.WithNoise.Binary(lolinit, binKey, true);
                //List<Byte> binoised = RE5.Encrypt.WithNoise.TextToBinary_Utf16(aboba, reKey, true);
                //List<Byte> binoised = Noise.Internal.AddFastBinary(bincrypted, binKey.Noisifier, []);

                string denoised = Noise.Remove.Text(noised, reKey, true);
                //List<Byte> bindenoised = Noise.Remove.Binary(binoised, binKey, true);

                Write("\n\tNoised:  ");
                Int32 count = 0, nonEntropy = 0, thisMaxNonEntropy = 0;
                bool newWorst = false, noiseAtTheEnd = false;


                for (var j = 0; j < noised.Length; j++)
                //for (var j = 0; j < binoised.Count; j++)
                {
                    if (j % (Int32) reKey.Noisifier.settings.ChunkSize == 0)
                    //if (j % (Int32)binKey.Noisifier.settings.ChunkSize == 0)
                        BackgroundColor = ConsoleColor.Blue;

                    if (!noiseAtTheEnd && noised[j] == encrypted[count])
                    {
                        ForegroundColor = ConsoleColor.Green;
                        count++;

                        if (count >= encrypted.Length)
                            noiseAtTheEnd = true;

                        nonEntropy++;
                        if (nonEntropy > maxNonEntropy)
                        {
                            maxNonEntropy = nonEntropy;
                            newWorst = true;
                        }
                        if (nonEntropy > thisMaxNonEntropy)
                            thisMaxNonEntropy = nonEntropy;
                    }
                    //if (!noiseAtTheEnd && binoised[j] == bincrypted[count])
                    //{
                    //    ForegroundColor = ConsoleColor.Green;
                    //    count++;

                    //    if (count >= bincrypted.Count)
                    //        noiseAtTheEnd = true;

                    //    nonEntropy++;
                    //    if (nonEntropy > maxNonEntropy)
                    //    {
                    //        maxNonEntropy = nonEntropy;
                    //        newWorst = true;
                    //    }
                    //    if (nonEntropy > thisMaxNonEntropy)
                    //        thisMaxNonEntropy = nonEntropy;
                    //}
                    else
                    {
                        ForegroundColor = ConsoleColor.DarkGray;
                        nonEntropy = 0;
                    }

                    Write(noised[j]);
                    //Write(binoised[j] + " ");
                    BackgroundColor = ConsoleColor.Black;
                }
                ForegroundColor = ConsoleColor.Gray;


                Write("\n\tDnoised: " + denoised);
                //Write("\n\tDnoised: ");
                //for (var j = 0; j < bindenoised.Count; j++)
                //    Write(bindenoised[j] + " ");



                Write("\n\tMatches: ");
                for (var j = 0; j < Math.Min(denoised.Length, encrypted.Length); j++)
                //for (var j = 0; j < Math.Min(bindenoised.Count, bincrypted.Count); j++)
                {
                    if (denoised[j] == encrypted[j])
                    //if (bindenoised[j] == bincrypted[j])
                         ForegroundColor = ConsoleColor.Green;
                    else ForegroundColor = ConsoleColor.DarkGray;

                    Write(denoised[j]);
                    //Write(bindenoised[j] + " ");
                }
                ForegroundColor = ConsoleColor.Red;
                Write
                (
                    denoised.AsSpan(
                        Math.Min(denoised.Length, encrypted.Length),
                        Math.Min
                        (
                            0,
                             denoised.Length - Math.Min
                            (denoised.Length, encrypted.Length)
                        )
                    )
                );
                Write
                (
                    encrypted.AsSpan(
                        Math.Min(denoised.Length, encrypted.Length),
                        Math.Min
                        (
                            0,
                             encrypted.Length - Math.Min
                            (denoised.Length, encrypted.Length)
                        )
                    )
                );



                //List<Byte> temp = bindenoised.GetRange(
                //        Math.Min(bindenoised.Count, bincrypted.Count),
                //        Math.Min
                //        (
                //            0,
                //             bindenoised.Count - Math.Min
                //            (bindenoised.Count, bincrypted.Count)
                //        )
                //    );
                //for (var j = 0; j < temp.Count; j++)
                //    Write(temp[j]);

                //temp = bincrypted.GetRange(
                //        Math.Min(binoised.Count, bincrypted.Count),
                //        Math.Min
                //        (
                //            0,
                //             bincrypted.Count - Math.Min
                //            (binoised.Count, bincrypted.Count)
                //        )
                //    );

                //for (var j = 0; j < temp.Count; j++)
                //    Write(temp[j] + " ");



                ForegroundColor = ConsoleColor.Gray;
                Write("\n\tInitial: " + encrypted);
                //Write("\n\tInitial: ");
                //for (var j = 0; j < bincrypted.Count; j++)
                //    Write(bincrypted[j] + " ");

                reKey.ChunkSize = ChunkSize.KByte8;
                binKey.ChunkSize = ChunkSize.Byte512;
                Write("\n\tSAFEENC: " + RE5.Encrypt.Text(aboba, reKey, true));
                Write("\n\tSAFEDEC: " + RE5.Decrypt.Text(RE5.Encrypt.Text(aboba, reKey, true), reKey, true));
                //List<Byte> safeBytes = RE5.Encrypt.Binary(lolinit, binKey, true);
                //List<Byte> safeBytes = RE5.Encrypt.TextToBinary_Utf16(aboba, reKey, true);
                //Write("\n\tSAFEENC: ");
                //for (var ij = 0; ij < safeBytes.Count; ij++) Write(safeBytes[ij] + " ");
                //List<Byte> safeByteDec = RE5.Decrypt.Binary(safeBytes, binKey, true);
                //Write("\n\tSAFEDEC: ");
                //for (var ij = 0; ij < safeByteDec.Count; ij++) Write(safeByteDec[ij] + " ");
                //Write("\n\tSAFEDEC: " + RE5.Decrypt.TextFromBinary_Utf16(safeBytes, reKey, true));

                //reKey.ChunkSize = TextChunkSize.cTEST;
                //binKey.ChunkSize = BinaryChunkSize.bTEST;
                //Write("\n\tDecrypt: " + RE5.Decrypt.Text(encrypted, reKey, true));
                //Write("\n\tDecrypt: " + RE5.Decrypt.TextFromBinary_Utf16(bincrypted, reKey, true));

                //List<Byte> bindec = RE5.Decrypt.Binary(bincrypted, binKey, false);
                //Write("\n\tDecrypt: ");
                //for (var j = 0; j < bindec.Count; j++)
                //    Write(bindec[j] + " ");

                //reKey.ChunkSize = TextChunkSize.cTEST;
                //binKey.ChunkSize = BinaryChunkSize.bTEST;


                extendBuffer = EXTEND;
                EXTEND = noised.Length;
                //EXTEND = binoised.Count;

                Write($"\n\tNonEntropy: {thisMaxNonEntropy}(" +
                    $"{Math.Ceiling
                        (
                            Math.Pow
                            (
                                encrypted.Length * valueBias /
                                (
                                    EXTEND - encrypted.Length + 1
                                ),
                                powerBias
                            )
                        )}), MaxNon: {maxNonEntropy}" +
                    $"\n\tInitial: {encrypted.Length}, extended: {noised.Length}({EXTEND})[{extendBuffer}]" +
                    $"\n\tAvgRatio: {(double)EXTEND / encrypted.Length}, " +
                    $"value: {(double)encrypted.Length / (EXTEND - encrypted.Length + 1)}" +
                    $"\n\tBiased ({valueBias}; {powerBias}) value: {encrypted.Length * powerBias / (EXTEND - encrypted.Length + 1)}" +
                    $"\n\n\tEnter new EXTEND length: ");



                //Write($"\n\tNonEntropy: {thisMaxNonEntropy}(" +
                //    $"{Math.Ceiling
                //        (
                //            Math.Pow
                //            (
                //                bincrypted.Count * valueBias /
                //                (
                //                    EXTEND - bincrypted.Count + 1
                //                ),
                //                powerBias
                //            )
                //        )}), MaxNon: {maxNonEntropy}" +
                //    $"\n\tInitial: {bincrypted.Count}, extended: {binoised.Count}({EXTEND})[{extendBuffer}]" +
                //    $"\n\tAvgRatio: {(double)EXTEND / bincrypted.Count}, " +
                //    $"value: {(double)bincrypted.Count / (EXTEND - bincrypted.Count + 1)}" +
                //    $"\n\tBiased ({valueBias}; {powerBias}) value: {bincrypted.Count * powerBias / (EXTEND - bincrypted.Count + 1)}" +
                //    $"\n\n\tEnter new EXTEND length: ");



                reKey.Next();
                binKey.Next();
                //if (
                //newWorst ||
                //thisMaxNonEntropy > Math.Ceiling
                //    (
                //        Math.Pow
                //        (
                //            encrypted.Length * valueBias /
                //            (
                //                EXTEND - encrypted.Length + 1
                //            ),
                //            powerBias
                //        )
                //    )
                //        ) ReadLine();
                //else ReadKey();

                EXTEND = extendBuffer;

                if (Int32.TryParse(ReadLine(), out Int32 newExtendBuffer) && (
                    newExtendBuffer > encrypted.Length || newExtendBuffer == 0))
                {
                    EXTEND = newExtendBuffer;
                    maxNonEntropy = 0;
                }
                //if (Int32.TryParse(ReadLine(), out Int32 newExtendBuffer) && (
                //    newExtendBuffer > encrypted.Length || newExtendBuffer == 0))
                //{
                //    EXTEND = newExtendBuffer;
                //    maxNonEntropy = 0;
                //}

                Clear();
            }


            //Noisifier initial2 = new(['1', '2', '3'], true);
            //Noisifier copy2 = new();

            //RE5.EncryptionKey initial2 = new(true);
            //RE5.EncryptionKey copy2 = new(false);

            //List<Byte> export = initial2.ExportAsBinary();

            //Write("\n\t\t\tInitial: ");
            //foreach (var infoByte in export)
            //    Write(infoByte + " ");

            //copy2.ImportFromBinary(export, true);
            //List<Byte> new_import = copy2.ExportAsBinary();
            //Write("\n\t\t\tImport:  ");
            //foreach (var infoByte in new_import)
            //    Write(infoByte + " ");

            //string export = initial2.ExportAsString();

            //Write("\n\t\t\tInitial: " + export);

            //copy2.ImportFromString(export, true);
            //string new_import = copy2.ExportAsString();
            //Write("\n\t\t\tImport:  " + new_import);

            //ReadKey();







            RE5.BinaryKey initial = new(true);
            RE5.BinaryKey copy = new(false);
            Stopwatch timer = new();

            Byte[] exportBuffer = [];

            for (var hide = 0; hide < 1; hide++)
            {
                List<Int64> ms1 = [], ms2 = [];
                const Int64 totalAttempts = 10, iterationsPerAttempt = 100_000;
                Write($"\n\n\n\t\t[i]  - Starting benchmark of {totalAttempts * iterationsPerAttempt / 1_000}k Key Export & Import");

                for (var attempt = 0; attempt < totalAttempts; attempt++)
                {
                    //if (File.Exists(Path.Combine(filePath, noisedFileName)))
                    //    File.Delete(Path.Combine(filePath, noisedFileName));
                    //if (File.Exists(Path.Combine(filePath, denoisFileName)))
                    //    File.Delete(Path.Combine(filePath, denoisFileName));

                    if (attempt % 2 == 0)
                    {
                        initial.Next();

                        Write("\n\t\t\tEXPORT     - ");
                        timer.Start();

                        for (var i = 0; i < iterationsPerAttempt; i++)
                            exportBuffer = initial.ExportAsBinary();
                            //Noise.Add.TextFile(filePath, fileName, reKey, true);

                        timer.Stop();
                        ms1.Add(timer.ElapsedMilliseconds);
                    }
                    else
                    {
                        Write("\n\t\t\tIMPORT     - ");
                        timer.Start();

                        for (var i = 0; i < iterationsPerAttempt; i++)
                            copy.ImportFromBinary(exportBuffer, true);
                            //Noise.Add.TextFile(filePath, fileName, reKey, true);

                        timer.Stop();
                        ms2.Add(timer.ElapsedMilliseconds);
                    }

                    string elapsed = ((double)timer.ElapsedMilliseconds / 1000).ToString().Replace(",", ".");
                    Write($"\tAttempt {attempt + 1})\t Exp & Imp     {iterationsPerAttempt / 1_000}k: {elapsed}");
                    timer.Reset();


                    if (attempt % 2 == 1)
                    {
                        Write("\n\t\t\tVALIDATING - ");

                        Byte[] import = copy.ExportAsBinary();

                        if (import.Length != exportBuffer.Length)
                        {
                            ForegroundColor = ConsoleColor.Green;
                            Write("\tFAILURE! See differences:");
                            ForegroundColor = ConsoleColor.Gray;

                            Write("\n\t\t\tInitial: ");
                            foreach (var infoByte in exportBuffer)
                                Write(infoByte + " ");

                            Write("\n\t\t\tImport:  ");
                            foreach (var infoByte in import)
                                Write(infoByte + " ");

                            ReadKey();
                        }
                        else
                        {
                            bool doesMatch = true;

                            for (var i = 0; i < exportBuffer.Length; i++)
                            {
                                if (import[i] != exportBuffer[i])
                                {
                                    ForegroundColor = ConsoleColor.Green;
                                    Write("\tFAILURE! See differences:");
                                    ForegroundColor = ConsoleColor.Gray;

                                    Write("\n\t\t\tInitial: ");
                                    foreach (var infoByte in exportBuffer)
                                        Write(infoByte);

                                    Write("\n\t\t\tImport:  ");
                                    foreach (var infoByte in import)
                                        Write(infoByte);

                                    i += exportBuffer.Length;
                                    doesMatch = false;

                                    ReadKey();
                                }
                            }
                            if (doesMatch)
                            {
                                ForegroundColor = ConsoleColor.Green;
                                Write("\tSUCCESS! Import matches export");
                                ForegroundColor = ConsoleColor.Gray;
                            }
                        }
                    }
                }

                double min1 = (double)ms1.Min() / 1000, max1 = (double)ms1.Max() / 1000;
                double min2 = (double)ms2.Min() / 1000, max2 = (double)ms2.Max() / 1000;
                double sum1 = (double)ms1.Sum() / 1000, sum2 = (double)ms2.Sum() / 1000;
                double avg1 = ms1.Average() / 1000, avg2 = ms2.Average() / 1000, avgGlobal = (avg1 + avg2) / 2;

                bool isFirst1 = min1 < min2, isFirst2 = max1 > max2;

                string i1 = min1.ToString().Replace(",", "."), a1 = max1.ToString().Replace(",", ".");
                string i2 = min2.ToString().Replace(",", "."), a2 = max2.ToString().Replace(",", ".");
                string v1 = avg1.ToString().Replace(",", "."), v2 = avg2.ToString().Replace(",", "."), v3 = avgGlobal.ToString().Replace(",", ".");
                string s1 = sum1.ToString().Replace(",", "."), s2 = sum2.ToString().Replace(",", "."), s3 = (sum1 + sum2).ToString().Replace(",", ".");

                Write("\n\n\t\t\tBenchmark finished");
                Write($"\n\t\tEXPORT   - {iterationsPerAttempt / 1_000}k operations   interval: {i1}-{a1}, average: {v1}");
                Write($"\n\t\tIMPORT   - {iterationsPerAttempt / 1_000}k operations   interval: {i2}-{a2}, average: {v2}");
                Write($"\n\t\tGLOBAL   - {iterationsPerAttempt / 1_000}k operations   interval: ");

                if (isFirst1) Write($"{i1}-");
                else Write($"{i2}-");
                if (isFirst2) Write($"{a1}, average: {v3}");
                else Write($"{a2}, average: {v3}");

                Write("\n\n\n\t\t\tTotal time elapsed for");
                Write($"\n\t\tEXPORT    - {totalAttempts * iterationsPerAttempt / 1_000}k operations: {s1}\t[{(Int32)(sum1 / (sum1 + sum2) * 100)} %]");
                Write($"\n\t\tIMPORT    - {totalAttempts * iterationsPerAttempt / 1_000}k operations: {s2}\t[{(Int32)(sum2 / (sum1 + sum2) * 100)} %]");
                Write($"\n\t\tGLOBAL    - {totalAttempts * iterationsPerAttempt / 1_000}k operations: {s3}");

                ReadKey();
            }










            //Write("\n\tExported:\n");

            //List<Byte> iniExp = initial.ExportAsBinary();
            //for (var i = 0; i < iniExp.Count; i++) Write(iniExp[i] + " ");
            //Write("\n\n");



            //copy.ImportFromBinary(iniExp, true);

            //Write("\n\tImport (COPY) - Exported:\n");

            //List<Byte> copExp = copy.ExportAsBinary();
            //for (var i = 0; i < copExp.Count; i++) Write(copExp[i] + " ");
            //Write("\n\n");





            //Write("\n\tString Exported:\n" + initial.ExportAsString());

            //copy.ImportFromString(initial.ExportAsString(), true);

            //Write("\n\tImport (COPY) - String Exported:\n" + copy.ExportAsString());

            //ReadKey();


            //TestBenchmarker.DecryptBenchmark();
            //Write("\n\n\n\t\t\tDecrypt benchmark finished. Press any key to launch full benchmark suite");
            //ReadKey();

            //TestBenchmarker.Run();

            //RE5.EncryptionKey reKey1 = new RE5.EncryptionKey(2), reKey2 = new RE3.EncryptionKey(2);
            //reKey1.Next();
            //reKey1.GenerateRandomShifts(0);
            //reKey2.Next();
            //reKey2.GenerateRandomShifts(0);

            //Write("\n\treKey1: " + reKey1.ExportAsString() + "\n");
            //Write("\n\treKey2: " + reKey2.ExportAsString() + "\n");

            //RE5.EncryptionKey reKey = new(true);
            //string initial = "aboba aboba aboba";
            //Write("\nInitial message:      " + initial);

            //string enc1 = RE4.Encrypt(initial, reKey1);
            //Write("\nEncrypted with key 1: " + enc1);

            //string enc2 = RE4.Encrypt(enc1, reKey2);
            //Write("\nEncrypted with 1 & 2: " + enc2);

            //string dec1 = RE4.Decrypt(enc2, reKey1);
            //Write("\nEnc12 decrypted w/ 1: " + dec1);

            //string dec2 = RE4.Decrypt(dec1, reKey2);
            //Write("\nFully decrypted w1,2: " + dec2);

            //string enc21 = RE4.Decrypt(enc2, reKey2);
            //Write("\n\nCorrect decrypted enc2 w2: " + enc21);

            //string dec21 = RE4.Decrypt(enc21, reKey1);
            //Write("\nCorrect decrypted fully: " + dec21);

            //ReadKey();



            //Write("\n\n\t\t\tStarting benchmark...");
            //for (var i = 1; i < 10_000; i++)
            //{
            //    reKey.Next();

            //    string dec5 = RE5.Decrypt.FastText(RE5.Encrypt.FastText(initial, reKey), reKey);
            //    if (dec5 != initial)
            //    {
            //        Write("\n\n\t\t\tRE5: Something went wrong at iteration " + i);
            //        Write("\n\tExpected: " + initial);
            //        Write("\n\tGot: " + dec5);
            //        Write("\n\tReKey: " + reKey);
            //        ReadKey();
            //        return;
            //    }

            //    //string dec4 = RE5.Decrypt.FastText(RE5.Encrypt.FastText(initial, reKey), reKey);
            //    //if (dec4 != initial)
            //    //{
            //    //    Write("\n\n\t\t\tRE4: Something went wrong at iteration " + i);
            //    //    Write("\n\tExpected: " + initial);
            //    //    Write("\n\tGot: " + dec4);
            //    //    Write("\n\tReKey: " + reKey);
            //    //    ReadKey();
            //    //    return;
            //    //}

            //    if (i % 1000 == 0)
            //    {
            //        Write("\n\t\tCompleted iteration " + i + ", ");
            //        ForegroundColor = ConsoleColor.Green;
            //        Write("no problems so far!");
            //        ForegroundColor = ConsoleColor.Gray;
            //    }
            //}

            //Write("\n\n\t\tBenchmark finished, no problems :) ");
            //ReadKey();
        }
    }
}