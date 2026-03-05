using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
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
    }
}