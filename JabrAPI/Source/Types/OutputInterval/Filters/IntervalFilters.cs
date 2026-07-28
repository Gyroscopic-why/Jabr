using System;



namespace JabrAPI
{
    public partial class OutputInterval
    {
        public partial class IntervalFilters
        {
            public FilterSelectionState OUT_LENGTH_RANGE = FilterSelectionState.ANY;
            public FilterSelectionState MIN_OUT_LENGTH = FilterSelectionState.MAX;
            public FilterSelectionState MAX_OUT_LENGTH = FilterSelectionState.MIN;

            public FilterSelectionState ABSOLUTE_DIFFERENCE = FilterSelectionState.MIN;
            public FilterSelectionState DIFFERENCE_TO_MIN = FilterSelectionState.ANY;
            public FilterSelectionState DIFFERENCE_TO_MAX = FilterSelectionState.MAX;

            public FilterType[] FiltersPriorities
            {
                get;
                set
                {
                    if (value is not { Length: 6 }) return;

                    var alreadyBitMask = 0;
                    for (var i = 0; i < 6; i++)
                    {
                        var val = (Byte)value[i];
                        if (val < 1 || val > 6) return;

                        var thisBit = 1 << (val - 1);
                        if ((alreadyBitMask & thisBit) != 0) return;
                        alreadyBitMask |= thisBit;
                    }
                    field = value;
                }
            } = [
                    FilterType.ABSOLUTE_DIFFERENCE,
                    FilterType.MAX_OUT_LENGTH,
                    FilterType.MIN_OUT_LENGTH,
                    FilterType.OUT_LENGTH_RANGE,
                    FilterType.DIFFERENCE_TO_MAX,
                    FilterType.DIFFERENCE_TO_MIN
                ];



            public IntervalFilters() { }
            public IntervalFilters
            (
                FilterSelectionState OUT_LENGTH_RANGE = FilterSelectionState.ANY,
                FilterSelectionState MIN_OUT_LENGTH   = FilterSelectionState.MAX,
                FilterSelectionState MAX_OUT_LENGTH   = FilterSelectionState.MIN,

                FilterSelectionState ABSOLUTE_DIFFERENCE = FilterSelectionState.MIN,
                FilterSelectionState DIFFERENCE_TO_MIN = FilterSelectionState.ANY,
                FilterSelectionState DIFFERENCE_TO_MAX = FilterSelectionState.MAX,
                FilterType[]? filterPriorities = null)
            {
                this.OUT_LENGTH_RANGE = OUT_LENGTH_RANGE;
                this.MIN_OUT_LENGTH = MIN_OUT_LENGTH;
                this.MAX_OUT_LENGTH = MAX_OUT_LENGTH;

                this.ABSOLUTE_DIFFERENCE = ABSOLUTE_DIFFERENCE;
                this.DIFFERENCE_TO_MIN = DIFFERENCE_TO_MIN;
                this.DIFFERENCE_TO_MAX = DIFFERENCE_TO_MAX;

                if (filterPriorities != null)
                    FiltersPriorities = filterPriorities;
            }



            public enum FilterSelectionState
            {
                ANY,
                MIN,
                MAX
            }
            public enum FilterType : Byte
            {
                OUT_LENGTH_RANGE = 1,
                MIN_OUT_LENGTH = 2,
                MAX_OUT_LENGTH = 3,

                ABSOLUTE_DIFFERENCE = 4,
                DIFFERENCE_TO_MIN = 5,
                DIFFERENCE_TO_MAX = 6
            }
        }



        public enum LengthChoiceSetting
        {
            CHOOSE_RANDOM_FROM_VALID,
            ALWAYS_PICK_SMALLEST_VALID,
            ALWAYS_PICK_LARGEEST_VALID,
        }
    }
}
