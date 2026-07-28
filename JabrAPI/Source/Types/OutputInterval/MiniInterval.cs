using System;



namespace JabrAPI
{
    public partial class OutputInterval
    {
        private class MiniInterval(
            Int32 minVal, Int32 maxVal,
            Int32 absDif, Int32 difToMin, Int32 difToMax,
            Int32 id, double probability)
        {
            public Int32 MinVal = minVal;
            public Int32 MaxVal = maxVal;
            public Int32 Length = maxVal - minVal;

            public Int32 AbsDif   = absDif;
            public Int32 DifToMin = difToMin;
            public Int32 DifToMax = difToMax;

            public Int32 Id = id;
            public double Probability = probability;
        }
    }
}