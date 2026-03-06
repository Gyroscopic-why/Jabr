using System;
using System.Collections.Generic;

using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
        private readonly SetHelper _setHelper;
        private readonly ValidateHelper _validationHelper;

        private readonly List<Byte> _primaryAlphabet = [];
        private readonly List<Byte> _externalAlphabet = [];

        private Byte _compactedPrMaxLength = 255, _compactedExMaxLength = 7;



        public BinaryKey(List<Byte> primary, List<Byte> external, List<Byte> shifts)
        {
            _setHelper = new(this);
            _validationHelper = new(this);

            Set.Sensitive.PrAlphabet(primary);
            Set.Sensitive.ExAlphabet(external);
            Set.Sensitive.Shifts(shifts);
        }
        public BinaryKey(List<Byte> primary, List<Byte> external, Byte shift)
        {
            _setHelper = new(this);
            _validationHelper = new(this);

            Set.Sensitive.PrAlphabet(primary);
            Set.Sensitive.ExAlphabet(external);
            Set.Sensitive.Shift(shift);
        }
        public BinaryKey(Int32 shiftCount)
        {
            _setHelper = new(this);
            _validationHelper = new(this);

            Set.ShiftCount(shiftCount);
        }
        public BinaryKey(BinaryKey otherKey, bool fullCopy = true)
        {
            _setHelper = new(this);
            _validationHelper = new(this);

            CopyFrom(otherKey, fullCopy);
        }
        public BinaryKey(bool autoGenerate = true)
        {
            _setHelper = new(this);
            _validationHelper = new(this);

            if (autoGenerate) DefaultGenerate();
            else Set.Default();
        }

        public BinaryKey(List<Byte> exportData)
        {
            _setHelper = new(this);
            _validationHelper = new(this);

            ImportFromBinary(exportData);
        }
    }
}