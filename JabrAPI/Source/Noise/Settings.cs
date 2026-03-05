using System;



namespace JabrAPI.Noise
{
    abstract public class INoiseSettings
    {
        public Int32 outputLength = 0;

        public bool useDynamicOutputAlignment = true;

        public bool forceOptimalEntropy = true;
        public ExpectedEntropy excpectedEntropy = ExpectedEntropy.C1_Medium;


        public double primaryNoiseBiasPercents         = 50.0;
        public double complexNoisePairBiasPercents     = 25.0;
        public double complexNoiseIntervalBiasPercents = 66.6;


        public Int32 chunkSizeForSplitting = 256;
    }
    


    public class NoiseSettings : INoiseSettings
    {
        public TextOutputBoundaryAlignment boundaryAlignment =
               TextOutputBoundaryAlignment.c1024;
    }
    public class BinaryNoiseSettings : INoiseSettings
    {
        public BinaryOutputBoundaryAlignment boundaryAlignment =
               BinaryOutputBoundaryAlignment.KByte1;
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