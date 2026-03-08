using System;



namespace JabrAPI.Template
{
    abstract public class INoiseSetHelper()
    {
        abstract public void Default(Int32 primaryCount = 8, Int32 complexCount = 16);

        abstract public void DefaultOnlyPr  (Int32 primaryCount = 8);
        abstract public void DefaultOnlyCplx(Int32 complexCount = 16);
    }
}