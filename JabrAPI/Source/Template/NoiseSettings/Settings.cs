using System;


using JabrAPI.Template;



namespace JabrAPI
{
    static public partial class Noise
    {
        public class Settings(
            MasqueradePreset masqueradePreset = MasqueradePreset.CUSTOM,

            Int32 outputLength = 0,
            bool useDynamicOutputIntervals = true,
            OutputInterval[]? dynamicOutputIntervals = null,

            bool doExtendOutputIfLessThanInitial = true,
            double dynamicOutputNoiseCoefficient = 1.8742,
            double hardChunkSizeToSoftCoefficient = 4.0,

            bool keepOriginalFileExtension = true,

            bool forceOptimalEntropy = true,
            ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

            double primaryNoiseBiasPercents = 50.0,
            double complexNoisePairBiasPercents = 25.0,
            double complexNoiseIntervalBiasPercents = 66.66,

            bool forceFullBoundary = false,
            DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize,
            TextChunkSize chunkSize = TextChunkSize.c4096,
            TextBoundaryAlignment boundaryAlignment = TextBoundaryAlignment.c256
            ) : INoiseSettings(
                masqueradePreset,

                outputLength,
                useDynamicOutputIntervals,
                dynamicOutputIntervals,

                dynamicOutputNoiseCoefficient,
                doExtendOutputIfLessThanInitial,
                hardChunkSizeToSoftCoefficient,

                keepOriginalFileExtension,

                forceOptimalEntropy,
                expectedEntropy,

                primaryNoiseBiasPercents,
                complexNoisePairBiasPercents,
                complexNoiseIntervalBiasPercents,

                forceFullBoundary,
                dynamicBoundaryOffset
            )
        {
            public TextBoundaryAlignment BoundaryAlignment
            { get; set; } = boundaryAlignment;

            public TextChunkSize ChunkSize { get; set; } = chunkSize;
        }



        public class BinarySettings(
            MasqueradePreset masqueradePreset = MasqueradePreset.CUSTOM,

            Int32 outputLength = 0,
            bool useDynamicOutputIntervals = true,
            OutputInterval[]? dynamicOutputIntervals = null,

            bool doExtendOutputIfLessThanInitial = true,
            double dynamicOutputNoiseCoefficient = 1.8742,
            double hardChunkSizeToSoftCoefficient = 4.0,

            bool keepOriginalFileExtension = true,

            bool forceOptimalEntropy = true,
            ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

            double primaryNoiseBiasPercents = 50.0,
            double complexNoisePairBiasPercents = 25.0,
            double complexNoiseIntervalBiasPercents = 66.66,

            bool forceFullBoundary = false,
            DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize,
            BinaryChunkSize chunkSize = BinaryChunkSize.KByte8,
            BinaryBoundaryAlignment boundaryAlignment = BinaryBoundaryAlignment.KByte1
            ) : INoiseSettings(
                masqueradePreset,

                outputLength,
                useDynamicOutputIntervals,
                dynamicOutputIntervals,

                dynamicOutputNoiseCoefficient,
                doExtendOutputIfLessThanInitial,
                hardChunkSizeToSoftCoefficient,

                keepOriginalFileExtension,

                forceOptimalEntropy,
                expectedEntropy,

                primaryNoiseBiasPercents,
                complexNoisePairBiasPercents,
                complexNoiseIntervalBiasPercents,

                forceFullBoundary,
                dynamicBoundaryOffset
            )
        {
            public BinaryBoundaryAlignment BoundaryAlignment
            { get; set; } = boundaryAlignment;

            public BinaryChunkSize ChunkSize { get; set; } = chunkSize;
        }
    }
}