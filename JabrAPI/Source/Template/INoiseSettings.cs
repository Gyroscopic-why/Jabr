using System;



namespace JabrAPI.Template
{
    public abstract class INoiseSettings(
        Int32 outputLength = 0,
        bool  useDynamicOutputAlignment = true,

        bool  forceOptimalEntropy = true,
        Noise.ExpectedEntropy expectedEntropy = Noise.ExpectedEntropy.C1_Medium,

        double primaryNoiseBiasPercents         = 50.0,
        double complexNoisePairBiasPercents     = 25.0,
        double complexNoiseIntervalBiasPercents = 66.6,

        Int32  chunkSizeForSplitting = 256)
    {
        public Int32 OutputLength              { get; set; } = outputLength;
        public bool  UseDynamicOutputAlignment { get; set; } = useDynamicOutputAlignment;


        public bool  ForceOptimalEntropy             { get; set; } = forceOptimalEntropy;
        public Noise.ExpectedEntropy ExpectedEntropy { get; set; } = expectedEntropy;


        public double PrimaryNoiseBiasPercents         { get; set; } = primaryNoiseBiasPercents;
        public double ComplexNoisePairBiasPercents     { get; set; } = complexNoisePairBiasPercents;
        public double ComplexNoiseIntervalBiasPercents { get; set; } = complexNoiseIntervalBiasPercents;


        public Int32  ChunkSizeForSplitting { get; set; } = chunkSizeForSplitting;
    }
}