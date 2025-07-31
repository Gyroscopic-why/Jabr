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

            public string ExternalAlphabet => _externalAlphabet;

            public Int32[] Shifts => _shifts;



            
            public void Next()
            {
                //  Generate new parameters with similar base as the current
            }



            
            public void SetDefault()
            {
                //  Set default parameters for random key generation
            }

            private void Default()
            {
                
            }


            
            public void GenerateRandomPrimary()
            {
                
            }


            
            public void GenerateRandomExternal()
            {
                
            }


            public void GenerateRandomShifts(Int32 count)
            {
                
            }
            public void GenerateRandomShifts()
            {
                
            }
        }



        static public string Encrypt(string message, string reKey)
        {
            return "aboba";
        }
        static public List<Byte> EncryptToBytes()
        {
            return null;
        }



        static public string Decrypt(string message, string reKey)
        {
            return "aboba";
        }
        static public string DecryptFromBytes()
        {
            return "";
        }
    }
}