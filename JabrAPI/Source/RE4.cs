using System;
using System.Text;
using System.Collections.Generic;


using AVcontrol;
using static JabrAPI.Source.Template;



namespace JabrAPI
{
    public class RE4
    {
        public class EncryptionKey : IEncryptionKey
        {
            protected string _alphabet = "";
            protected List<char> _necessary = [], _allowed = [], _banned = [];
            protected Int32 _alphabetMaxLength = -1;

            public EncryptionKey(string alphabet, List<Int32> shifts)
            {
                _alphabet = alphabet;

                _shifts.Clear();
                if (shifts == null || shifts.Count == 0) _shifts.Add(0);
                else _shifts.AddRange(shifts);
            }
            public EncryptionKey(string alphabet, Int32 shift)
            {
                _alphabet = alphabet;

                _shifts.Clear();
                _shifts.Add(shift);
            }
            public EncryptionKey(string alphabet) => _alphabet = alphabet;
            public EncryptionKey(Int32 shiftCount) => _shCount = shiftCount;
            public EncryptionKey(BinaryKey binKey)
            {
                _alphabet = Encoding.Unicode.GetString(ToBinary.LittleEndian(binKey.Alphabet.ToArray()));

                _shifts.Clear();
                if (binKey.Shifts == null || binKey.Shifts.Count == 0) _shifts.Add(0);
                else _shifts.AddRange(binKey.Shifts);
            }
            public EncryptionKey(bool autoGenerate = true)
            {
                if (autoGenerate) Default();
                else SetDefault();
            }


            static public BinaryKey Convert(EncryptionKey reKey) => new(reKey);
            static public EncryptionKey Convert(BinaryKey binKey) => new(binKey);



            public string Alphabet => _alphabet;
            public Int32 AlphabetLength => _alphabet == null ? -1 : _alphabet.Length;



            override public string ExportAsString(string splitBy, string splitShifts = " ")
            {
                if (_alphabet.Contains(splitBy))
                {
                    throw new ArgumentException
                    (
                        "split argument is not unique, impossible to distinguish data using it",
                        nameof(splitBy)
                    );
                }

                string result = _alphabet + splitBy;

                for (var curId = 0; curId < _shifts.Count - 1; curId++)
                    result += _shifts[curId] + splitShifts;
                if (_shifts.Count > 0) result += _shifts[^1];

                return result;
            }
            override public string ExportAsString()
            {
                if (Byte.TryParse(_alphabet[0].ToString(), out _))
                {
                    throw new ArgumentException
                    (
                        "impossible to use length as a splitter due to the alphabet having numbers in its start",
                        nameof(_alphabet)
                    );
                }

                string result = _alphabet.Length + _alphabet;

                for (var curId = 0; curId < _shifts.Count - 1; curId++)
                    result += _shifts[curId] + " ";
                if (_shifts.Count > 0) result += _shifts[^1];

                return result;
            }
            override public List<Byte> ExportAsBinaryBE()
            {
                List<Byte> result = [.. ToBinary.BigEndian(_shifts.Count)];

                for (var curId = 0; curId < _shifts.Count; curId++)
                    result.AddRange(ToBinary.BigEndian(_shifts[curId]));

                Byte[] buffer = ToBinary.Utf8(_alphabet);
                result.AddRange(ToBinary.BigEndian(buffer.Length));
                result.AddRange(buffer);

                return result;
            }
            override public List<Byte> ExportAsBinaryLE()
            {
                List<Byte> result = [.. ToBinary.LittleEndian(_shifts.Count)];

                for (var curId = 0; curId < _shifts.Count; curId++)
                    result.AddRange(ToBinary.LittleEndian(_shifts[curId]));

                Byte[] buffer = ToBinary.Utf8(_alphabet);
                result.AddRange(ToBinary.LittleEndian(buffer.Length));
                result.AddRange(buffer);

                return result;
            }



            public bool IsAlphabetValid(string message, bool throwException = false)
                => IsAlphabetValid(message, _alphabet, throwException);
            static public bool IsAlphabetValid(string message, string alphabet, bool throwException = false)
            {
                if (!IsAlphabetPartiallyValid(alphabet, throwException)) return false;
                foreach (char c in message)
                {
                    if (!alphabet.Contains(c))
                    {
                        if (throwException)
                        {
                            throw new ArgumentException
                            (
                                "Message contains characters not present in the alphabet"
                                + "\nchar from message: " + c,
                                nameof(message)
                            );
                        }
                        return false;
                    }
                }

                return true;
            }

