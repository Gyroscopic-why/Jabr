using static JabrAPI.OutputInterval.IntervalFilters.FilterType;
using static JabrAPI.OutputInterval.IntervalFilters.FilterSelectionState;



namespace JabrAPI.Template
{
    static internal partial class SettingsPresets
    {
        static internal readonly Noise.Settings WEBSOCKET_WSS = new
        (
            MasqueradePreset.WEBSOCKET_WSS,
            true,
            [
                new (5,  64,   120),
                new (45, 100,  350),
                new (30, 500,  950),
                new (20, 1200, 1440)
            ],
            new
            (
                ANY, MAX, MIN,
                MIN, ANY, MAX,
                [
                    ABSOLUTE_DIFFERENCE,
                    MAX_OUT_LENGTH,
                    MIN_OUT_LENGTH,
                    OUT_LENGTH_RANGE,
                    DIFFERENCE_TO_MAX,
                    DIFFERENCE_TO_MIN
                ]
            ),
            OutputInterval.LengthChoiceSetting.CHOOSE_RANDOM_FROM_VALID,
            ChunkSize.KByte16,
            4.0,
            true,
            ExpectedEntropy.C1_Medium,
            50.0,
            25.0,
            66.66
        );
    }
}