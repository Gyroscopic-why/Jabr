namespace JabrAPI.Template
{
    static internal partial class SettingsPresets
    {
        static internal readonly INoiseSettings WEBSOCKET_WSS = new
        (
            MasqueradePreset.WEBSOCKET_WSS,
            0,
            true,
            [
                new (5,  64,   120),
                new (45, 100,  350),
                new (30, 500,  950),
                new (20, 1200, 1440)
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