using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI
{
    public partial class Noisifier
    {
        public SetHelper Set => _setHelper;


        public class SetHelper
        {
            private readonly Noisifier _noisifier;
            private readonly SensetiveSetHelper _sensitiveSetHelper;

            internal SetHelper(Noisifier noisifier)
            {
                _noisifier = noisifier;
                _sensitiveSetHelper = new(_noisifier);
            }



            public SensetiveSetHelper Sensitive => _sensitiveSetHelper;

            public class SensetiveSetHelper
            {
                private readonly Noisifier _noisifier;

                internal SensetiveSetHelper(Noisifier noisifier)
                {
                    _noisifier = noisifier;
                }



                public void PrNoise(string prNoise)
                         => PrimaryNoise(prNoise);
                public void PrimaryNoise(string primaryNoise)
                         => _noisifier._primaryNoise = primaryNoise;
                public bool SafePrNoise(IEncryptionKey reKey, string prNoise)
                         => SafePrimaryNoise(reKey, prNoise);
                public bool SafePrimaryNoise(IEncryptionKey reKey, string primaryNoise)
                {
                    if (!ValidateHelper.PrimaryForReKey(reKey, primaryNoise)) return false;
                    _noisifier._primaryNoise = primaryNoise;
                    return true;
                }



                public void CplxNoise(string cplxNoise)
                         => ComplexNoise(cplxNoise);
                public void ComplexNoise(string complexNoise)
                         => _noisifier._complexNoise = complexNoise;
                public bool SafeCplxNoise(IEncryptionKey reKey, string cplxNoise)
                         => SafeComplexNoise(reKey, cplxNoise);
                public bool SafeComplexNoise(IEncryptionKey reKey, string complexNoise)
                {
                    if (!ValidateHelper.ComplexForReKey(reKey, complexNoise)) return false;
                    _noisifier._complexNoise = complexNoise;
                    return true;
                }
            }



            public void Default(List<char> banned)
            {
                _noisifier._banned.Clear();
                _noisifier._banned.AddRange(banned);
            }
            public void Default(Int32 primaryCount = 8, Int32 complexCount = 16)
            {
                DefaultOnlyPr(primaryCount);
                DefaultOnlyCplx(complexCount);
                _noisifier._complexCount = complexCount;
            }
            public void Default(List<char> banned, Int32 primaryCount, Int32 complexCount)
            {
                Default(banned);
                Default(primaryCount, complexCount);
            }


            public void DefaultOnlyPr(Int32 prCount = 8)
                => DefaultOnlyPrimary(prCount);
            public void DefaultOnlyCplx(Int32 cplxCount = 16)
                => DefaultOnlyComplex(cplxCount);

            public void DefaultOnlyPrimary(Int32 primaryCount = 8)
                => _noisifier._primaryCount = primaryCount;
            public void DefaultOnlyComplex(Int32 complexCount = 16)
                => _noisifier._complexCount = complexCount;
        }
    }
}