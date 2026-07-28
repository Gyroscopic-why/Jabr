using static JabrAPI.OutputInterval.IntervalFilters.FilterType;
using static JabrAPI.OutputInterval.IntervalFilters.FilterSelectionState;



namespace JabrAPI.Template
{
    static internal partial class SettingsPresets
    {
        static internal readonly Noise.Settings HTTP_2_gRPC = new
        (
            MasqueradePreset.HTTP_2_gRPC,
            true,
            [
                new (10, 64,   150),
                new (20, 100,  300),
                new (40, 400,  700),
                new (30, 1100, 1350)
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