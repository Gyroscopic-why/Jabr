using System;
using System.Collections.Generic;


using AVcontrol;
using JabrAPI.Template;



namespace JabrAPI
{
    static public partial class RE5
    {
        public partial class BinaryKey : IBinaryKey
        {
            public bool ImportFromBinary(List<Byte> exportData, bool throwExceptions = false)
                => ImportFromBinary(exportData.ToArray(), throwExceptions);
            public override bool ImportFromBinary(Byte[] exportData, bool throwExceptions = false)
            {
                try
                {
                    if (exportData.Length < 2)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data length is insufficient even for an empty noisifier",
                                nameof(exportData)
                            );
                        return false;
                    }

                    Int32 noisifierBytesCount = exportData[0];
                    if (exportData.Length < noisifierBytesCount + 1)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data length is insufficient for the specified PrimaryNoise in the imported noisifier\n" +
                                $"PrimaryNoiseCount: {noisifierBytesCount} from exportData[0]",
                                nameof(exportData)
                            );
                        return false;
                    }

                    noisifierBytesCount += exportData[noisifierBytesCount + 1];
                    if (exportData.Length < noisifierBytesCount + 2)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data length is insufficient for the exported noisifier in bytes count\n" +
                                $"Specified noisifier bytes: {noisifierBytesCount} from exportData[0] + exportData[" +
                                $"{noisifierBytesCount - exportData[noisifierBytesCount + 1]}",
                                nameof(exportData)
                            );
                        return false;
                    }


                    _noisifier.ImportFromBinary(exportData[0..(noisifierBytesCount + 2)], throwExceptions);
                    Byte[] onlyReKeyData = exportData[(noisifierBytesCount + 2)..];
                    

                    if (onlyReKeyData.Length < 10)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data length is insufficient even for an empty BinaryKey",
                                nameof(onlyReKeyData)
                            );
                        return false;
                    }
                    Int32 parsedShiftCount = FromBinary.BigEndian<Int32>(onlyReKeyData[0..4]);


                    //  6 is the lowest possible length of an exported key
                    //  1x2 bytes reserved for PrLength and ExLength
                    //  and 1x2x2 reserved for both smallest primary and external alphabets of 2 value
                    if (onlyReKeyData.Length < parsedShiftCount + 6)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data length is insufficient for the specified shifts count" +
                                $" {parsedShiftCount} from onlyReKeyData[0-4]",
                                nameof(onlyReKeyData)
                            );
                        return false;
                    }

                    _shifts.Clear();
                    if (parsedShiftCount > 0) _shifts.AddRange(onlyReKeyData[4..(4 + parsedShiftCount)]);
                    else _shifts.Add(0);


                    // +1 is transforming our length range back from 1-255 to 2-256
                    Int32 parsedLengthGuide = onlyReKeyData[parsedShiftCount + 4] + 1;

                    if (onlyReKeyData.Length < parsedShiftCount + parsedLengthGuide + 3)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data length is insufficient for the specified primary alphabet length" +
                                $" {parsedLengthGuide} from onlyReKeyData[{parsedShiftCount + 4}]",
                                nameof(onlyReKeyData)
                            );
                        return false;
                    }
                    else if (parsedLengthGuide < 2)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Primary alphabet length cant be less than 2 (required)" +
                                $"\nParsed length: {parsedLengthGuide} " +
                                $"from onlyReKeyData[{parsedShiftCount + 4}]",
                                nameof(onlyReKeyData)
                            );
                        return false;
                    }

                    _primaryAlphabet.Clear();
                    _primaryAlphabet.AddRange(onlyReKeyData[(parsedShiftCount + 5)..(parsedShiftCount + 5 + parsedLengthGuide)]);


                    //  reusing parsedShiftCount as a offset for what we have already read
                    parsedShiftCount += parsedLengthGuide + 4;

                    // +1 is transforming our length range back from 1-255 to 2-256
                    parsedLengthGuide = onlyReKeyData[parsedShiftCount + 1] + 1;

                    if (onlyReKeyData.Length < parsedShiftCount + parsedLengthGuide)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data length is insufficient for the specified external alphabet length" +
                                $" {parsedLengthGuide} from onlyReKeyData[{parsedShiftCount + 1}]",
                                nameof(onlyReKeyData)
                            );
                        return false;
                    }
                    else if (parsedLengthGuide < 2)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"External alphabet length cant be less than 2 (required)" +
                                $"\nParsed length: {parsedLengthGuide} " +
                                $"from onlyReKeyData[{parsedShiftCount + 1}]",
                                nameof(onlyReKeyData)
                            );
                        return false;
                    }

                    _externalAlphabet.Clear();
                    _externalAlphabet.AddRange(onlyReKeyData[(parsedShiftCount + 2)..(parsedShiftCount + 2 + parsedLengthGuide)]);
                }
                catch (Exception)
                {
                    if (throwExceptions) throw;
                    return false;
                }
                return true;
            }


            public override Byte[] ExportAsBinary()
            {
                return
                [
                    .. _noisifier.ExportAsBinary(),

                    .. ToBinary.BigEndian(ShCount),
                    .. _shifts,

                    (Byte)(PrLength - 1),
                    .. _primaryAlphabet,

                    (Byte)(ExLength - 1),
                    .. _externalAlphabet
                ];
            }
            public List<Byte> ExportAsBinaryList()
            {
                return
                [
                    .. _noisifier.ExportAsBinary(),

                    .. ToBinary.BigEndian(ShCount),
                    .. _shifts,

                    (Byte)(PrLength - 1),
                    .. _primaryAlphabet,

                    (Byte)(ExLength - 1),
                    .. _externalAlphabet
                ];
            }
        }
    }
}