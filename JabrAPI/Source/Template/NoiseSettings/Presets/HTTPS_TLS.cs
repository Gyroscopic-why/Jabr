namespace JabrAPI.Template
{
    static internal partial class SettingsPresets
    {
        static internal readonly INoiseSettings HTTPS_TLS = new
        (
            MasqueradePreset.HTTPS_TLS,
            0,
            true,
            [
                new (20, 64,   256),
                new (20, 512,  900),
                new (60, 1350, 1440)
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