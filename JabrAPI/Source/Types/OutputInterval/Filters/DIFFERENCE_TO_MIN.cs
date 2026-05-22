using System.Collections.Generic;



namespace JabrAPI
{
    public partial class OutputInterval
    {
        static private List<MiniInterval> ApplyFilter_DIFFERENCE_TO_MIN(
            List<MiniInterval> valid,
            IntervalFilters.FilterSelectionState filterSelectionState,
            ref ParameterCounting paramCountingRef,
            ref double allChanceRef)
        {
            List<MiniInterval> result = new(valid.Count);
            ParameterCounting paramCountingOut = new();
            double allChanceOut = 0.0;

            switch (filterSelectionState)
            {
                case IntervalFilters.FilterSelectionState.MIN:
                    {
                        foreach (MiniInterval interval in valid)
                        {
                            if (interval.DifToMin <= paramCountingRef.LowDifToMin)
                            {
                                result.Add(interval);
                                paramCountingOut.Update(interval);
                                allChanceOut += interval.Probability;
                            }
                        }
                        break;
                    }
                case IntervalFilters.FilterSelectionState.MAX:
                    {
                        foreach (MiniInterval interval in valid)
                        {
                            if (interval.DifToMin >= paramCountingRef.HighDifToMin)
                            {
                                result.Add(interval);
                                paramCountingOut.Update(interval);
                                allChanceOut += interval.Probability;
                            }
                        }
                        break;
                    }
                case IntervalFilters.FilterSelectionState.ANY or _: return valid;
            }

            if (result.Count > 0)
            {
                valid = result;
                paramCountingRef = paramCountingOut;
                allChanceRef = allChanceOut;
            }
            return valid;
        }
    }
}