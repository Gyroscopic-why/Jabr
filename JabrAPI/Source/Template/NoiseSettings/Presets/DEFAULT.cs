using static JabrAPI.OutputInterval.IntervalFilters.FilterType;
using static JabrAPI.OutputInterval.IntervalFilters.FilterSelectionState;



namespace JabrAPI.Template
{
    static internal partial class SettingsPresets
    {
        static internal readonly Noise.Settings DEFAULT = new
        (
            MasqueradePreset.DEFAULT,
            true,
            true,
            [],
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
            1.8742,
            false,
            4.0,
            true,
            ExpectedEntropy.C1_Medium,
            50.0,
            25.0,
            66.66,
            false,
            DynamicBoundaryOffset.Minimize,
            ChunkSize.KByte16
        );
    }
}