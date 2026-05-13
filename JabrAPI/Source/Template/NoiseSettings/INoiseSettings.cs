using System;



namespace JabrAPI.Template
{    
    public class INoiseSettings
    {
        public MasqueradePreset MasqueradePreset = MasqueradePreset.CUSTOM;

        public Int32  OutputLength = 0;
        public bool   UseDynamicOutputIntervals = true;
        public OutputInterval[] DynamicOutputIntervals = [];

        public double OutputExtendingCoefficient = 1.8742;
        public bool   DoExtendOutputIfLessThanInitial = true;
        public double HardChunkSizeToSoftCoefficient = 4.0;

        public bool   KeepOriginalFileExtension = true;

        public bool   ForceOptimalEntropy = true;
        public ExpectedEntropy ExpectedEntropy = ExpectedEntropy.C1_Medium;

        public double PrimaryNoiseBiasPercents = 50.0;
        public double ComplexNoisePairBiasPercents = 25.0;
        public double ComplexNoiseIntervalBiasPercents = 66.66;

        public bool   ForceFullBoundary = false;
        public DynamicBoundaryOffset DynamicBoundaryOffset = DynamicBoundaryOffset.Minimize;



        public INoiseSettings(MasqueradePreset masqueradePreset)
            => CopyFrom(masqueradePreset);
        public INoiseSettings(
            MasqueradePreset masqueradePreset = MasqueradePreset.CUSTOM,
            
            Int32 outputLength = 0,
            bool  useDynamicOutputIntervals = true,
            OutputInterval[]? dynamicOutputIntervals = null,
            
            double outputExtendingCoefficient = 1.8742,
            bool   doExtendOutputIfLessThanInitial = true,
            double hardChunkSizeToSoftCoefficient  = 4.0,
            
            bool keepOriginalFileExtension = true,
            
            bool forceOptimalEntropy = true,
            ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,
            
            double primaryNoiseBiasPercents = 50.0,
            double complexNoisePairBiasPercents = 25.0,
            double complexNoiseIntervalBiasPercents = 66.66,
            
            bool forceFullBoundary = false,
            DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize)
            => CopyFrom(masqueradePreset,
                outputLength,
                useDynamicOutputIntervals,
                dynamicOutputIntervals,
                    outputExtendingCoefficient,
                    doExtendOutputIfLessThanInitial,
                    hardChunkSizeToSoftCoefficient,
                keepOriginalFileExtension,
                    forceOptimalEntropy,
                    expectedEntropy,
                primaryNoiseBiasPercents,
                complexNoisePairBiasPercents,
                complexNoiseIntervalBiasPercents,
                    forceFullBoundary,
                    dynamicBoundaryOffset);
        


        public void InitFromPreset(MasqueradePreset masqueradePreset)
        {
            INoiseSettings preset = masqueradePreset switch
            {
                MasqueradePreset.DEFAULT => SettingsPresets.DEFAULT,

                MasqueradePreset.HTTPS_TLS => SettingsPresets.HTTPS_TLS,
                MasqueradePreset.HTTPS_DNS => SettingsPresets.HTTPS_DNS,

                MasqueradePreset.HTTP_3_QUIC => SettingsPresets.HTTP_3_QUIC,
                MasqueradePreset.HTTP_2_gRPC => SettingsPresets.HTTP_2_gRPC,

                MasqueradePreset.WEBSOCKET_WSS => SettingsPresets.WEBSOCKET_WSS,
                MasqueradePreset.CUSTOM or _   => this
            };

            CopyFrom(preset);
        }



        public void CopyFrom(INoiseSettings initial)
            => CopyFrom(initial.MasqueradePreset,
                initial.OutputLength,
                initial.UseDynamicOutputIntervals,
                initial.DynamicOutputIntervals,
                    initial.OutputExtendingCoefficient,
                    initial.DoExtendOutputIfLessThanInitial,
                    initial.HardChunkSizeToSoftCoefficient,
                initial.KeepOriginalFileExtension,
                    initial.ForceOptimalEntropy,
                    initial.ExpectedEntropy,
                initial.PrimaryNoiseBiasPercents,
                initial.ComplexNoisePairBiasPercents,
                initial.ComplexNoiseIntervalBiasPercents,
                    initial.ForceFullBoundary,
                    initial.DynamicBoundaryOffset);
        public void CopyFrom
        (
            MasqueradePreset masqueradePreset = MasqueradePreset.CUSTOM,

            Int32 outputLength = 0,
            bool useDynamicOutputIntervals = true,
            OutputInterval[]? dynamicOutputIntervals = null,

            double outputExtendingCoefficient = 1.8742,
            bool doExtendOutputIfLessThanInitial = true,
            double hardChunkSizeToSoftCoefficient = 4.0,

            bool keepOriginalFileExtension = true,

            bool forceOptimalEntropy = true,
            ExpectedEntropy expectedEntropy = ExpectedEntropy.C1_Medium,

            double primaryNoiseBiasPercents = 50.0,
            double complexNoisePairBiasPercents = 25.0,
            double complexNoiseIntervalBiasPercents = 66.6,

            bool forceFullBoundary = false,
            DynamicBoundaryOffset dynamicBoundaryOffset = DynamicBoundaryOffset.Minimize
        )
        {
            MasqueradePreset = masqueradePreset;

            OutputLength              = outputLength;
            UseDynamicOutputIntervals = useDynamicOutputIntervals;
            DynamicOutputIntervals    = [.. dynamicOutputIntervals ?? []];

            OutputExtendingCoefficient      = outputExtendingCoefficient;
            DoExtendOutputIfLessThanInitial = doExtendOutputIfLessThanInitial;
            HardChunkSizeToSoftCoefficient  = hardChunkSizeToSoftCoefficient;

            KeepOriginalFileExtension = keepOriginalFileExtension;

            ForceOptimalEntropy = forceOptimalEntropy;
            ExpectedEntropy     = expectedEntropy;

            PrimaryNoiseBiasPercents         = primaryNoiseBiasPercents;
            ComplexNoisePairBiasPercents     = complexNoisePairBiasPercents;
            ComplexNoiseIntervalBiasPercents = complexNoiseIntervalBiasPercents;

            ForceFullBoundary     = forceFullBoundary;
            DynamicBoundaryOffset = dynamicBoundaryOffset;
        }
    }
}