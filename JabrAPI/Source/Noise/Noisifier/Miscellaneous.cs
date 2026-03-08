using System;
using System.Collections.Generic;



namespace JabrAPI
{
    public partial class Noisifier
    {
        public  void CopyFrom(Noisifier otherNoisifier, bool fullCopy = true)
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
        private void CopyFrom(string primaryNoise, string complexNoise)
        {
            _primaryNoise = primaryNoise;
            _complexNoise = complexNoise;
        }



        private void ReconfigureNoiseParametersCount(Int32 allowedTotal)
        {
            if (allowedTotal < _primaryCount + _complexCount)
            {
                Int32 compromiseLeft = allowedTotal / 3;
                _primaryCount = compromiseLeft;
                _complexCount = allowedTotal - compromiseLeft;
            }
        }
        private List<char> GetRemainingAllowed(List<char> banned, Int32 targetCount, bool banAlreadyUsed)
        {
            List<char> result = [.. DEFAULT.CHARACTERS.WITHOUT_SPACE];

            if (!banAlreadyUsed) ReconfigureNoiseParametersCount(256 - banned.Count);
            else foreach (char bannedChar in _primaryNoise == ""
                    ? _complexNoise : _primaryNoise)
                    result.RemoveAll(c => c == bannedChar);

            foreach (char bannedChar in banned)
                result.RemoveAll(c => c == bannedChar);


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