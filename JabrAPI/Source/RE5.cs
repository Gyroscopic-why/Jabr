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
            private string _primaryAlphabet;
            private string _externalAlphabet;
            private List<Int32> _shifts;

            private List<char> _primaryNecessary,  _primaryAllowed,  _primaryBanned;
            private List<char> _externalNecessary, _externalAllowed, _externalBanned;
            private Int32  _primaryMaxLength = -1, _externalMaxLength = -1;
            private const string defaultChars = "`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№";


            public EncryptionKey(string primaryAlphabet, string externalAlphabet, List<Int32> shifts)
            {
                _primaryAlphabet  = primaryAlphabet;
                _externalAlphabet = externalAlphabet;
                _shifts = shifts;
            }
            public EncryptionKey(string primaryAlphabet, string externalAlphabet, Int32 shift)
            {
                _primaryAlphabet  = primaryAlphabet;
                _externalAlphabet = externalAlphabet;
                _shifts = new List<Int32>() { shift };
            }
            public EncryptionKey(string primaryAlphabet, string externalAlphabet)
            {
                _primaryAlphabet  = primaryAlphabet;
                _externalAlphabet = externalAlphabet;
            }
            public EncryptionKey() => SetDefault();



            public string PrimaryAlphabet => _primaryAlphabet;
            public string PrAlphabet => _primaryAlphabet;
            public string Primary => _primaryAlphabet;
            public Int32  PrimaryLength => _primaryAlphabet == null ? -1 : _primaryAlphabet.Length;
            public Int32  PrLength => _primaryAlphabet == null ? -1 : _primaryAlphabet.Length;

            public string ExternalAlphabet => _externalAlphabet;
            public string ExAlphabet => _externalAlphabet;
            public string External => _externalAlphabet;
            public Int32  ExternalLength => _externalAlphabet == null ? -1 : _externalAlphabet.Length;
            public Int32  ExLength => _externalAlphabet == null ? -1 : _externalAlphabet.Length;

            public List<Int32> Shifts => _shifts;
            public Int32 ShAmount => _shifts.Count;
            public Int32 ShLength => _shifts.Count;
            public Int32 ShCount  => _shifts.Count;



            public void Next()
            {
                try { GenerateAll(); }
                catch { Default(); }
            }



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
                for (Int32 curId = 0; curId < _shifts.Count - 1; curId++) result += _shifts[curId] + splitShifts;
                if (_shifts.Count > 0) result += _shifts[_shifts.Count - 1];

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

                for (Int32 curId = 0; curId < _shifts.Count - 1; curId++)
                    result += _shifts[curId] + " ";
                if (_shifts.Count > 0) result += _shifts[_shifts.Count - 1];

                return result;
            }
            public List<Byte> ExportAsBinaryBE()
            {
                List<Byte> result = new List<Byte>();

                Byte[] buffer = ToBinary.BigEndian(_shifts.Count), bufferLength;
                for (Int32 id = 0; id < 4; id++) result.Add(buffer[id]);

                for (Int32 curId = 0; curId < _shifts.Count; curId++)
                {
                    buffer = ToBinary.BigEndian(_shifts[curId]);

                    for (Int32 id = 0; id < 4; id++) result.Add(buffer[id]);
                }



                buffer = ToBinary.Utf8(_primaryAlphabet);
                bufferLength = ToBinary.BigEndian(buffer.Length);
                for (Int32 id = 0; id < 4; id++) result.Add(bufferLength[id]);
                for (Int32 id = 0; id < 4; id++) result.Add(buffer[id]);



                buffer = ToBinary.Utf8(_externalAlphabet);
                bufferLength = ToBinary.BigEndian(buffer.Length);
                for (Int32 id = 0; id < 4; id++) result.Add(bufferLength[id]);
                for (Int32 id = 0; id < 4; id++) result.Add(buffer[id]);


                return result;
            }
            public List<Byte> ExportAsBinaryLE()
            {
                List<Byte> result = new List<Byte>();

                Byte[] buffer = ToBinary.LittleEndian(_shifts.Count), bufferLength;
                result.Add(buffer[0]);
                result.Add(buffer[1]);
                result.Add(buffer[2]);
                result.Add(buffer[3]);

                for (Int32 curId = 0; curId < _shifts.Count; curId++)
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
                => IsPrimaryValid(message, _primaryAlphabet, throwException);
            static public bool IsPrimaryValid(string message, string primary, bool throwException = false)
            {
                if (!IsPrimaryPartiallyValid(primary, throwException)) return false;
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

            public bool IsPrimaryPartiallyValid(bool throwException = false)
                => IsPrimaryPartiallyValid(_primaryAlphabet, throwException);
            static public bool IsPrimaryPartiallyValid(string primary, bool throwException = false)
            {
                if (primary == null || primary == "" || primary.Length < 2)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Primary alphabet is not set or is too short",
                            "primary (or _primaryAlphabet)"
                        );
                    }
                    return false;
                }

                return true;
            }



            public bool IsExternalValid(string encMessage, bool throwException = false)
                => IsExternalValid(encMessage, _externalAlphabet, throwException);
            static public bool IsExternalValid(string encMessage, string external, bool throwException = false)
            {
                if (!IsExternalPartiallyValid(external, throwException)) return false;

                foreach (char c in encMessage)
                {
                    if (!external.Contains(c))
                    {
                        if (throwException)
                        {
                            throw new ArgumentException
                            (
                                "Message contains characters not in the external alphabet",
                                "message, char: " + c
                            );
                        }
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
                    {
                        throw new ArgumentException
                        (
                            "External alphabet is not set or is too short",
                            "external (or _externalAlphabet)"
                        );
                    }
                    return false;
                }
                for (Int32 curId = 0; curId < external.Length; curId++)
                {
                    for (Int32 id2 = curId + 1; id2 < external.Length; id2++)
                    {
                        if (external[curId] == external[id2])
                        {
                            if (throwException)
                            {
                                throw new ArgumentException
                                (
                                    "external alphabet contains duplicates characters",
                                    "external (or _externalAlphabet) duplicate char: " + external[curId]
                                );
                            }
                            return false;
                        }
                    }
                }

                return true;
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
                => SetDefault(pNecessary.ToList(), pAllowed.ToList(), pBanned.ToList(), pMaxLength,
                              eNecessary.ToList(), eAllowed.ToList(), eBanned.ToList(), eMaxLength);


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
                => SetDefault(pNecessary.ToList(), pAllowed.ToList(), pMaxLength,
                              eNecessary.ToList(), eAllowed.ToList(), eMaxLength);


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
                => SetDefault(pMaxLength, pNecessary.ToList(), pBanned.ToList(), 
                              eMaxLength, eNecessary.ToList(), eBanned.ToList());


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
                => SetDefault(pMaxLength, pBanned.ToList(), 
                              eMaxLength, eBanned.ToList());


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
                => SetDefault(necessary.ToList(), allowed.ToList(), banned.ToList(), maxLength);

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
                => SetDefault(necessary.ToList(), allowed.ToList(), maxLength);

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
                => SetDefault(maxLength, necessary.ToList(), banned.ToList());
            
            public void SetDefault(Int32 maxLength, List<char> banned)
            {
                _primaryBanned  = banned;
                _externalBanned = banned;

                _primaryMaxLength  = maxLength;
                _externalMaxLength = maxLength;
            }
            public void SetDefault(Int32 maxLength, string banned)
                => SetDefault(maxLength, banned.ToList());
            

            public  void SetDefault()
            {
                _primaryNecessary  = (" " + defaultChars).ToList();
                _externalAllowed   = defaultChars.ToList();

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




            private void ValidateForPrimaryGeneration (List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                if (necessary != null && necessary.Count > 0)
                {
                    _primaryNecessary = necessary;

                    for (Int32 id1 = 0; id1 < necessary.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < necessary.Count; id2++)
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
                else _primaryNecessary = new List<char>();

                if (allowed != null)
                {
                    _primaryAllowed = allowed;

                    for (Int32 id1 = 0; id1 < allowed.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < allowed.Count; id2++)
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
                        for (Int32 id2 = 0; id2 < _primaryNecessary.Count; id2++)
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
                else _primaryAllowed = new List<char>();


                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the smallest possible alphabet length (2)",
                        "maxLength < 2"
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

                    for (Int32 id1 = 0; id1 < necessary.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < necessary.Count; id2++)
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
                else _externalNecessary = new List<char>();

                if (allowed != null)
                {
                    _externalAllowed = allowed;

                    for (Int32 id1 = 0; id1 < allowed.Count; id1++)
                    {
                        for (Int32 id2 = id1 + 1; id2 < allowed.Count; id2++)
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
                        for (Int32 id2 = 0; id2 < _externalNecessary.Count; id2++)
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
                else _externalAllowed = new List<char>();


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

                Random random = new Random();
                Int32 buffer, bufferId;
                _primaryAlphabet = "";


                while (necessary.Count > 0)
                {
                    buffer = random.Next(0, necessary.Count);
                    _primaryAlphabet += necessary[buffer];
                    necessary.RemoveAt(buffer);
                }
                for (Int32 remaining = Math.Min(maxLength, allowed.Count); remaining > 0; remaining--)
                {
                    buffer   = random.Next(0, allowed.Count);
                    bufferId = random.Next(0, _primaryAlphabet.Length);

                    _primaryAlphabet = _primaryAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.RemoveAt(buffer);
                }
            }
            public void GenerateRandomPrimary(string necessary, string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomPrimary(necessary.ToList(), allowed.ToList(), maxLength, validateParameters);


            public void GenerateRandomPrimary(List<char> allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomPrimary(null, allowed, maxLength, validateParameters);
            public void GenerateRandomPrimary(string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomPrimary(null, allowed.ToList(), maxLength, validateParameters);


            public void GenerateRandomPrimary(Int32 maxLength, List<char> necessary, List<char> banned, bool validateParameters = true)
            {
                List<char> allowed = (" " + defaultChars).ToList();

                if (banned != null && banned.Count > 0)
                {
                    for (Int32 curId = 0; curId < banned.Count; curId++)
                    {
                        for (Int32 id2 = 0; id2 < allowed.Count; id2++)
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
                => GenerateRandomPrimary(maxLength, necessary.ToList(), banned.ToList(), validateParameters);
            

            public void GenerateRandomPrimary(Int32 maxLength, List<char> banned, bool validateParameters = true)
                => GenerateRandomPrimary(maxLength, null, banned, validateParameters);
            public void GenerateRandomPrimary(Int32 maxLength, string banned, bool validateParameters = true)
                => GenerateRandomPrimary(maxLength, null, banned.ToList(), validateParameters);
            public void GenerateRandomPrimary(Int32 maxLength, bool validateParameters = true) 
                => GenerateRandomPrimary(maxLength, "", validateParameters);

            public void GenerateRandomPrimary()
            { 
                List<char> allowed = _primaryAllowed;

                if (_primaryBanned != null && _primaryBanned.Count > 0)
                {
                    for (Int32 curId = 0; curId < _primaryBanned.Count; curId++)
                    {
                        for (Int32 id2 = 0; id2 < allowed.Count; id2++)
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




            public void GenerateRandomExternal(List<char> necessary, List<char> allowed, Int32 maxLength, bool validateParameters = true)
            {
                if (validateParameters)
                {
                    ValidateForExternalGeneration(necessary, allowed, maxLength);

                    necessary = _externalNecessary;  //  to avoid nullpointer exceptions
                    allowed   = _externalAllowed;    //  the _external variants are always defaulted
                                                     //  (to a new empty List in worst case)
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
                for (Int32 remaining = Math.Min(maxLength, allowed.Count); remaining > 0; remaining--)
                {
                    buffer = random.Next(0, allowed.Count);
                    bufferId = random.Next(0, _externalAlphabet.Length);

                    _externalAlphabet = _externalAlphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.RemoveAt(buffer);
                }
            }
            public void GenerateRandomExternal(string necessary, string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomExternal(necessary.ToList(), allowed.ToList(), maxLength, validateParameters);


            public void GenerateRandomExternal(List<char> allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomExternal(null, allowed, maxLength, validateParameters);
            public void GenerateRandomExternal(string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomExternal(null, allowed.ToList(), maxLength, validateParameters);


            public void GenerateRandomExternal(Int32 maxLength, List<char> necessary, List<char> banned, bool validateParameters = true)
            {
                List<char> allowed = (" " + defaultChars).ToList();

                if (banned != null && banned.Count > 0)
                {
                    for (Int32 curId = 0; curId < banned.Count; curId++)
                    {
                        for (Int32 id2 = 0; id2 < allowed.Count; id2++)
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
                => GenerateRandomExternal(maxLength, necessary.ToList(), banned.ToList(), validateParameters);


            public void GenerateRandomExternal(Int32 maxLength, List<char> banned, bool validateParameters = true)
                => GenerateRandomExternal(maxLength, null, banned, validateParameters);
            public void GenerateRandomExternal(Int32 maxLength, string banned, bool validateParameters = true)
                => GenerateRandomExternal(maxLength, null, banned.ToList(), validateParameters);
            public void GenerateRandomExternal(Int32 maxLength, bool validateParameters = true) 
                => GenerateRandomExternal(maxLength, "", validateParameters);


            public void GenerateRandomExternal()
            {
                List<char> allowed = _externalAllowed;

                if (_externalBanned != null && _externalBanned.Count > 0)
                {
                    for (Int32 curId = 0; curId < _externalBanned.Count; curId++)
                    {
                        for (Int32 id2 = 0; id2 < allowed.Count; id2++)
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



            public void GenerateRandomShifts(Int32 count, Int32 minVal, Int32 maxVal)
            {
                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException
                    (
                        "count",
                        "count cannot be negative"
                    );
                }
                else if (minVal > maxVal)
                {
                    throw new ArgumentException
                    (
                        "min value cannot be greater than max value",
                        "minVal, maxVal"
                    );
                }
                else if (minVal < 0)
                {
                    throw new ArgumentOutOfRangeException
                    (
                        "minVal",
                        "minVal cannot be negative"
                    );
                }

                _shifts = new List<Int32>();
                Random random = new Random();

                for (Int32 curId = 0; curId < count; curId++)
                    _shifts.Add(random.Next(minVal, maxVal));
            }
            public void GenerateRandomShifts(Int32 minVal, Int32 maxVal)
                => GenerateRandomShifts(new Random().Next(512, 1025), minVal, maxVal);
            public void GenerateRandomShifts(Int32 count)
            {
                if (_externalAlphabet == null || _externalAlphabet.Length < 2)
                {
                    throw new ArgumentException
                    (
                        "Unable to generate shifts, external alphabet is undefined",
                        "_externalAlphabet"
                    );
                }

                GenerateRandomShifts(count, 0, _externalAlphabet.Length - 1);
            }
            public void GenerateRandomShifts()
                => GenerateRandomShifts(new Random().Next(512, 1025));
        }

        public class BinaryKey
        {
            private List<Int16> _primaryAlphabet;
            private List<Int16> _externalAlphabet;
            private List<Int32> _shifts;


            public BinaryKey(string primaryAlphabet, string externalAlphabet, List<Int32> shifts)
            {
                _primaryAlphabet = Conversions.AutoLEBytesToInt16
                (
                    ToBinary.Utf16
                    (
                        primaryAlphabet
                    ).ToList()
                );

                _externalAlphabet = Conversions.AutoLEBytesToInt16
                (
                    ToBinary.Utf16
                    (
                        externalAlphabet
                    ).ToList()
                );

                _shifts = shifts;
            }
            public BinaryKey(EncryptionKey reKey)
            {
                _primaryAlphabet = Conversions.AutoLEBytesToInt16
                (
                    ToBinary.Utf16
                    (
                        reKey.PrAlphabet
                    ).ToList()
                );

                _externalAlphabet = Conversions.AutoLEBytesToInt16
                (
                    ToBinary.Utf16
                    (
                        reKey.ExAlphabet
                    ).ToList()
                );

                _shifts = reKey.Shifts.ToList();
            }
            public BinaryKey() { }


            public List<Int16> PrimaryAlphabet => _primaryAlphabet;
            public List<Int16> PrAlphabet => _primaryAlphabet;
            public List<Int16> Primary => _primaryAlphabet;
            public Int32 PrimaryLength => _primaryAlphabet == null ? -1 : _primaryAlphabet.Count / 2;
            public Int32 PrLength => _primaryAlphabet == null ? -1 : _primaryAlphabet.Count / 2;

            public List<Int16> ExternalAlphabet => _externalAlphabet;
            public List<Int16> ExAlphabet => _externalAlphabet;
            public List<Int16> External => _externalAlphabet;
            public Int32 ExternalLength => _externalAlphabet == null ? -1 : _externalAlphabet.Count / 2;
            public Int32 ExLength => _externalAlphabet == null ? -1 : _externalAlphabet.Count / 2;

            public List<Int32> Shifts => _shifts;
            public Int32 ShAmount => _shifts == null ? -1 : _shifts.Count;
            public Int32 ShLength => _shifts == null ? -1 : _shifts.Count;
            public Int32 ShCount  => _shifts == null ? -1 : _shifts.Count;



            public void Set(string primaryAlphabet, string externalAlphabet, List<Int32> shifts)
            {
                _primaryAlphabet = Conversions.AutoLEBytesToInt16
                (
                    ToBinary.Utf16
                    (
                        primaryAlphabet
                    ).ToList()
                );

                _externalAlphabet = Conversions.AutoLEBytesToInt16
                (
                    ToBinary.Utf16
                    (
                        externalAlphabet
                    ).ToList()
                );

                _shifts = shifts;
            }
            public void Set(EncryptionKey reKey)
            {
                _primaryAlphabet = Conversions.AutoLEBytesToInt16
                (
                    ToBinary.Utf16
                    (
                        reKey.PrAlphabet
                    ).ToList()
                );
                _externalAlphabet = Conversions.AutoLEBytesToInt16
                (
                    ToBinary.Utf16
                    (
                        reKey.ExAlphabet
                    ).ToList()
                );
                _shifts = reKey.Shifts.ToList();
            }

        }



        static public string Encrypt(string message, EncryptionKey reKey, bool throwException = false)
        {
            if (message == null || message == "" || message.Length < 1)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Message is invalid - cannot be null or empty",
                        "message"
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
                        "reKey"
                    );
                }
            }
            else if (reKey.IsExternalPartiallyValid(throwException))
            {
                if (reKey.IsPrimaryValid(message, throwException))
                {
                    try { 
                        return FastEncrypt(message, reKey); 
                    }

                    catch (Exception innerException) { 
                        if (throwException) throw innerException; 
                    }
                }
            }
            return null;
        }
        static public string Encrypt(string message, EncryptionKey reKey, out Exception exception)
        {
            if (message == null || message == "" || message.Length < 1)
            {
                exception = new ArgumentException
                (
                    "Message is invalid - cannot be null or empty",
                    "message"
                );
            }
            else if (reKey == null)
            {
                exception = new ArgumentException
                (
                    "Encryption key is undefined (null or empty)",
                    "reKey"
                );
            }
            else
            {
                try
                {
                    reKey.IsExternalPartiallyValid(true);
                    reKey.IsPrimaryValid(message, true);

                    string result = FastEncrypt(message, reKey);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return null;
        }
        static public string FastEncrypt(string message, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, messageLength = message.Length;
            Int32 shCount = reKey.Shifts != null ? reKey.Shifts.Count : 0;
            Int32[] buffer = new Int32[messageLength];

            List<Int32> ids = new List<Int32>();
            for (Int32 curChar = 0; curChar < messageLength; curChar++)
                ids.Add(reKey.PrAlphabet.IndexOf(message[curChar]));

            if (shCount > 1)
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
                            (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrLength * 2 + reKey.Shifts.Max() - 4
                            ) / exLength
                        )
                    );

                    Int32 bufferId = buffer[0] / exLength;
                    char encodingDefault = reKey.ExAlphabet[0];
                    string encoding = "";
                    while (bufferId > 0)
                    {
                        encoding += reKey.ExAlphabet[bufferId % 10];
                        bufferId /= 10;
                    }
                    for (Int32 extend = encoding.Length; extend < maxEncodingLength; extend++)
                        encoding += encodingDefault;


                    string encrypted = reKey.ExAlphabet[buffer[0] % exLength] + new string(encoding.Reverse().ToArray());

                    for (Int32 curId = 1; curId < messageLength; curId++)
                    {
                        buffer[curId] = ids[curId] + reKey.Shifts[curId % shCount] + ids[curId - 1];
                        encoding = "";

                        while (bufferId > 0)
                        {
                            encoding += reKey.ExAlphabet[bufferId % 10];
                            bufferId /= 10;
                        }
                        for (Int32 extend = encoding.Length; extend < maxEncodingLength; extend++)
                            encoding += encodingDefault;

                        encrypted += reKey.ExAlphabet[buffer[curId] % exLength] + new string(encoding.Reverse().ToArray());
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
                            (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrLength * 2 + reKey.Shifts.Max() - 4
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
                        reKey.ExAlphabet,
                        maxEncodingLength
                    );


                    string encrypted = reKey.ExAlphabet[buffer[0] % exLength] + encoding;

                    for (Int32 curId = 1; curId < messageLength; curId++)
                    {
                        buffer[curId] = ids[curId] + reKey.Shifts[curId % shCount] + ids[curId - 1];
                        encoding = Numsys.ToCustomAsString
                        (
                            (buffer[curId] / exLength).ToString(),
                            10,
                            exLength,
                            reKey.ExAlphabet,
                            maxEncodingLength
                        );

                        encrypted += reKey.ExAlphabet[buffer[curId] % exLength] + encoding;
                    }
                    return encrypted;
                }
            }
            else
            {
                Int32 shift = shCount > 0 ? reKey.Shifts[0] : 0;
                buffer[0] = ids[0] + shift;

                // Optimisation for base 10 encoding
                if (exLength == 10)
                {
                    Int32 maxEncodingLength = DigitsInPositive
                    (
                        (Int32)Math.Ceiling
                        (
                            (float)
                            (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrLength * 2 + shift - 4
                            ) / exLength
                        )
                    );

                    Int32 bufferId = buffer[0] / exLength;
                    char encodingDefault = reKey.ExAlphabet[0];
                    string encoding = "";
                    while (bufferId > 0)
                    {
                        encoding += reKey.ExAlphabet[bufferId % 10];
                        bufferId /= 10;
                    }
                    for (Int32 extend = encoding.Length; extend < maxEncodingLength; extend++)
                        encoding += encodingDefault;


                    string encrypted = reKey.ExAlphabet[buffer[0] % exLength] + new string(encoding.Reverse().ToArray());

                    for (Int32 curId = 1; curId < messageLength; curId++)
                    {
                        buffer[curId] = ids[curId] + shift + ids[curId - 1];
                        encoding = "";

                        while (bufferId > 0)
                        {
                            encoding += reKey.ExAlphabet[bufferId % 10];
                            bufferId /= 10;
                        }
                        for (Int32 extend = encoding.Length; extend < maxEncodingLength; extend++)
                            encoding += encodingDefault;

                        encrypted += reKey.ExAlphabet[buffer[curId] % exLength] + new string(encoding.Reverse().ToArray());
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
                            (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrLength * 2 + shift - 4
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
                        reKey.ExAlphabet,
                        maxEncodingLength
                    );


                    string encrypted = reKey.ExAlphabet[buffer[0] % exLength] + encoding;

                    for (Int32 curId = 1; curId < messageLength; curId++)
                    {
                        buffer[curId] = ids[curId] + shift + ids[curId - 1];
                        encoding = Numsys.ToCustomAsString
                        (
                            (buffer[curId] / exLength).ToString(),
                            10,
                            exLength,
                            reKey.ExAlphabet,
                            maxEncodingLength
                        );

                        encrypted += reKey.ExAlphabet[buffer[curId] % exLength] + encoding;
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
                string result = UnsafeEncryptWithConsoleInfo(message, reKey);
                exception = null;

                return result;
            }
            catch (Exception innerException)
            {
                exception = innerException;
                return null;
            }
        }
        static private string UnsafeEncryptWithConsoleInfo(string message, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, messageLength = message.Length;
            Int32 shCount = reKey.Shifts != null ? reKey.ShCount : 0;
            Int32[] buffer = new Int32[messageLength], shifts = shCount > 0 ? reKey.Shifts.ToArray() : new Int32[] { 0 };
            shCount = shCount > 0 ? shCount : 1; //  prevent division by 0

            List<Int32> ids = new List<Int32>();
            for (Int32 curChar = 0; curChar < messageLength; curChar++)
                ids.Add(reKey.PrAlphabet.IndexOf(message[curChar]));

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


            buffer[0] = ids[0] + shifts[0];
            string encoding = Numsys.ToCustomAsString
            (
                (buffer[0] / exLength).ToString(),
                10,
                exLength,
                reKey.ExAlphabet,
                maxEncodingLength
            );


            string encrypted = reKey.ExAlphabet[buffer[0] % exLength].ToString() + encoding;

            for (Int32 curId = 1; curId < messageLength; curId++)
            {
                buffer[curId] = ids[curId] + shifts[curId % shCount] + ids[curId - 1];
                encoding = Numsys.ToCustomAsString
                (
                    (buffer[curId] / exLength).ToString(),
                    10,
                    exLength,
                    reKey.ExAlphabet,
                    maxEncodingLength
                );

                encrypted += reKey.ExAlphabet[buffer[curId] % exLength] + encoding;
            }

            EncryptingInfo(buffer, exLength, maxEncodingLength, shifts, shCount, ids, encrypted, message, messageLength);
            return encrypted;
        }
        static public List<Byte> EncryptToBytes(string message, EncryptionKey reKey)
            => EncryptToBytes(message, new BinaryKey(reKey));
        static public List<Byte> EncryptToBytes(string message, BinaryKey binKey)
        {
            List<Int16> encodedMessage = Conversions.AutoLEBytesToInt16
            (
                ToBinary.Utf16
                (
                    message
                ).ToList()
            );

            Int32 exLength = binKey.ExLength, messageLength = message.Length;
            Int32 shCount = binKey.ShCount;
            Int32[] buffer = new Int32[messageLength];

            List<Int32> ids = new List<Int32>();
            for (Int32 curChar = 0; curChar < messageLength; curChar++)
                ids.Add(binKey.PrAlphabet.IndexOf(encodedMessage[curChar]));

            return null;
        }




        static public string Decrypt(string encMessage, EncryptionKey reKey, bool throwException = false)
        {
            if (encMessage == null || encMessage == "" || encMessage.Length < 1)
            {
                if (throwException)
                {
                    throw new ArgumentException
                    (
                        "Encrypted message is invalid - cannot be null or empty",
                        "encMessage"
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
                        "reKey"
                    );
                }
            }
            else if (reKey.IsPrimaryPartiallyValid(throwException))
            {
                if (reKey.IsExternalValid(encMessage, throwException))
                {
                    try { 
                        return FastDecrypt(encMessage, reKey); 
                    }

                    catch (Exception innerException) {
                        if (throwException) throw innerException;
                    }
                }
            }
            return null;
        }
        static public string Decrypt(string encMessage, EncryptionKey reKey, out Exception exception)
        {
            if (encMessage == null || encMessage == "" || encMessage.Length < 1)
            {
                exception = new ArgumentException
                (
                    "Encrypted message is invalid - cannot be null or empty",
                    "encMessage"
                );
            }
            else if (reKey == null)
            {
                exception = new ArgumentException
                (
                    "Encryption key is undefined (null or empty)",
                    "reKey"
                );
            }
            else
            {
                try
                {
                    reKey.IsPrimaryPartiallyValid(true);
                    reKey.IsExternalValid(encMessage, true);

                    string result = FastDecrypt(encMessage, reKey);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return null;
        }
        static public string FastDecrypt(string encMessage, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, encMessageLength = encMessage.Length;
            Int32 shCount = reKey.Shifts != null ? reKey.ShCount : 0;
            Int32[] buffer = new Int32[encMessageLength];

            List<Int32> ids = new List<Int32>();
            for (Int32 curChar = 0; curChar < encMessageLength; curChar++)
                ids.Add(reKey.ExAlphabet.IndexOf(encMessage[curChar]));

            if (shCount > 1)
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
                            (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrLength * 2 + reKey.Shifts.Max() - 4
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
                        reKey.ExAlphabet
                    );


                    //  Decode 1st message character
                    Int32.TryParse(encoding, out Int32 parsedEncoding);
                    decodedIds[0] = buffer[0] + parsedEncoding * exLength;
                    string decrypted = reKey.PrAlphabet[decodedIds[0]].ToString();


                    for (Int32 curId = 1; curId < encMessageLength; curId++)
                    {
                        buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - reKey.Shifts[curId % shCount];

                        encoding = Numsys.FromCustomAsString
                        (
                            encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                            exLength,
                            10,
                            reKey.ExAlphabet
                        );

                        Int32.TryParse(encoding, out parsedEncoding);
                        decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                        decrypted += reKey.PrAlphabet[decodedIds[curId]].ToString();
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
                            (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrLength * 2 + reKey.Shifts.Max() - 4
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
                        reKey.ExAlphabet
                    );


                    //  Decode 1st message character
                    Int32.TryParse(encoding, out Int32 parsedEncoding);
                    decodedIds[0] = buffer[0] + parsedEncoding * exLength;
                    string decrypted = reKey.PrAlphabet[decodedIds[0]].ToString();


                    for (Int32 curId = 1; curId < realMessageLength; curId++)
                    {
                        buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - reKey.Shifts[curId % shCount];

                        encoding = Numsys.FromCustomAsString
                        (
                            encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                            exLength,
                            10,
                            reKey.ExAlphabet
                        );

                        Int32.TryParse(encoding, out parsedEncoding);
                        decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                        decrypted += reKey.PrAlphabet[decodedIds[curId]].ToString();
                    }

                    return decrypted;
                }
            }
            else
            {
                Int32 shift = shCount > 0 ? reKey.Shifts[0] : 0;
                buffer[0] = ids[0] - shift;

                // Optimisation for base 10 encoding
                if (exLength == 10)
                {
                    Int32 maxEncodingLength = DigitsInPositive
                    (
                        (Int32)Math.Ceiling
                        (
                            (float)
                            (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrLength * 2 + reKey.Shifts.Max() - 4
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
                        reKey.ExAlphabet
                    );


                    //  Decode 1st message character
                    Int32.TryParse(encoding, out Int32 parsedEncoding);
                    decodedIds[0] = buffer[0] + parsedEncoding * exLength;
                    string decrypted = reKey.PrAlphabet[decodedIds[0]].ToString();


                    for (Int32 curId = 1; curId < encMessageLength; curId++)
                    {
                        buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - shift;

                        encoding = Numsys.FromCustomAsString
                        (
                            encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                            exLength,
                            10,
                            reKey.ExAlphabet
                        );

                        Int32.TryParse(encoding, out parsedEncoding);
                        decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                        decrypted += reKey.PrAlphabet[decodedIds[curId]].ToString();
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
                            (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                                reKey.PrLength * 2 + shift - 4
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
                        reKey.ExAlphabet
                    );


                    //  Decode 1st message character
                    Int32.TryParse(encoding, out Int32 parsedEncoding);
                    decodedIds[0] = buffer[0] + parsedEncoding * exLength;
                    string decrypted = reKey.PrAlphabet[decodedIds[0]].ToString();


                    for (Int32 curId = 1; curId < realMessageLength; curId++)
                    {
                        buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - shift;

                        encoding = Numsys.FromCustomAsString
                        (
                            encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                            exLength,
                            10,
                            reKey.ExAlphabet
                        );

                        Int32.TryParse(encoding, out parsedEncoding);
                        decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                        decrypted += reKey.PrAlphabet[decodedIds[curId]].ToString();
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
                string result = UnsafeDecryptWithConsoleInfo(encMessage, reKey);
                exception = null;

                return result;
            }
            catch (Exception innerException)
            {
                exception = innerException;
                return null; 
            }
        }
        static private string UnsafeDecryptWithConsoleInfo(string encMessage, EncryptionKey reKey)
        {
            Int32 exLength = reKey.ExLength, encMessageLength = encMessage.Length;
            Int32 shCount = reKey.Shifts != null ? reKey.ShCount : 0;
            Int32[] buffer = new Int32[encMessageLength];

            List<Int32> ids = new List<Int32>();
            for (Int32 curChar = 0; curChar < encMessageLength; curChar++)
                ids.Add(reKey.ExAlphabet.IndexOf(encMessage[curChar]));

            buffer[0] = ids[0] - reKey.Shifts[0];

            Int32 maxEncodingLength = Numsys.AsList
            (
                Math.Ceiling
                (
                    (float)
                    (   //  -4 bcs: (alphabet ids start at zero & dont reach .Length value) x 2
                        reKey.PrLength * 2 + reKey.Shifts.Max() - 4
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
                reKey.ExAlphabet
            );


            //  Decode 1st message character
            Int32.TryParse(encoding, out Int32 parsedEncoding);
            decodedIds[0] = buffer[0] + parsedEncoding * exLength;
            string decrypted = reKey.PrAlphabet[decodedIds[0]].ToString();


            for (Int32 curId = 1; curId < realMessageLength; curId++)
            {
                buffer[curId] = ids[curId * (maxEncodingLength + 1)] - decodedIds[curId - 1] - reKey.Shifts[curId % shCount];

                encoding = Numsys.FromCustomAsString
                (
                    encMessage.Substring(curId * (maxEncodingLength + 1) + 1, maxEncodingLength).ToString(),
                    exLength,
                    10,
                    reKey.ExAlphabet
                );

                Int32.TryParse(encoding, out parsedEncoding);
                decodedIds[curId] = buffer[curId] + parsedEncoding * exLength;
                decrypted += reKey.PrAlphabet[decodedIds[curId]].ToString();
            }

            DecryptingInfo(buffer, exLength, maxEncodingLength, reKey.Shifts.ToArray(), shCount, ids, decodedIds, encMessage, decrypted, realMessageLength);
            return decrypted;

        }
        static public string DecryptFromBytes()
        {
            return "aboba";
        }




        static public bool IsPrimaryValid(string message, string primary, bool throwException = false) => EncryptionKey.IsPrimaryValid(message, primary, throwException);
        static public bool IsPrimaryPartiallyValid(string primary, bool throwException = false) => EncryptionKey.IsPrimaryPartiallyValid(primary, throwException);


        static public bool IsExternalValid(string message, string external, bool throwException = false) => EncryptionKey.IsExternalValid(message, external, throwException);
        static public bool IsExternalPartiallyValid(string external, bool throwException = false) => EncryptionKey.IsExternalPartiallyValid(external, throwException);



        static private Int32 DigitsAmount(Int32 number)
        {
            if (number >= 0) return DigitsInPositive(number);
            else return number > -10 ? 1 : number > -100 ? 2 : number > -1000 ? 3 : number > -10000 ? 4 : 5;
        }
        static private Int32 DigitsInPositive(Int32 posNumber)
            => posNumber < 10 ? 1 : posNumber < 100 ? 2 : posNumber < 1000 ? 3 : posNumber < 10000 ? 4 : 5;
        


        static private void EncryptingInfo(Int32[] buffer, Int32 exLength, Int32 maxEncodingLength, Int32[] shifts, Int32 shCount, List<Int32> ids, string encrypted, string message, Int32 messageLength)
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

                for (Int32 ext = DigitsAmount(shifts[curId % shCount]); ext < marginSh; ext++) Write(" ");
                ForegroundColor = shifts[curId % shCount] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
                Write(shifts[curId % shCount]);
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
        static private void DecryptingInfo(Int32[] buffer, Int32 exLength, Int32 maxEncodingLength, Int32[] shifts, Int32 shCount, List<Int32> ids, Int32[] decodedIds, string encrypted, string decrypted, Int32 messageLength)
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

            for (Int32 ext = 0; ext < marginBf + margin * 2 + marginEn; ext++) Write(" ");
            Write("out: ");
            //for (Int32 ext = 0; ext < margin * 2; ext++) Write(" ");
            //Write("   out: ");
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

                for (Int32 ext = DigitsAmount(shifts[curId % shCount]); ext < marginSh; ext++) Write(" ");
                ForegroundColor = shifts[curId % shCount] == 0 ? ConsoleColor.Red : ConsoleColor.Cyan;
                Write(shifts[curId % shCount]);
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
    
    }
}