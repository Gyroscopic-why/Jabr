using System;
using System.Linq;
using System.Collections.Generic;



namespace JabrAPI
{
    public partial class Noisifier
    {
        public Int32 PrimaryNoiseCount => _primaryNoise.Length;
        public Int32 ComplexNoiseCount => _complexNoise.Length;


        public string PrimaryNoise => _primaryNoise;
        public string ComplexNoise => _complexNoise;
        public List<char> Banned   => _banned;


        public char RandomPrimaryChar => _primaryNoise[_random.Next(PrimaryNoiseCount)];
        public char RandomComplexChar => _complexNoise[_random.Next(ComplexNoiseCount)];


        public string RandomPrimarySequence(Int32 count) =>
            string.Concat(
                Enumerable.Range(0, count).Select
                (
                    _ => _primaryNoise[_random.Next(PrimaryNoiseCount)]
                )
            );
        public string RandomComplexSequence(Int32 count) =>
            string.Concat(
                Enumerable.Range(0, count).Select
                (
                    _ => _complexNoise[_random.Next(ComplexNoiseCount)]
                )
            );
    }
}