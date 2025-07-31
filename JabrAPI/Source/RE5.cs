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
            private Int32[] _shifts;

            private List<char> _primaryNecessary, _primaryAllowed;
            private List<char> _externalNecessary, _externalAllowed;
            private Int32 _primaryMaxLength, _externalMaxLength;


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

            }



            public string PrimaryAlphabet => _primaryAlphabet;
            public Int32 PrimaryLength => _primaryAlphabet.Length;

            public string ExternalAlphabet => _externalAlphabet;
            public Int32 ExternalLength => _externalAlphabet.Length;

            public Int32[] Shifts => _shifts;



            public void ExportKey()
            {
                SetDefault();
                GenerateRandomPrimary();
                GenerateRandomExternal();
                GenerateRandomShifts(10);
            }





            public void Next()
            {

            }



            public void SetDefault(List<char> pNecessary, List<char> pAllowed, Int32 pMaxLength,
                                   List<char> eNecessary, List<char> eAllowed, Int32 eMaxLength)
            {
                _primaryNecessary = pNecessary;
                _externalNecessary = eNecessary;

                _primaryAllowed = pAllowed;
                _externalAllowed = eAllowed;

                _primaryMaxLength = pMaxLength;
                _externalMaxLength = eMaxLength;
            }
            public void SetDefault(string pNecessary, string pAllowed, Int32 pMaxLength,
                                   string eNecessary, string eAllowed, Int32 eMaxLength)
            {
                _primaryNecessary = pNecessary.ToList();
                _externalNecessary = eNecessary.ToList();

                _primaryAllowed = pAllowed.ToList();
                _externalAllowed = eAllowed.ToList();

                _primaryMaxLength = pMaxLength;
                _externalMaxLength = eMaxLength;
            }



            public void SetDefault(List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                _primaryNecessary = necessary;
                _externalNecessary = necessary;

                _primaryAllowed = allowed;
                _externalAllowed = allowed;

                _primaryMaxLength = maxLength;
                _externalMaxLength = maxLength;
            }
            public void SetDefault(string necessary, string allowed, Int32 maxLength)
            {
                _primaryNecessary = necessary.ToList();
                _externalNecessary = necessary.ToList();

                _primaryAllowed = allowed.ToList();
                _externalAllowed = allowed.ToList();

                _primaryMaxLength = maxLength;
                _externalMaxLength = maxLength;
            }

            public void SetDefault()
            {
                _primaryNecessary = " `1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№".ToList();

                _primaryAllowed = " `1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№".ToList();
                _externalAllowed = " `1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?ёйцукенгшщзхъфывапролджэячсмитьбюЁЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ№".ToList();

                _primaryMaxLength = _primaryNecessary.Count;
                _externalMaxLength = 8;
            }



            public void GenerateRandomPrimary(List<char> necessary, List<char> allowed, Int32 maxLength)
            {
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
            public void GenerateRandomPrimary()
            {
                
            }


            public void GenerateRandomExternal(List<char> necessary, List<char> allowed, Int32 maxLength)
            {
                
            }
            public void GenerateRandomExternal(string necessary, string allowed, Int32 maxLength)
            {
                
            }
            public void GenerateRandomExternal(List<char> allowed, Int32 maxLength)
            {
                
            }
            public void GenerateRandomExternal(string allowed, Int32 maxLength)
            {

            }
            public void GenerateRandomExternal()
            {
                
            }


            public void GenerateRandomShifts(Int32 amount)
            {
                _shifts = new Int32[amount];
                Random random = new Random();

                for (Int32 curId = 0; curId < amount; curId++)
                    _shifts[curId] = random.Next(0, _primaryAlphabet.Length - 1);
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