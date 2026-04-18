using System;
using System.Collections.Generic;



namespace JabrAPI
{
    public partial class BinaryNoisifier
    {
        public void Next(bool throwExceptions = true)
        {
            try { GenerateAll(); }
            catch { if (throwExceptions) throw; }
        }
        public void Next(List<Byte> bannedForFailsafeRegeneration,
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
        public void DefaultGenerate(List<Byte> banned)
        {
            Set.Default(banned);
            GenerateAll();
        }



        public List<Byte> GenerateNoise(Byte count, List<Byte> allowed)
        {
            if (count <= 0) return [];
            if (count > allowed.Count) throw new ArgumentOutOfRangeException
                (
                    $"Count is greater than max possible length: {allowed.Count}"
                );

            List<Byte> result = new(count);
            Int32  totalCount = allowed.Count;

            for (var lastUsedId = 0; lastUsedId < count; lastUsedId++)
            {
                Int32 chosenUnused = _random.Next(lastUsedId, totalCount);

                (allowed[chosenUnused], allowed[lastUsedId]) =
                (allowed[lastUsedId], allowed[chosenUnused]);

                result.Add(allowed[lastUsedId]);
            }

            return result;
        }



        public void GeneratePrimary(List<Byte> banned, bool banAlreadyUsedInComplex)
        {
            //  Important to get allowed separately from GenerateNoise
            //  because _primaryCount can change here in default fail case
            _primaryNoise.Clear();
            List<Byte> allowed = GetRemainingAllowed(banned, _primaryCount, banAlreadyUsedInComplex);

            _primaryNoise.AddRange(GenerateNoise(_primaryCount, allowed));
        }
        public void GeneratePrimary(bool banAlreadyUsedInComplex)
             => GeneratePrimary(_banned, banAlreadyUsedInComplex);


        public void GenerateComplex(List<Byte> banned, bool banAlreadyUsedInPrimary)
        {
            //  Important to get allowed separately from GenerateNoise
            //  because _complexCount can change here in default fail case
            _complexNoise.Clear();
            List<Byte> allowed = GetRemainingAllowed(banned, _complexCount, banAlreadyUsedInPrimary);

            _complexNoise.AddRange(GenerateNoise(_complexCount, allowed));
        }
        public void GenerateComplex(bool banAlreadyUsedInPrimary)
             => GenerateComplex(_banned, banAlreadyUsedInPrimary);
    }
}