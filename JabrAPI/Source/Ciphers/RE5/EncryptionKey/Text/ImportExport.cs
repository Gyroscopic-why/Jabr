using System;
using System.Collections.Generic;


using AVcontrol;
using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class EncryptionKey : IEncryptionKey
    {
        override public bool ImportFromString(
            string exportedData, bool throwExceptions = false)
        {
            try
            {
                string data = exportedData;

                Int32 splitterId = data.IndexOf(':'), noisifierImportLength, parsedLength;

                if (splitterId == -1)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data doesnt contain splitter for determining PrimaryNoise length" +
                            $"\ndata.IndexOf(':') == -1",
                            nameof(data) + "," + nameof(splitterId)
                        );
                    return false;
                }
                else if (!Int32.TryParse(data.AsSpan(0, splitterId), out parsedLength))
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Unable to parse PrimaryNoise length" +
                            $"\nExpected length to be at indexes 0-{splitterId}" +
                            $"\nInvalid sequence: {data[..splitterId]}",
                            nameof(data) + "," + nameof(splitterId)
                        );
                    return false;
                }
                else if (data.Length < splitterId + 1 + parsedLength + 4)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data is insufficient for expected PrimaryNoise & smallest possible ComplexNoise" +
                            $"Data length: {data.Length}   <   expected: {splitterId + 1 + parsedLength + 4}",
                            nameof(data) + "," + nameof(splitterId) + "," + nameof(parsedLength)
                        );
                    return false;
                }


                noisifierImportLength = splitterId + 1 + parsedLength;
                splitterId = data.IndexOf(':', noisifierImportLength);

                if (splitterId == -1)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data doesnt contain another splitter for determining ComplexNoise length" +
                            $"\ndata.IndexOf(':') == -1",
                            nameof(data) + "," + nameof(splitterId)
                        );
                    return false;
                }
                else if (!Int32.TryParse(data.AsSpan(noisifierImportLength, splitterId - noisifierImportLength), out parsedLength))
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Unable to parse ComplexNoise length" +
                            $"\nExpected length to be at indexes {noisifierImportLength}-{splitterId}" +
                            $"\nInvalid sequence: {data[noisifierImportLength..]}",
                            nameof(data) + "," + nameof(splitterId)
                        );
                    return false;
                }
                else if (data.Length < splitterId + 1 + parsedLength)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data is insufficient for expected ComplexNoise" +
                            $"Data length: {data.Length}   <   expected: " +
                            $"{noisifierImportLength + splitterId + 1 + parsedLength}",
                            nameof(data) + "," + nameof(splitterId) + "," + nameof(parsedLength)
                        );
                    return false;
                }

                noisifierImportLength = splitterId + parsedLength + 1;
                _noisifier.ImportFromString(data[..noisifierImportLength], throwExceptions);

                data = data[noisifierImportLength..];

                splitterId = data.IndexOf(':');
                Int32 parsedShiftsCount = 0, offset;

                if (splitterId == -1)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data doesnt contain splitter for determining alphabet length" +
                            $"\ndata.IndexOf(':') == -1",
                            nameof(data) + "," + nameof(splitterId)
                        );
                    return false;
                }
                else if (!Int32.TryParse(data.AsSpan(0, splitterId), out parsedLength))
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Unable to parse primary alphabet length" +
                            $"\nExpected length to be at indexes 0-{splitterId}" +
                            $"\nInvalid sequence: {data[..splitterId]}",
                            nameof(data) + "," + nameof(splitterId)
                        );
                    return false;
                }
                else if (data.Length < splitterId + 1 + parsedLength + 4)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data is insufficient for expected primary & smallest possible external alphabet" +
                            $"Data length: {data.Length}   <   expected: {splitterId + 1 + parsedLength + 4}",
                            nameof(data) + "," + nameof(splitterId) + "," + nameof(parsedLength)
                        );
                    return false;
                }

                _primaryAlphabet = data.Substring(splitterId + 1, parsedLength);


                offset = splitterId + 1 + parsedLength;
                splitterId = data.IndexOf(':', offset);

                if (splitterId == -1)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data doesnt contain another splitter for determining external alphabet length" +
                            $"\ndata.IndexOf(':') == -1",
                            nameof(data) + "," + nameof(splitterId)
                        );
                    return false;
                }
                else if (!Int32.TryParse(data.AsSpan(offset, splitterId - offset), out parsedLength))
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Unable to parse external alphabet length" +
                            $"\nExpected length to be at indexes {offset}-{splitterId}" +
                            $"\nInvalid sequence: {data[offset..]}",
                            nameof(data) + "," + nameof(splitterId)
                        );
                    return false;
                }
                else if (data.Length < splitterId + 1 + parsedLength)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data is insufficient for expected external alphabet" +
                            $"Data length: {data.Length}   <   expected: {splitterId + 1 + parsedLength}",
                            nameof(data) + "," + nameof(splitterId) + "," + nameof(parsedLength)
                        );
                    return false;
                }

                _externalAlphabet = data.Substring(splitterId + 1, parsedLength);


                offset += parsedLength + 2;
                string[] unparsedShifts = data[offset..].Split(',');

                _shifts.Clear();
                for (var i = 0; i < unparsedShifts.Length; i++)
                {
                    if (Int16.TryParse(unparsedShifts[i], out Int16 shift))
                    {
                        parsedShiftsCount++;
                        _shifts.Add(shift);
                    }
                    else
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Unable to parse shift[{i}]" +
                                $"\nInvalid string: {unparsedShifts[i]}", nameof(unparsedShifts)
                            );
                        return false;
                    }
                }
                if (parsedShiftsCount == 0) _shifts.Add(0);
            }
            catch
            {
                if (throwExceptions) throw;
                else return false;
            }
            return true;
        }
        override public string ExportAsString()
        {
            string mainPart = $"{_primaryAlphabet.Length}:{_primaryAlphabet}" +
                $"{_externalAlphabet.Length}:{_externalAlphabet}";

            string shiftsPart = _shifts.Count > 0 ? string.Join(",", _shifts) : "";

            return _noisifier.ExportAsString() + mainPart + shiftsPart;
        }


        override public bool ImportFromBinary(
            List<Byte> exportData, bool throwExceptions = false)
        {
            try
            {
                List<Byte> data = [.. exportData];
                if (data.Count < 8)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient even for an empty noisifier",
                            nameof(data)
                        );
                    return false;
                }


                Int32 noisifierBytesCount = FromBinary.BigEndian<Int32>
                (
                    [..
                            data.GetRange(0, 4)
                    ]
                );

                if (data.Count < noisifierBytesCount + 4)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified PrimaryNoise in the imported noisifier\n" +
                            $"PrimaryNoiseCount: {noisifierBytesCount + 4} from data[0-4]",
                            nameof(data)
                        );
                    return false;
                }


                noisifierBytesCount += FromBinary.BigEndian<Int32>
                (
                    [..
                            data.GetRange(noisifierBytesCount + 4, 4)
                    ]
                );

                if (data.Count < noisifierBytesCount + 4)
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


                _noisifier.ImportFromBinary(data.GetRange(0, noisifierBytesCount + 8), throwExceptions);
                data.RemoveRange(0, noisifierBytesCount + 8);


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


                Int32 parsedShiftCountInBytes = FromBinary.BigEndian<Int32>
                (
                    [..
                            data.GetRange(0, 4)
                    ]
                ) * 2;

                //  12 (Bytes) is the lowest possible length of an exported key
                //  2x2 bytes reserved for PrLength and ExLength
                //  and 2x2x2 reserved for both smallest primary and external alphabets of 2 value
                if (data.Count < parsedShiftCountInBytes + 12)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified shifts count:" +
                            $" {parsedShiftCountInBytes / 4} from data[0-4]",
                            nameof(data)
                        );
                    return false;
                }

                _shifts.Clear();
                if (parsedShiftCountInBytes > 0)
                {
                    for (var i = 4; i < parsedShiftCountInBytes + 4; i += 2)
                        _shifts.Add(FromBinary.BigEndian<Int16>([.. data.GetRange(i, 2)]));
                }
                else _shifts.Add(0);



                Int32 parsedLengthInBytes = FromBinary.BigEndian<Int32>
                (
                    [..
                            data.GetRange(parsedShiftCountInBytes + 4, 4)
                    ]
                );

                if (data.Count < parsedShiftCountInBytes + 4 + parsedLengthInBytes + 8)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified primary alphabet length" +
                            $" {parsedShiftCountInBytes + 4 + parsedLengthInBytes} " +
                            $"from data[{parsedShiftCountInBytes + 4}-{parsedShiftCountInBytes + 8}]",
                            nameof(data)
                        );
                    return false;
                }
                else if (parsedLengthInBytes < 2)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Primary alphabet length cant be less than 2 (required)" +
                            $"\nParsed length: {parsedLengthInBytes} " +
                            $"from data[{parsedShiftCountInBytes + 4}-{parsedShiftCountInBytes + 8}]",
                            nameof(data)
                        );
                    return false;
                }

                _primaryAlphabet = FromBinary.Utf16
                (
                    data.GetRange
                    (
                        parsedShiftCountInBytes + 8,
                        parsedLengthInBytes
                    )
                );



                //  reusing parsedShiftCount as a offset for what we have already read
                parsedShiftCountInBytes += parsedLengthInBytes + 4;
                parsedLengthInBytes = FromBinary.BigEndian<Int32>
                (
                    [..
                            data.GetRange(parsedShiftCountInBytes + 4, 4)
                    ]
                );

                if (data.Count < parsedShiftCountInBytes + 4 + parsedLengthInBytes)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified external alphabet length" +
                            $" {parsedShiftCountInBytes + 4 + parsedLengthInBytes}" +
                            $"from data[{parsedShiftCountInBytes + 4}-{parsedShiftCountInBytes + 8}]",
                            nameof(data)
                        );
                    return false;
                }
                else if (parsedLengthInBytes < 2)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"External alphabet length cant be less than 2 (required)" +
                            $"\nParsed length: {parsedLengthInBytes} " +
                            $"from data[{parsedShiftCountInBytes + 4}-{parsedShiftCountInBytes + 8}]",
                            nameof(data)
                        );
                    return false;
                }


                _externalAlphabet = FromBinary.Utf16
                (
                    data.GetRange
                    (
                        parsedShiftCountInBytes + 8,
                        parsedLengthInBytes
                    )
                );
            }
            catch (Exception)
            {
                if (throwExceptions) throw;
                return false;
            }
            return true;
        }
        override public List<Byte> ExportAsBinary()
        {
            Byte[] exportedPrimary  = ToBinary.Utf16(_primaryAlphabet);
            Byte[] exportedExternal = ToBinary.Utf16(_externalAlphabet);

            List<Byte> exportedShifts = new (_shifts.Count * 2);
            foreach (Int16 shift in _shifts)
                exportedShifts.AddRange(ToBinary.BigEndian(shift));

            return
            [
                .. _noisifier.ExportAsBinary(),

                .. ToBinary.BigEndian(exportedShifts.Count / 2),
                .. exportedShifts,

                .. ToBinary.BigEndian(exportedPrimary.Length),
                .. exportedPrimary,

                .. ToBinary.BigEndian(exportedExternal.Length),
                .. exportedExternal
            ];
        }
    }
}