using System;



namespace JabrAPI
{
    public partial class OutputInterval
    {
        private class ParameterCounting
        {
            public Int32 LowMinVal  = Int32.MaxValue;
            public Int32 HighMinVal = Int32.MinValue;

            public Int32 LowMaxVal  = Int32.MinValue;
            public Int32 HighMaxVal = Int32.MaxValue;

            public Int32 MinLength = Int32.MaxValue;
            public Int32 MaxLength = Int32.MinValue;


            public Int32 LowAbsDif  = Int32.MaxValue;
            public Int32 HighAbsDif = Int32.MinValue;

            public Int32 LowDifToMin  = Int32.MaxValue;
            public Int32 HighDifToMin = Int32.MinValue;

            public Int32 LowDifToMax  = Int32.MaxValue;
            public Int32 HighDifToMax = Int32.MinValue;



            public ParameterCounting() { }
            public ParameterCounting(
                Int32 minVal, Int32 maxVal,
                Int32 absDif, Int32 difToMin, Int32 difToMax)
            {
                LowMinVal  = minVal;
                HighMinVal = minVal;

                LowMaxVal  = maxVal;
                HighMaxVal = maxVal;

                var length = maxVal - minVal;
                MinLength  = length;
                MaxLength  = length;


                LowAbsDif  = absDif;
                HighAbsDif = absDif;

                LowDifToMin  = difToMin;
                HighDifToMin = difToMin;

                LowDifToMax  = difToMax;
                HighDifToMax = difToMax;
            }



            public void Update(
                Int32 minVal, Int32 maxVal,
                Int32 absDif, Int32 difToMin, Int32 difToMax)
            {

                if (minVal < LowMinVal)  LowMinVal  = minVal;
                if (minVal > HighMinVal) HighMinVal = minVal;

                if (maxVal < LowMaxVal)  LowMaxVal  = maxVal;
                if (maxVal > HighMaxVal) HighMaxVal = maxVal;

                Int32 length = maxVal - minVal;
                if (length < MinLength) MinLength = length;
                if (length > MaxLength) MaxLength = length;


                if (absDif < LowAbsDif)  LowAbsDif  = absDif;
                if (absDif > HighAbsDif) HighAbsDif = absDif;

                if (difToMin < LowDifToMin)  LowDifToMin  = difToMin;
                if (difToMin > HighDifToMin) HighDifToMin = difToMin;

                if (difToMax < LowDifToMax)  LowDifToMax  = difToMax;
                if (difToMax > HighDifToMax) HighDifToMax = difToMax;

            }
            public void Update(MiniInterval interval)
                => Update
                    (
                        interval.MinVal,
                        interval.MaxVal,
                        interval.AbsDif,
                        interval.DifToMin,
                        interval.DifToMax
                    );
        }
    }
}