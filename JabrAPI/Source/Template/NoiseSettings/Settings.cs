using System;


using JabrAPI.Template;



namespace JabrAPI
{
    static public partial class Noise
    {
        public class Settings(
            MasqueradePreset masqueradePreset = MasqueradePreset.CUSTOM,
            bool keepOriginalFileExtension = true,

            bool useDynamicOutputIntervals = true,
            OutputInterval[]? dynamicOutputIntervals = null,
            OutputInterval.IntervalFilters? intervalChoiceSetting = null,
            OutputInterval.LengthChoiceSetting lengthChoiceSetting
            = OutputInterval.LengthChoiceSetting.CHOOSE_RANDOM_FROM_VALID,

            bool doExtendOutputIfLessThanInitial = true,
            double dynamicOutputNoiseCoefficient = 1.8742,
            double hardChunkSizeToSoftCoefficient = 4.0,

            bool forceOptimalEntropy = true,
            ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

            double primaryNoiseBiasPercents = 50.0,
            double complexNoisePairBiasPercents = 25.0,
            double complexNoiseIntervalBiasPercents = 66.66,

            bool forceFullBoundary = false,
            DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize,
            TextChunkSize chunkSize = TextChunkSize.c4096
            ) : INoiseSettings(
                masqueradePreset,
                keepOriginalFileExtension,

                useDynamicOutputIntervals,
                dynamicOutputIntervals,
                intervalChoiceSetting,
                lengthChoiceSetting,

                dynamicOutputNoiseCoefficient,
                doExtendOutputIfLessThanInitial,
                hardChunkSizeToSoftCoefficient,

                forceOptimalEntropy,
                expectedEntropy,

                primaryNoiseBiasPercents,
                complexNoisePairBiasPercents,
                complexNoiseIntervalBiasPercents,

                forceFullBoundary,
                dynamicBoundaryOffset
        )   {
            public TextChunkSize ChunkSize { get; set; } = chunkSize;
        }



        public class BinarySettings(
            MasqueradePreset masqueradePreset = MasqueradePreset.CUSTOM,
            bool keepOriginalFileExtension = true,

            bool useDynamicOutputIntervals = true,
            OutputInterval[]? dynamicOutputIntervals = null,
            OutputInterval.IntervalFilters? intervalChoiceSetting = null,
            OutputInterval.LengthChoiceSetting lengthChoiceSetting
            = OutputInterval.LengthChoiceSetting.CHOOSE_RANDOM_FROM_VALID,

            bool doExtendOutputIfLessThanInitial = true,
            double dynamicOutputNoiseCoefficient = 1.8742,
            double hardChunkSizeToSoftCoefficient = 4.0,


            bool forceOptimalEntropy = true,
            ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

            double primaryNoiseBiasPercents = 50.0,
            double complexNoisePairBiasPercents = 25.0,
            double complexNoiseIntervalBiasPercents = 66.66,

            bool forceFullBoundary = false,
            DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize,
            BinaryChunkSize chunkSize = BinaryChunkSize.KByte8
            ) : INoiseSettings(
                masqueradePreset,
                keepOriginalFileExtension,

                useDynamicOutputIntervals,
                dynamicOutputIntervals,
                intervalChoiceSetting,
                lengthChoiceSetting,

                dynamicOutputNoiseCoefficient,
                doExtendOutputIfLessThanInitial,
                hardChunkSizeToSoftCoefficient,

                forceOptimalEntropy,
                expectedEntropy,

                primaryNoiseBiasPercents,
                complexNoisePairBiasPercents,
                complexNoiseIntervalBiasPercents,

                forceFullBoundary,
                dynamicBoundaryOffset
        )   {
            public BinaryChunkSize ChunkSize { get; set; } = chunkSize;
        }
    }
}