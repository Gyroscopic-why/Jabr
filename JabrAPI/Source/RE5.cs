using System;
using System.Linq;
using System.Collections.Generic;

using static System.Console;


using AVcontrol;



namespace JabrAPI
{
    public class RE5
    {
        public class EncryptionKey
        {
            private string  _primaryAlphabet;
            private string  _externalAlphabet;
            private Int32[] _shifts;

            private List<char> _primaryNecessary,  _primaryAllowed,  _primaryBanned;
            private List<char> _externalNecessary, _externalAllowed, _externalBanned;
            private Int32 _primaryMaxLength = -1,  _externalMaxLength = -1;


            public EncryptionKey(string primaryAlphabet, string externalAlphabet, Int32[] shifts)
            {
                _primaryAlphabet = primaryAlphabet;
                _externalAlphabet = externalAlphabet;
                _shifts = shifts;
            }
            public EncryptionKey(string primaryAlphabet, string externalAlphabet, Int32 shift)
            {
                _primaryAlphabet = primaryAlphabet;
                _externalAlphabet = externalAlphabet;
                _shifts = new Int32[] { shift };
            }
            public EncryptionKey(string primaryAlphabet, string externalAlphabet)
            {
                _primaryAlphabet = primaryAlphabet;
                _externalAlphabet = externalAlphabet;
            }
            public EncryptionKey()
            {
                SetDefault();
                GenerateRandomPrimary();
                GenerateRandomExternal();
                GenerateRandomShifts();
            }



            public string PrimaryAlphabet => _primaryAlphabet;
            public Int32  PrLength => _primaryAlphabet.Length;

            public string ExternalAlphabet => _externalAlphabet;
            public Int32  ExLength => _externalAlphabet.Length;

            public Int32[] Shifts => _shifts;



            public string ExportAsString(string splitBy, string splitShifts = " ")
            {
                if (_primaryAlphabet.Contains(splitBy) || _externalAlphabet.Contains(splitBy))
                {
                    throw new ArgumentException
                    (
                        "split argument is not unique, impossible to distinguish data using it",
                        "splitBy"
                    );
                }

                string result = _primaryAlphabet + splitBy + _externalAlphabet + splitBy;
                for (Int32 curId = 0; curId < _shifts.Length - 1; curId++) result += _shifts[curId] + splitShifts;
                if (_shifts.Length > 0) result += _shifts[_shifts.Length - 1];

                return result;
            }
            public string ExportAsString()
            {
                if (!Byte.TryParse(_primaryAlphabet[0].ToString(), out _) ||
                    !Byte.TryParse(_externalAlphabet[0].ToString(), out _))
                {
                    throw new ArgumentException
                    (
                        "impossible to use length as a splitter due to one of the alphabets having numbers in its start",
                        "_primaryAlphabet or _externalAlphabet"
                    );
                }

                string result = _primaryAlphabet.Length + _primaryAlphabet
                             + _externalAlphabet.Length + _externalAlphabet;

                for (Int32 curId = 0; curId < _shifts.Length - 1; curId++)
                    result += _shifts[curId] + " ";
                if (_shifts.Length > 0) result += _shifts[_shifts.Length - 1];

                return result;
            }
            public List<Byte> ExportAsBinaryBE()
            {
                List<Byte> result = new List<Byte>();

                Byte[] buffer = ToBinary.BigEndian(_shifts.Length), bufferLength;
                result.Add(buffer[0]);
                result.Add(buffer[1]);
                result.Add(buffer[2]);
                result.Add(buffer[3]);

                for (Int32 curId = 0; curId < _shifts.Length; curId++)
                {
                    buffer = ToBinary.BigEndian(_shifts[curId]);

                    result.Add(buffer[0]);
                    result.Add(buffer[1]);
                    result.Add(buffer[2]);
                    result.Add(buffer[3]);
                }



                buffer = ToBinary.Utf8(_primaryAlphabet);
                bufferLength = ToBinary.BigEndian(buffer.Length);
                result.Add(bufferLength[0]);
                result.Add(bufferLength[1]);
                result.Add(bufferLength[2]);
                result.Add(bufferLength[3]);

                for (Int32 curId = 0; curId < buffer.Length; curId++)
                    result.Add(buffer[curId]);



                buffer = ToBinary.Utf8(_externalAlphabet);
                bufferLength = ToBinary.BigEndian(buffer.Length);
                result.Add(bufferLength[0]);
                result.Add(bufferLength[1]);
                result.Add(bufferLength[2]);
                result.Add(bufferLength[3]);

                for (Int32 curId = 0; curId < buffer.Length; curId++)
                    result.Add(buffer[curId]);


                return result;
            }
            public List<Byte> ExportAsBinaryLE()
            {
                List<Byte> result = new List<Byte>();

                Byte[] buffer = ToBinary.LittleEndian(_shifts.Length), bufferLength;
                result.Add(buffer[0]);
                result.Add(buffer[1]);
                result.Add(buffer[2]);
                result.Add(buffer[3]);

                for (Int32 curId = 0; curId < _shifts.Length; curId++)
                {
                    buffer = ToBinary.LittleEndian(_shifts[curId]);

                    result.Add(buffer[0]);
                    result.Add(buffer[1]);
                    result.Add(buffer[2]);
                    result.Add(buffer[3]);
                }



                buffer = ToBinary.Utf8(_primaryAlphabet);
                bufferLength = ToBinary.LittleEndian(buffer.Length);
                result.Add(bufferLength[0]);
                result.Add(bufferLength[1]);
                result.Add(bufferLength[2]);
                result.Add(bufferLength[3]);

                for (Int32 curId = 0; curId < buffer.Length; curId++)
                    result.Add(buffer[curId]);



                buffer = ToBinary.Utf8(_externalAlphabet);
                bufferLength = ToBinary.LittleEndian(buffer.Length);
                result.Add(bufferLength[0]);
                result.Add(bufferLength[1]);
                result.Add(bufferLength[2]);
                result.Add(bufferLength[3]);

                for (Int32 curId = 0; curId < buffer.Length; curId++)
                    result.Add(buffer[curId]);


                return result;
            }



