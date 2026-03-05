using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
        private readonly List<Byte> _primaryAlphabet = [];
        private readonly List<Byte> _externalAlphabet = [];

        private Byte _compactedPrMaxLength = 255, _compactedExMaxLength = 7;



        public BinaryKey(List<Byte> primary, List<Byte> external, List<Byte> shifts)
            => Set(primary, external, shifts);
        public BinaryKey(BinaryKey otherKey, bool fullCopy = true)
            => Copy(otherKey, fullCopy);

        public BinaryKey(List<Byte> exported) => ImportFromBinary(exported);
        public BinaryKey(bool autoGenerate = true)
        {
            if (autoGenerate) DefaultGenerate();
            else SetDefault();
        }
    }
}