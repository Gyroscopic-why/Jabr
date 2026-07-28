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


            binKey.Noisifier.Set.Default([0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15]);
            binKey.Noisifier.Next();

            binKey.Noisifier.RandomReseedInterval = 8192;
            binKey.Noisifier.settings.KeepOriginalFileExtension = false;


            reKey.Noisifier.settings.InitFromPreset(MasqueradePreset.HTTP_2_gRPC);
            reKey.Noisifier.Next();

            reKey.Noisifier.RandomReseedInterval = 8192;
            reKey.Noisifier.settings.KeepOriginalFileExtension = false;


            string fileContent = "Aboba\nhello world\n228 baobab ,.!?";
            double valueBias = 1.4, powerBias = 1.33;

            Int32 maxNonEntropy = 0, extendBuffer;
            for (var i = 0; i < 1_0; i++)
            {
                Write("\n\tAttempt: " + ++attemptCount);
                string encrypted = RE5.Encrypt.Text(aboba, reKey, true);


                Write($"\n\tReKey: {reKey.ExAlphabet}, PrNoise: {reKey.Noisifier.PrimaryNoise}, CplxNoise: {reKey.Noisifier.ComplexNoise}");
                Write("\n\tInitial: " + encrypted);

                Write("\n\tAdding noise to data..");

                string noised = Noise.Internal.AddFastText(encrypted, reKey.Noisifier, "");
                string denoised = Noise.Remove.Text(noised, reKey, true);

                Write("\n\tNoised:  ");
                Int32 count = 0, nonEntropy = 0, thisMaxNonEntropy = 0;
                bool newWorst = false, noiseAtTheEnd = false;


                for (var j = 0; j < noised.Length; j++)
                {
                    if (j % (Int32) reKey.Noisifier.settings.ChunkSize == 0)
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
                    else
                    {
                        ForegroundColor = ConsoleColor.DarkGray;
                        nonEntropy = 0;
                    }

                    Write(noised[j]);
                    BackgroundColor = ConsoleColor.Black;
                }
                ForegroundColor = ConsoleColor.Gray;


                Write("\n\tDnoised: " + denoised);

                Write("\n\tMatches: ");
                for (var j = 0; j < Math.Min(denoised.Length, encrypted.Length); j++)
                {
                    if (denoised[j] == encrypted[j])
                         ForegroundColor = ConsoleColor.Green;
                    else ForegroundColor = ConsoleColor.DarkGray;

                    Write(denoised[j]);
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




                ForegroundColor = ConsoleColor.Gray;
                Write("\n\tInitial: " + encrypted);

                reKey.ChunkSize = ChunkSize.KByte8;
                binKey.ChunkSize = ChunkSize.Byte512;
                Write("\n\tSAFEENC: " + RE5.Encrypt.Text(aboba, reKey, true));
                Write("\n\tSAFEDEC: " + RE5.Decrypt.Text(RE5.Encrypt.Text(aboba, reKey, true), reKey, true));

                extendBuffer = EXTEND;
                EXTEND = noised.Length;

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


                reKey.Next();
                binKey.Next();

                EXTEND = extendBuffer;

                if (Int32.TryParse(ReadLine(), out Int32 newExtendBuffer) && (
                    newExtendBuffer > encrypted.Length || newExtendBuffer == 0))
                {
                    EXTEND = newExtendBuffer;
                    maxNonEntropy = 0;
                }

                Clear();
            }


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
                    if (attempt % 2 == 0)
                    {
                        initial.Next();

                        Write("\n\t\t\tEXPORT     - ");
                        timer.Start();

                        for (var i = 0; i < iterationsPerAttempt; i++)
                            exportBuffer = initial.ExportAsBinary();

                        timer.Stop();
                        ms1.Add(timer.ElapsedMilliseconds);
                    }
                    else
                    {
                        Write("\n\t\t\tIMPORT     - ");
                        timer.Start();

                        for (var i = 0; i < iterationsPerAttempt; i++)
                            copy.ImportFromBinary(exportBuffer, true);

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
        }
    }
}