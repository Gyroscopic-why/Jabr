using System;
using System.Text;
using System.Collections.Generic;



namespace JabrAPI
{
    public partial class Noisifier
    {
        public void Next(bool throwExceptions = true)
        {
            try { GenerateAll(); }
            catch { if (throwExceptions) throw; }
        }
        public void Next(List<char> bannedForFailsafeRegeneration,
            bool resetSettingsToDefaultIfFailed = true, bool throwExceptions = true)
        {
            try
            {
                GenerateAll();
            }
            catch
            {
                if (resetSettingsToDefaultIfFailed)
                {
                    try
                    {
                        DefaultGenerate(bannedForFailsafeRegeneration);
                    }
                    catch
                    {
                        if (throwExceptions) throw;
                    }
                }
                else if (throwExceptions) throw;
            }
        }



        private void GenerateAll()
        {
            GeneratePrimary(false);
            GenerateComplex(true);
        }
        public void DefaultGenerate(List<char> banned)
        {
            Set.Default(banned);
            GenerateAll();
        }



        public string GenerateNoise(Int32 count, List<char> allowed)
        {
            if (count <= 0) return string.Empty;
            if (count > allowed.Count) throw new ArgumentOutOfRangeException
                (
                    $"Count is greater than max possible length: {allowed.Count}"
                );

            StringBuilder result = new(count);
            Int32 totalCount = allowed.Count;

            for (var lastUsedId = 0; lastUsedId < count; lastUsedId++)
            {
                Int32 chosenUnused = _random.Next(lastUsedId, totalCount);

                (allowed[lastUsedId], allowed[chosenUnused]) =
                (allowed[chosenUnused], allowed[lastUsedId]);

                result.Append(allowed[lastUsedId]);
            }

            return result.ToString();
        }



        public void GeneratePrimary(List<char> banned, bool banAlreadyUsedInComplex)
        {
            //  Important to get allowed separately from GenerateNoise
            //  because _primaryCount can change here in default fail case
            _primaryNoise = "";
            List<char> allowed = GetRemainingAllowed(banned, _primaryCount, banAlreadyUsedInComplex);

            _primaryNoise = GenerateNoise(_primaryCount, allowed);
        }
        public void GeneratePrimary(bool banAlreadyUsedInComplex)
            => GeneratePrimary(_banned, banAlreadyUsedInComplex);


        public void GenerateComplex(List<char> banned, bool banAlreadyUsedInPrimary)
        {
            //  Important to get allowed separately from GenerateNoise
            //  because _complexCount can change here in default fail case
            _complexNoise = "";
            List<char> allowed = GetRemainingAllowed(banned, _complexCount, banAlreadyUsedInPrimary);

            _complexNoise = GenerateNoise(_complexCount, allowed);
        }
        public void GenerateComplex(bool banAlreadyUsedInPrimary)
            => GenerateComplex(_banned, banAlreadyUsedInPrimary);
    }
}