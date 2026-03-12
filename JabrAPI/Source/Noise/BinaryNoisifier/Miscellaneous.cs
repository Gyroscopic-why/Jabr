using System;
using System.Collections.Generic;



namespace JabrAPI
{
    public partial class BinaryNoisifier
    {
        public void CopyFrom(BinaryNoisifier otherNoisifier, bool fullCopy = true)
        {
            CopyFrom(otherNoisifier.PrimaryNoise, otherNoisifier.ComplexNoise);

            if (fullCopy)
                Set.Default
                (
                    otherNoisifier.Banned,
                    otherNoisifier._primaryCount,
                    otherNoisifier._complexCount
                );
        }
        private void CopyFrom(List<Byte> primaryNoise, List<Byte> complexNoise)
        {
            _primaryNoise.Clear();
            _primaryNoise.AddRange(primaryNoise);

            _complexNoise.Clear();
            _complexNoise.AddRange(complexNoise);
        }



        private void ReconfigureNoiseParametersCount(Int32 allowedTotal)
        {
            if (allowedTotal < _primaryCount + _complexCount)
            {
                Byte compromiseLeft = (Byte)(allowedTotal / 3);
                _primaryCount = compromiseLeft;
                _complexCount = (Byte)(allowedTotal - compromiseLeft);
            }
        }
        private List<Byte> GetRemainingAllowed(List<Byte> banned, Int32 targetCount, bool banAlreadyUsed)
        {
            List<Byte> result = [.. DEFAULT.BYTES];

            if (!banAlreadyUsed) ReconfigureNoiseParametersCount(256 - banned.Count);
            else foreach (Byte bannedByte in _primaryNoise.Count == 0
                    ? _complexNoise : _primaryNoise)
                    result.RemoveAll(c => c == bannedByte);

            foreach (Byte bannedByte in banned)
                result.RemoveAll(c => c == bannedByte);


            Int32 chosenId, curCount = result.Count;
            while (curCount > targetCount)
            {
                chosenId = _random.Next(curCount);

                result.RemoveRange
                (
                    chosenId,
                    Math.Min
                    (
                        curCount - chosenId - 1,
                        _random.Next(1, curCount - targetCount)
                    )
                );

                curCount = result.Count;
            }

            return result;
        }
    }
}