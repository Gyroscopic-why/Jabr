using System;


using JabrAPI.Template;



namespace JabrAPI
{
    public partial class Noisifier
    {
        public ValidateHelper IsValid => _validateHelper;


        
        public partial class ValidateHelper
        {
            private readonly Noisifier _noisifier;


            internal ValidateHelper(Noisifier noisifier)
            {
                _noisifier = noisifier;
            }



            public bool ForMessage(string message, out Exception? exception)
                     => ComplexForMessage(message, out exception) &&
                        PrimaryForMessage(message, out exception);
            public bool ForReKey(IEncryptionKey reKey, out Exception? exception)
                     => ComplexForReKey(reKey, out exception) &&
                        PrimaryForReKey(reKey, out exception);
            public bool ForMessageAndReKey(IEncryptionKey reKey, string message, out Exception? exception)
                     => ForMessage(message, out exception) &&
                        ForReKey(reKey, out exception);



            static public bool PrimaryForReKey(IEncryptionKey reKey, string primaryNoise, out Exception? exception)
                => PrimaryForReKey(reKey.FinalAlphabet, primaryNoise, out exception);
            static public bool PrimaryForReKey(string  exAlphabet, string primaryNoise, out Exception? exception)
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
            static public bool PrimaryForMessage(string message, string primaryNoise, out Exception? exception)
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

            public bool PrimaryForReKey(IEncryptionKey reKey, out Exception? exception)
                => PrimaryForReKey(reKey.FinalAlphabet, _noisifier._primaryNoise, out exception);
            public bool PrimaryForReKey(string  exAlphabet, out Exception? exception)
                => PrimaryForReKey(exAlphabet,  _noisifier._primaryNoise, out exception);
            public bool PrimaryForMessage(string message, out Exception? exception)
                => PrimaryForMessage(message, _noisifier._primaryNoise, out exception);



            static public bool ComplexForReKey(IEncryptionKey reKey, string complexNoise, out Exception? exception)
                => ComplexForReKey(reKey.FinalAlphabet, complexNoise, out exception);
            static public bool ComplexForReKey(string  exAlphabet, string complexNoise, out Exception? exception)
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
            static public bool ComplexForMessage(string message, string complexNoise, out Exception? exception)
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

            public bool ComplexForReKey(IEncryptionKey reKey, out Exception? exception)
                => ComplexForReKey(reKey.FinalAlphabet, _noisifier._complexNoise, out exception);
            public bool ComplexForReKey(string  exAlphabet, out Exception? exception)
                => ComplexForReKey(exAlphabet,  _noisifier._complexNoise, out exception);
            public bool ComplexForMessage(string message, out Exception? exception)
                => ComplexForMessage(message, _noisifier._complexNoise, out exception);



            static private bool IsNoiseValid(string exAlphabet_or_message, string primaryNoise_or_complexNoise,
                ArgumentException errorMessage, out Exception? exception)
            {
                foreach (char noiseChar in primaryNoise_or_complexNoise)
                {
                    if (exAlphabet_or_message.Contains(noiseChar))
                    {
                        exception = new ArgumentException
                            (
                            errorMessage.Message +
                            $"\nDuplicate char: {noiseChar}",
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