            public bool IsAlphabetPartiallyValid(bool throwException = false)
                => IsAlphabetPartiallyValid(_alphabet, throwException);
            static public bool IsAlphabetPartiallyValid(string alphabet, bool throwException = false)
            {
                if (alphabet == null || alphabet == "" || alphabet.Length < 2)
                {
                    if (throwException)
                    {
                        throw new ArgumentException
                        (
                            "Alphabet is not set or is too short",
                            nameof(alphabet)
                        );
                    }
                    return false;
                }

                return true;
            }




            public void SetDefault(List<char> necessary, List<char> allowed, List<char> banned, Int32 maxLength)
            {
                _necessary = necessary;
                _allowed   = allowed;
                _banned    = banned;

                _alphabetMaxLength = maxLength;
            }
            public void SetDefault(string necessary, string allowed, string banned, Int32 maxLength)
                => SetDefault([.. necessary], [.. allowed], [.. banned], maxLength);


            public void SetDefault(List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                _necessary = necessary;
                _allowed   = allowed;

                _alphabetMaxLength = maxLength;
            }
            public void SetDefault(string necessary, string allowed, Int32 maxLength)
                => SetDefault([.. necessary], [.. allowed], maxLength);


            public void SetDefault(Int32 maxLength, List<char> necessary, List<char> banned)
            {
                _necessary = necessary;
                _banned    = banned;

                _alphabetMaxLength = maxLength;
            }
            public void SetDefault(Int32 maxLength, string necessary, string banned)
                => SetDefault(maxLength, [.. necessary], [.. banned]);


            public void SetDefault(Int32 maxLength, List<char> banned)
            {
                _banned = banned;
                _alphabetMaxLength = maxLength;
            }
            public void SetDefault(Int32 maxLength, string banned)
                => SetDefault(maxLength, [.. banned]);




            override public void SetDefault()
            {
                _necessary = [.. " " + _defaultChars];
                _allowed   = [];

                _alphabetMaxLength = _necessary.Count;
            }
            protected void Default()
            {
                SetDefault();
                GenerateAll();
            }
            override protected private void GenerateAll()
            {
                GenerateRandomAlphabet();

                if (_shifts.Count < 2)
                {
                    if (_shCount < 2) GenerateRandomShifts();
                    else GenerateRandomShifts(_shCount);
                }
                else GenerateRandomShifts(_shifts.Count);
            }




            private void ValidateForAlphabetGeneration(List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                if (necessary != null && necessary.Count > 0)
                {
                    _necessary = necessary;

                    for (var id1 = 0; id1 < necessary.Count; id1++)
                    {
                        for (var id2 = id1 + 1; id2 < necessary.Count; id2++)
                        {
                            if (necessary[id1] == necessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "necessary list cannot include duplicates"
                                     + "\nduplicate char: " + necessary[id1],
                                    nameof(necessary)
                                );
                            }
                        }
                    }
                }
                else _necessary = [];

                if (allowed != null)
                {
                    _allowed = allowed;

                    for (var id1 = 0; id1 < allowed.Count; id1++)
                    {
                        for (var id2 = id1 + 1; id2 < allowed.Count; id2++)
                        {
                            if (allowed[id1] == allowed[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicates"
                                    + "\nduplicate char: " + allowed[id1],
                                    nameof(allowed)
                                );
                            }
                        }
                        for (var id2 = 0; id2 < _necessary.Count; id2++)
                        {
                            if (allowed[id1] == _necessary[id2])
                            {
                                throw new ArgumentException
                                (
                                    "allowed list cannot include duplicate chars of necessary list"
                                    + "\nduplicate char: " + allowed[id1],
                                    nameof(allowed) + ", " + nameof(necessary)
                                );
                            }
                        }
                    }
                }
                else _allowed = [];


                if (maxLength < 2)
                {
                    throw new ArgumentException
                    (
                        "Max length is less than the smallest possible alphabet length (2)",
                        nameof(maxLength)
                    );
                }
                maxLength -= _necessary.Count;
                if (maxLength < 0)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be less than the number of necessary characters.",
                        nameof(maxLength) + ", " + nameof(_necessary.Count)
                    );
                }
                else if (maxLength > _allowed.Count)
                {
                    throw new ArgumentException
                    (
                        "Max length cannot be more than allowed characters count",
                        nameof(maxLength) + ", " + nameof(allowed.Count)
                    );
                }

