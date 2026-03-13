using System;


using JabrAPI.Template;



namespace JabrAPI
{
    public partial class Noisifier
    {
        public partial class ValidateHelper
        {
            public bool ForMessage(string message, bool throwExceptions = false)
                     => ComplexForMessage(message, throwExceptions) &&
                        PrimaryForMessage(message, throwExceptions);
            public bool ForReKey(IEncryptionKey reKey, bool throwExceptions = false)
                     => ComplexForReKey(reKey, throwExceptions) &&
                        PrimaryForReKey(reKey, throwExceptions);
            public bool ForMessageAndReKey(IEncryptionKey reKey, string message, bool throwExceptions = false)
                     => ForMessage(message, throwExceptions) &&
                        ForReKey(reKey, throwExceptions);



            static public bool PrimaryForReKey(IEncryptionKey reKey, string primaryNoise, bool throwExceptions = false)
                => PrimaryForReKey(reKey.FinalAlphabet, primaryNoise, throwExceptions);
            static public bool PrimaryForReKey(string  exAlphabet, string primaryNoise, bool throwExceptions = false)
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
            static public bool PrimaryForMessage(string message, string primaryNoise, bool throwExceptions = false)
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


            public bool PrimaryForReKey(IEncryptionKey reKey, bool throwExceptions = false)
                => PrimaryForReKey(reKey.FinalAlphabet, _noisifier._primaryNoise, throwExceptions);
            public bool PrimaryForReKey(string  exAlphabet, bool throwExceptions = false)
                => PrimaryForReKey(exAlphabet, _noisifier._primaryNoise, throwExceptions);
            public bool PrimaryForMessage(string message, bool throwExceptions = false)
                => PrimaryForMessage(message, _noisifier._primaryNoise, throwExceptions);



            static public bool ComplexForReKey(IEncryptionKey reKey, string complexNoise, bool throwExceptions = false)
                => ComplexForReKey(reKey.FinalAlphabet, complexNoise, throwExceptions);
            static public bool ComplexForReKey(string  exAlphabet, string complexNoise, bool throwExceptions = false)
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
            static public bool ComplexForMessage(string message, string complexNoise, bool throwExceptions = false)
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

            public bool ComplexForReKey(IEncryptionKey reKey, bool throwExceptions = false)
                => ComplexForReKey(reKey.FinalAlphabet, _noisifier._complexNoise, throwExceptions);
            public bool ComplexForReKey(string  exAlphabet, bool throwExceptions = false)
                => ComplexForReKey(exAlphabet, _noisifier._complexNoise, throwExceptions);
            public bool ComplexForMessage(string message, bool throwExceptions = false)
                => ComplexForMessage(message, _noisifier._complexNoise, throwExceptions);



            static private bool IsNoiseValid(string exAlphabet_or_message, string primaryNoise_or_complexNoise,
                ArgumentException errorMessage, bool throwExceptions = false)
            {
                foreach (char noiseChar in primaryNoise_or_complexNoise)
                {
                    if (exAlphabet_or_message.Contains(noiseChar))
                    {
                        if (throwExceptions) throw new ArgumentException
                            (
                                errorMessage.Message +
                                $"\nDuplicate char: {noiseChar}",
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