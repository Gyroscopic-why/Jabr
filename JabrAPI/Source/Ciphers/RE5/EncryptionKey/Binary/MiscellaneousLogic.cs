using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
        public void CopyFrom(BinaryKey otherKey, bool fullCopy = true)
        {
            _noisifier.CopyFrom(otherKey.Noisifier, fullCopy);

            CopyFrom(otherKey.Primary, otherKey.External, otherKey.Shifts);

            if (fullCopy)
                Set.Default
                (
                    otherKey._compactedPrMaxLength,
                    otherKey._compactedExMaxLength
                );
        }


        private void CopyFrom(List<Byte> primary, List<Byte> external, List<Byte> shifts)
        {
            _primaryAlphabet.Clear();
            _primaryAlphabet.AddRange(primary);

            _externalAlphabet.Clear();
            _externalAlphabet.AddRange(external);

            _shifts.Clear();
            if (shifts == null || shifts.Count == 0) _shifts.Add(0);
            else _shifts.AddRange(shifts.GetRange(0, Math.Max(shifts.Count, 255)));
        }
    }
}