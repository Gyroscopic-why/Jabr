namespace JabrAPI.Template
{
    static internal partial class SettingsPresets
    {
        static internal readonly INoiseSettings HTTP_2_gRPC = new
        (
            MasqueradePreset.HTTP_2_gRPC,
            0,
            true,
            [
                new (10, 64,   150),
                new (20, 100,  300),
                new (40, 400,  700),
                new (30, 1100, 1350)
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