using JabrAPI.Template;



namespace JabrAPI
{
    static public partial class Noise
    {
        public class Settings
        {
            public MasqueradePreset MasqueradePreset = MasqueradePreset.CUSTOM;
            public bool KeepOriginalFileExtension = true;

            public bool UseDynamicOutputIntervals = true;
            public OutputInterval[] DynamicOutputIntervals = [];
            public OutputInterval.IntervalFilters IntervalChoiceSetting = new();
            public OutputInterval.LengthChoiceSetting LengthChoiceSetting
                 = OutputInterval.LengthChoiceSetting.CHOOSE_RANDOM_FROM_VALID;

            public double OutputExtendingCoefficient = 1.8742;
            public bool DoExtendOutputIfLessThanInitial = true;
            public double HardChunkSizeToSoftCoefficient = 4.0;

            public bool ForceOptimalEntropy = true;
            public ExpectedEntropy ExpectedEntropy = ExpectedEntropy.C1_Medium;

            public double PrimaryNoiseBiasPercents = 50.0;
            public double ComplexNoisePairBiasPercents = 25.0;
            public double ComplexNoiseIntervalBiasPercents = 66.66;

            public bool ForceFullBoundary = false;
            public DynamicBoundaryOffset DynamicBoundaryOffset = DynamicBoundaryOffset.Minimize;
            public ChunkSize ChunkSize { get; set; } = ChunkSize.KByte16;



            public Settings(MasqueradePreset masqueradePreset)
                => CopyFrom(masqueradePreset);
            public Settings(
                MasqueradePreset masqueradePreset = MasqueradePreset.CUSTOM,
                bool keepOriginalFileExtension = true,

                bool useDynamicOutputIntervals = true,
                OutputInterval[]? dynamicOutputIntervals = null,
                OutputInterval.IntervalFilters? intervalChoiceSetting = null,
                OutputInterval.LengthChoiceSetting lengthChoiceSetting
                = OutputInterval.LengthChoiceSetting.CHOOSE_RANDOM_FROM_VALID,

                double outputExtendingCoefficient = 1.8742,
                bool doExtendOutputIfLessThanInitial = true,
                double hardChunkSizeToSoftCoefficient = 4.0,

                bool forceOptimalEntropy = true,
                ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

                double primaryNoiseBiasPercents = 50.0,
                double complexNoisePairBiasPercents = 25.0,
                double complexNoiseIntervalBiasPercents = 66.66,

                bool forceFullBoundary = false,
                DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize,
                ChunkSize chunkSize = ChunkSize.KByte16)
                => CopyFrom(masqueradePreset,
                        keepOriginalFileExtension,
                    useDynamicOutputIntervals,
                    dynamicOutputIntervals,
                    intervalChoiceSetting,
                    lengthChoiceSetting,
                        outputExtendingCoefficient,
                        doExtendOutputIfLessThanInitial,
                        hardChunkSizeToSoftCoefficient,
                    forceOptimalEntropy,
                    expectedEntropy,
                        primaryNoiseBiasPercents,
                        complexNoisePairBiasPercents,
                        complexNoiseIntervalBiasPercents,
                    forceFullBoundary,
                    dynamicBoundaryOffset,
                    chunkSize);



            public void InitFromPreset(MasqueradePreset masqueradePreset)
            {
                Settings preset = masqueradePreset switch
                {
                    MasqueradePreset.DEFAULT => SettingsPresets.DEFAULT,

                    MasqueradePreset.HTTPS_TLS => SettingsPresets.HTTPS_TLS,
                    MasqueradePreset.HTTPS_DNS => SettingsPresets.HTTPS_DNS,

                    MasqueradePreset.HTTP_3_QUIC => SettingsPresets.HTTP_3_QUIC,
                    MasqueradePreset.HTTP_2_gRPC => SettingsPresets.HTTP_2_gRPC,

                    MasqueradePreset.WEBSOCKET_WSS => SettingsPresets.WEBSOCKET_WSS,
                    MasqueradePreset.CUSTOM or _ => this
                };

                CopyFrom(preset);
            }



            public void CopyFrom(Settings initial)
                => CopyFrom(initial.MasqueradePreset,
                        initial.KeepOriginalFileExtension,
                    initial.UseDynamicOutputIntervals,
                    initial.DynamicOutputIntervals,
                    initial.IntervalChoiceSetting,
                    initial.LengthChoiceSetting,
                        initial.OutputExtendingCoefficient,
                        initial.DoExtendOutputIfLessThanInitial,
                        initial.HardChunkSizeToSoftCoefficient,
                    initial.ForceOptimalEntropy,
                    initial.ExpectedEntropy,
                        initial.PrimaryNoiseBiasPercents,
                        initial.ComplexNoisePairBiasPercents,
                        initial.ComplexNoiseIntervalBiasPercents,
                    initial.ForceFullBoundary,
                    initial.DynamicBoundaryOffset,
                    initial.ChunkSize);
            public void CopyFrom
            (
                MasqueradePreset masqueradePreset = MasqueradePreset.CUSTOM,
                bool keepOriginalFileExtension = true,

                bool useDynamicOutputIntervals = true,
                OutputInterval[]? dynamicOutputIntervals = null,
                OutputInterval.IntervalFilters? intervalChoiceSetting = null,
                OutputInterval.LengthChoiceSetting lengthChoiceSetting
                = OutputInterval.LengthChoiceSetting.CHOOSE_RANDOM_FROM_VALID,

                double outputExtendingCoefficient = 1.8742,
                bool doExtendOutputIfLessThanInitial = true,
                double hardChunkSizeToSoftCoefficient = 4.0,

                bool forceOptimalEntropy = true,
                ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

                double primaryNoiseBiasPercents = 50.0,
                double complexNoisePairBiasPercents = 25.0,
                double complexNoiseIntervalBiasPercents = 66.6,

                bool forceFullBoundary = false,
                DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize,
                ChunkSize chunkSize = ChunkSize.KByte16
            )
            {
                MasqueradePreset = masqueradePreset;
                KeepOriginalFileExtension = keepOriginalFileExtension;

                UseDynamicOutputIntervals = useDynamicOutputIntervals;
                DynamicOutputIntervals = [.. dynamicOutputIntervals ?? []];
                IntervalChoiceSetting = intervalChoiceSetting ?? new();
                LengthChoiceSetting = lengthChoiceSetting;

                OutputExtendingCoefficient = outputExtendingCoefficient;
                DoExtendOutputIfLessThanInitial = doExtendOutputIfLessThanInitial;
                HardChunkSizeToSoftCoefficient = hardChunkSizeToSoftCoefficient;

                ForceOptimalEntropy = forceOptimalEntropy;
                ExpectedEntropy = expectedEntropy;

                PrimaryNoiseBiasPercents = primaryNoiseBiasPercents;
                ComplexNoisePairBiasPercents = complexNoisePairBiasPercents;
                ComplexNoiseIntervalBiasPercents = complexNoiseIntervalBiasPercents;

                ForceFullBoundary = forceFullBoundary;
                DynamicBoundaryOffset = dynamicBoundaryOffset;
                ChunkSize = chunkSize;
            }
        }
    }
}