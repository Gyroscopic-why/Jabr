using System;



namespace JabrAPI.Template
{
    abstract public class INoiseSettings(
        Int32  outputLength = 0,
        bool   useDynamicOutputAlignment = true,
        double hardChunkSizeToSoftCoefficient = 4.0,

        bool   keepOriginalFileExtension = true,
        UInt64 reseedRandomAfterBytesGenerated = 128,

        bool   forceOptimalEntropy = true,
        Noise.ExpectedEntropy expectedEntropy = Noise.ExpectedEntropy.C1_Medium,

        double primaryNoiseBiasPercents         = 50.0,
        double complexNoisePairBiasPercents     = 25.0,
        double complexNoiseIntervalBiasPercents = 66.6,

        bool  forceFullBoundary = false,
        Noise.DynamicBoundaryOffset dynamicBoundaryOffset = Noise.DynamicBoundaryOffset.Minimize)
    {
        public Int32  OutputLength                     { get; set; } = outputLength;
        public bool   UseDynamicOutputAlignment        { get; set; } = useDynamicOutputAlignment;
        public double HardChunkSizeToSoftCoefficient   { get; set; } = hardChunkSizeToSoftCoefficient;


        public bool   KeepOriginalFileExtension        { get; set; } = keepOriginalFileExtension;
        public UInt64 ReseedRandomAfterBytesGenerated  { get; set; } = reseedRandomAfterBytesGenerated;


        public bool   ForceOptimalEntropy              { get; set; } = forceOptimalEntropy;
        public Noise.ExpectedEntropy ExpectedEntropy   { get; set; } = expectedEntropy;


        public double PrimaryNoiseBiasPercents         { get; set; } = primaryNoiseBiasPercents;
        public double ComplexNoisePairBiasPercents     { get; set; } = complexNoisePairBiasPercents;
        public double ComplexNoiseIntervalBiasPercents { get; set; } = complexNoiseIntervalBiasPercents;


        public bool   ForceFullBoundary                          { get; set; } = forceFullBoundary;
        public Noise.DynamicBoundaryOffset DynamicBoundaryOffset { get; set; } = dynamicBoundaryOffset;
    }
}