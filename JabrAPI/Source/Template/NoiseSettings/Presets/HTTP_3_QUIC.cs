namespace JabrAPI.Template
{
    static internal partial class SettingsPresets
    {
        static internal readonly INoiseSettings HTTP_3_QUIC = new
        (
            MasqueradePreset.HTTP_3_QUIC,
            0,
            true,
            [
                new (5,  40,   60),
                new (10, 64,   256),
                new (15, 500,  1000),
                new (70, 1250, 1420)
            ],
            1.8742,
            false,
            4,
            true,
            true,
            ExpectedEntropy.C1_Medium,
            50.0,
            25.0,
            66.66,
            false,
            DynamicBoundaryOffset.Minimize
        );
    }
}