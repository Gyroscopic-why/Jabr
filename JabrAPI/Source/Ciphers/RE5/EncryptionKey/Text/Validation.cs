using System;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class EncryptionKey : IEncryptionKey
    {
        override public ValidateHelper IsValid => _validateHelper;


        public class ValidateHelper : IValidateHelper
        {
            private readonly EncryptionKey _reKey;
            private readonly PartiallyHelper _partiallyHelper;

            
            
            internal ValidateHelper(EncryptionKey reKey)
            {
                _reKey = reKey;
                _partiallyHelper = new(reKey);
            }



            public PartiallyHelper Partially => _partiallyHelper;
            public class PartiallyHelper
            {
                private readonly EncryptionKey _reKey;

                internal PartiallyHelper(EncryptionKey reKey)
                {
                    _reKey = reKey;
                }



                public bool Primary(out Exception? exception)
                         => Primary(_reKey.PrAlphabet, out exception);
                static public bool Primary(string primary, out Exception? exception)
                {
                    if (primary == null || primary == "" || primary.Length < 2)
                    {
                        exception = new ArgumentException
                        (
                            "Primary alphabet is not set or is too short",
                            nameof(primary)
                        );
                        return false;
                    }
                    for (var curId = 0; curId < primary.Length; curId++)
                    {
                        for (var id2 = curId + 1; id2 < primary.Length; id2++)
                        {
                            if (primary[curId] == primary[id2])
                            {
                                exception = new ArgumentException
                                (
                                    $"Primary alphabet contains duplicates characters" +
                                    $"\nDuplicate char: {primary[curId]}",
                                    nameof(primary)
                                );
                                return false;
                            }
                        }
                    }

                    exception = null;
                    return true;
                }

                public bool Primary(bool throwExceptions = false)
                         => Primary(_reKey.PrAlphabet, throwExceptions);
                static public bool Primary(string primary, bool throwExceptions = false)
                {
                    bool isValid = Primary(primary, out Exception? exception);
                    if (!isValid && throwExceptions) throw exception!;
                    return isValid;
                }



                public bool External(out Exception? exception)
                         => External(_reKey.ExAlphabet, out exception);
                static public bool External(string external, out Exception? exception)
                {
                    if (external == null || external == "" || external.Length < 2)
                    {
                        exception = new ArgumentException
                            (
                                "External alphabet is not set or is too short",
                                nameof(external)
                            );
                        return false;
                    }
                    for (var curId = 0; curId < external.Length; curId++)
                    {
                        for (var id2 = curId + 1; id2 < external.Length; id2++)
                        {
                            if (external[curId] == external[id2])
                            {
                                exception = new ArgumentException
                                    (
                                        $"External alphabet contains duplicates characters" +
                                        $"Duplicate char: {external[curId]}",
                                        nameof(external)
                                    );
                                return false;
                            }
                        }
                    }

                    exception = null;
                    return true;
                }

                public bool External(bool throwExceptions = false)
                         => External(_reKey.ExAlphabet, throwExceptions);
                static public bool External(string external, bool throwExceptions = false)
                {
                    bool isValid = External(external, out Exception? exception);
                    if (!isValid && throwExceptions) throw exception!;
                    return isValid;
                }
            }


            public override bool ForEncryption(string message, out Exception? exception)
            {
                return PartiallyHelper.External(_reKey.ExAlphabet, out exception)
                            && Primary(message, _reKey.PrAlphabet, out exception);
            }
            public override bool ForDecryption(string message, out Exception? exception)
            {
                return PartiallyHelper.Primary(_reKey.PrAlphabet, out exception)
                          && External(message, _reKey.ExAlphabet, out exception);
            }

            public override bool ForEncryption(string message, bool throwExceptions = false)
            {
                return PartiallyHelper.External(_reKey.ExAlphabet, throwExceptions)
                            && Primary(message, _reKey.PrAlphabet, throwExceptions);
            }
            public override bool ForDecryption(string message, bool throwExceptions = false)
            {
                return PartiallyHelper.Primary(_reKey.PrAlphabet, throwExceptions)
                          && External(message, _reKey.ExAlphabet, throwExceptions);
            }



            public bool Primary(string message, out Exception? exception)
                     => Primary(message, _reKey.PrAlphabet, out exception);
            static public bool Primary(string message, string primary, out Exception? exception)
            {
                if (!PartiallyHelper.Primary(primary, out exception)) return false;

                foreach (char c in message)
                {
                    if (!primary.Contains(c))
                    {
                        exception = new ArgumentException
                        (
                            $"Message contains characters not present in the primary alphabet" +
                            $"\nMissing character: {c}",
                            nameof(primary)
                        );
                        return false;
                    }
                }
                return true;
            }

            public bool Primary(string message, bool throwExceptions = false)
                     => Primary(message, _reKey.PrAlphabet, throwExceptions);
            static public bool Primary(string message, string primary, bool throwExceptions = false)
            {
                bool isValid = Primary(message, primary, out Exception? exception);
                if (!isValid && throwExceptions) throw exception!;
                return isValid;
            }



            public bool External(string message, out Exception? exception)
                     => External(message, _reKey.ExAlphabet, out exception);
            static public bool External(string encrypted, string external, out Exception? exception)
            {
                if (!PartiallyHelper.External(external, out exception)) return false;

                foreach (char c in encrypted)
                {
                    if (!external.Contains(c))
                    {
                        exception = new ArgumentException
                        (
                            $"Message contains characters not present in the external alphabet" +
                            $"\nMissing character: {c}",
                            nameof(external)
                        );
                        return false;
                    }
                }
                return true;
            }
            public bool External(string encrypted, bool throwExceptions = false)
                     => External(encrypted, _reKey.ExAlphabet, throwExceptions);
            static public bool External(string encrypted, string external, bool throwExceptions = false)
            {
                bool isValid = External(encrypted, external, out Exception? exception);
                if (!isValid && throwExceptions) throw exception!;
                return isValid;
            }
        }
    }
}