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
            public EncryptionKey() => Default();



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
            static public bool IsPrimaryValid(string message, string primary, bool throwException = false)
            {
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
                try { GenerateAll(); }
                catch { Default(); }
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
                _primaryNecessary  = " `1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№".ToList();
                _externalAllowed   = " `1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№".ToList();

                _primaryMaxLength  = _primaryNecessary.Count;
                _externalMaxLength = 8;
            }

            private void Default()
            {
                SetDefault();
                GenerateAll();
            }
            private void GenerateAll()
            {
                GenerateRandomPrimary();
                GenerateRandomExternal();
                GenerateRandomShifts();
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


            public void GenerateRandomShifts(Int32 count)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException
                    (
                        "count",
                        "count cannot be negative"
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

                _shifts = new Int32[count];
                Random random = new Random();

                for (Int32 curId = 0; curId < count; curId++)
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
            List<Int16> _primaryAlpabet;
            List<Int16> _externalAlphabet;
            List<Int32> _shifts;
        }



        static public string Encrypt(string message, EncryptionKey reKey)
        {
            if (message == null || message == "" || message.Length < 1)
            {
                throw new ArgumentException
                (
                    "Message is invalid - cannot be null or empty",
                    "message"
                );
            }
            else if (reKey == null)
            {
                throw new ArgumentException
                (
                    "Encryption key is undefined (null or empty)",
                    "reKey"
                );
            }
            else if (reKey.ExternalAlphabet == null || reKey.ExternalAlphabet == "" || reKey.ExLength < 2)
            {
                throw new ArgumentException
                (
                    "The external alphabet of the encryption key is undefined or too small",
                    "reKey.ExternalAlphabet"
                );
            }

            try { reKey.IsPrimaryValid(message, true); }
            catch { return null; }

            try { return FastEncrypt(message, reKey); }
            catch { return null; }
        }
        static public string Encrypt(string message, EncryptionKey reKey, out Exception exception)
        {
            if (message == null || message == "" || message.Length < 1)
            {
                throw new ArgumentException
                (
                    "Message is invalid - cannot be null or empty",
                    "message"
                );
            }
            else if (reKey == null)
            {
                throw new ArgumentException
                (
                    "Encryption key is undefined (null or empty)",
                    "reKey"
                );
            }
            else if (reKey.ExternalAlphabet == null || reKey.ExternalAlphabet == "" || reKey.ExLength < 2)
            {
                throw new ArgumentException
                (
                    "The external alphabet of the encryption key is undefined or too small",
                    "reKey.ExternalAlphabet"
                );
            }

            exception = null;
            try { reKey.IsPrimaryValid(message, true); }
            catch (Exception innerException)
            {
                exception = innerException;
                return null;  
            }

            try { return FastEncrypt(message, reKey); }
            catch (Exception innerException)
            {
                exception = innerException;
                return null;
            }
        }
        static public string FastEncrypt(string message, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, messageLength = message.Length;
            Int32 shLength = reKey.Shifts != null ? reKey.Shifts.Length : 0;
            Int32[] buffer = new Int32[messageLength];

            List<Int32> ids = new List<Int32>();
            for (Int32 curChar = 0; curChar < messageLength; curChar++)
                ids.Add(reKey.PrimaryAlphabet.IndexOf(message[curChar]));

            if (shLength > 1)
            {
                buffer[0] = ids[0] + reKey.Shifts[0];

                // Optimisation for base 10 encoding
                if (exLength == 10)
                {
                    Int32 maxEncodingLength = DigitsInPositive
                    (
                        (Int32) Math.Ceiling
                        (
                            (float)
                            (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrimaryAlphabet.Length * 2 + reKey.Shifts.Max() - 4
                            ) / exLength
                        )
                    );

                    Int32 bufferId = buffer[0] / exLength;
                    char encodingDefault = reKey.ExternalAlphabet[0];
                    string encoding = "";
                    while (bufferId > 0)
                    {
                        encoding += reKey.ExternalAlphabet[bufferId % 10];
                        bufferId /= 10;
                    }
                    for (Int32 extend = encoding.Length; extend < maxEncodingLength; extend++)
                        encoding += encodingDefault;


                    string encrypted = reKey.ExternalAlphabet[buffer[0] % exLength] + new string(encoding.Reverse().ToArray());

                    for (Int32 curId = 1; curId < messageLength; curId++)
                    {
                        buffer[curId] = ids[curId] + reKey.Shifts[curId % shLength] + ids[curId - 1];
                        encoding = "";

                        while (bufferId > 0)
                        {
                            encoding += reKey.ExternalAlphabet[bufferId % 10];
                            bufferId /= 10;
                        }
                        for (Int32 extend = encoding.Length; extend < maxEncodingLength; extend++)
                            encoding += encodingDefault;

                        encrypted += reKey.ExternalAlphabet[buffer[curId] % exLength] + new string(encoding.Reverse().ToArray());
                    }
                    return encrypted;
                }
                else
                {
                    Int32 maxEncodingLength = Numsys.AutoAsList
                    (
                        Math.Ceiling
                        (
                            (float)
                            (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrimaryAlphabet.Length * 2 + reKey.Shifts.Max() - 4
                            ) / exLength
                        ).ToString(),
                        10,
                        exLength
                    ).Count;
                    string encoding = Numsys.ToCustomAsString
                    (
                        (buffer[0] / exLength).ToString(),
                        10,
                        exLength,
                        reKey.ExternalAlphabet,
                        maxEncodingLength
                    );


                    string encrypted = reKey.ExternalAlphabet[buffer[0] % exLength] + encoding;

                    for (Int32 curId = 1; curId < messageLength; curId++)
                    {
                        buffer[curId] = ids[curId] + reKey.Shifts[curId % shLength] + ids[curId - 1];
                        encoding = Numsys.ToCustomAsString
                        (
                            (buffer[curId] / exLength).ToString(),
                            10,
                            exLength,
                            reKey.ExternalAlphabet,
                            maxEncodingLength
                        );

                        encrypted += reKey.ExternalAlphabet[buffer[curId] % exLength] + encoding;
                    }
                    return encrypted;
                }
            }
            else
            {
                Int32 shift = shLength > 0 ? reKey.Shifts[0] : 0;
                buffer[0] = ids[0] + shift;

                // Optimisation for base 10 encoding
                if (exLength == 10)
                {
                    Int32 maxEncodingLength = DigitsInPositive
                    (
                        (Int32)Math.Ceiling
                        (
                            (float)
                            (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrimaryAlphabet.Length * 2 + shift - 4
                            ) / exLength
                        )
                    );

                    Int32 bufferId = buffer[0] / exLength;
                    char encodingDefault = reKey.ExternalAlphabet[0];
                    string encoding = "";
                    while (bufferId > 0)
                    {
                        encoding += reKey.ExternalAlphabet[bufferId % 10];
                        bufferId /= 10;
                    }
                    for (Int32 extend = encoding.Length; extend < maxEncodingLength; extend++)
                        encoding += encodingDefault;


                    string encrypted = reKey.ExternalAlphabet[buffer[0] % exLength] + new string(encoding.Reverse().ToArray());

                    for (Int32 curId = 1; curId < messageLength; curId++)
                    {
                        buffer[curId] = ids[curId] + shift + ids[curId - 1];
                        encoding = "";

                        while (bufferId > 0)
                        {
                            encoding += reKey.ExternalAlphabet[bufferId % 10];
                            bufferId /= 10;
                        }
                        for (Int32 extend = encoding.Length; extend < maxEncodingLength; extend++)
                            encoding += encodingDefault;

                        encrypted += reKey.ExternalAlphabet[buffer[curId] % exLength] + new string(encoding.Reverse().ToArray());
                    }
                    return encrypted;
                }
                else
                {
                    Int32 maxEncodingLength = Numsys.AutoAsList
                    (
                        Math.Ceiling
                        (
                            (float)
                            (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrimaryAlphabet.Length * 2 + shift - 4
                            ) / exLength
                        ).ToString(),
                        10,
                        exLength
                    ).Count;
                    string encoding = Numsys.ToCustomAsString
                    (
                        (buffer[0] / exLength).ToString(),
                        10,
                        exLength,
                        reKey.ExternalAlphabet,
                        maxEncodingLength
                    );


                    string encrypted = reKey.ExternalAlphabet[buffer[0] % exLength] + encoding;

                    for (Int32 curId = 1; curId < messageLength; curId++)
                    {
                        buffer[curId] = ids[curId] + shift + ids[curId - 1];
                        encoding = Numsys.ToCustomAsString
                        (
                            (buffer[curId] / exLength).ToString(),
                            10,
                            exLength,
                            reKey.ExternalAlphabet,
                            maxEncodingLength
                        );

                        encrypted += reKey.ExternalAlphabet[buffer[curId] % exLength] + encoding;
                    }
                    return encrypted;
                }
            }
        }
        static public string EncryptWithConsoleInfo(string message, EncryptionKey reKey)
        {
            try { return UnsafeEncryptWithConsoleInfo(message, reKey); }
            catch { return null; }
        }
        static public string EncryptWithConsoleInfo(string message, EncryptionKey reKey, out Exception exception)
        {
            try
            {
                exception = null;
                return UnsafeEncryptWithConsoleInfo(message, reKey);
            }
            catch (Exception innerException)
            {
                exception = innerException;
                return null;
            }
        }
        static public string UnsafeEncryptWithConsoleInfo(string message, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, messageLength = message.Length;
            Int32 shLength = reKey.Shifts != null ? reKey.Shifts.Length : 0;
            Int32[] buffer = new Int32[messageLength], shifts = shLength > 0 ? reKey.Shifts : new Int32[] { 0 };
            shLength = shLength > 0 ? shLength : 1; //  prevent division by 0

            List<Int32> ids = new List<Int32>();
            for (Int32 curChar = 0; curChar < messageLength; curChar++)
                ids.Add(reKey.PrimaryAlphabet.IndexOf(message[curChar]));

            Int32 maxEncodingLength = Numsys.AutoAsList
            (
                Math.Ceiling
                (
                    (float)
                    (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                        reKey.PrimaryAlphabet.Length * 2 + shifts.Max() - 4
                    ) / exLength
                ).ToString(),
                10,
                exLength
            ).Count;


            buffer[0] = ids[0] + shifts[0];
            string encoding = Numsys.ToCustomAsString
            (
                (buffer[0] / exLength).ToString(),
                10,
                exLength,
                reKey.ExternalAlphabet,
                maxEncodingLength
            );


            string encrypted = reKey.ExternalAlphabet[buffer[0] % exLength].ToString() + encoding;

            for (Int32 curId = 1; curId < messageLength; curId++)
            {
                buffer[curId] = ids[curId] + shifts[curId % shLength] + ids[curId - 1];
                encoding = Numsys.ToCustomAsString
                (
                    (buffer[curId] / exLength).ToString(),
                    10,
                    exLength,
                    reKey.ExternalAlphabet,
                    maxEncodingLength
                );

                encrypted += reKey.ExternalAlphabet[buffer[curId] % exLength] + encoding;
            }

            EncryptingInfo(buffer, exLength, maxEncodingLength, shifts, shLength, ids, encrypted, message, messageLength);
            return encrypted;
        }
        static public List<Byte> EncryptToBytes(EncryptionKey reKey)
        {
            BinaryReadyKey binReKey = new BinaryReadyKey();

            return null;
        }
        static public List<Byte> EncryptToBytes(BinaryReadyKey binReKey)
        {
            return null;
        }



        static public string Decrypt(string encMessage, EncryptionKey reKey)
        {
            if (encMessage == null || encMessage == "" || encMessage.Length < 1)
            {
                throw new ArgumentException
                (
                    "Message is invalid - cannot be null or empty",
                    "message"
                );
            }
            else if (reKey == null)
            {
                throw new ArgumentException
                (
                    "Encryption key is undefined (null or empty)",
                    "reKey"
                );
            }
            else if (reKey.ExternalAlphabet == null || reKey.ExternalAlphabet == "" || reKey.ExLength < 2)
            {
                throw new ArgumentException
                (
                    "The external alphabet of the encryption key is undefined or too small",
                    "reKey.ExternalAlphabet"
                );
            }


            //  Replace check for IsExternalValid   <-   need to make it
            //try { reKey.IsPrimaryValid(encMessage, true); }
            //catch { return null; }

            try { return FastDecrypt(encMessage, reKey); }
            catch { return null; }
        }
        static public string FastDecrypt(string encMessage, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, encMessageLength = encMessage.Length;
            Int32 shLength = reKey.Shifts != null ? reKey.Shifts.Length : 0;
            Int32[] buffer = new Int32[encMessageLength];

            List<Int32> ids = new List<Int32>();
            for (Int32 curChar = 0; curChar < encMessageLength; curChar++)
                ids.Add(reKey.ExternalAlphabet.IndexOf(encMessage[curChar]));

            if (shLength > 1)
            {
                buffer[0] = ids[0] - reKey.Shifts[0];

                // Optimisation for base 10 encoding
                if (exLength == 10)
                {
                    Int32 maxEncodingLength = DigitsInPositive
                    (
                        (Int32)Math.Ceiling
                        (
                            (float)
                            (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrimaryAlphabet.Length * 2 + reKey.Shifts.Max() - 4
                            ) / exLength
                        )
                    );

                    Int32 realMessageLength = encMessageLength / (maxEncodingLength + 1);
                    Int32[] decodedIds = new Int32[realMessageLength];

                    string encoding = Numsys.FromCustomAsString
                    (
                        encMessage.Substring(1, maxEncodingLength).ToString(),
                        exLength,
                        10,
                        reKey.ExternalAlphabet
                    );


                    //  Decode 1st message character
                    Int32.TryParse(encoding, out Int32 parsedEncoding);
                    decodedIds[0] = buffer[0] + parsedEncoding * exLength;
                    string decrypted = reKey.PrimaryAlphabet[decodedIds[0]].ToString();


                    for (Int32 curId = 1; curId < encMessageLength; curId++)
                    {
                        buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - reKey.Shifts[curId % shLength];

                        encoding = Numsys.FromCustomAsString
                        (
                            encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                            exLength,
                            10,
                            reKey.ExternalAlphabet
                        );

                        Int32.TryParse(encoding, out parsedEncoding);
                        decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                        decrypted += reKey.PrimaryAlphabet[decodedIds[curId]].ToString();
                    }

                    return decrypted;
                }
                else
                {
                    Int32 maxEncodingLength = Numsys.AsList
                    (
                        Math.Ceiling
                        (
                            (float)
                            (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrimaryAlphabet.Length * 2 + reKey.Shifts.Max() - 4
                            ) / exLength
                        ).ToString(),
                        10,
                        exLength
                    ).Count;

                    Int32 realMessageLength = encMessageLength / (maxEncodingLength + 1);
                    Int32[] decodedIds = new Int32[realMessageLength];

                    string encoding = Numsys.FromCustomAsString
                    (
                        encMessage.Substring(1, maxEncodingLength).ToString(),
                        exLength,
                        10,
                        reKey.ExternalAlphabet
                    );


                    //  Decode 1st message character
                    Int32.TryParse(encoding, out Int32 parsedEncoding);
                    decodedIds[0] = buffer[0] + parsedEncoding * exLength;
                    string decrypted = reKey.PrimaryAlphabet[decodedIds[0]].ToString();


                    for (Int32 curId = 1; curId < realMessageLength; curId++)
                    {
                        buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - reKey.Shifts[curId % shLength];

                        encoding = Numsys.FromCustomAsString
                        (
                            encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                            exLength,
                            10,
                            reKey.ExternalAlphabet
                        );

                        Int32.TryParse(encoding, out parsedEncoding);
                        decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                        decrypted += reKey.PrimaryAlphabet[decodedIds[curId]].ToString();
                    }

                    return decrypted;
                }
            }
            else
            {
                Int32 shift = shLength > 0 ? reKey.Shifts[0] : 0;
                buffer[0] = ids[0] - shift;

                // Optimisation for base 10 encoding
                if (exLength == 10)
                {
                    Int32 maxEncodingLength = DigitsInPositive
                    (
                        (Int32)Math.Ceiling
                        (
                            (float)
                            (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrimaryAlphabet.Length * 2 + reKey.Shifts.Max() - 4
                            ) / exLength
                        )
                    );

                    Int32 realMessageLength = encMessageLength / (maxEncodingLength + 1);
                    Int32[] decodedIds = new Int32[realMessageLength];

                    string encoding = Numsys.FromCustomAsString
                    (
                        encMessage.Substring(1, maxEncodingLength).ToString(),
                        exLength,
                        10,
                        reKey.ExternalAlphabet
                    );


                    //  Decode 1st message character
                    Int32.TryParse(encoding, out Int32 parsedEncoding);
                    decodedIds[0] = buffer[0] + parsedEncoding * exLength;
                    string decrypted = reKey.PrimaryAlphabet[decodedIds[0]].ToString();


                    for (Int32 curId = 1; curId < encMessageLength; curId++)
                    {
                        buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - shift;

                        encoding = Numsys.FromCustomAsString
                        (
                            encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                            exLength,
                            10,
                            reKey.ExternalAlphabet
                        );

                        Int32.TryParse(encoding, out parsedEncoding);
                        decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                        decrypted += reKey.PrimaryAlphabet[decodedIds[curId]].ToString();
                    }

                    return decrypted;
                }
                else
                {
                    Int32 maxEncodingLength = Numsys.AsList
                    (
                        Math.Ceiling
                        (
                            (float)
                            (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrimaryAlphabet.Length * 2 + shift - 4
                            ) / exLength
                        ).ToString(),
                        10,
                        exLength
                    ).Count;

                    Int32 realMessageLength = encMessageLength / (maxEncodingLength + 1);
                    Int32[] decodedIds = new Int32[realMessageLength];

                    string encoding = Numsys.FromCustomAsString
                    (
                        encMessage.Substring(1, maxEncodingLength).ToString(),
                        exLength,
                        10,
                        reKey.ExternalAlphabet
                    );


                    //  Decode 1st message character
                    Int32.TryParse(encoding, out Int32 parsedEncoding);
                    decodedIds[0] = buffer[0] + parsedEncoding * exLength;
                    string decrypted = reKey.PrimaryAlphabet[decodedIds[0]].ToString();


                    for (Int32 curId = 1; curId < realMessageLength; curId++)
                    {
                        buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - shift;

                        encoding = Numsys.FromCustomAsString
                        (
                            encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                            exLength,
                            10,
                            reKey.ExternalAlphabet
                        );

                        Int32.TryParse(encoding, out parsedEncoding);
                        decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                        decrypted += reKey.PrimaryAlphabet[decodedIds[curId]].ToString();
                    }

                    return decrypted;
                }
            }
        }
        static public string DecryptWithConsoleInfo(string encMessage, EncryptionKey reKey)
        {
            try { return UnsafeDecryptWithConsoleInfo(encMessage, reKey); }
            catch { return null; }
        }
        static public string DecryptWithConsoleInfo(string encMessage, EncryptionKey reKey, out Exception exception)
        {
            try
            {
                exception = null;
                return UnsafeDecryptWithConsoleInfo(encMessage, reKey);
            }
            catch (Exception innerException)
            {
                exception = innerException;
                return null; 
            }
        }
        static public string UnsafeDecryptWithConsoleInfo(string encMessage, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, encMessageLength = encMessage.Length;
            Int32 shLength = reKey.Shifts != null ? reKey.Shifts.Length : 0;
            Int32[] buffer = new Int32[encMessageLength];

            List<Int32> ids = new List<Int32>();
            for (Int32 curChar = 0; curChar < encMessageLength; curChar++)
                ids.Add(reKey.ExternalAlphabet.IndexOf(encMessage[curChar]));

            buffer[0] = ids[0] - reKey.Shifts[0];

            Int32 maxEncodingLength = Numsys.AsList
            (
                Math.Ceiling
                (
                    (float)
                    (   //  Numer 4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                        reKey.PrimaryAlphabet.Length * 2 + reKey.Shifts.Max() - 4
                    ) / exLength
                ).ToString(),
                10,
                exLength
            ).Count;

            Int32 realMessageLength = encMessageLength / (maxEncodingLength + 1);
            Int32[] decodedIds = new Int32[realMessageLength];

            string encoding = Numsys.FromCustomAsString
            (
                encMessage.Substring(1, maxEncodingLength).ToString(),
                exLength,
                10,
                reKey.ExternalAlphabet
            );


            //  Decode 1st message character
            Int32.TryParse(encoding, out Int32 parsedEncoding);
            decodedIds[0] = buffer[0] + parsedEncoding * exLength;
            string decrypted = reKey.PrimaryAlphabet[decodedIds[0]].ToString();


            for (Int32 curId = 1; curId < realMessageLength; curId++)
            {
                buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - reKey.Shifts[curId % shLength];

                encoding = Numsys.FromCustomAsString
                (
                    encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                    exLength,
                    10,
                    reKey.ExternalAlphabet
                );

                Int32.TryParse(encoding, out parsedEncoding);
                decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                decrypted += reKey.PrimaryAlphabet[decodedIds[curId]].ToString();
            }

            DecryptingInfo(buffer, exLength, maxEncodingLength, reKey.Shifts, shLength, ids, decodedIds, encMessage, decrypted, realMessageLength);
            return decrypted;

        }
        static public string DecryptFromBytes()
        {
            return "aboba";
        }



        static public bool IsPrimaryValid(string message, string primary, bool throwException = false) => EncryptionKey.IsPrimaryValid(message, primary, throwException);


        static private void EncryptingInfo(Int32[] buffer, Int32 exLength, Int32 maxEncodingLength, Int32[] shifts, Int32 shLength, List<Int32> ids, string encrypted, string message, Int32 messageLength)
        {
            //  margin = for ids, bf = buffer, sh = shifts, el = externalAlphabet.Length, sz = message length
            Int32 margin   = DigitsAmount(ids.Max());
            Int32 marginBf = DigitsAmount(buffer.Max()),  marginSh = DigitsAmount(shifts.Max());
            Int32 marginSz = DigitsAmount(messageLength), marginEl = DigitsAmount(exLength);


            //---  First character (its encryption is a bit different so we do it manually)
            Write("\n\t");
            for (Int32 ext = 1; ext < marginSz; ext++) Write(" ");
            Write("0] total:  ");

            for (Int32 ext = DigitsAmount(buffer[0]); ext < marginBf; ext++) Write(" ");
            Write(buffer[0] + " (");

            for (Int32 ext = DigitsAmount(buffer[0] % exLength); ext < marginEl; ext++) Write(" ");
            ForegroundColor = ConsoleColor.Magenta;
            Write(buffer[0] % exLength);
            ForegroundColor = ConsoleColor.Gray;
            Write(") = ");

            for (Int32 ext = DigitsAmount(shifts[0]); ext < marginSh; ext++) Write(" ");
            ForegroundColor = shifts[0] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
            Write(shifts[0]);
            ForegroundColor = ConsoleColor.Gray;
            Write(" + ");

            for (Int32 ext = DigitsAmount(ids[0]); ext < margin; ext++) Write(" ");
            ForegroundColor = ConsoleColor.Green;
            Write(ids[0]);
            ForegroundColor = ConsoleColor.Gray;

            for (Int32 ext = 0; ext < margin * 2; ext++) Write(" ");
            Write("   out: ");
            ForegroundColor = ConsoleColor.DarkCyan;
            Write(encrypted[0]);
            ForegroundColor = ConsoleColor.Gray;


            //  Rest of the message
            for (Int32 curId = 1; curId < messageLength; curId++)
            {
                Write("\n\t");
                for (Int32 ext = DigitsAmount(curId); ext < marginSz; ext++) Write(" ");
                Write(curId + "] total:  ");
                
                for (Int32 ext = DigitsAmount(buffer[curId]); ext < marginBf; ext++) Write(" ");
                Write(buffer[curId] + " (");

                for (Int32 ext = DigitsAmount(buffer[0] % exLength); ext < marginEl; ext++) Write(" ");
                ForegroundColor = ConsoleColor.Magenta;
                Write(buffer[curId] % exLength);
                ForegroundColor = ConsoleColor.Gray;
                Write(") = ");

                for (Int32 ext = DigitsAmount(shifts[curId % shLength]); ext < marginSh; ext++) Write(" ");
                ForegroundColor = shifts[curId % shLength] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
                Write(shifts[curId % shLength]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" + ");

                for (Int32 ext = DigitsAmount(ids[curId]); ext < margin; ext++) Write(" ");
                ForegroundColor = curId % 2 == 0 ? ConsoleColor.Green : ConsoleColor.DarkYellow;
                Write(ids[curId]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" + ");

                for (Int32 ext = DigitsAmount(ids[curId - 1]); ext < margin; ext++) Write(" ");
                ForegroundColor = curId % 2 == 0 ? ConsoleColor.DarkYellow : ConsoleColor.Green;
                Write(ids[curId - 1]);
                ForegroundColor = ConsoleColor.Gray;

                for (Int32 ext = 0; ext < margin; ext++) Write(" ");
                Write("out: ");
                ForegroundColor = ConsoleColor.DarkCyan;
                Write(encrypted[curId * (maxEncodingLength + 1)]);
                ForegroundColor = ConsoleColor.Gray;
            }



            //  Original message
            Write("\n\tOriginal:  ");
            for (Int32 curChar = 0; curChar < message.Length; curChar++)
            {
                ForegroundColor = curChar % 2 == 0 ? ConsoleColor.Green : ConsoleColor.DarkYellow;
                Write(message[curChar]);

                ForegroundColor = ConsoleColor.DarkGray;
                for (Int32 curId = 0; curId < maxEncodingLength; curId++) Write("_");
            }


            //  Algorithm end result
            ForegroundColor = ConsoleColor.Gray;
            Write("\n\tEncrypted: ");
            for (Int32 curChar = 0; curChar < encrypted.Length; curChar++)
            {
                ForegroundColor = curChar % (maxEncodingLength + 1) == 0 ? ConsoleColor.DarkCyan : ConsoleColor.Gray;
                Write(encrypted[curChar]);
            }
            ForegroundColor = ConsoleColor.Gray;
        }
        static private void DecryptingInfo(Int32[] buffer, Int32 exLength, Int32 maxEncodingLength, Int32[] shifts, Int32 shLength, List<Int32> ids, Int32[] decodedIds, string encrypted, string decrypted, Int32 messageLength)
        {
            //  margin = for ids, bf = buffer, sh = shifts, el = externalAlphabet.Length, sz = message length, en = parsed encoding
            Int32 margin   = DigitsAmount(ids.Max()), marginEn = DigitsAmount(decodedIds.Max() - buffer.Min());
            Int32 marginSz = DigitsAmount(messageLength), marginEl = DigitsAmount(exLength);
            Int32 marginBf = DigitsAmount(decodedIds.Max()),  marginSh = DigitsAmount(shifts.Max());


            //---  First character (its encryption is a bit different so we do it manually)
            Write("\n\t");
            for (Int32 ext = 1; ext < marginSz; ext++) Write(" ");
            Write("0] total:  ");

            for (Int32 ext = DigitsAmount(decodedIds[0]); ext < marginBf; ext++) Write(" ");
            Write(decodedIds[0] + " (");

            for (Int32 ext = DigitsAmount(decodedIds[0] % exLength); ext < marginEl; ext++) Write(" ");
            ForegroundColor = ConsoleColor.Magenta;
            Write(decodedIds[0] % exLength);
            ForegroundColor = ConsoleColor.Gray;
            Write(") = ");

            for (Int32 ext = DigitsAmount(decodedIds[0] - buffer[0]); ext < marginEn; ext++) Write(" ");
            ForegroundColor = ConsoleColor.DarkGray;
            Write(decodedIds[0] - buffer[0]);
            ForegroundColor = ConsoleColor.Gray;
            Write(" - ");

            for (Int32 ext = DigitsAmount(shifts[0]); ext < marginSh; ext++) Write(" ");
            ForegroundColor = shifts[0] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
            Write(shifts[0]);
            ForegroundColor = ConsoleColor.Gray;
            Write(" + ");

            for (Int32 ext = DigitsAmount(ids[0]); ext < margin; ext++) Write(" ");
            ForegroundColor = ConsoleColor.Green;
            Write(ids[0]);
            ForegroundColor = ConsoleColor.Gray;

            for (Int32 ext = 0; ext < margin * 2; ext++) Write(" ");
            Write("    out: ");
            ForegroundColor = ConsoleColor.DarkCyan;
            Write(decrypted[0]);
            ForegroundColor = ConsoleColor.Gray;



            //---  Rest of the message
            for (Int32 curId = 1; curId < messageLength; curId++)
            {
                Write("\n\t");
                for (Int32 ext = DigitsAmount(curId); ext < marginSz; ext++) Write(" ");
                Write(curId + "] total:  ");

                for (Int32 ext = DigitsAmount(decodedIds[curId]); ext < marginBf; ext++) Write(" ");
                Write(decodedIds[curId] + " (");

                for (Int32 ext = DigitsAmount(decodedIds[curId] % exLength); ext < marginEl; ext++) Write(" ");
                ForegroundColor = ConsoleColor.Magenta;
                Write(decodedIds[curId] % exLength);
                ForegroundColor = ConsoleColor.Gray;
                Write(") = ");

                for (Int32 ext = DigitsAmount(decodedIds[curId] - buffer[curId]); ext < marginEn; ext++) Write(" ");
                ForegroundColor = ConsoleColor.DarkGray;
                Write(decodedIds[curId] - buffer[curId]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" - ");

                for (Int32 ext = DigitsAmount(shifts[curId % shLength]); ext < marginSh; ext++) Write(" ");
                ForegroundColor = shifts[curId % shLength] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
                Write(shifts[curId % shLength]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" + ");

                for (Int32 ext = DigitsAmount(ids[curId * (maxEncodingLength + 1)]); ext < margin; ext++) Write(" ");
                ForegroundColor = curId % 2 == 0 ? ConsoleColor.Green : ConsoleColor.DarkYellow;
                Write(ids[curId * (maxEncodingLength + 1)]);
                ForegroundColor = ConsoleColor.Gray;
                Write(" - ");

                for (Int32 ext = DigitsAmount(decodedIds[curId -1]); ext < marginBf; ext++) Write(" ");
                ForegroundColor = curId % 2 == 0 ? ConsoleColor.DarkYellow : ConsoleColor.Green;
                Write(decodedIds[curId - 1]);
                ForegroundColor = ConsoleColor.Gray;
                
                for (Int32 ext = 0; ext < margin * 2; ext++) Write(" ");
                Write("out: ");
                ForegroundColor = ConsoleColor.DarkCyan;
                Write(decrypted[curId]);
                ForegroundColor = ConsoleColor.Gray;
            }


            //  Original message
            Write("\n\tOriginal:  ");
            for (Int32 curChar = 0; curChar < encrypted.Length; curChar++)
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
            for (Int32 curChar = 0; curChar < decrypted.Length; curChar++)
            {
                ForegroundColor = ConsoleColor.DarkCyan;
                Write(decrypted[curChar]);

                ForegroundColor = ConsoleColor.DarkGray;
                for (Int32 curId = 0; curId < maxEncodingLength; curId++) Write("_");
            }


            //  Only readable end result
            ForegroundColor = ConsoleColor.Gray;
            Write("\n\tReadable:  " + decrypted);
        }


        static private Int32 DigitsAmount(Int32 number)
        {
            if (number >= 0) return DigitsInPositive(number);
            else return number > -10 ? 1 : number > -100 ? 2 : number > -1000 ? 3 : number > -10000 ? 4 : 5;
        }
        static private Int32 DigitsInPositive(Int32 posNumber)
        {
            return posNumber < 10 ? 1 : posNumber < 100 ? 2 : posNumber < 1000 ? 3 : posNumber < 10000 ? 4 : 5;
        }
    }
}