                if (_necessary.Count + _allowed.Count < 2)
                {
                    throw new ArgumentException
                    (
                        "Not enough characters in necessary and allowed list for a valid key (< 2)",
                        nameof(_necessary.Count) + " + " + nameof(_allowed.Count)
                    );
                }
            }

            public void GenerateRandomAlphabet(List<char> necessary, List<char> allowed, Int32 maxLength, bool validateParameters = true)
            {
                if (validateParameters)
                {
                    ValidateForAlphabetGeneration(necessary, allowed, maxLength);

                    necessary = _necessary;  //  to avoid nullpointer exceptions
                    allowed = _allowed;    //  the _alphabet variants are always defaulted
                                           //  (to a new empty List in worst case)
                }

                Int32 buffer, bufferId;
                _alphabet = "";


                while (necessary.Count > 0)
                {
                    buffer = _random.Next(0, necessary.Count);
                    _alphabet += necessary[buffer];
                    necessary.RemoveAt(buffer);
                }
                for (var remaining = Math.Min(maxLength, allowed.Count); remaining > 0; remaining--)
                {
                    buffer = _random.Next(0, allowed.Count);
                    bufferId = _random.Next(0, _alphabet.Length);

                    _alphabet = _alphabet.Insert(bufferId, allowed[buffer].ToString());
                    allowed.RemoveAt(buffer);
                }
            }
            public void GenerateRandomAlphabet(string necessary, string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomAlphabet([.. necessary], [.. allowed], maxLength, validateParameters);


            public void GenerateRandomAlphabet(List<char> allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomAlphabet([], allowed, maxLength, validateParameters);
            public void GenerateRandomAlphabet(string allowed, Int32 maxLength, bool validateParameters = true)
                => GenerateRandomAlphabet([], [.. allowed], maxLength, validateParameters);


            public void GenerateRandomAlphabet(Int32 maxLength, List<char> necessary, List<char> banned, bool validateParameters = true)
            {
                List<char> allowed = [.. (" " + _defaultChars)];

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

                GenerateRandomAlphabet(necessary, allowed, maxLength, validateParameters);
            }
            public void GenerateRandomAlphabet(Int32 maxLength, string necessary, string banned, bool validateParameters = true)
                => GenerateRandomAlphabet(maxLength, [.. necessary], [.. banned], validateParameters);


            public void GenerateRandomAlphabet(Int32 maxLength, List<char> banned, bool validateParameters = true)
                => GenerateRandomAlphabet(maxLength, [], banned, validateParameters);
            public void GenerateRandomAlphabet(Int32 maxLength, string banned, bool validateParameters = true)
                => GenerateRandomAlphabet(maxLength, [], [.. banned], validateParameters);
            public void GenerateRandomAlphabet(Int32 maxLength, bool validateParameters = true)
                => GenerateRandomAlphabet(maxLength, "", validateParameters);

            public void GenerateRandomAlphabet()
            {
                if (_allowed != null && _allowed.Count > 0)
                {
                    List<char> allowed = [.. _allowed];

                    if (_banned != null && _banned.Count > 0)
                    {
                        for (var curId = 0; curId < _banned.Count; curId++)
                        {
                            for (var id2 = 0; id2 < allowed.Count; id2++)
                            {
                                if (_banned[curId] == allowed[id2])
                                {
                                    allowed.RemoveAt(id2);
                                    id2--;
                                }
                            }
                        }
                    }
                    GenerateRandomAlphabet(_necessary, allowed, _alphabetMaxLength);
                }
                else GenerateRandomAlphabet(_necessary, [], _necessary.Count);
            }





            public void GenerateRandomShifts(Int32 count)
            {
                if (_alphabet == null || _alphabet.Length < 2)
                {
                    throw new ArgumentException
                    (
                        "Unable to generate shifts, alphabet is undefined",
                        nameof(_alphabet)
                    );
                }

                GenerateRandomShifts(count, 0, _alphabet.Length - 1);
            }
            public void GenerateRandomShifts()
                => GenerateRandomShifts(_random.Next(256, 512));
        }
        public class BinaryKey
        {
            private List<Int16> _alphabet = [];
            private readonly List<Int32> _shifts = [0];


            public BinaryKey(string alphabet, List<Int32> shifts)
                => Set(alphabet, shifts);
            public BinaryKey(EncryptionKey reKey) => Set(reKey);
            public BinaryKey() { }


            public List<Int16> Alphabet => _alphabet;
            public Int32 AlphabetLength => _alphabet == null ? -1 : _alphabet.Count;


            public List<Int32> Shifts => _shifts;
            public Int32 ShAmount => _shifts == null ? -1 : _shifts.Count;
            public Int32 ShLength => _shifts == null ? -1 : _shifts.Count;
            public Int32 ShCount => _shifts == null ? -1 : _shifts.Count;



            public void Set(string alphabet, List<Int32> shifts)
            {
                _alphabet = FromBinary.AutoLEBytesToInt16
                (
                    [..
                        ToBinary.Utf16
                        (
                            alphabet
                        )
                    ]
                );

                _shifts.Clear();
                if (shifts == null || shifts.Count == 0) _shifts.Add(0);
                else _shifts.AddRange(shifts);
            }
            public void Set(EncryptionKey reKey)
            {
                _alphabet = FromBinary.AutoLEBytesToInt16
                (
                    [..
                        ToBinary.Utf16
                        (
                            reKey.Alphabet
                        )
                    ]
                );

                _shifts.Clear();
                if (reKey.Shifts == null || reKey.Shifts.Count == 0) _shifts.Add(0);
                else _shifts.AddRange(reKey.Shifts);
            }



            static public BinaryKey Convert(EncryptionKey reKey) => new(reKey);
            static public EncryptionKey Convert(BinaryKey binKey) => new(binKey);
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
            else if (reKey.IsAlphabetValid(message, throwException))
            {
                try
                {
                    return FastEncrypt(message, reKey);
                }

                catch (Exception)
                {
                    if (throwException) throw;
                }
            }

            return "";
        }
        static public string Encrypt(string message, EncryptionKey reKey, out Exception? exception)
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
                    reKey.IsAlphabetValid(message, true);

                    string result = FastEncrypt(message, reKey);
                    exception = null;

                    return result;
                }
                catch (Exception innerException) { exception = innerException; }
            }
            return "";
        }
        static public string FastEncrypt(string message, EncryptionKey reKey)
        {
            Int32 aLength = reKey.AlphabetLength, messageLength = message.Length, shCount = reKey.ShCount;
            Int32[] eID = new Int32[messageLength];
            List<Int32> shifts = reKey.Shifts;
            string alphabet = reKey.Alphabet;

            eID[0] = alphabet.IndexOf(message[0]);
            string encrypted = alphabet[(eID[0] + shifts[0]) % aLength].ToString();

            for (var i = 1; i < messageLength; i++)
            {
                eID[i] = alphabet.IndexOf(message[i]) + eID[i - 1];
                encrypted += alphabet[(eID[i] + shifts[i % shCount] * (i % 2 + 1)) % aLength];
            }
            return encrypted;
        }



        static public string Decrypt(string encrypted, EncryptionKey reKey, bool throwException = false)
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
            else if (reKey.IsAlphabetValid(encrypted, throwException))
            {
                try
                {
                    return FastDecrypt(encrypted, reKey);
                }

                catch (Exception)
                {
                    if (throwException) throw;
                }
            }

            return "";
        }
        static public string Decrypt(string encrypted, EncryptionKey reKey, out Exception? exception)
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
                    reKey.IsAlphabetValid(encrypted, true);

                    string result = FastDecrypt(encrypted, reKey);
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
        static public string FastDecrypt(string encrypted, EncryptionKey reKey)
        {
            Int32 aLength = reKey.AlphabetLength, messageLength = encrypted.Length, shCount = reKey.ShCount;
            Int32[] dID = new Int32[messageLength];
            List<Int32> shifts = reKey.Shifts;
            string alphabet = reKey.Alphabet;

            dID[0] = alphabet.IndexOf(encrypted[0]) - shifts[0];
            string decrypted = alphabet[(dID[0] + aLength) % aLength].ToString();

            for (var i = 1; i < messageLength; i++)
            {
                dID[i] = alphabet.IndexOf(encrypted[i]) - shifts[i % shCount] * (i % 2 + 1);
                decrypted += alphabet[(dID[i] - dID[i - 1] + 4 * aLength) % aLength];
            }
            return decrypted;
        }



        static public bool IsAlphabetValid(string message, string primary, bool throwException = false) => EncryptionKey.IsAlphabetValid(message, primary, throwException);
        static public bool IsAlphabetPartiallyValid(string primary, bool throwException = false) => EncryptionKey.IsAlphabetPartiallyValid(primary, throwException);

    }
}