using System;
using System.Linq;
using System.Collections.Generic;



namespace JabrAPI
{
    public partial class BinaryNoisifier
    {
        public Int32 PrimaryNoiseCount => _primaryNoise.Count;
        public Int32 ComplexNoiseCount => _complexNoise.Count;

        public List<Byte> PrimaryNoise => _primaryNoise;
        public List<Byte> ComplexNoise => _complexNoise;
        public List<Byte> Banned => _banned;

        public Byte RandomPrimaryByte => _primaryNoise[_random.Next(PrimaryNoiseCount)];
        public Byte RandomComplexByte => _complexNoise[_random.Next(ComplexNoiseCount)];


        public UInt64 RandomReseedInterval
        {
            get;
            set
            {
                field = value >= 128 ? value : 128;
                _random.UpdateReseedInterval(field);
            }
        }


        public List<Byte> RandomPrimarySequence(Int32 count) =>
            [..
                Enumerable.Range(0, count).Select
                (
                    _ => _primaryNoise[_random.Next(PrimaryNoiseCount)]
                )
            ];
        public List<Byte> RandomComplexSequence(Int32 count) =>
            [..
                Enumerable.Range(0, count).Select
                (
                    _ => _complexNoise[_random.Next(ComplexNoiseCount)]
                )
            ];
    }
}