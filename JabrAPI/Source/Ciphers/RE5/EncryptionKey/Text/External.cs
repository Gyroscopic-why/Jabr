using System;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class EncryptionKey : IEncryptionKey
    {
        public string PrimaryAlphabet => _primaryAlphabet;
        public string PrAlphabet      => _primaryAlphabet;
        public string Primary         => _primaryAlphabet;
        public Int32 PrimaryLength => _primaryAlphabet == null ? -1 : _primaryAlphabet.Length;
        public Int32 PrLength      => _primaryAlphabet == null ? -1 : _primaryAlphabet.Length;

        public string ExternalAlphabet => _externalAlphabet;
        public string ExAlphabet       => _externalAlphabet;
        public string External         => _externalAlphabet;
        public Int32 ExternalLength => _externalAlphabet == null ? -1 : _externalAlphabet.Length;
        public Int32 ExLength       => _externalAlphabet == null ? -1 : _externalAlphabet.Length;

        override public string FinalAlphabet => _externalAlphabet;
    }
}