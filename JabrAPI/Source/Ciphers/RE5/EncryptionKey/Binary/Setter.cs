using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
        public  void Copy(BinaryKey otherKey, bool fullCopy = true)
        {
            _noisifier.Copy(otherKey.Noisifier, fullCopy);

            Set(otherKey.Primary, otherKey.External, otherKey.Shifts);

            if (fullCopy)
                SetDefault
                (
                    otherKey._compactedPrMaxLength,
                    otherKey._compactedExMaxLength
                );
        }
        private void Set(List<Byte> primary, List<Byte> external, List<Byte> shifts)
        {
            _primaryAlphabet.Clear();
            _primaryAlphabet.AddRange(primary);

            _externalAlphabet.Clear();
            _externalAlphabet.AddRange(external);

            _shifts.Clear();
            if (shifts == null || shifts.Count == 0) _shifts.Add(0);
            else _shifts.AddRange(shifts.GetRange(0, Math.Max(shifts.Count, 255)));
        }



        public void SetDefaultOnlyEx(Byte compactedExMaxLength) => _compactedExMaxLength = compactedExMaxLength;
        public void SetDefaultOnlyPr(Byte compactedPrMaxLength) => _compactedPrMaxLength = compactedPrMaxLength;
        public void SetDefault(Byte compactedPrMaxLength, Byte compactedExMaxLength)
        {
            _compactedPrMaxLength = compactedPrMaxLength;
            _compactedExMaxLength = compactedExMaxLength;
        }
        public override void SetDefault()
        {
            _compactedPrMaxLength = 255;
            _compactedExMaxLength = 7;
        }
    }
}