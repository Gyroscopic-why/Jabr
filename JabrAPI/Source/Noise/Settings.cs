using System;


using JabrAPI.Template;



namespace JabrAPI
{
    static public partial class Noise
    {
        public class Settings(
            Int32  outputLength                   = 0,
            bool   useDynamicOutputAlignment      = true,
            double hardChunkSizeToSoftCoefficient = 4.0,

            bool   keepOriginalFileExtension       = true,
            UInt64 reseedRandomAfterBytesGenerated = 128,

            bool   forceOptimalEntropy      = true,
            ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

            double primaryNoiseBiasPercents         = 50.0,
            double complexNoisePairBiasPercents     = 25.0,
            double complexNoiseIntervalBiasPercents = 66.6,

            bool   forceFullBoundary                    = false,
            DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize,
            TextChunkSize chunkSize                     = TextChunkSize.c4096,
            TextOutputBoundaryAlignment boundaryAlignment
                                                        = TextOutputBoundaryAlignment.c256
            ) : INoiseSettings(
                outputLength,
                useDynamicOutputAlignment,
                hardChunkSizeToSoftCoefficient,

                keepOriginalFileExtension,
                reseedRandomAfterBytesGenerated,

                forceOptimalEntropy,
                expectedEntropy,

                primaryNoiseBiasPercents,
                complexNoisePairBiasPercents,
                complexNoiseIntervalBiasPercents,

                forceFullBoundary,
                dynamicBoundaryOffset
            )
        {
            public TextOutputBoundaryAlignment BoundaryAlignment
            { get; set; } = boundaryAlignment;

            public TextChunkSize ChunkSize { get; set; } = chunkSize;
        }



        public class BinarySettings(
            Int32  outputLength                   = 0,
            bool   useDynamicOutputAlignment      = true,
            double hardChunkSizeToSoftCoefficient = 4.0,

            bool   keepOriginalFileExtension       = true,
            UInt64 reseedRandomAfterBytesGenerated = 128,

            bool   forceOptimalEntropy      = true,
            ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

            double primaryNoiseBiasPercents         = 50.0,
            double complexNoisePairBiasPercents     = 25.0,
            double complexNoiseIntervalBiasPercents = 66.6,

            bool forceFullBoundary                      = false,
            DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize,
            BinaryChunkSize chunkSize                   = BinaryChunkSize.KByte8,
            BinaryOutputBoundaryAlignment boundaryAlignment
                                                        = BinaryOutputBoundaryAlignment.KByte1
            ) : INoiseSettings(
                outputLength,
                useDynamicOutputAlignment,
                hardChunkSizeToSoftCoefficient,

                keepOriginalFileExtension,
                reseedRandomAfterBytesGenerated,

                forceOptimalEntropy,
                expectedEntropy,

                primaryNoiseBiasPercents,
                complexNoisePairBiasPercents,
                complexNoiseIntervalBiasPercents,

                forceFullBoundary,
                dynamicBoundaryOffset
            )
        {
            public BinaryOutputBoundaryAlignment BoundaryAlignment
            { get; set; } = boundaryAlignment;

            public BinaryChunkSize ChunkSize { get; set; } = chunkSize;
        }



        public enum BinaryOutputBoundaryAlignment
        {
            Byte32 = 5,
            Byte64 = 6,
            Byte128 = 7,
            Byte256 = 8,
            Byte512 = 9,

            KByte1 = 10,
            KByte2 = 11,
            KByte4 = 12,
            KByte8 = 13,
            KByte16 = 14,

            KByte32 = 15,
            KByte64 = 16,
            KByte128 = 17,
            KByte256 = 18,
            KByte512 = 19,

            MByte1 = 20
        }
        public enum TextOutputBoundaryAlignment
        {
            c32 = 5,
            c64 = 6,
            c128 = 7,
            c256 = 8,
            c512 = 9,

            c1024 = 10,
            c2048 = 11,
            c4096 = 12,
            c8192 = 13,

            c16384 = 14,
            c32768 = 15,
            c65536 = 16,
            c131072 = 17,
            c262144 = 18,
            c524288 = 19,

            c1048576 = 20
        }

        public enum DynamicBoundaryOffset
        {
            Minimize = 0,

            x2 = 1,
            x4 = 2,
            x8 = 3,

            x16 = 4,
            x32 = 5,
            x64 = 6,

            x128 = 7,
            x256 = 8,
            x512 = 9,

            x1024 = 10,
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
}