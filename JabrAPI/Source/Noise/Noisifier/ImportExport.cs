using System;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    public partial class Noisifier
    {
        public bool ImportFromString(string data, bool throwExceptions = false)
        {
            try
            {
                Int32 splitterId = data.IndexOf(':'), offset, parsedLength;

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

                _primaryNoise = data.Substring(splitterId + 1, parsedLength);


                offset = splitterId + 1 + parsedLength;
                splitterId = data.IndexOf(':', offset);

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
                else if (!Int32.TryParse(data.AsSpan(offset, splitterId - offset), out parsedLength))
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Unable to parse ComplexNoise length" +
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
                            $"Data is insufficient for expected ComplexNoise" +
                            $"Data length: {data.Length}   <   expected: {splitterId + 1 + parsedLength}",
                            nameof(data) + "," + nameof(splitterId) + "," + nameof(parsedLength)
                        );
                    return false;
                }

                _complexNoise = data.Substring(splitterId + 1, parsedLength);
            }
            catch
            {
                if (throwExceptions) throw;
                return false;
            }
            return true;
        }
        public string ExportAsString()
            => _primaryNoise.Length + ":" + _primaryNoise +
               _complexNoise.Length + ":" + _complexNoise;


        public bool ImportFromBinary(List<Byte> data, bool throwExceptions = false)
        {
            try
            {
                Int32 primaryCount = FromBinary.BigEndian<Int32>
                (
                    [..
                        data.GetRange(0, 4)
                    ]
                );


                if (data.Count < primaryCount + 8)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified primaryNoiseCount" +
                            $" {primaryCount + 8} from data[0-4]",
                            nameof(data)
                        );
                    return false;
                }


                _primaryNoise = FromBinary.Utf16
                (
                    data.GetRange
                    (
                        4,
                        primaryCount
                    )
                );


                Int32 complexCount = FromBinary.BigEndian<Int32>
                (
                    [..
                        data.GetRange(primaryCount + 4, 4)
                    ]
                );

                if (data.Count < complexCount + primaryCount + 8)
                {
                    if (throwExceptions)
                        throw new ArgumentException
                        (
                            $"Data length is insufficient for the specified complexNoiseCount" +
                            $" {complexCount + primaryCount + 8}" +
                            $"from data[{primaryCount + 4}-{primaryCount + 8}]",
                            nameof(data)
                        );
                    return false;
                }


                _complexNoise = FromBinary.Utf16
                (
                    data.GetRange
                    (
                        primaryCount + 8,
                        complexCount
                    )
                );
            }
            catch
            {
                if (throwExceptions) throw;
                return false;
            }
            return true;
        }
        public List<Byte> ExportAsBinary()
        {
            Byte[] exportedPrimary = ToBinary.Utf16(_primaryNoise);
            Byte[] exportedComplex = ToBinary.Utf16(_complexNoise);

            return
            [
                .. ToBinary.BigEndian(exportedPrimary.Length),
                .. exportedPrimary,

                .. ToBinary.BigEndian(exportedComplex.Length),
                .. exportedComplex
            ];
        }
    }
}