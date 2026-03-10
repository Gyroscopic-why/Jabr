using System;


using JabrAPI.Template;



namespace JabrAPI
{
    public partial class Noisifier
    {
        public ValidateHelper IsValid => _validateHelper;


        
        public class ValidateHelper : INoiseValidateHelper
        {
            private readonly Noisifier _noisifier;


            internal ValidateHelper(Noisifier noisifier)
            {
                _noisifier = noisifier;
            }



            public override bool ForAdding  (string message, bool throwException = false)
            {
                return ComplexForMessage(message, throwException) ||
                       PrimaryForMessage(message, throwException);
            }
            public override bool ForAdding  (IEncryptionKey reKey, string message, bool throwException = false)
            {
                return ForAdding(message, throwException)     ||
                       ComplexForKey(reKey, throwException) ||
                       PrimaryForKey(reKey, throwException);
            }
            public override bool ForRemoving(IEncryptionKey reKey, string message, bool throwException = false)
            {
                return ComplexForKey(reKey, throwException) ||
                       PrimaryForKey(reKey, throwException);
            }



            static public bool PrimaryForKey(IEncryptionKey reKey, string primaryNoise, bool throwException = false)
                => PrimaryForKey(reKey.FinalAlphabet, primaryNoise, throwException);
            static public bool PrimaryForKey(string  exAlphabet, string primaryNoise,   bool throwException = false)
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
                    throwException
                );
            }
            static public bool PrimaryForMessage(string message, string primaryNoise,   bool throwException = false)
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
                    throwException
                );
            }

            public bool PrimaryForKey(IEncryptionKey reKey, bool throwException = false)
                => PrimaryForKey(reKey.FinalAlphabet, _noisifier._primaryNoise, throwException);
            public bool PrimaryForKey(string  exAlphabet, bool throwException = false)
                => PrimaryForKey(exAlphabet,  _noisifier._primaryNoise, throwException);
            public bool PrimaryForMessage(string message, bool throwException = false)
                => PrimaryForMessage(message, _noisifier._primaryNoise, throwException);


            static public bool ComplexForKey(IEncryptionKey reKey, string complexNoise, bool throwException = false)
                => ComplexForKey(reKey.FinalAlphabet, complexNoise, throwException);
            static public bool ComplexForKey(string  exAlphabet, string complexNoise,   bool throwException = false)
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
                    throwException
                );
            }
            static public bool ComplexForMessage(string message, string complexNoise,   bool throwException = false)
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
                    throwException
                );
            }

            public bool ComplexForKey(Template.IEncryptionKey reKey, bool throwException = false)
                => ComplexForKey(reKey.FinalAlphabet, _noisifier._complexNoise, throwException);
            public bool ComplexForKey(string  exAlphabet, bool throwException = false)
                => ComplexForKey(exAlphabet,  _noisifier._complexNoise, throwException);
            public bool ComplexForMessage(string message, bool throwException = false)
                => ComplexForMessage(message, _noisifier._complexNoise, throwException);



            static private bool IsNoiseValid(string exAlphabet_or_message, string primaryNoise_or_complexNoise,
                ArgumentException errorMessage, bool throwException = false)
            {
                foreach (char noiseChar in primaryNoise_or_complexNoise)
                {
                    if (exAlphabet_or_message.Contains(noiseChar))
                    {
                        if (throwException) throw new ArgumentException
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