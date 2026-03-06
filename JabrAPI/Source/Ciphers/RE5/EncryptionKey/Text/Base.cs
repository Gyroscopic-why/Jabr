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
            _validateHelper = new ValidateHelper(this);

            _primaryAlphabet = primaryAlphabet;
            _externalAlphabet = externalAlphabet;

            _shifts.Clear();
            if (shifts == null || shifts.Count == 0) _shifts.Add(0);
            else _shifts.AddRange(shifts);
        }
        public EncryptionKey(string primaryAlphabet, string externalAlphabet, Int16 shift)
        {
            _setHelper = new(this);
            _validateHelper = new ValidateHelper(this);

            _primaryAlphabet = primaryAlphabet;
            _externalAlphabet = externalAlphabet;

            _shifts.Clear();
            _shifts.Add(shift);
        }
        public EncryptionKey(string primaryAlphabet, string externalAlphabet)
        {
            _setHelper = new(this);
            _validateHelper = new ValidateHelper(this);

            _primaryAlphabet = primaryAlphabet;
            _externalAlphabet = externalAlphabet;
        }
        public EncryptionKey(Int32 shiftCount)
        {
            _setHelper = new(this);
            _validateHelper = new ValidateHelper(this);
            _shCount = shiftCount;
        }
        public EncryptionKey(EncryptionKey otherKey, bool fullCopy = true)
        {
            _setHelper = new(this);
            _validateHelper = new ValidateHelper(this);
            CopyFrom(otherKey, fullCopy);
        }
        public EncryptionKey(bool autoGenerate = true)
        {
            _setHelper = new(this);
            _validateHelper = new ValidateHelper(this);

            if (autoGenerate) DefaultGenerate();
            else Set.Default();
        }

        public EncryptionKey(List<Byte> binaryExportData, bool throwExceptions = false)
        {
            _setHelper = new(this);
            _validateHelper = new ValidateHelper(this);
            ImportFromBinary(binaryExportData, throwExceptions);
        }
        public EncryptionKey(string stringExportData, bool throwExceptions = false)
        {
            _setHelper = new(this);
            _validateHelper = new ValidateHelper(this);
            ImportFromString(stringExportData, throwExceptions);
        }
    }
}