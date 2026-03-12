using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI
{
    public partial class BinaryNoisifier
    {
        public ValidateHelper IsValid => _validateHelper;



        public class ValidateHelper
        {
            private readonly BinaryNoisifier _noisifier;


            internal ValidateHelper(BinaryNoisifier noisifier)
            {
                _noisifier = noisifier;
            }



            public bool ForAdding(List<Byte> message, bool throwExceptions = false)
            {
                return ComplexForMessage(message, throwExceptions) ||
                       PrimaryForMessage(message, throwExceptions);
            }
            public bool ForAdding(IBinaryKey reKey, List<Byte> message, bool throwExceptions = false)
            {
                return ForAdding(message, throwExceptions) ||
                       ComplexForKey(reKey, throwExceptions) ||
                       PrimaryForKey(reKey, throwExceptions);
            }
            public bool ForRemoving(IBinaryKey reKey, List<Byte> message, bool throwExceptions = false)
            {
                return ComplexForKey(reKey, throwExceptions) ||
                       PrimaryForKey(reKey, throwExceptions);
            }



            static public bool PrimaryForKey(IBinaryKey reKey, List<Byte> primaryNoise, bool throwExceptions = false)
                => PrimaryForKey(reKey.FinalAlphabet, primaryNoise, throwExceptions);
            static public bool PrimaryForKey(List<Byte> exAlphabet, List<Byte> primaryNoise, bool throwExceptions = false)
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
            static public bool PrimaryForMessage(List<Byte> message, List<Byte> primaryNoise, bool throwExceptions = false)
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

            public bool PrimaryForKey(IBinaryKey reKey, bool throwExceptions = false)
                => PrimaryForKey(reKey.FinalAlphabet, _noisifier._primaryNoise, throwExceptions);
            public bool PrimaryForKey(List<Byte> exAlphabet, bool throwExceptions = false)
                => PrimaryForKey(exAlphabet, _noisifier._primaryNoise, throwExceptions);
            public bool PrimaryForMessage(List<Byte> message, bool throwExceptions = false)
                => PrimaryForMessage(message, _noisifier._primaryNoise, throwExceptions);


            static public bool ComplexForKey(IBinaryKey reKey, List<Byte> complexNoise, bool throwExceptions = false)
                => ComplexForKey(reKey.FinalAlphabet, complexNoise, throwExceptions);
            static public bool ComplexForKey(List<Byte> exAlphabet, List<Byte> complexNoise, bool throwExceptions = false)
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
            static public bool ComplexForMessage(List<Byte> message, List<Byte> complexNoise, bool throwExceptions = false)
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

            public bool ComplexForKey(IBinaryKey reKey, bool throwExceptions = false)
                => ComplexForKey(reKey.FinalAlphabet, _noisifier._complexNoise, throwExceptions);
            public bool ComplexForKey(List<Byte> exAlphabet, bool throwExceptions = false)
                => ComplexForKey(exAlphabet, _noisifier._complexNoise, throwExceptions);
            public bool ComplexForMessage(List<Byte> message, bool throwExceptions = false)
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