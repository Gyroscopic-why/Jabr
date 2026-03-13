using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI
{
    public partial class BinaryNoisifier
    {
        public partial class ValidateHelper
        {
            public bool ForMessage(List<Byte> message, bool throwExceptions = false)
                     => ComplexForMessage(message, throwExceptions)
                     && PrimaryForMessage(message, throwExceptions);
            public bool ForReKey(IBinaryKey reKey, bool throwExceptions = false)
                     => ComplexForReKey(reKey, throwExceptions)
                     && PrimaryForReKey(reKey, throwExceptions);
            public bool ForMessageAndReKey(IBinaryKey reKey, List<Byte> message, bool throwExceptions = false)
                     => ForMessage(message, throwExceptions)
                     && ForReKey(reKey, throwExceptions);



            static public bool PrimaryForReKey(IBinaryKey reKey, List<Byte> primaryNoise, bool throwExceptions = false)
                => PrimaryForReKey(reKey.FinalAlphabet, primaryNoise, throwExceptions);
            static public bool PrimaryForReKey(List<Byte> exAlphabet, List<Byte> primaryNoise, bool throwExceptions = false)
            {
                return IsNoiseValid
                (
                    exAlphabet,
                    primaryNoise,
                    new ArgumentException
                    (
                        $"PrimaryNoise bytes can not overlap with ExternalAlphabet chars",
                        nameof(primaryNoise)
                    ),
                    throwExceptions
                );
            }
            static public bool PrimaryForMessage(List<Byte>  message, List<Byte> primaryNoise, bool throwExceptions = false)
            {
                return IsNoiseValid
                (
                    message,
                    primaryNoise,
                    new ArgumentException
                    (
                        $"PrimaryNoise bytes can not overlap with message chars",
                        nameof(primaryNoise)
                    ),
                    throwExceptions
                );
            }


            public bool PrimaryForReKey(IBinaryKey reKey, bool throwExceptions = false)
                => PrimaryForReKey(reKey.FinalAlphabet, _noisifier._primaryNoise, throwExceptions);
            public bool PrimaryForReKey(List<Byte> exAlphabet, bool throwExceptions = false)
                => PrimaryForReKey(exAlphabet, _noisifier._primaryNoise, throwExceptions);
            public bool PrimaryForMessage(List<Byte>  message, bool throwExceptions = false)
                => PrimaryForMessage(message, _noisifier._primaryNoise, throwExceptions);



            static public bool ComplexForReKey(IBinaryKey reKey, List<Byte> complexNoise, bool throwExceptions = false)
                => ComplexForReKey(reKey.FinalAlphabet, complexNoise, throwExceptions);
            static public bool ComplexForReKey(List<Byte> exAlphabet, List<Byte> complexNoise, bool throwExceptions = false)
            {
                return IsNoiseValid
                (
                    exAlphabet,
                    complexNoise,
                    new ArgumentException
                    (
                        $"PrimaryNoise bytes can not overlap with ExternalAlphabet chars",
                        nameof(complexNoise)
                    ),
                    throwExceptions
                );
            }
            static public bool ComplexForMessage(List<Byte>  message, List<Byte> complexNoise, bool throwExceptions = false)
            {
                return IsNoiseValid
                (
                    message,
                    complexNoise,
                    new ArgumentException
                    (
                        $"PrimaryNoise bytes can not overlap with message chars",
                        nameof(complexNoise)
                    ),
                    throwExceptions
                );
            }


            public bool ComplexForReKey(IBinaryKey reKey, bool throwExceptions = false)
                => ComplexForReKey(reKey.FinalAlphabet, _noisifier._complexNoise, throwExceptions);
            public bool ComplexForReKey(List<Byte> exAlphabet, bool throwExceptions = false)
                => ComplexForReKey(exAlphabet, _noisifier._complexNoise, throwExceptions);
            public bool ComplexForMessage(List<Byte>  message, bool throwExceptions = false)
                => ComplexForMessage(message, _noisifier._complexNoise, throwExceptions);



            static private bool IsNoiseValid(List<Byte> exAlphabet_or_message, List<Byte> primaryNoise_or_complexNoise,
                ArgumentException errorMessage, bool throwExceptions = false)
            {
                foreach (Byte noiseByte in primaryNoise_or_complexNoise)
                {
                    if (exAlphabet_or_message.Contains(noiseByte))
                    {
                        if (throwExceptions) throw new ArgumentException
                            (
                                errorMessage.Message +
                                $"\nDuplicate byte: {noiseByte}",
                                errorMessage.ParamName
                            );
                        return false;
                    }
                }
                return true;
            }
        }
    }
}