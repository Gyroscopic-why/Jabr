using System;
using System.Linq;
using System.Collections.Generic;

using AVcontrol;

using static JabrAPI.Source.Template;



namespace JabrAPI
{
    public class RE5
    { 
        public class EncryptionKey : IEncryptionKey
        {
            private string _primaryAlphabet  = "";
            private string _externalAlphabet = "";

            private List<char>  _primaryNecessary = [],  _primaryAllowed = [],  _primaryBanned = [];
            private List<char> _externalNecessary = [], _externalAllowed = [], _externalBanned = [];
            private Int32  _primaryMaxLength = -1, _externalMaxLength = -1;


            public EncryptionKey(string primaryAlphabet, string externalAlphabet, List<Int16> shifts)
            {
                _primaryAlphabet  = primaryAlphabet;
                _externalAlphabet = externalAlphabet;

                _shifts.Clear();
                if (shifts == null || shifts.Count == 0) _shifts.Add(0);
                else _shifts.AddRange(shifts);
            }
            public EncryptionKey(string primaryAlphabet, string externalAlphabet, Int16 shift)
            {
                _primaryAlphabet  = primaryAlphabet;
                _externalAlphabet = externalAlphabet;

                _shifts.Clear();
                _shifts.Add(shift);
            }
            public EncryptionKey(string primaryAlphabet, string externalAlphabet)
            {
                _primaryAlphabet  = primaryAlphabet;
                _externalAlphabet = externalAlphabet;
            }
            public EncryptionKey(Int32 shiftCount) => _shCount = shiftCount;
            public EncryptionKey(EncryptionKey otherKey, bool fullCopy = true)
            {
                _primaryAlphabet  = otherKey.PrAlphabet;
                _externalAlphabet = otherKey.ExAlphabet;

                _shifts.Clear();
                _shifts.AddRange(otherKey.Shifts);

                if (fullCopy)
                {
                    _primaryNecessary  = otherKey._primaryNecessary;
                    _primaryAllowed    = otherKey._primaryAllowed;
                    _primaryBanned     = otherKey._primaryBanned;
                    _primaryMaxLength  = otherKey._primaryMaxLength;

                    _externalNecessary = otherKey._externalNecessary;
                    _externalAllowed   = otherKey._externalAllowed;
                    _externalBanned    = otherKey._externalBanned;
                    _externalMaxLength = otherKey._externalMaxLength;

                    _shCount = otherKey._shCount;
                }
            }
            public EncryptionKey(bool autoGenerate = true)
            {
                if (autoGenerate) Default();
                else SetDefault();
            }



            public string PrimaryAlphabet => _primaryAlphabet;
            public string PrAlphabet      => _primaryAlphabet;
            public string Primary         => _primaryAlphabet;
            public Int32  PrimaryLength => _primaryAlphabet == null ? -1 : _primaryAlphabet.Length;
            public Int32  PrLength      => _primaryAlphabet == null ? -1 : _primaryAlphabet.Length;

            public string ExternalAlphabet => _externalAlphabet;
            public string ExAlphabet       => _externalAlphabet;
            public string External         => _externalAlphabet;
            public Int32  ExternalLength => _externalAlphabet == null ? -1 : _externalAlphabet.Length;
            public Int32  ExLength       => _externalAlphabet == null ? -1 : _externalAlphabet.Length;

            override public string FinalAlphabet => _externalAlphabet;




            override public bool   ImportFromString(string data, bool throwExceptions = false)
            {
                try
                {
                    Int32 splitterId = data.IndexOf(':'), offset;
                    Int16 parsedLength, parsedShiftsCount = 0;

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
                    else if (!Int16.TryParse(data.AsSpan(0, splitterId), out parsedLength))
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
                    else if (!Int16.TryParse(data.AsSpan(offset, splitterId - offset), out parsedLength))
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
                    else if (data.Length < offset + splitterId + 1 + parsedLength)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data is insufficient for expected external alphabet" +
                                $"Data length: {data.Length}   <   expected: {offset + splitterId + 1 + parsedLength}",
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
                string result = _primaryAlphabet.Length + ":" + _primaryAlphabet
                             + _externalAlphabet.Length + ":" + _externalAlphabet;

                for (var curId = 0; curId < _shifts.Count - 1; curId++)
                    result += _shifts[curId] + ",";
                if (_shifts.Count > 0) result += _shifts[^1];

                return result;
            }


            override public bool ImportFromBinary(List<byte> data, bool throwExceptions = false)
            {
                try
                {
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



                    Int32 parsedLengthInBytes = FromBinary.BigEndian<Int16>
                        (
                            [..
                                data.GetRange(parsedShiftCountInBytes + 4, 2)
                            ]
                        ) * 2;  //  each char is in UTF16 (2 bytes)

                    if (data.Count < parsedShiftCountInBytes + 4 + parsedLengthInBytes + 6)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data length is insufficient for the specified primary alphabet length" +
                                $" {parsedLengthInBytes / 2} " +
                                $"from data[{parsedShiftCountInBytes + 4}-{parsedShiftCountInBytes + 6}]",
                                nameof(data)
                            );
                        return false;
                    }
                    else if (parsedLengthInBytes < 4)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Primary alphabet length cant be less than 2 (required)" +
                                $"\nParsed length: {parsedLengthInBytes / 2} " +
                                $"from data[{parsedShiftCountInBytes + 4}-{parsedShiftCountInBytes + 6}]",
                                nameof(data)
                            );
                        return false;
                    }

                    _primaryAlphabet = "";
                    for (var i = parsedShiftCountInBytes + 6; i < parsedLengthInBytes + parsedShiftCountInBytes + 6; i += 2)
                        _primaryAlphabet += FromBinary.Utf16(data.GetRange(i, 2));



                    //  reusing parsedShiftCount as a offset for what we have already read
                    parsedShiftCountInBytes += parsedLengthInBytes + 4;
                    parsedLengthInBytes = FromBinary.BigEndian<Int16>
                        (
                            [..
                                data.GetRange(parsedShiftCountInBytes + 2, 2)
                            ]
                        ) * 2;

