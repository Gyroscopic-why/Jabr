using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class EncryptionKey : IEncryptionKey
    {
        override private protected void GenerateAll()
        {
            GenerateRandomPrimary();
            GenerateRandomExternal();

            if (_shifts.Count < 2)
            {
                if (_shCount < 2) GenerateRandomShifts();
                else GenerateRandomShifts(_shCount);
            }
            else GenerateRandomShifts(_shifts.Count);

            _noisifier.Set.Default([.. External]);
            _noisifier.Next(false);
        }



        static private void ValidateForGeneration(List<char> necessary, List<char> allowed, Int32 maxLength)
        {
            necessary ??= [];
            allowed ??= [];

            Int32 nCount = necessary.Count, aCount = allowed.Count;

            for (var idN1 = 0; idN1 < nCount; idN1++)
            {
                for (var idN2 = idN1 + 1; idN2 < nCount; idN2++)
                {
                    if (necessary[idN1] == necessary[idN2])
                    {
                        throw new ArgumentException
                        (
                            $"Necessary list cannot include duplicates" +
                            $"\nDuplicate char: " + necessary[idN1],
                            nameof(necessary)
                        );
                    }
                }
            }

            for (var idA = 0; idA < aCount; idA++)
            {
                for (var idA2 = idA + 1; idA2 < aCount; idA2++)
                {
                    if (allowed[idA] == allowed[idA2])
                    {
                        throw new ArgumentException
                        (
                            $"Allowed list cannot include duplicates" +
                            $"\nDuplicate char: " + allowed[idA],
                            nameof(allowed)
                        );
                    }
                }
                for (var idN = 0; idN < nCount; idN++)
                {
                    if (allowed[idA] == necessary[idN])
                    {
                        throw new ArgumentException
                        (
                            $"Allowed list cannot include duplicates of Necessary list" +
                            $"\nDuplicate char: " + allowed[idA],
                            nameof(allowed) + " " + nameof(necessary)
                        );
                    }
                }
            }


            if (maxLength < 2)
            {
                throw new ArgumentException
                (
                    $"Max length is less than the smallest possible alphabet length (2)",
                    nameof(maxLength)
                );
            }
            maxLength -= nCount;
            if (maxLength < 0)
            {
                throw new ArgumentException
                (
                    $"Max length cannot be less than the number of necessary characters." +
                    $"\nmaxLength: {maxLength + nCount}, " +
                    $"Necessary count: {nCount}",
                    nameof(maxLength)
                );
            }
            else if (maxLength > aCount)
            {
                throw new ArgumentException
                (
                    $"Max length cannot be more than allowed characters count" +
                    $"\nmaxLength: {maxLength}, allowed count: {aCount}",
                    nameof(maxLength)
                );
            }

            if (nCount + aCount < 2)
            {
                throw new ArgumentException
                (
                    $"Not enough characters in necessary and allowed list for a valid key" +
                    $"\nneccessary.Count + allowed.Count: {nCount + aCount} < 2",
                    nameof(necessary) + " " + nameof(allowed)
                );
            }
        }



        public string GenerateRandomAlphabet(
            List<char> necessary, List<char> allowed,
            Int32 maxLength, bool validateParameters = true)
        {
            necessary ??= [];
            allowed ??= [];

            if (validateParameters)
                ValidateForGeneration(necessary, allowed, maxLength);

            Int32 chosenPosition, chosenUnused, nCount = necessary.Count, aCount = allowed.Count; ;
            maxLength = Math.Min(maxLength - nCount, allowed.Count);
            List<char> result = new(maxLength);

            for (var lastUsedId = 0; lastUsedId < nCount; lastUsedId++)
            {
                chosenUnused = _random.Next(lastUsedId, nCount);

                result.Add(necessary[chosenUnused]);

                (necessary[lastUsedId], necessary[chosenUnused]) =
                (necessary[chosenUnused], necessary[lastUsedId]);
            }

            for (var lastUsedId = 0; lastUsedId < maxLength; lastUsedId++)
            {
                chosenUnused = _random.Next(lastUsedId, aCount);
                chosenPosition = _random.Next(nCount + lastUsedId);

                result.Insert(chosenPosition, allowed[chosenUnused]);

                (allowed[lastUsedId], allowed[chosenUnused]) =
                (allowed[chosenUnused], allowed[lastUsedId]);
            }

            return new string([.. result]);
        }
        public string GenerateRandomAlphabet(
            Int32 maxLength, List<char> necessary,
            List<char> banned, bool validateParameters = true)
        {
            List<char> allowed = [.. DEFAULT.CHARACTERS.WITHOUT_SPACE];

            if (banned != null)
            {
                Int32 bCount = banned.Count;

                for (var curId = 0; curId < bCount; curId++)
                {
                    for (var id2 = 0; id2 < allowed.Count; id2++)
                    {
                        if (banned[curId] == allowed[id2])
                        {
                            allowed.RemoveAt(id2);
                            id2--;
                        }
                    }
                }
            }

            return GenerateRandomAlphabet(necessary, allowed, maxLength, validateParameters);
        }





        public void GenerateRandomPrimary(
            List<char> necessary, List<char> allowed,
            Int32 maxLength, bool validateParameters = true)
                => _primaryAlphabet = GenerateRandomAlphabet(
                        necessary, allowed,
                        maxLength, validateParameters);
        public void GenerateRandomPrimary(
            string necessary, string allowed,
            Int32 maxLength, bool validateParameters = true)
                => _primaryAlphabet = GenerateRandomAlphabet(
                        [.. necessary], [.. allowed],
                        maxLength, validateParameters);

        public void GenerateRandomPrimary(
            Int32 maxLength, List<char> necessary,
            List<char> banned, bool validateParameters = true)
                => _primaryAlphabet = GenerateRandomAlphabet(
                        maxLength, necessary,
                        banned, validateParameters);
        public void GenerateRandomPrimary(
            Int32 maxLength, string necessary,
            string banned, bool validateParameters = true)
                => _primaryAlphabet = GenerateRandomAlphabet(
                        maxLength, [.. necessary],
                        [.. banned], validateParameters);

        public void GenerateRandomPrimary(
            Int32 maxLength, bool validateParameters = true)
                => _primaryAlphabet = GenerateRandomAlphabet(
                        maxLength, [], [], validateParameters);
        public void GenerateRandomPrimary()
        {
            if (_primaryAllowed != null && _primaryAllowed.Count > 0)
            {
                List<char> allowed = [.. _primaryAllowed];

                if (_primaryBanned != null && _primaryBanned.Count > 0)
                {
                    for (var curId = 0; curId < _primaryBanned.Count; curId++)
                    {
                        for (var id2 = 0; id2 < allowed.Count; id2++)
                        {
                            if (_primaryBanned[curId] == allowed[id2])
                            {
                                allowed.RemoveAt(id2);
                                id2--;
                            }
                        }
                    }
                }
                _primaryAlphabet = GenerateRandomAlphabet(_primaryNecessary, allowed, _primaryMaxLength);
            }
            else _primaryAlphabet = GenerateRandomAlphabet(_primaryNecessary, [], _primaryNecessary.Count);
        }





        public void GenerateRandomExternal(
            List<char> necessary, List<char> allowed,
            Int32 maxLength, bool validateParameters = true)
                => _externalAlphabet = GenerateRandomAlphabet(
                        necessary, allowed,
                        maxLength, validateParameters);
        public void GenerateRandomExternal(
            string necessary, string allowed,
            Int32 maxLength, bool validateParameters = true)
                => _externalAlphabet = GenerateRandomAlphabet(
                        [.. necessary], [.. allowed],
                        maxLength, validateParameters);

        public void GenerateRandomExternal(
            Int32 maxLength, List<char> necessary,
            List<char> banned, bool validateParameters = true)
                => _externalAlphabet = GenerateRandomAlphabet(
                        maxLength, necessary,
                        banned, validateParameters);
        public void GenerateRandomExternal(
            Int32 maxLength, string necessary,
            string banned, bool validateParameters = true)
                => _externalAlphabet = GenerateRandomAlphabet(
                        maxLength, [.. necessary],
                        [.. banned], validateParameters);

        public void GenerateRandomExternal(
            Int32 maxLength, bool validateParameters = true)
                => _externalAlphabet = GenerateRandomAlphabet(
                        maxLength, [], [], validateParameters);
        public void GenerateRandomExternal()
        {
            if (_externalAllowed != null && _externalAllowed.Count > 0)
            {
                List<char> allowed = [.. _externalAllowed];

                if (_externalBanned != null && _externalBanned.Count > 0)
                {
                    for (var curId = 0; curId < _externalBanned.Count; curId++)
                    {
                        for (var id2 = 0; id2 < allowed.Count; id2++)
                        {
                            if (_externalBanned[curId] == allowed[id2])
                            {
                                allowed.RemoveAt(id2);
                                id2--;
                            }
                        }
                    }
                }
                _externalAlphabet = GenerateRandomAlphabet(_externalNecessary, allowed, _externalMaxLength);
            }
            else _externalAlphabet = GenerateRandomAlphabet(_externalNecessary, [], _externalNecessary.Count);
        }



        public void GenerateRandomShifts(Int32 count)
        {
            if (_externalAlphabet == null || _externalAlphabet.Length < 2)
            {
                throw new ArgumentException
                (
                    "Unable to generate shifts, external alphabet is undefined",
                    nameof(_externalAlphabet)
                );
            }

            GenerateRandomShifts(count, 0, (Int16)(_externalAlphabet.Length - 1));
        }
        public void GenerateRandomShifts()
                => GenerateRandomShifts(_random.Next(128, 384));
    }
}