            public bool IsPrimaryValid(string message, bool throwException = false)
            {
                string primary = _primaryAlphabet;
                if (primary == null || primary == "" || primary.Length < 2)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Primary alphabet is not set or is too short",
                            "PrimaryAlphabet"
                        );
                    }
                    return false;
                }
                foreach (char c in message)
                {
                    if (!primary.Contains(c))
                    {
                        if (throwException)
                        {
                            throw new ArgumentException
                            (
                                "Message contains characters not in the primary alphabet",
                                "message, char: " + c
                            );
                        }
                        return false;
                    }
                }

                return true;
            }


            public void Next()
            {
                try
                {
                    GenerateRandomPrimary();
                    GenerateRandomExternal();
                    GenerateRandomShifts();
                }
                catch 
                {
                    GenerateRandomPrimary(_primaryAlphabet, _primaryAlphabet.Length);
                    GenerateRandomExternal(_primaryAlphabet, _externalAlphabet.Length);
                    GenerateRandomShifts(_shifts.Length);
                }
            }



            public void SetDefault(List<char> pNecessary, List<char> pAllowed, List<char> pBanned, Int32 pMaxLength,
                                   List<char> eNecessary, List<char> eAllowed, List<char> eBanned, Int32 eMaxLength)
            {
                _primaryNecessary  = pNecessary;
                _externalNecessary = eNecessary;

                _primaryAllowed  = pAllowed;
                _externalAllowed = eAllowed;

                _primaryBanned  = pBanned;
                _externalBanned = eBanned;

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(string pNecessary, string pAllowed, string pBanned, Int32 pMaxLength,
                                   string eNecessary, string eAllowed, string eBanned, Int32 eMaxLength)
            {
                _primaryNecessary  = pNecessary.ToList();
                _externalNecessary = eNecessary.ToList();

                _primaryAllowed  = pAllowed.ToList();
                _externalAllowed = eAllowed.ToList();

                _primaryBanned  = pBanned.ToList();
                _externalBanned = eBanned.ToList();

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }

            public void SetDefault(List<char> pNecessary, List<char> pAllowed, Int32 pMaxLength,
                                   List<char> eNecessary, List<char> eAllowed, Int32 eMaxLength)
            {
                _primaryNecessary  = pNecessary;
                _externalNecessary = eNecessary;

                _primaryAllowed  = pAllowed;
                _externalAllowed = eAllowed;

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(string pNecessary, string pAllowed, Int32 pMaxLength,
                                   string eNecessary, string eAllowed, Int32 eMaxLength)
            {
                _primaryNecessary  = pNecessary.ToList();
                _externalNecessary = eNecessary.ToList();

                _primaryAllowed  = pAllowed.ToList();
                _externalAllowed = eAllowed.ToList();

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }

            public void SetDefault(List<char> necessary, List<char> allowed, List<char> banned, Int32 maxLength)
            {
                _primaryNecessary  = necessary;
                _externalNecessary = necessary;

                _primaryAllowed  = allowed;
                _externalAllowed = allowed;

                _primaryBanned  = banned;
                _externalBanned = banned;

                _primaryMaxLength  = maxLength;
                _externalMaxLength = maxLength;
            }
            public void SetDefault(string necessary, string allowed, string banned, Int32 maxLength)
            {
                _primaryNecessary  = necessary.ToList();
                _externalNecessary = necessary.ToList();

                _primaryAllowed  = allowed.ToList();
                _externalAllowed = allowed.ToList();

                _primaryBanned  = banned.ToList();
                _externalBanned = banned.ToList();

                _primaryMaxLength  = maxLength;
                _externalMaxLength = maxLength;
            }

            public void SetDefault(List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                _primaryNecessary  = necessary;
                _externalNecessary = necessary;

                _primaryAllowed  = allowed;
                _externalAllowed = allowed;

                _primaryMaxLength  = maxLength;
                _externalMaxLength = maxLength;
            }
            public void SetDefault(string necessary, string allowed, Int32 maxLength)
            {
                _primaryNecessary  = necessary.ToList();
                _externalNecessary = necessary.ToList();

                _primaryAllowed  = allowed.ToList();
                _externalAllowed = allowed.ToList();

                _primaryMaxLength  = maxLength;
                _externalMaxLength = maxLength;
            }

            public void SetDefault(Int32 pMaxLength, List<char> pNecessary, List<char> pBanned,
                                   Int32 eMaxLength, List<char> eNecessary, List<char> eBanned)
            {
                _primaryNecessary  = pNecessary;
                _externalNecessary = eNecessary;

                _primaryBanned  = pBanned;
                _externalBanned = eBanned;

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(Int32 pMaxLength, string pNecessary, string pBanned,
                                   Int32 eMaxLength, string eNecessary, string eBanned)
            {
                _primaryNecessary  = pNecessary.ToList();
                _externalNecessary = eNecessary.ToList();

                _primaryBanned  = pBanned.ToList();
                _externalBanned = eBanned.ToList();

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(Int32 pMaxLength, List<char> pBanned,
                                   Int32 eMaxLength, List<char> eBanned)
            {
                _primaryBanned  = pBanned;
                _externalBanned = eBanned;

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(Int32 pMaxLength, string pBanned,
                                   Int32 eMaxLength, string eBanned)
            {
                _primaryBanned  = pBanned.ToList();
                _externalBanned = eBanned.ToList();

                _primaryMaxLength  = pMaxLength;
                _externalMaxLength = eMaxLength;
            }

            public void SetDefault(Int32 maxLength, List<char> necessary, List<char> banned)
            {
                _primaryNecessary  = necessary;
                _externalNecessary = necessary;

                _primaryBanned  = banned;
                _externalBanned = banned;

                _primaryMaxLength  = maxLength;
                _externalMaxLength = maxLength;
            }
            public void SetDefault(Int32 maxLength, string necessary, string banned)
            {
                _primaryNecessary  = necessary.ToList();
                _externalNecessary = necessary.ToList();

                _primaryBanned  = banned.ToList();
                _externalBanned = banned.ToList();

                _primaryMaxLength  = maxLength;
                _externalMaxLength = maxLength;
            }
            public void SetDefault(Int32 maxLength, List<char> banned)
            {
                _primaryBanned  = banned;
                _externalBanned = banned;

                _primaryMaxLength  = maxLength;
                _externalMaxLength = maxLength;
            }
            public void SetDefault(Int32 maxLength, string banned)
            {
                _primaryBanned  = banned.ToList();
                _externalBanned = banned.ToList();

                _primaryMaxLength  = maxLength;
                _externalMaxLength = maxLength;
            }
            public void SetDefault()
            {
                _primaryNecessary  = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№".ToList();

                _primaryAllowed    = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№".ToList();
                _externalAllowed   = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№".ToList();

                _primaryMaxLength  = _primaryNecessary.Count;
                _externalMaxLength = 8;
            }



            public void GenerateRandomPrimary(List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                maxLength -= necessary.Count;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "Max length: " + maxLength + necessary.Count +
                        ", Necessary characters count: " + necessary.Count
                    );
                }
                else if (maxLength > allowed.Count)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be more than allowed characters count",
                        "maxLength: " + maxLength +
                        ", allowed count: " + allowed.Count
                    );
                }
                else if (necessary != null)
                {
                    for (Int32 id1 = 0; id1 < necessary.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < necessary.Count; id2++)
                        {
                            if (necessary[id1] == necessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "necessary list cannot include duplicates",
                                    "necessary, char: " + necessary[id1]
                                );
                            }
                        }
                    }
                }
                else if (allowed != null)
                {
                    for (Int32 id1 = 0; id1 < allowed.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < allowed.Count; id2++)
                        {
                            if (allowed[id1] == allowed[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicates",
                                    "allowed, char: " + allowed[id1]
                                );
                            }
                        }
                        for (Int32 id2 = 0; id2 < _primaryNecessary.Count; id2++)
                        {
                            if (_primaryAllowed[id1] == _primaryNecessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "primary allowed list cannot include duplicates of necessary list",
                                    "_primaryAllowed, char: " + _primaryAllowed[id1]
                                );
                            }
                        }
                    }
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";

                while (necessary.Count > 0)
                {
                    buffer = random.Next(0, necessary.Count);
                    _primaryAlphabet += necessary[buffer];
                    necessary.RemoveAt(buffer);
                }
                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Count);
                    bufferId = random.Next(0, _primaryAlphabet.Length);

                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.RemoveAt(buffer);
                }
            }
            public void GenerateRandomPrimary(string necessary, string allowed, Int32 maxLength)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                maxLength -= necessary.Length;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "Max length: " + maxLength + necessary.Length +
                        ", Necessary characters length: " + necessary.Length
                    );
                }
                else if (maxLength > allowed.Length)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be more than allowed characters length",
                        "maxLength: " + maxLength +
                        ", allowed length: " + allowed.Length.ToString()
                    );
                }
                else if (necessary != null)
                {
                    for (Int32 id1 = 0; id1 < necessary.Length; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < necessary.Length; id2++)
                        {
                            if (necessary[id1] == necessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "necessary list cannot include duplicates",
                                    "necessary, char: " + necessary[id1]
                                );
                            }
                        }
                    }
                }
                else if (allowed != null)
                {
                    for (Int32 id1 = 0; id1 < allowed.Length; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < allowed.Length; id2++)
                        {
                            if (allowed[id1] == allowed[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicates",
                                    "allowed, char: " + allowed[id1]
                                );
                            }
                        }
                        for (Int32 id2 = 0; id2 < _primaryNecessary.Count; id2++)
                        {
                            if (_primaryAllowed[id1] == _primaryNecessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "primary allowed list cannot include duplicates of necessary list",
                                    "_primaryAllowed, char: " + _primaryAllowed[id1]
                                );
                            }
                        }
                    }
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";

                while (necessary.Length > 0)
                {
                    buffer = random.Next(0, necessary.Length);
                    _primaryAlphabet += necessary[buffer];
                    necessary.Remove(buffer, 1);
                }
                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    bufferId = random.Next(0, _primaryAlphabet.Length);

                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.Remove(buffer, 1);
                }
            }
            public void GenerateRandomPrimary(List<char> allowed, Int32 maxLength)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                else if (allowed != null)
                {
                    for (Int32 id1 = 0; id1 < allowed.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < allowed.Count; id2++)
                        {
                            if (allowed[id1] == allowed[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicates",
                                    "allowed, char: " + allowed[id1]
                                );
                            }
                        }
                    }
                }
                else if (maxLength > allowed.Count)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be more than allowed characters length",
                        "maxLength: " + maxLength +
                        ", allowed length: " + allowed.Count.ToString()
                    );
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";

                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Count);
                    bufferId = random.Next(0, _primaryAlphabet.Length);

                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.RemoveAt(buffer);
                }
            }
            public void GenerateRandomPrimary(string allowed, Int32 maxLength)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                else if (allowed != null)
                {
                    for (Int32 id1 = 0; id1 < allowed.Length; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < allowed.Length; id2++)
                        {
                            if (allowed[id1] == allowed[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicates",
                                    "allowed, char: " + allowed[id1]
                                );
                            }
                        }
                    }
                }
                else if (maxLength > allowed.Length)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be more than allowed characters length",
                        "maxLength: " + maxLength +
                        ", allowed length: " + allowed.Length.ToString()
                    );
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";

                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    bufferId = random.Next(0, _primaryAlphabet.Length);

                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.Remove(buffer, 1);
                }
            }
            public void GenerateRandomPrimary(Int32 maxLength, List<char> necessary, List<char> banned)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                maxLength -= necessary.Count;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "maxLength: " + maxLength + necessary.Count +
                        ", Necessary characters count: " + necessary.Count
                    );
                }
                else if (maxLength > 163)
                {
                    throw new ArgumentException
                    (
                        "Max length is more than the default array of allowed chars count (163)",
                        "maxLength > 163"
                    );
                }
                else if (necessary != null)
                {
                    for (Int32 id1 = 0; id1 < necessary.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < necessary.Count; id2++)
                        {
                            if (necessary[id1] == necessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "necessary list cannot include duplicates",
                                    "necessary, char: " + necessary[id1]
                                );
                            }
                        }
                    }
                }

                string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";

                while (necessary.Count > 0)
                {
                    buffer = random.Next(0, necessary.Count);
                    _primaryAlphabet += necessary[buffer];
                    necessary.RemoveAt(buffer);
                }
                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    for (Int32 curId = 0; curId < banned.Count; curId++)
                    {
                        if (allowed[buffer] == banned[curId])
                        {
                            curId = 0;
                            buffer = random.Next(0, allowed.Length);
                        }
                    }

                    bufferId = random.Next(0, _externalAlphabet.Length);
                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.Remove(buffer, 1);
                }
            }
            public void GenerateRandomPrimary(Int32 maxLength, string necessary, string banned)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                maxLength -= necessary.Length;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "maxLength: " + maxLength + necessary.Length +
                        ", Necessary characters length: " + necessary.Length
                    );
                }
                else if (maxLength > 163)
                {
                    throw new ArgumentException
                    (
                        "Max length is more than the default array of allowed chars length (163)",
                        "maxLength > 163"
                    );
                }
                else if (necessary != null)
                {
                    for (Int32 id1 = 0; id1 < necessary.Length; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < necessary.Length; id2++)
                        {
                            if (necessary[id1] == necessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "necessary list cannot include duplicates",
                                    "necessary, char: " + necessary[id1]
                                );
                            }
                        }
                    }
                }

                string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";

                while (necessary.Length > 0)
                {
                    buffer = random.Next(0, necessary.Length);
                    _primaryAlphabet += necessary[buffer];
                    necessary.Remove(buffer, 1);
                }
                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    for (Int32 curId = 0; curId < banned.Length; curId++)
                    {
                        if (allowed[buffer] == banned[curId])
                        {
                            curId = 0;
                            buffer = random.Next(0, allowed.Length);
                        }
                    }

                    bufferId = random.Next(0, _externalAlphabet.Length);
                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.Remove(buffer, 1);
                }
            }
            public void GenerateRandomPrimary(Int32 maxLength, List<char> banned)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                else if (maxLength > 163)
                {
                    throw new ArgumentException
                    (
                        "Max length is more than the default array of allowed chars length (163)",
                        "maxLength > 163"
                    );
                }

                string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";

                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    for (Int32 curId = 0; curId < banned.Count; curId++)
                    {
                        if (allowed[buffer] == banned[curId])
                        {
                            curId = 0;
                            buffer = random.Next(0, allowed.Length);
                        }
                    }

                    bufferId = random.Next(0, _primaryAlphabet.Length);
                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.Remove(buffer, 1);
                }
            }
            public void GenerateRandomPrimary(Int32 maxLength, string banned)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                else if (maxLength > 163)
                {
                    throw new ArgumentException
                    (
                        "Max length is more than the default array of allowed chars length (163)",
                        "maxLength > 163"
                    );
                }

                string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";

                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    for (Int32 curId = 0; curId < banned.Length; curId++)
                    {
                        if (allowed[buffer] == banned[curId])
                        {
                            curId = 0;
                            buffer = random.Next(0, allowed.Length);
                        }
                    }

                    bufferId = random.Next(0, _primaryAlphabet.Length);
                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.Remove(buffer, 1);
                }
            }
            public void GenerateRandomPrimary(Int32 maxLength) => GenerateRandomPrimary(maxLength, "");
            public void GenerateRandomPrimary()
            {
                if (_primaryAllowed == null && _primaryBanned == null && _primaryNecessary == null)
                {
                    throw new ArgumentException
                    (
                        "Unable to use undefined default parameters",
                        "_primaryAllowed and _primaryBanned And _primaryNecessary"
                    );
                }
                else if (_primaryMaxLength < 2 ||
                    _primaryNecessary != null && _primaryMaxLength < _primaryNecessary.Count)
                {
                    throw new ArgumentException
                    (
                        "Max length is undefined or less than neccesary chars amount",
                        "_primaryMaxLength, _primaryNecessary.Count"
                    );
                }
                else if (_primaryAllowed != null && _primaryMaxLength > _primaryAllowed.Count ||
                         _primaryAllowed == null && _primaryMaxLength > 163)
                {
                    throw new ArgumentException
                    (
                        "Max length is more than the max allowed count or max default count 163 (if _primaryAllowed == null)",
                        "_primaryMaxLength > _primaryAllowed.Count or 163"
                    );
                }
                else if (_primaryNecessary != null)
                {
                    for (Int32 id1 = 0; id1 < _primaryNecessary.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < _primaryNecessary.Count; id2++)
                        {
                            if (_primaryNecessary[id1] == _primaryNecessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "primary necessary list cannot include duplicates",
                                    "_primaryNecessary, char: " + _primaryNecessary[id1]
                                );
                            }
                        }
                    }
                }
                else if (_primaryAllowed != null)
                {
                    for (Int32 id1 = 0; id1 < _primaryAllowed.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < _primaryAllowed.Count; id2++)
                        {
                            if (_primaryAllowed[id1] == _primaryAllowed[id2])
                            {
                                throw new ArgumentException
                                (
                                    "primary allowed list cannot include duplicates",
                                    "_primaryAllowed, char: " + _primaryAllowed[id1]
                                );
                            }
                        }
                        for (Int32 id2 = 0; id2 < _primaryNecessary.Count; id2++)
                        {
                            if (_primaryAllowed[id1] == _primaryNecessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "primary allowed list cannot include duplicates of necessary list",
                                    "_primaryAllowed, char: " + _primaryAllowed[id1]
                                );
                            }
                        }
                    }
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";

                if (_primaryNecessary != null)
                {
                    List<char> necessary = _primaryNecessary;
                    while (necessary.Count > 0)
                    {
                        buffer = random.Next(0, necessary.Count);
                        _primaryAlphabet += necessary[buffer];
                        necessary.RemoveAt(buffer);
                    }
                }
                if (_primaryAllowed != null)
                {
                    List<char> allowed = _primaryAllowed;

                    for (Int32 remaining = _primaryMaxLength - _primaryNecessary.Count; remaining > 0; remaining--)
                    {
                        buffer = random.Next(0, allowed.Count);
                        bufferId = random.Next(0, _primaryAlphabet.Length);

                        _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                        allowed.RemoveAt(buffer);
                    }
                }
                else if (_primaryBanned != null)
                {
                    for (Int32 remaining = _primaryMaxLength - _primaryNecessary.Count; remaining > 0; remaining--)
                    {
                        const string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                        buffer = random.Next(0, allowed.Length);
                        for (Int32 curId = 0; curId < _primaryBanned.Count; curId++)
                        {
                            if (allowed[buffer] == _primaryBanned[curId])
                            {
                                curId = 0;
                                buffer = random.Next(0, allowed.Length);
                            }
                        }

                        bufferId = random.Next(0, _primaryAlphabet.Length);
                        _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                        allowed.Remove(buffer, 1);
                    }
                }
            }


            public void GenerateRandomExternal(List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                maxLength -= necessary.Count;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "Max length: " + maxLength + necessary.Count +
                        ", Necessary characters count: " + necessary.Count.ToString()
                    );
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _externalAlphabet = "";

                while (necessary.Count > 0)
                {
                    buffer = random.Next(0, necessary.Count);
                    _externalAlphabet += necessary[buffer];
                    necessary.RemoveAt(buffer);
                }
                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Count);
                    bufferId = random.Next(0, _externalAlphabet.Length);

                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                }
            }
            public void GenerateRandomExternal(string necessary, string allowed, Int32 maxLength)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                maxLength -= necessary.Length;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "Max length: " + maxLength + necessary.Length +
                        ", Necessary characters count: " + necessary.Length.ToString()
                    );
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _externalAlphabet = "";

                while (necessary.Length > 0)
                {
                    buffer = random.Next(0, necessary.Length);
                    _externalAlphabet += necessary[buffer];
                    necessary.Remove(buffer, 1);
                }
                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    bufferId = random.Next(0, _externalAlphabet.Length);

                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                }
            }
            public void GenerateRandomExternal(List<char> allowed, Int32 maxLength)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _externalAlphabet = "";

                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Count);
                    bufferId = random.Next(0, _externalAlphabet.Length);

                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.RemoveAt(buffer);
                }
            }
            public void GenerateRandomExternal(string allowed, Int32 maxLength)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _externalAlphabet = "";

                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    bufferId = random.Next(0, _externalAlphabet.Length);

                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.Remove(buffer, 1);
                }
            }
            public void GenerateRandomExternal(Int32 maxLength, List<char> necessary, List<char> banned)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                maxLength -= necessary.Count;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "Max length: " + maxLength + necessary.Count +
                        ", Necessary characters count: " + necessary.Count.ToString()
                    );
                }

                const string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                Random random = new Random();
                Int32 buffer, bufferId;
                _externalAlphabet = "";

                while (necessary.Count > 0)
                {
                    buffer = random.Next(0, necessary.Count);
                    _externalAlphabet += necessary[buffer];
                    necessary.RemoveAt(buffer);
                }
                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    for (Int32 curId = 0; curId < banned.Count; curId++)
                    {
                        if (allowed[buffer] == banned[curId])
                        {
                            curId = 0;
                            buffer = random.Next(0, allowed.Length);
                        }
                    }

                    bufferId = random.Next(0, _externalAlphabet.Length);
                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                }
            }
            public void GenerateRandomExternal(Int32 maxLength, string necessary, string banned)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }
                maxLength -= necessary.Length;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        "Max length: " + maxLength + necessary.Length +
                        ", Necessary characters length: " + necessary.Length.ToString()
                    );
                }

                const string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                Random random = new Random();
                Int32 buffer, bufferId;
                _externalAlphabet = "";

                while (necessary.Length > 0)
                {
                    buffer = random.Next(0, necessary.Length);
                    _externalAlphabet += necessary[buffer];
                    necessary.Remove(buffer, 1);
                }
                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    for (Int32 curId = 0; curId < banned.Length; curId++)
                    {
                        if (allowed[buffer] == banned[curId])
                        {
                            curId = 0;
                            buffer = random.Next(0, allowed.Length);
                        }
                    }

                    bufferId = random.Next(0, _externalAlphabet.Length);
                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                }
            }
            public void GenerateRandomExternal(Int32 maxLength, List<char> banned)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }

                const string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                Random random = new Random();
                Int32 buffer, bufferId;
                _externalAlphabet = "";

                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    for (Int32 curId = 0; curId < banned.Count; curId++)
                    {
                        if (allowed[buffer] == banned[curId])
                        {
                            curId = 0;
                            buffer = random.Next(0, allowed.Length);
                        }
                    }

                    bufferId = random.Next(0, _externalAlphabet.Length);
                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                }
            }
            public void GenerateRandomExternal(Int32 maxLength, string banned)
            {
                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the lowest possible alphabet length (2)",
                        "maxLength < 2"
                    );
                }

                const string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                Random random = new Random();
                Int32 buffer, bufferId;
                _externalAlphabet = "";

                for (Int32 remaining = maxLength; remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Length);
                    for (Int32 curId = 0; curId < banned.Length; curId++)
                    {
                        if (allowed[buffer] == banned[curId])
                        {
                            curId = 0;
                            buffer = random.Next(0, allowed.Length);
                        }
                    }

                    bufferId = random.Next(0, _externalAlphabet.Length);
                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                }
            }
            public void GenerateRandomExternal(Int32 maxLength) => GenerateRandomExternal(maxLength, "");
            public void GenerateRandomExternal()
            {
                if (_externalAllowed == null && _externalBanned == null)
                {
                    throw new ArgumentException
                    (
                        "Unable to use undefined default parameters",
                        "_externalAllowed and _externalBanned"
                    );
                }
                if (_externalMaxLength < 2 ||
                    _externalNecessary != null && _externalMaxLength < _externalNecessary.Count)
                {
                    throw new ArgumentException
                    (
                        "Max length is undefined or less than neccesary chars amount",
                        "_externalMaxLength, _externalNecessary.Count"
                    );
                }

                Random random = new Random();
                Int32 buffer, bufferId;
                _externalAlphabet = "";

                if (_externalNecessary != null)
                {
                    List<char> necessary = _externalNecessary;
                    while (necessary.Count > 0)
                    {
                        buffer = random.Next(0, necessary.Count);
                        _externalAlphabet += necessary[buffer];
                        necessary.RemoveAt(buffer);
                    }

                    if (_externalAllowed != null)
                    {
                        List<char> allowed = _externalAllowed;

                        for (Int32 remaining = _externalMaxLength - _externalNecessary.Count; remaining > 0; remaining--)
                        {
                            buffer = random.Next(0, allowed.Count);
                            bufferId = random.Next(0, _externalAlphabet.Length);

                            _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                        }
                    }
                }
                else if (_externalAllowed != null)
                {
                    List<char> allowed = _externalAllowed;

                    for (Int32 remaining = _externalMaxLength; remaining > 0; remaining--)
                    {
                        buffer = random.Next(0, allowed.Count);
                        bufferId = random.Next(0, _externalAlphabet.Length);

                        _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    }
                }
                else
                {
                    for (Int32 remaining = _externalMaxLength; remaining > 0; remaining--)
                    {
                        const string allowed = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";
                        buffer = random.Next(0, allowed.Length);
                        for (Int32 curId = 0; curId < _externalBanned.Count; curId++)
                        {
                            if (allowed[buffer] == _externalBanned[curId])
                            {
                                curId = 0;
                                buffer = random.Next(0, allowed.Length);
                            }
                        }

                        bufferId = random.Next(0, _externalAlphabet.Length);
                        _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    }
                }
            }


            public void GenerateRandomShifts(Int32 amount)
            {
                if (amount < 0)
                {
                    throw new ArgumentOutOfRangeException
                    (
                        "amount",
                        "amount cannot be negative"
                    );
                }
                else if (_externalAlphabet == null || _externalAlphabet.Length < 2)
                {
                    throw new ArgumentException
                    (
                        "Unable to generate shifts, external alphabet is undefined",
                        "_externalAlphabet"
                    );
                }

                _shifts = new Int32[amount];
                Random random = new Random();

                for (Int32 curId = 0; curId < amount; curId++)
                    _shifts[curId] = random.Next(0, _externalAlphabet.Length - 1);
            }
            public void GenerateRandomShifts()
            {
                if (_externalAlphabet == null || _externalAlphabet.Length < 2)
                {
                    throw new ArgumentException
                    (
                        "Unable to generate shifts, external alphabet is undefined",
                        "_externalAlphabet"
                    );
                }

                Random random = new Random();
                Int32 count = random.Next(512, 1025);
                _shifts = new Int32[count];

                for (Int32 curId = 0; curId < count; curId++)
                    _shifts[curId] = random.Next(0, _externalAlphabet.Length - 1);
            }
        }

        public class BinaryReadyKey
        {

        }



        static public string Encrypt(string message, EncryptionKey reKey)
        {
            return "aboba";
        }
        static public List<Byte> EncryptToBytes(EncryptionKey reKey)
        {
            return null;
        }
        static public List<Byte> EncryptToBytes(BinaryReadyKey binReKey)
        {
            return null;
        }


        static public string Decrypt(string message, EncryptionKey reKey)
        {
            return "aboba";
        }
        static public string DecryptFromBytes()
        {
            return "aboba";
        }
    }
}