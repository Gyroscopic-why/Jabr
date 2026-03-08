using System;


using JabrAPI.Template;



namespace JabrAPI.Noise
{
    public class NoiseSettings(
            Int32 outputLength = 0,
            bool  useDynamicOutputAlignment = true,

            bool  forceOptimalEntropy = true,
            ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

            double primaryNoiseBiasPercents         = 50.0,
            double complexNoisePairBiasPercents     = 25.0,
            double complexNoiseIntervalBiasPercents = 66.6,

            Int32 chunkSizeForSplitting = 64,
            TextOutputBoundaryAlignment boundaryAlignment
                = TextOutputBoundaryAlignment.c256
        ) : INoiseSettings (
            outputLength,
            useDynamicOutputAlignment,

            forceOptimalEntropy,
            expectedEntropy,

            primaryNoiseBiasPercents,
            complexNoisePairBiasPercents,
            complexNoiseIntervalBiasPercents,

            chunkSizeForSplitting
    ) {
        public TextOutputBoundaryAlignment BoundaryAlignment
            { get; set; } = boundaryAlignment;
    }


    public class BinaryNoiseSettings(
        Int32 outputLength = 0,
        bool useDynamicOutputAlignment = true,

        bool forceOptimalEntropy = true,
        ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

        double primaryNoiseBiasPercents = 50.0,
        double complexNoisePairBiasPercents = 25.0,
        double complexNoiseIntervalBiasPercents = 66.6,

        Int32 chunkSizeForSplitting = 256,
        BinaryOutputBoundaryAlignment boundaryAlignment
            = BinaryOutputBoundaryAlignment.KByte1
        ) : INoiseSettings (
            outputLength,
            useDynamicOutputAlignment,

            forceOptimalEntropy,
            expectedEntropy,

            primaryNoiseBiasPercents,
            complexNoisePairBiasPercents,
            complexNoiseIntervalBiasPercents,

            chunkSizeForSplitting
    ) {
        public BinaryOutputBoundaryAlignment BoundaryAlignment
            { get; set; } = boundaryAlignment;
    }



    public enum BinaryOutputBoundaryAlignment
    {
        Byte32   = 5,
        Byte64   = 6,
        Byte128  = 7,
        Byte256  = 8,
        Byte512  = 9,
        
        KByte1   = 10,
        KByte2   = 11,
        KByte4   = 12,
        KByte8   = 13,
        KByte16  = 14,

        KByte32  = 15,
        KByte64  = 16,
        KByte128 = 17,
        KByte256 = 18,
        KByte512 = 19,

        MByte1   = 20
    }
    public enum TextOutputBoundaryAlignment
    {
        c32  = 5,
        c64  = 6,
        c128 = 7,
        c256 = 8,
        c512 = 9,

        c1024 = 10,
        c2048 = 11,
        c4096 = 12,
        c8192 = 13,

        c16384  = 14,
        c32768  = 15,
        c65536  = 16,
        c131072 = 17,
        c262144 = 18,
        c524288 = 19,

        c1048576 = 20
    }


    public enum ExpectedEntropy
    {
        L0_Fast_Anything,
        L1_Fast_Low,
        L2_Fast_Low,
        
        C0_Medium,
        C1_Medium,
        C2_Medium,
        
        H0_Slow_High,
        H1_Slow_High,
        H2_Slow_Maximal,
    }
}