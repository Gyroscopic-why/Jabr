using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class EncryptionKey : IEncryptionKey
    {
        public void CopyFrom(EncryptionKey otherKey, bool fullCopy = true)
        {
            _noisifier.CopyFrom(otherKey.Noisifier, fullCopy);

            CopyFrom(otherKey.Primary, otherKey.External, otherKey.Shifts);

            if (fullCopy)
                Set.Default
                (
                    otherKey._primaryNecessary,
                    otherKey._primaryAllowed,
                    otherKey._primaryBanned,
                    otherKey._primaryMaxLength,
                    otherKey._externalNecessary,
                    otherKey._externalAllowed,
                    otherKey._externalBanned,
                    otherKey._externalMaxLength
                );
        }


        private void CopyFrom(string primary, string external, List<Int16> shifts)
        {
            _primaryAlphabet = primary;
            _externalAlphabet = external;

            _shifts.Clear();
            if (shifts == null || shifts.Count == 0) _shifts.Add(0);
            else _shifts.AddRange
                (
                    shifts.GetRange
                    (0, Math.Max(shifts.Count, 255))
                );
        }
    }
}