namespace JabrAPI.Template
{
    static internal partial class SettingsPresets
    {
        static internal readonly INoiseSettings HTTPS_DNS = new
        (
            MasqueradePreset.HTTPS_DNS,
            0,
            true,
            [
                new (40, 128, 300),
                new (50, 300, 600),
                new (10, 600, 1024)
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