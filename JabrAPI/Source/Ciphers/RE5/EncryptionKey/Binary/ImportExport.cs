using System;
using System.Collections.Generic;


using AVcontrol;
using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
        public override bool ImportFromBinary(List<byte> exportData, bool throwExceptions = false)
        {
            try
            {
                List<Byte> data = [.. exportData];
                if (data.Count < 2)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient even for an empty noisifier",
                            nameof(data)
                        );
                    return false;
                }

                Int32 noisifierBytesCount = data[0];
                if (data.Count < noisifierBytesCount + 1)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified PrimaryNoise in the imported noisifier\n" +
                            $"PrimaryNoiseCount: {noisifierBytesCount} from data[0]",
                            nameof(data)
                        );
                    return false;
                }

                noisifierBytesCount += data[noisifierBytesCount + 1];
                if (data.Count < noisifierBytesCount + 2)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the exported noisifier in bytes count\n" +
                            $"Specified noisifier bytes: {noisifierBytesCount} from data[0] + data[" +
                            $"{noisifierBytesCount - data[noisifierBytesCount + 1]}",
                            nameof(data)
                        );
                    return false;
                }


                _noisifier.ImportFromBinary(data.GetRange(0, noisifierBytesCount + 2), throwExceptions);
                data.RemoveRange(0, noisifierBytesCount + 2);


                if (data.Count < 10)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient even for an empty BinaryKey",
                            nameof(data)
                        );
                    return false;
                }
                Int32 parsedShiftCount = FromBinary.BigEndian<Int32>([.. data.GetRange(0, 4)]);


                //  6 is the lowest possible length of an exported key
                //  1x2 bytes reserved for PrLength and ExLength
                //  and 1x2x2 reserved for both smallest primary and external alphabets of 2 value
                if (data.Count < parsedShiftCount + 6)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified shifts count" +
                            $" {parsedShiftCount} from data[0-4]",
                            nameof(data)
                        );
                    return false;
                }

                _shifts.Clear();
                if (parsedShiftCount > 0) _shifts.AddRange(data.GetRange(4, parsedShiftCount));
                else _shifts.Add(0);



                // +1 is transforming our length range back from 1-255 to 2-256
                Int32 parsedLengthGuide = data[parsedShiftCount + 4] + 1;

                if (data.Count < parsedShiftCount + parsedLengthGuide + 3)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified primary alphabet length" +
                            $" {parsedLengthGuide} from data[{parsedShiftCount + 4}]",
                            nameof(data)
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
                            $"from data[{parsedShiftCount + 4}]",
                            nameof(data)
                        );
                    return false;
                }

                _primaryAlphabet.Clear();
                _primaryAlphabet.AddRange(data.GetRange(parsedShiftCount + 5, parsedLengthGuide));



                //  reusing parsedShiftCount as a offset for what we have already read
                parsedShiftCount += parsedLengthGuide + 4;

                // +1 is transforming our length range back from 1-255 to 2-256
                parsedLengthGuide = data[parsedShiftCount + 1] + 1;

                if (data.Count < parsedShiftCount + parsedLengthGuide)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified external alphabet length" +
                            $" {parsedLengthGuide} from data[{parsedShiftCount + 1}]",
                            nameof(data)
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
                            $"from data[{parsedShiftCount + 1}]",
                            nameof(data)
                        );
                    return false;
                }

                _externalAlphabet.Clear();
                _externalAlphabet.AddRange(data.GetRange(parsedShiftCount + 2, parsedLengthGuide));
            }
            catch (Exception)
            {
                if (throwExceptions) throw;
                return false;
            }
            return true;
        }
        public override List<Byte> ExportAsBinary()
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