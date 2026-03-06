using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
        private protected override void GenerateAll()
        {
            GenerateRandomPrimary();
            GenerateRandomExternal();
            GenerateRandomShifts();

            _noisifier.SetDefault(External);
            _noisifier.Next(false);
        }



        private List<Byte> GenerateRandomAlphabet(Int32 length)
        {
            List<Byte> remainingChoices = [.. DEFAULT.BYTES];
            List<Byte> resultAlphabet = [];

            for (var remaining = 0; remaining < length; remaining++)
            {
                Byte maxValueInclusive = (Byte)Math.Min(255, remainingChoices.Count - 1);
                var chosen = _random.Next(maxValueInclusive);
                var chosenId = _random.Next(resultAlphabet.Count);

                resultAlphabet.Insert(chosenId, remainingChoices[chosen]);
                remainingChoices.RemoveAt(chosen);
            }
            return resultAlphabet;
        }


        public void GenerateRandomPrimary(Byte compactedLength_willBeIncreasedByOne = 255)
        {
            if (compactedLength_willBeIncreasedByOne < 1)
                throw new ArgumentOutOfRangeException
                (
                    "Provided PrimaryAlphabet length must be in 1-255 range" +
                    "\nIt will later be converted from a 1-255 range to a 2-256",
                    nameof(compactedLength_willBeIncreasedByOne)
                );

            _primaryAlphabet.Clear();
            _primaryAlphabet.AddRange(GenerateRandomAlphabet(compactedLength_willBeIncreasedByOne + 1));
        }
        public void GenerateRandomPrimary()
        {
            _primaryAlphabet.Clear();

            _primaryAlphabet.AddRange(_compactedPrMaxLength > 0
                ? GenerateRandomAlphabet(_compactedPrMaxLength + 1)
                : GenerateRandomAlphabet(255));
        }

        public void GenerateRandomExternal(Byte compactedLength_willBeIncreasedByOne = 255)
        {
            if (compactedLength_willBeIncreasedByOne < 1)
                throw new ArgumentOutOfRangeException
                (
                    "Provided PrimaryAlphabet length must be in 1-255 range" +
                    "\nIt will later be converted from a 1-255 range to a 2-256",
                    nameof(compactedLength_willBeIncreasedByOne)
                );

            _externalAlphabet.Clear();
            _externalAlphabet.AddRange(GenerateRandomAlphabet(compactedLength_willBeIncreasedByOne + 1));
        }
        public void GenerateRandomExternal()
        {
            _externalAlphabet.Clear();

            _externalAlphabet.AddRange(_compactedExMaxLength > 0
                ? GenerateRandomAlphabet(_compactedExMaxLength + 1)
                : GenerateRandomAlphabet(8));
        }



        public void GenerateRandomShifts(Int32 count)
        {
            if (_externalAlphabet == null || _externalAlphabet.Count < 2)
            {
                throw new ArgumentException
                (
                    "Unable to generate shifts, external alphabet is undefined",
                    nameof(_externalAlphabet)
                );
            }

            GenerateRandomShifts(count, 0, (Byte)(_externalAlphabet.Count - 1));
        }
        public void GenerateRandomShifts()
            => GenerateRandomShifts(_random.Next(128, 384));
    }
}