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


            public void ExportKey()
            {
                //  think about easy key exporting as its sometimes hard to transfer classes
            }

            public bool AreParametersValid()
            {
                //  some test alg for determining if the the current key values are safe to use

                return false;
            }


            static public bool AreParametersValid(string primary, string external, Int32[] shifts)
            {
                return false;
            }
            static public bool AreParametersValid(string primary, string external, Int32[] shifts, string message)
            {
                bool valid = AreParametersValid(primary, external, shifts);

                //  Add additional logic for message checking

                return false;
            }


            public void Next()
            {
                GenerateRandomPrimary();
                GenerateRandomExternal();
                GenerateRandomShifts();
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
                _externalNecessary  = necessary;

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
                else if (maxLength > allowed.Count) maxLength = allowed.Count;

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
                else if (maxLength > allowed.Length) maxLength = allowed.Length;

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
            public void GenerateRandomPrimary(Int32 maxLength)
            {

            }
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
            public void GenerateRandomExternal()
            {
                //  Remove bullshit
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
                else if (_primaryAlphabet == null || _primaryAlphabet.Length < 2)
                {
                    throw new ArgumentException
                    (
                        "Unable to generate shifts, primary alphabet is undefined",
                        "_primaryAlphabet"
                    );
                }

                _shifts = new Int32[amount];
                Random random = new Random();

                for (Int32 curId = 0; curId < amount; curId++)
                    _shifts[curId] = random.Next(0, _primaryAlphabet.Length - 1);
            }
            public void GenerateRandomShifts()
            {
                // select default option or use reference - needs to resolve issue
            }
        }




        static public string Encrypt(string message, EncryptionKey reKey)
        {
            return "aboba";
        }
        static public List<Byte> EncryptToBytes()
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