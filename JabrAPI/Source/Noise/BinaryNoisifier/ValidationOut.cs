using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI
{
    public partial class BinaryNoisifier
    {
        public ValidateHelper IsValid => _validateHelper;



        public partial class ValidateHelper
        {
            private readonly BinaryNoisifier _noisifier;


            internal ValidateHelper(BinaryNoisifier noisifier)
            {
                _noisifier = noisifier;
            }



            public bool ForMessage(List<Byte> message, out Exception? exception)
                     => ComplexForMessage(message, out exception)
                     && PrimaryForMessage(message, out exception);
            public bool ForReKey(IBinaryKey reKey, out Exception? exception)
                     => ComplexForReKey(reKey, out exception)
                     && PrimaryForReKey(reKey, out exception);
            public bool ForMessageAndReKey(IBinaryKey reKey, List<Byte> message, out Exception? exception)
                     => ForMessage(message, out exception)
                     && ForReKey(reKey, out exception);



            static public bool PrimaryForReKey(IBinaryKey reKey, List<Byte> primaryNoise, out Exception? exception)
                => PrimaryForReKey(reKey.FinalAlphabet, primaryNoise, out exception);
            static public bool PrimaryForReKey(List<Byte> exAlphabet, List<Byte> primaryNoise, out Exception? exception)
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
                    out exception
                );
            }
            static public bool PrimaryForMessage(List<Byte>  message, List<Byte> primaryNoise, out Exception? exception)
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
                    out exception
                );
            }


            public bool PrimaryForReKey(IBinaryKey reKey, out Exception? exception)
                => PrimaryForReKey(reKey.FinalAlphabet, _noisifier._primaryNoise, out exception);
            public bool PrimaryForReKey(List<Byte> exAlphabet, out Exception? exception)
                => PrimaryForReKey(exAlphabet, _noisifier._primaryNoise, out exception);
            public bool PrimaryForMessage(List<Byte>  message, out Exception? exception)
                => PrimaryForMessage(message, _noisifier._primaryNoise, out exception);



            static public bool ComplexForReKey(IBinaryKey reKey, List<Byte> complexNoise, out Exception? exception)
                => ComplexForReKey(reKey.FinalAlphabet, complexNoise, out exception);
            static public bool ComplexForReKey(List<Byte> exAlphabet, List<Byte> complexNoise, out Exception? exception)
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
                    out exception
                );
            }
            static public bool ComplexForMessage(List<Byte>  message, List<Byte> complexNoise, out Exception? exception)
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
                    out exception
                );
            }

            public bool ComplexForReKey(IBinaryKey reKey, out Exception? exception)
                => ComplexForReKey(reKey.FinalAlphabet, _noisifier._complexNoise, out exception);
            public bool ComplexForReKey(List<Byte> exAlphabet, out Exception? exception)
                => ComplexForReKey(exAlphabet, _noisifier._complexNoise, out exception);
            public bool ComplexForMessage(List<Byte>  message, out Exception? exception)
                => ComplexForMessage(message, _noisifier._complexNoise, out exception);



            static private bool IsNoiseValid(List<Byte> exAlphabet_or_message, List<Byte> primaryNoise_or_complexNoise,
                ArgumentException errorMessage, out Exception? exception)
            {
                foreach (Byte noiseByte in primaryNoise_or_complexNoise)
                {
                    if (exAlphabet_or_message.Contains(noiseByte))
                    {
                        exception = new ArgumentException
                            (
                                errorMessage.Message +
                                $"\nDuplicate byte: {noiseByte}",
                                errorMessage.ParamName
                            );
                        return false;
                    }
                }
                exception = null;
                return true;
            }
        }
    }
}