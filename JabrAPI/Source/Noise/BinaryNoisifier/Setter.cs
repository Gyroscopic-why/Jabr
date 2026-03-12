using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI
{
    public partial class BinaryNoisifier
    {
        public SetHelper Set => _setHelper;


        public class SetHelper
        {
            private readonly BinaryNoisifier _noisifier;
            private readonly SensetiveSetHelper _sensitiveSetHelper;

            internal SetHelper(BinaryNoisifier noisifier)
            {
                _noisifier = noisifier;
                _sensitiveSetHelper = new(_noisifier);
            }



            public SensetiveSetHelper Sensitive => _sensitiveSetHelper;

            public class SensetiveSetHelper
            {
                private readonly BinaryNoisifier _noisifier;

                internal SensetiveSetHelper(BinaryNoisifier noisifier)
                {
                    _noisifier = noisifier;
                }



                public void PrNoise(List<Byte> prNoise)
                {
                    _noisifier._primaryNoise.Clear();
                    _noisifier._primaryNoise.AddRange(prNoise);
                }
                public void PrimaryNoise(List<Byte> primaryNoise)
                {
                    _noisifier._primaryNoise.Clear();
                    _noisifier._primaryNoise.AddRange(primaryNoise);
                }
                public bool SafePrNoise(IBinaryKey reKey, List<Byte> prNoise)
                {
                    if (!ValidateHelper.PrimaryForKey(reKey, prNoise)) return false;
                    _noisifier._primaryNoise.Clear();
                    _noisifier._primaryNoise.AddRange(prNoise);
                    return true;
                }
                public bool SafePrimaryNoise(IBinaryKey reKey, List<Byte> primaryNoise)
                {
                    if (!ValidateHelper.PrimaryForKey(reKey, primaryNoise)) return false;
                    _noisifier._primaryNoise.Clear();
                    _noisifier._primaryNoise.AddRange(primaryNoise);
                    return true;
                }



                public void CplxNoise(List<Byte> cplxNoise)
                {
                    _noisifier._complexNoise.Clear();
                    _noisifier._complexNoise.AddRange(cplxNoise);
                }
                public void ComplexNoise(List<Byte> complexNoise)
                {
                    _noisifier._complexNoise.Clear();
                    _noisifier._complexNoise.AddRange(complexNoise);
                }
                public bool SafeCplxNoise(IBinaryKey reKey, List<Byte> cplxNoise)
                {
                    if (!ValidateHelper.ComplexForKey(reKey, cplxNoise)) return false;
                    _noisifier._complexNoise.Clear();
                    _noisifier._complexNoise.AddRange(cplxNoise);
                    return true;
                }
                public bool SafeComplexNoise(IBinaryKey reKey, List<Byte> complexNoise)
                {
                    if (!ValidateHelper.ComplexForKey(reKey, complexNoise)) return false;
                    _noisifier._complexNoise.Clear();
                    _noisifier._complexNoise.AddRange(complexNoise);
                    return true;
                }
            }



            public void Default(Byte primaryCount = 8, Byte complexCount = 16)
            {
                _noisifier._primaryCount = primaryCount;
                _noisifier._complexCount = complexCount;
            }


            public void DefaultOnlyPr(Byte primaryCount = 8)
                => _noisifier._primaryCount = primaryCount;
            public void DefaultOnlyCplx(Byte complexCount = 16)
                => _noisifier._complexCount = complexCount;


            public void Default(List<Byte> banned, Byte primaryCount, Byte complexCount)
            {
                _noisifier._banned.Clear();
                _noisifier._banned.AddRange(banned);

                Default(primaryCount, complexCount);
            }
            public void Default(List<Byte> banned)
            {
                _noisifier._banned.Clear();
                _noisifier._banned.AddRange(banned);
            }
        }
    }
}