                    if (data.Count < parsedShiftCountInBytes + 4 + parsedLengthInBytes)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"Data length is insufficient for the specified external alphabet length" +
                                $" {parsedLengthInBytes} from data[{parsedShiftCountInBytes + 2}-{parsedShiftCountInBytes + 4}]",
                                nameof(data)
                            );
                        return false;
                    }
                    else if (parsedLengthInBytes < 4)
                    {
                        if (throwExceptions)
                            throw new ArgumentException
                            (
                                $"External alphabet length cant be less than 2 (required)" +
                                $"\nParsed length: {parsedLengthInBytes / 2} " +
                                $"from data[{parsedShiftCountInBytes + 2}-{parsedShiftCountInBytes + 4}]",
                                nameof(data)
                            );
                        return false;
                    }

                    _externalAlphabet = "";
                    for (var i = parsedShiftCountInBytes + 4; i < parsedLengthInBytes + parsedShiftCountInBytes + 4; i += 2)
                        _externalAlphabet += FromBinary.Utf16(data.GetRange(i, 2));
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
                List<Byte> result = [.. ToBinary.BigEndian(_shifts.Count)];
                for (var curId = 0; curId < _shifts.Count; curId++)
                    result.AddRange(ToBinary.BigEndian(_shifts[curId]));
                
                result.AddRange(ToBinary.BigEndian((UInt16)PrLength));
                result.AddRange(ToBinary.Utf16(_primaryAlphabet));

                result.AddRange(ToBinary.BigEndian((UInt16)ExLength));
                result.AddRange(ToBinary.Utf16(_externalAlphabet));

                return result;
            }



            public bool IsPrimaryValid(string message, bool throwException = false)
                => IsPrimaryValid(message, _primaryAlphabet, throwException);
            static public bool IsPrimaryValid(string message, string primary, bool throwException = false)
            {
                if (!IsPrimaryPartiallyValid(primary, throwException)) return false;

                foreach (char c in message)
                {
                    if (!primary.Contains(c))
                    {
                        if (throwException)
                            throw new ArgumentException
                            (
                                $"Message contains characters not present in the primary alphabet" +
                                $"\nMissing character: {c}",
                                nameof(primary)
                            );
                        return false;
                    }
                }
                return true;
            }

            public bool IsPrimaryPartiallyValid(bool throwException = false)
                => IsPrimaryPartiallyValid(_primaryAlphabet, throwException);
            static public bool IsPrimaryPartiallyValid(string primary, bool throwException = false)
            {
                if (primary == null || primary == "" || primary.Length < 2)
                {
                    if (throwException)
                        throw new ArgumentException
                        (
                            "Primary alphabet is not set or is too short",
                            nameof(primary)
                        );
                    return false;
                }
                for (var curId = 0; curId < primary.Length; curId++)
                {
                    for (var id2 = curId + 1; id2 < primary.Length; id2++)
                    {
                        if (primary[curId] == primary[id2])
                        {
                            if (throwException)
                                throw new ArgumentException
                                (
                                    $"Primary alphabet contains duplicates characters" +
                                    $"\nDuplicate char: {primary[curId]}",
                                    nameof(primary)
                                );
                            return false;
                        }
                    }
                }

                return true;
            }



            public bool IsExternalValid(string encrypted, bool throwException = false)
                => IsExternalValid(encrypted, _externalAlphabet, throwException);
            static public bool IsExternalValid(string encrypted, string external, bool throwException = false)
            {
                if (!IsExternalPartiallyValid(external, throwException)) return false;

                foreach (char c in encrypted)
                {
                    if (!external.Contains(c))
                    {
                        if (throwException)
                            throw new ArgumentException
                            (
                                $"Message contains characters not present in the external alphabet" +
                                $"\nMissing character: {c}",
                                nameof(external)
                            );
                        return false;
                    }
                }
                return true;
            }

            public bool IsExternalPartiallyValid(bool throwException = false)
                => IsExternalPartiallyValid(_externalAlphabet, throwException);
            static public bool IsExternalPartiallyValid(string external, bool throwException = false)
            {
                if (external == null || external == "" || external.Length < 2)
                {
                    if (throwException)
                        throw new ArgumentException
                        (
                            "External alphabet is not set or is too short",
                            nameof(external)
                        );
                    return false;
                }
                for (var curId = 0; curId < external.Length; curId++)
                {
                    for (var id2 = curId + 1; id2 < external.Length; id2++)
                    {
                        if (external[curId] == external[id2])
                        {
                            if (throwException)
                                throw new ArgumentException
                                (
                                    $"External alphabet contains duplicates characters" +
                                    $"Duplicate char: {external[curId]}",
                                    nameof(external)
                                );
                            return false;
                        }
                    }
                }

                return true;
            }





            public void SetDefault(List<char> pNecessary, List<char> pAllowed, List<char> pBanned, Int32 pMaxLength,
                                   List<char> eNecessary, List<char> eAllowed, List<char> eBanned, Int32 eMaxLength)
            {
                _primaryNecessary  = [.. pNecessary];
                _externalNecessary = [.. eNecessary];

                _primaryAllowed    = [.. pAllowed];
                _externalAllowed   = [.. eAllowed];

                _primaryBanned     = [.. pBanned];
                _externalBanned    = [.. eBanned];

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(string pNecessary, string pAllowed, string pBanned, Int32 pMaxLength,
                                   string eNecessary, string eAllowed, string eBanned, Int32 eMaxLength)
                => SetDefault([.. pNecessary], [.. pAllowed], [.. pBanned], pMaxLength,
                              [.. eNecessary], [.. eAllowed], [.. eBanned], eMaxLength);


            public void SetDefault(List<char> pNecessary, List<char> pAllowed, Int32 pMaxLength,
                                   List<char> eNecessary, List<char> eAllowed, Int32 eMaxLength)
            {
                _primaryNecessary  = [.. pNecessary];
                _externalNecessary = [.. eNecessary];

                _primaryAllowed    = [.. pAllowed];
                _externalAllowed   = [.. eAllowed];

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(string pNecessary, string pAllowed, Int32 pMaxLength,
                                   string eNecessary, string eAllowed, Int32 eMaxLength)
                => SetDefault([.. pNecessary], [.. pAllowed], pMaxLength,
                              [.. eNecessary], [.. eAllowed], eMaxLength);


            public void SetDefault(Int32 pMaxLength, List<char> pNecessary, List<char> pBanned,
                                   Int32 eMaxLength, List<char> eNecessary, List<char> eBanned)
            {
                _primaryNecessary  = [.. pNecessary];
                _externalNecessary = [.. eNecessary];

                _primaryBanned     = [.. pBanned];
                _externalBanned    = [.. eBanned];

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(Int32 pMaxLength, string pNecessary, string pBanned,
                                   Int32 eMaxLength, string eNecessary, string eBanned)
                => SetDefault(pMaxLength, [.. pNecessary], [.. pBanned], 
                              eMaxLength, [.. eNecessary], [.. eBanned]);


            public void SetDefault(Int32 pMaxLength, List<char> pBanned,
                                   Int32 eMaxLength, List<char> eBanned)
            {
                _primaryBanned     = [.. pBanned];
                _externalBanned    = [.. eBanned];

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(Int32 pMaxLength, string pBanned,
                                   Int32 eMaxLength, string eBanned)
                => SetDefault(pMaxLength, [.. pBanned], 
                              eMaxLength, [.. eBanned]);


            public void SetDefault(List<char> necessary, List<char> allowed, List<char> banned, Int32 maxLength)
                => SetDefault(necessary, allowed, banned, maxLength, necessary, allowed, banned, maxLength);
            public void SetDefault(string necessary, string allowed, string banned, Int32 maxLength)
                => SetDefault([.. necessary], [.. allowed], [.. banned], maxLength);

            public void SetDefault(List<char> necessary, List<char> allowed, Int32 maxLength)
                => SetDefault(necessary, allowed, maxLength, necessary, allowed, maxLength);
            public void SetDefault(string necessary, string allowed, Int32 maxLength)
                => SetDefault([.. necessary], [.. allowed], maxLength);

            public void SetDefault(Int32 maxLength, List<char> necessary, List<char> banned)
                => SetDefault(maxLength, necessary, banned, maxLength, necessary, banned);
            public void SetDefault(Int32 maxLength, string necessary, string banned)
                => SetDefault(maxLength, [.. necessary], [.. banned]);

            public void SetDefault(Int32 maxLength, List<char> banned)
                => SetDefault(maxLength, banned, maxLength, banned);
            public void SetDefault(Int32 maxLength, string banned)
                => SetDefault(maxLength, [.. banned]);


            override public  void SetDefault()
            {
                _primaryNecessary  = [.. " " + DEFAULT_CHARS];
                _externalAllowed   = [.. DEFAULT_CHARS];

                _primaryMaxLength  = _primaryNecessary.Count;
                _externalMaxLength = 8;
            }
            override private protected void GenerateAll()
            {
                GenerateRandomPrimary();
                GenerateRandomExternal();

                if (_shifts.Count < 2)
                {
                    if (_shCount < 2) GenerateRandomShifts();
                    else GenerateRandomShifts(_shCount);
                }
                else GenerateRandomShifts(_shifts.Count);
            }




            private void ValidateForPrimaryGeneration (List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                if (necessary != null && necessary.Count > 0)
                {
                    _primaryNecessary = necessary;

                    for (var id1 = 0; id1 < necessary.Count; id1++)
                    {
                        for (var id2 = id1 + 1; id2 < necessary.Count; id2++)
                        {
                            if (necessary[id1] == necessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "necessary list cannot include duplicates",
                                    "necessary, duplicate char: " + necessary[id1]
                                );
                            }
                        }
                    }
                }
                else _primaryNecessary = [];

                if (allowed != null)
                {
                    _primaryAllowed = allowed;

                    for (var id1 = 0; id1 < allowed.Count; id1++)
                    {
                        for (var id2 = id1 + 1; id2 < allowed.Count; id2++)
                        {
                            if (allowed[id1] == allowed[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicates",
                                    "allowed, duplicate char: " + allowed[id1]
                                );
                            }
                        }
                        for (var id2 = 0; id2 < _primaryNecessary.Count; id2++)
                        {
                            if (allowed[id1] == _primaryNecessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicates of necessary list",
                                    "allowed, _primaryNecessary, duplicate char: " + allowed[id1]
                                );
                            }
                        }
                    }
                }
                else _primaryAllowed = [];


                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the smallest possible alphabet length (2)",
                        nameof(maxLength)
                    );
                }
                maxLength -= _primaryNecessary.Count;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "maxLength: " + maxLength + _primaryNecessary.Count +
                        ", necessary characters count: " + _primaryNecessary.Count
                    );
                }
                else if (maxLength > _primaryAllowed.Count)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be more than allowed characters count",
                        "maxLength: " + maxLength + ", allowed count: " + _primaryAllowed.Count
                    );
                }

                if (_primaryNecessary.Count + _primaryAllowed.Count < 2)
                {
                    throw new ArgumentException
                    (
                        "Not enough characters in necessary and allowed list for a valid key",
                        "_primaryNecessary.Count + _primaryAllowed.Count < 2"
                    );
                }
            }
            private void ValidateForExternalGeneration(List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                if (necessary != null && necessary.Count > 0)
                {
                    _externalNecessary = necessary;

                    for (var id1 = 0; id1 < necessary.Count; id1++)
                    {
                        for (var id2 = id1 + 1; id2 < necessary.Count; id2++)
                        {
                            if (necessary[id1] == necessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "necessary list cannot include duplicates",
                                    "necessary, duplicate char: " + necessary[id1]
                                );
                            }
                        }
                    }
                }
                else _externalNecessary = [];

                if (allowed != null)
                {
                    _externalAllowed = allowed;

                    for (var id1 = 0; id1 < allowed.Count; id1++)
                    {
                        for (var id2 = id1 + 1; id2 < allowed.Count; id2++)
                        {
                            if (allowed[id1] == allowed[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicates",
                                    "allowed, duplicate char: " + allowed[id1]
                                );
                            }
                        }
                        for (var id2 = 0; id2 < _externalNecessary.Count; id2++)
                        {
                            if (allowed[id1] == _externalNecessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicates of necessary list",
                                    "allowed, _externalNecessary, duplicate char: " + allowed[id1]
                                );
                            }
                        }
                    }
                }
                else _externalAllowed = [];


                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the smallest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                maxLength -= _externalNecessary.Count;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "maxLength: " + maxLength + _externalNecessary.Count +
                        ", necessary characters count: " + _externalNecessary.Count
                    );
                }
                else if (maxLength > _externalAllowed.Count)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be more than allowed characters count",
                        "maxLength: " + maxLength + ", allowed count: " + _externalAllowed.Count
                    );
                }

                if (_externalNecessary.Count + _externalAllowed.Count < 2)
                {
                    throw new ArgumentException
                    (
                        "Not enough characters in necessary and allowed list for a valid key",
                        "_externalNecessary.Count + _externalAllowed.Count < 2"
                    );
                }
            }



            public void GenerateRandomPrimary(List<char> necessary, List<char> allowed, Int32 maxLength, bool validateParameters = true)
            {
                if (validateParameters)
                {
                    ValidateForPrimaryGeneration(necessary, allowed, maxLength);

                    necessary = _primaryNecessary;  //  to avoid nullpointer exceptions
                    allowed   = _primaryAllowed;    //  the _primary variants are always defaulted
                                                    //  (to a new empty List in worst case)
                }

                Int32 buffer, bufferId;
                _primaryAlphabet = "";


                while (necessary.Count > 0)
                {
                    buffer = _random.Next(0, necessary.Count);
                    _primaryAlphabet += necessary[buffer];
                    necessary.RemoveAt(buffer);
                }
                for (var remaining = Math.Min(maxLength, allowed.Count); remaining > 0; remaining--)
                {
                    buffer   = _random.Next(0, allowed.Count);
                    bufferId = _random.Next(0, _primaryAlphabet.Length);

                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.RemoveAt(buffer);
                }
            }
            public void GenerateRandomPrimary(string necessary, string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomPrimary([.. necessary], [.. allowed], maxLength, validateParameters);


            public void GenerateRandomPrimary(List<char> allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomPrimary([], allowed, maxLength, validateParameters);
            public void GenerateRandomPrimary(string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomPrimary([], [.. allowed], maxLength, validateParameters);


            public void GenerateRandomPrimary(Int32 maxLength, List<char> necessary, List<char> banned, bool validateParameters = true)
            {
                List<char> allowed = [.. (" " + DEFAULT_CHARS)];

                if (banned != null && banned.Count > 0)
                {
                    for (var curId = 0; curId < banned.Count; curId++)
                    {
                        for (var id2 = 0; id2 < allowed.Count; id2++)
                        {
                            if (banned[curId] == allowed[id2])
                            {
                                allowed.RemoveAt(id2);
                                id2--;
                            }
                        }
                    }
                }

                GenerateRandomPrimary(necessary, allowed, maxLength, validateParameters);
            }
            public void GenerateRandomPrimary(Int32 maxLength, string necessary, string banned, bool validateParameters = true)
                => GenerateRandomPrimary(maxLength, [.. necessary], [.. banned], validateParameters);
            

            public void GenerateRandomPrimary(Int32 maxLength, List<char> banned, bool validateParameters = true)
                => GenerateRandomPrimary(maxLength, [], banned, validateParameters);
            public void GenerateRandomPrimary(Int32 maxLength, string banned, bool validateParameters = true)
                => GenerateRandomPrimary(maxLength, [], [.. banned], validateParameters);
            public void GenerateRandomPrimary(Int32 maxLength, bool validateParameters = true) 
                => GenerateRandomPrimary(maxLength, "", validateParameters);

            public void GenerateRandomPrimary()
            {
                if (_primaryAllowed != null && _primaryAllowed.Count > 0)
                {
                    List<char> allowed = [.. _primaryAllowed];

                    if (_primaryBanned != null && _primaryBanned.Count > 0)
                    {
                        for (var curId = 0; curId < _primaryBanned.Count; curId++)
                        {
                            for (var id2 = 0; id2 < allowed.Count; id2++)
                            {
                                if (_primaryBanned[curId] == allowed[id2])
                                {
                                    allowed.RemoveAt(id2);
                                    id2--;
                                }
                            }
                        }
                    }
                    GenerateRandomPrimary(_primaryNecessary, allowed, _primaryMaxLength);
                }
                else GenerateRandomPrimary(_primaryNecessary, [], _primaryNecessary.Count);
            }




            public void GenerateRandomExternal(List<char> necessary, List<char> allowed, Int32 maxLength, bool validateParameters = true)
            {
                if (validateParameters)
                {
                    ValidateForExternalGeneration(necessary, allowed, maxLength);

                    necessary = _externalNecessary;  //  to avoid nullpointer exceptions
                    allowed   = _externalAllowed;    //  the _external variants are always defaulted
                                                     //  (to a new empty List in worst case)
                }

                Int32 buffer, bufferId;
                _externalAlphabet = "";

                while (necessary.Count > 0)
                {
                    buffer = _random.Next(0, necessary.Count);
                    _externalAlphabet += necessary[buffer];
                    necessary.RemoveAt(buffer);
                }
                for (var remaining = Math.Min(maxLength, allowed.Count); remaining > 0; remaining--)
                {
                    buffer = _random.Next(0, allowed.Count);
                    bufferId = _random.Next(0, _externalAlphabet.Length);

                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.RemoveAt(buffer);
                }
            }
            public void GenerateRandomExternal(string necessary, string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomExternal([.. necessary], [.. allowed], maxLength, validateParameters);


            public void GenerateRandomExternal(List<char> allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomExternal([], allowed, maxLength, validateParameters);
            public void GenerateRandomExternal(string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomExternal([], [.. allowed], maxLength, validateParameters);


            public void GenerateRandomExternal(Int32 maxLength, List<char> necessary, List<char> banned, bool validateParameters = true)
            {
                List<char> allowed = [.. " " + DEFAULT_CHARS];

                if (banned != null && banned.Count > 0)
                {
                    for (var curId = 0; curId < banned.Count; curId++)
                    {
                        for (var id2 = 0; id2 < allowed.Count; id2++)
                        {
                            if (banned[curId] == allowed[id2])
                            {
                                allowed.RemoveAt(id2);
                                id2--;
                            }
                        }
                    }
                }

                GenerateRandomExternal(necessary, allowed, maxLength, validateParameters);
            }
            public void GenerateRandomExternal(Int32 maxLength, string necessary, string banned, bool validateParameters = true)
                => GenerateRandomExternal(maxLength, [.. necessary], [.. banned], validateParameters);


            public void GenerateRandomExternal(Int32 maxLength, List<char> banned, bool validateParameters = true)
                => GenerateRandomExternal(maxLength, [], banned, validateParameters);
            public void GenerateRandomExternal(Int32 maxLength, string banned, bool validateParameters = true)
                => GenerateRandomExternal(maxLength, [], [.. banned], validateParameters);
            public void GenerateRandomExternal(Int32 maxLength, bool validateParameters = true) 
                => GenerateRandomExternal(maxLength, "", validateParameters);


            public void GenerateRandomExternal()
            {
                if (_externalAllowed != null &&  _externalAllowed.Count > 0)
                {
                    List<char> allowed = [.. _externalAllowed];

                    if (_externalBanned != null && _externalBanned.Count > 0)
                    {
                        for (var curId = 0; curId < _externalBanned.Count; curId++)
                        {
                            for (var id2 = 0; id2 < allowed.Count; id2++)
                            {
                                if (_externalBanned[curId] == allowed[id2])
                                {
                                    allowed.RemoveAt(id2);
                                    id2--;
                                }
                            }
                        }
                    }
                    GenerateRandomExternal(_externalNecessary, allowed, _externalMaxLength);
                }
                else GenerateRandomExternal(_externalNecessary, [], _externalNecessary.Count);
            }



            public void GenerateRandomShifts(Int32 count)
            {
                if (_externalAlphabet == null || _externalAlphabet.Length < 2)
                {
                    throw new ArgumentException
                    (
                        "Unable to generate shifts, external alphabet is undefined",
                        nameof(_externalAlphabet)
                    );
                }

                GenerateRandomShifts(count, 0, (Int16)(_externalAlphabet.Length - 1));
            }
            public void GenerateRandomShifts()
                => GenerateRandomShifts(_random.Next(128, 384));
        }

        public class BinaryKey : IBinaryKey
        {
            private readonly List<Byte>  _primaryAlphabet = [];
            private readonly List<Byte> _externalAlphabet = [];

            private Byte _compactedPrMaxLength = 255, _compactedExMaxLength = 7;



            public BinaryKey(List<Byte> primary, List<Byte> external, List<Byte> shifts)
                => Set(primary, external, shifts);
            public BinaryKey(BinaryKey otherKey, bool fullCopy = true)
            {
                Set(otherKey.Primary, otherKey.External, otherKey.Shifts);

                if (fullCopy) 
                    SetDefault
                    (
                        otherKey._compactedPrMaxLength, 
                        otherKey._compactedExMaxLength
                    );
            }
            public BinaryKey(List<Byte> exported) => ImportFromBinary(exported);
            public BinaryKey(bool autoGenerate = true)
            {
                if (autoGenerate) Default();
                else SetDefault();
            }



            public List<Byte> PrimaryAlphabet => _primaryAlphabet;
            public List<Byte> PrAlphabet      => _primaryAlphabet;
            public List<Byte> Primary         => _primaryAlphabet;
            public Int32 PrimaryLength => _primaryAlphabet == null ? -1 : _primaryAlphabet.Count;
            public Int32 PrLength      => _primaryAlphabet == null ? -1 : _primaryAlphabet.Count;

            public List<Byte> ExternalAlphabet => _externalAlphabet;
            public List<Byte> ExAlphabet       => _externalAlphabet;
            public List<Byte> External         => _externalAlphabet;
            public Int32 ExternalLength => _externalAlphabet == null ? -1 : _externalAlphabet.Count;
            public Int32 ExLength       => _externalAlphabet == null ? -1 : _externalAlphabet.Count;

            override public List<Byte> FinalAlphabet => _externalAlphabet;





            public override bool ImportFromBinary(List<byte> data, bool throwExceptions = false)
            {
                try
                {
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
                List<Byte> result = [.. ToBinary.BigEndian(_shifts.Count)];
                result.AddRange(_shifts);

                //  -1 is needed because we cant physically have alphabets
                //  longer than 256 and also smaller than 2
                //  but sadly a byte can only fit a range 0-255, while we need 2-256
                //  so we transform it from 2-256 into 1-255 and later reconstruct it back
                IsPrimaryPartiallyValid();
                result.Add((Byte)(PrLength - 1));
                result.AddRange(_primaryAlphabet);

                result.Add((Byte)(ExLength - 1));
                result.AddRange(_externalAlphabet);

                return result;
            }



            public bool IsPrimaryValid(List<Byte> message, bool throwException = false)
                => IsPrimaryValid(message, _primaryAlphabet, throwException);
            static public bool IsPrimaryValid(List<Byte> message, List<Byte> primary, bool throwException = false)
            {
                if (!IsPrimaryPartiallyValid(primary, throwException)) return false;

                foreach (Byte b in message)
                {
                    if (!primary.Contains(b))
                    {
                        if (throwException)
                        {
                            throw new ArgumentException
                            (
                                $"Message contains bytes not present in the primary alphabet" +
                                $"\nMissing byte: {b}",
                                nameof(primary)
                            );
                        }
                        return false;
                    }
                }
                return true;
            }

            public bool IsPrimaryPartiallyValid(bool throwException = false)
                => IsPrimaryPartiallyValid(_primaryAlphabet, throwException);
            static public bool IsPrimaryPartiallyValid(List<Byte> primary, bool throwException = false)
            {
                if (primary == null || primary.Count < 2 || primary.Count > 256)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Primary alphabet is not set, too short or too long",
                            nameof(primary)
                        );
                    }
                    return false;
                }
                for (var curId = 0; curId < primary.Count; curId++)
                {
                    for (var id2 = curId + 1; id2 < primary.Count; id2++)
                    {
                        if (primary[curId] == primary[id2])
                        {
                            if (throwException)
                            {
                                throw new ArgumentException
                                (
                                    $"Primary alphabet contains duplicates characters" +
                                    $"\nDuplicate byte: {primary[curId]}",
                                    nameof(primary)
                                );
                            }
                            return false;
                        }
                    }
                }

                return true;
            }



            public bool IsExternalValid(List<Byte> encrypted, bool throwException = false)
                => IsExternalValid(encrypted, _externalAlphabet, throwException);
            static public bool IsExternalValid(List<Byte> encrypted, List<Byte> external, bool throwException = false)
            {
                if (!IsExternalPartiallyValid(external, throwException)) return false;

                foreach (Byte b in encrypted)
                {
                    if (!external.Contains(b))
                    {
                        if (throwException)
                        {
                            throw new ArgumentException
                            (
                                $"Message contains bytes not present in the external alphabet" +
                                $"\nMissing byte: {b}",
                                nameof(external)
                            );
                        }
                        return false;
                    }
                }
                return true;
            }

            public bool IsExternalPartiallyValid(bool throwException = false)
                => IsExternalPartiallyValid(_externalAlphabet, throwException);
            static public bool IsExternalPartiallyValid(List<Byte> external, bool throwException = false)
            {
                if (external == null || external.Count < 2 || external.Count > 256)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "External alphabet is not set, too short or too long",
                            nameof(external)
                        );
                    }
                    return false;
                }
                for (var curId = 0; curId < external.Count; curId++)
                {
                    for (var id2 = curId + 1; id2 < external.Count; id2++)
                    {
                        if (external[curId] == external[id2])
                        {
                            if (throwException)
                            {
                                throw new ArgumentException
                                (
                                    "External alphabet contains duplicates characters" +
                                    $"Duplicate byte: {external[curId]}",
                                    nameof(external)
                                );
                            }
                            return false;
                        }
                    }
                }

                return true;
            }





            private void Set(List<Byte> primary, List<Byte> external, List<Byte> shifts)
            {
                _primaryAlphabet.Clear();
                _primaryAlphabet.AddRange(primary);

                _externalAlphabet.Clear();
                _externalAlphabet.AddRange(external);

                _shifts.Clear();
                if (shifts == null || shifts.Count == 0) _shifts.Add(0);
                else _shifts.AddRange(shifts.GetRange(0, Math.Max(shifts.Count, 255)));
            }



            public void SetDefaultOnlyEx(Byte compactedExMaxLength) => _compactedExMaxLength = compactedExMaxLength;
            public void SetDefaultOnlyPr(Byte compactedPrMaxLength) => _compactedPrMaxLength = compactedPrMaxLength;
            public void SetDefault(Byte compactedPrMaxLength, Byte compactedExMaxLength)
            {
                _compactedPrMaxLength = compactedPrMaxLength;
                _compactedExMaxLength = compactedExMaxLength;
            }
            public override void SetDefault()
            {
                _compactedPrMaxLength = 255;
                _compactedExMaxLength = 7;
            }





            private protected override void GenerateAll()
            {
                GenerateRandomPrimary();
                GenerateRandomExternal();
                GenerateRandomShifts();
            }



            private List<Byte> GenerateRandomAlphabet(Int32 length)
            {
                List<Byte> remainingChoices = [.. DEFAULT_BYTES];
                List<Byte> resultAlphabet   = [];

                for (var remaining = 0; remaining < length; remaining++)
                {
                    Byte maxValueInclusive = (Byte)Math.Min(255, remainingChoices.Count - 1);
                    var chosen   = _random.NextByte(0, maxValueInclusive);
                    var chosenId = _random.Next    (resultAlphabet.Count);

                    resultAlphabet.Insert(chosenId, remainingChoices[chosen]);
                    remainingChoices.RemoveAt(chosen);
                }
                return resultAlphabet;
            }


            public void GenerateRandomPrimary(Byte compactedLength_willBeIncreasedByOne = 255)
            {
                if (compactedLength_willBeIncreasedByOne < 1)
                    throw new ArgumentOutOfRangeException
                    (
                        "Provided PrimaryAlphabet length must be in 1-255 range" +
                        "\nIt will later be converted from a 1-255 range to a 2-256",
                        nameof(compactedLength_willBeIncreasedByOne)
                    );

                _primaryAlphabet.Clear();
                _primaryAlphabet.AddRange(GenerateRandomAlphabet(compactedLength_willBeIncreasedByOne + 1));
            }
            public void GenerateRandomPrimary()
            {
                _primaryAlphabet.Clear();

                _primaryAlphabet.AddRange(_compactedPrMaxLength > 0
                    ? GenerateRandomAlphabet(_compactedPrMaxLength + 1)
                    : GenerateRandomAlphabet(255));
            }

            public void GenerateRandomExternal(Byte compactedLength_willBeIncreasedByOne = 255)
            {
                if (compactedLength_willBeIncreasedByOne < 1)
                    throw new ArgumentOutOfRangeException
                    (
                        "Provided PrimaryAlphabet length must be in 1-255 range" +
                        "\nIt will later be converted from a 1-255 range to a 2-256",
                        nameof(compactedLength_willBeIncreasedByOne)
                    );

                _externalAlphabet.Clear();
                _externalAlphabet.AddRange(GenerateRandomAlphabet(compactedLength_willBeIncreasedByOne + 1));
            }
            public void GenerateRandomExternal()
            {
                _externalAlphabet.Clear();

                _externalAlphabet.AddRange(_compactedPrMaxLength > 0
                    ? GenerateRandomAlphabet(_compactedExMaxLength + 1)
                    : GenerateRandomAlphabet(8));
            }



            public void GenerateRandomShifts(Int32 count)
            {
                if (_externalAlphabet == null || _externalAlphabet.Count < 2)
                {
                    throw new ArgumentException
                    (
                        "Unable to generate shifts, external alphabet is undefined",
                        nameof(_externalAlphabet)
                    );
                }

                GenerateRandomShifts(count, 0, (Byte)(_externalAlphabet.Count - 1));
            }
            public void GenerateRandomShifts()
                => GenerateRandomShifts(_random.Next(128, 384));
        }





        static public class Encrypt
        {
            static public string Text(string message, EncryptionKey reKey, bool throwException = false)
            {
                if (message == null || message == "" || message.Length < 1)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Message is invalid - cannot be null or empty",
                            nameof(message)
                        );
                    }
                }
                else if (reKey == null)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Encryption key is undefined (null or empty)",
                            nameof(reKey)
                        );
                    }
                }
                else if (reKey.IsExternalPartiallyValid(throwException))
                {
                    if (reKey.IsPrimaryValid(message, throwException))
                    {
                        try
                        {
                            return FastText(message, reKey);
                        }

                        catch (Exception)
                        {
                            if (throwException) throw;
                        }
                    }
                }
                return "";
            }
            static public string Text(string message, EncryptionKey reKey, out Exception? exception)
            {
                if (message == null || message == "" || message.Length < 1)
                {
                    exception = new ArgumentException
                    (
                        "Message is invalid - cannot be null or empty",
                        nameof(message)
                    );
                }
                else if (reKey == null)
                {
                    exception = new ArgumentException
                    (
                        "Encryption key is undefined (null or empty)",
                        nameof(reKey)
                    );
                }
                else
                {
                    try
                    {
                        reKey.IsExternalPartiallyValid(true);
                        reKey.IsPrimaryValid(message, true);

                        string result = FastText(message, reKey);
                        exception = null;

                        return result;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return "";
            }
            static public string FastText(string message, EncryptionKey reKey)
            {
                Int32 exLength = reKey.ExLength, messageLength = message.Length, shCount = reKey.ShCount, buffer;
                List<Int16> shifts = reKey.Shifts; string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;
            

                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + shifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    DigitsInPositive(helper)  //  Optimisation for base 10 encoding
                  : Numsys.AsList
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count;

                Int32[] ids = new Int32[messageLength];
                ids[0] = prAlphabet.IndexOf(message[0]);
                buffer = ids[0] + shifts[0];

                string encoding = Numsys.ToCustomAsString
                (
                    (buffer / exLength).ToString(),
                    10,
                    exLength,
                    exAlphabet,
                    maxEncodingLength
                );
                string encrypted = exAlphabet[buffer % exLength] + encoding;


                for (var curId = 1; curId < messageLength; curId++)
                {
                    ids[curId] = prAlphabet.IndexOf(message[curId]);
                    buffer = ids[curId] + shifts[curId % shCount] + ids[curId - 1];

                    encoding = Numsys.ToCustomAsString
                    (
                        (buffer / exLength).ToString(),
                        10,
                        exLength,
                        exAlphabet,
                        maxEncodingLength
                    );

                    encrypted += exAlphabet[buffer % exLength] + encoding;
                }

                return encrypted;
            }


            static public Byte[] TextToBytesUtf8(string message, EncryptionKey reKey, bool throwException = false)
                => ToBinary.Utf8(Text(message, reKey, throwException));
            static public Byte[] TextToBytesUtf8(string message, EncryptionKey reKey, out Exception? exception)
                => ToBinary.Utf8(Text(message, reKey, out exception));
            static public Byte[] FastTextToBytesUtf8(string message, EncryptionKey reKey)
                => ToBinary.Utf8(FastText(message, reKey));


            static public Byte[] TextToBytesUtf16(string message, EncryptionKey reKey, bool throwException = false)
                => ToBinary.Utf16(Text(message, reKey, throwException));
            static public Byte[] TextToBytesUtf16(string message, EncryptionKey reKey, out Exception? exception)
                => ToBinary.Utf16(Text(message, reKey, out exception));
            static public Byte[] FastTextToBytesUtf16(string message, EncryptionKey reKey)
                => ToBinary.Utf16(FastText(message, reKey));







            static public List<Byte> Bytes(Byte[] message, BinaryKey reKey, bool throwException = false)
            {
                if (message == null || message.Length < 1)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Message is invalid - cannot be null or empty",
                            nameof(message)
                        );
                    }
                }
                else if (reKey == null)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Encryption key is undefined (null or empty)",
                            nameof(reKey)
                        );
                    }
                }
                else if (reKey.IsExternalPartiallyValid(throwException))
                {
                    if (reKey.IsPrimaryValid([.. message], throwException))
                    {
                        try
                        {
                            return FastBytes(message, reKey);
                        }

                        catch (Exception)
                        {
                            if (throwException) throw;
                        }
                    }
                }
                return [];
            }
            static public List<Byte> Bytes(Byte[] message, BinaryKey reKey, out Exception? exception)
            {
                if (message == null || message.Length < 1)
                {
                    exception = new ArgumentException
                    (
                        "Message is invalid - cannot be null or empty",
                        nameof(message)
                    );
                }
                else if (reKey == null)
                {
                    exception = new ArgumentException
                    (
                        "Encryption key is undefined (null or empty)",
                        nameof(reKey)
                    );
                }
                else
                {
                    try
                    {
                        reKey.IsExternalPartiallyValid(true);
                        reKey.IsPrimaryValid([.. message], true);

                        List<Byte> result = FastBytes(message, reKey);
                        exception = null;

                        return result;
                    }
                    catch (Exception innerException) { exception = innerException; }
                }
                return [];
            }
            static public List<Byte> FastBytes(Byte[] message, BinaryKey reKey)
            {
                Int32 exLength = reKey.ExLength, messageLength = message.Length, shCount = reKey.ShCount, buffer;
                List<Byte> shifts = reKey.Shifts, prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;


                Int32 helper = (Int32)Math.Ceiling
                    (
                        (double)
                        (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                            reKey.PrLength * 2 + shifts.Max() - 4
                        ) / exLength
                    );
                Int32 maxEncodingLength = exLength == 10 ?
                    DigitsInPositive(helper)  //  Optimisation for base 10 encoding
                  : Numsys.AsList
                    (
                        helper.ToString(),
                        10,
                        exLength
                    ).Count;

                Int32[] ids = new Int32[messageLength];
                ids[0] = prAlphabet.IndexOf(message[0]);
                buffer = ids[0] + shifts[0];

                List<Byte> encoding = Numsys.ToCustomAsBinary
                (
                    Split.BigEndianByteList(buffer / exLength, 10),
                    10,
                    exLength,
                    exAlphabet,
                    maxEncodingLength
                );
                List<Byte> encrypted = [exAlphabet[buffer % exLength], .. encoding];


                for (var curId = 1; curId < messageLength; curId++)
                {
                    ids[curId] = prAlphabet.IndexOf(message[curId]);
                    buffer = ids[curId] + shifts[curId % shCount] + ids[curId - 1];

                    encoding = Numsys.ToCustomAsBinary
                    (
                        Split.BigEndianByteList(buffer / exLength, 10),
                        10,
                        exLength,
                        exAlphabet,
                        maxEncodingLength
                    );

                    encrypted.AddRange([exAlphabet[buffer % exLength], .. encoding]);
                }

                return encrypted;
            }

        }
        
        static public class Decrypt
        {
            static public string Text(string encrypted, EncryptionKey reKey, bool throwException = false)
            {
                if (encrypted == null || encrypted == "" || encrypted.Length < 1)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Encrypted message is invalid - cannot be null or empty",
                            nameof(encrypted)
                        );
                    }
                }
                else if (reKey == null)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Encryption key is undefined (null or empty)",
                            nameof(reKey)
                        );
                    }
                }
                else if (reKey.IsPrimaryPartiallyValid(throwException))
                {
                    if (reKey.IsExternalValid(encrypted, throwException))
                    {
                        try
                        {
                            return FastText(encrypted, reKey);
                        }

                        catch (Exception)
                        {
                            if (throwException) throw;
                        }
                    }
                }
                return "";
            }
            static public string Text(string encrypted, EncryptionKey reKey, out Exception? exception)
            {
                if (encrypted == null || encrypted == "" || encrypted.Length < 1)
                {
                    exception = new ArgumentException
                    (
                        "Encrypted message is invalid - cannot be null or empty",
                        nameof(encrypted)
                    );
                }
                else if (reKey == null)
                {
                    exception = new ArgumentException
                    (
                        "Encryption key is undefined (null or empty)",
                        nameof(reKey)
                    );
                }
                else
                {
                    try
                    {
                        reKey.IsPrimaryPartiallyValid(true);
                        reKey.IsExternalValid(encrypted, true);

                        string result = FastText(encrypted, reKey);
                        exception = null;

                        return result;
                    }
                    catch (Exception innerException)
                    {
                        exception = innerException;
                    }
                }
                return "";
            }
            static public string FastText(string encrypted, EncryptionKey reKey)
            {
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount, encCurId = 0, buffer;
                List<Int16> shifts = reKey.Shifts; string prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;

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

                Int32 realMessageLength = encrypted.Length / (maxEncodingLength + 1);
                Int32 parsedEncoding = (Int32) Numsys.ToDecimalFromCustom
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

                Int32[] decodedIds = new Int32[realMessageLength];
                decodedIds[0] = exAlphabet.IndexOf(encrypted[0]) - shifts[0] + parsedEncoding * exLength;
                string decrypted = prAlphabet[decodedIds[0]].ToString();


                for (var curId = 1; curId < realMessageLength; curId++)
                {
                    encCurId += maxEncodingLength + 1;
                    buffer = exAlphabet.IndexOf(encrypted[encCurId])
                        - decodedIds[curId - 1]
                        - shifts[curId % shCount];

                    parsedEncoding = (Int32) Numsys.ToDecimalFromCustom
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

                    decodedIds[curId] = buffer + parsedEncoding * exLength;
                    decrypted += prAlphabet[decodedIds[curId]];
                }

                return decrypted;
            }


            static public string TextFromBytesUtf8(Byte[] message, EncryptionKey reKey, bool throwException = false)
                => Text(FromBinary.Utf8(message), reKey, throwException);
            static public string TextFromBytesUtf8(Byte[] message, EncryptionKey reKey, out Exception? exception)
                => Text(FromBinary.Utf8(message), reKey, out exception);
            static public string FastTextFromBytesUtf8(Byte[] message, EncryptionKey reKey)
                => FastText(FromBinary.Utf8(message), reKey);


            static public string TextFromBytesUtf16(Byte[] message, EncryptionKey reKey, bool throwException = false)
                => Text(FromBinary.Utf16(message), reKey, throwException);
            static public string TextFromBytesUtf16(Byte[] message, EncryptionKey reKey, out Exception? exception)
                => Text(FromBinary.Utf16(message), reKey, out exception);
            static public string FastTextFromBytesUtf16(Byte[] message, EncryptionKey reKey)
                => FastText(FromBinary.Utf16(message), reKey);




            static public List<Byte> Bytes(List<Byte> encrypted, BinaryKey reKey, bool throwException = false)
            {
                if (encrypted == null || encrypted.Count < 1)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Encrypted message is invalid - cannot be null or empty",
                            nameof(encrypted)
                        );
                    }
                }
                else if (reKey == null)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Encryption key is undefined (null or empty)",
                            nameof(reKey)
                        );
                    }
                }
                else if (reKey.IsPrimaryPartiallyValid(throwException))
                {
                    if (reKey.IsExternalValid(encrypted, throwException))
                    {
                        try
                        {
                            return FastBytes(encrypted, reKey);
                        }

                        catch (Exception)
                        {
                            if (throwException) throw;
                        }
                    }
                }
                return [];
            }
            static public List<Byte> Bytes(List<Byte> encrypted, BinaryKey reKey, out Exception? exception)
            {
                if (encrypted == null || encrypted.Count < 1)
                {
                    exception = new ArgumentException
                    (
                        "Encrypted message is invalid - cannot be null or empty",
                        nameof(encrypted)
                    );
                }
                else if (reKey == null)
                {
                    exception = new ArgumentException
                    (
                        "Encryption key is undefined (null or empty)",
                        nameof(reKey)
                    );
                }
                else
                {
                    try
                    {
                        reKey.IsPrimaryPartiallyValid(true);
                        reKey.IsExternalValid(encrypted, true);

                        List<Byte> result = FastBytes(encrypted, reKey);
                        exception = null;

                        return result;
                    }
                    catch (Exception innerException)
                    {
                        exception = innerException;
                    }
                }
                return [];
            }
            static public List<Byte> FastBytes(List<Byte> encrypted, BinaryKey reKey)
            {
                Int32 exLength = reKey.ExLength, shCount = reKey.ShCount, encCurId = 0, buffer;
                List<Byte> shifts = reKey.Shifts; List<Byte> prAlphabet = reKey.PrAlphabet, exAlphabet = reKey.ExAlphabet;

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

                Int32 realMessageLength = encrypted.Count / (maxEncodingLength + 1);
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

                Int32[] decodedIds = new Int32[realMessageLength];
                decodedIds[0] = exAlphabet.IndexOf(encrypted[0]) - shifts[0] + parsedEncoding * exLength;
                List<Byte> decrypted = [prAlphabet[decodedIds[0]]];


                for (var curId = 1; curId < realMessageLength; curId++)
                {
                    encCurId += maxEncodingLength + 1;
                    buffer = exAlphabet.IndexOf(encrypted[encCurId])
                        - decodedIds[curId - 1]
                        - shifts[curId % shCount];

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

                    decodedIds[curId] = buffer + parsedEncoding * exLength;
                    decrypted.Add(prAlphabet[decodedIds[curId]]);
                }

                return decrypted;
            }
        }



        static private Int32 DigitsInPositive(Int32 posNumber)
            => posNumber < 10 ? 1 : posNumber < 100 ? 2 : posNumber < 1_000 ? 3
            :  posNumber < 10_000 ? 4 : posNumber < 100_000 ? 5 : posNumber < 1_000_000 ? 6
            :  posNumber < 10_000_000 ? 7 : posNumber < 100_000_000 ? 8 : posNumber < 1_000_000_000 ? 9 : 10;
    }
}