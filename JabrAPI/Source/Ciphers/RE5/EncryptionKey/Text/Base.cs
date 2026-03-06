using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class EncryptionKey : IEncryptionKey
    {
        private readonly SetHelper _setHelper;
        private readonly ValidateHelper _validateHelper;

        private string _primaryAlphabet  = "";
        private string _externalAlphabet = "";

        private List<char> _primaryNecessary  = [], _primaryAllowed  = [], _primaryBanned  = [];
        private List<char> _externalNecessary = [], _externalAllowed = [], _externalBanned = [];
        private Int32 _primaryMaxLength = -1, _externalMaxLength = -1;



        public EncryptionKey(string primaryAlphabet, string externalAlphabet, List<Int16> shifts)
        {
            _setHelper = new(this);
            _validateHelper = new(this);

            Set.Sensitive.PrAlphabet(primaryAlphabet);
            Set.Sensitive.PrAlphabet(externalAlphabet);
            Set.Sensitive.Shifts(shifts);
        }
        public EncryptionKey(string primaryAlphabet, string externalAlphabet, Int16 shift)
        {
            _setHelper = new(this);
            _validateHelper = new(this);

            Set.Sensitive.PrAlphabet(primaryAlphabet);
            Set.Sensitive.PrAlphabet(externalAlphabet);
            Set.Sensitive.Shift(shift);
        }
        public EncryptionKey(string primaryAlphabet, string externalAlphabet)
        {
            _setHelper = new(this);
            _validateHelper = new(this);

            Set.Sensitive.PrAlphabet(primaryAlphabet);
            Set.Sensitive.ExAlphabet(externalAlphabet);
        }
        public EncryptionKey(Int32 shiftCount)
        {
            _setHelper = new(this);
            _validateHelper = new(this);

            Set.ShiftCount(shiftCount);
        }
        public EncryptionKey(EncryptionKey otherKey, bool fullCopy = true)
        {
            _setHelper = new(this);
            _validateHelper = new(this);

            CopyFrom(otherKey, fullCopy);
        }
        public EncryptionKey(bool autoGenerate = true)
        {
            _setHelper = new(this);
            _validateHelper = new(this);

            if (autoGenerate) DefaultGenerate();
            else Set.Default();
        }

        public EncryptionKey(List<Byte> binaryExportData, bool throwExceptions = false)
        {
            _setHelper = new(this);
            _validateHelper = new(this);

            ImportFromBinary(binaryExportData, throwExceptions);
        }
        public EncryptionKey(string stringExportData, bool throwExceptions = false)
        {
            _setHelper = new(this);
            _validateHelper = new(this);

            ImportFromString(stringExportData, throwExceptions);
        }
    }
}