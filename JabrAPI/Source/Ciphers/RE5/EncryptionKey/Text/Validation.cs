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



                public bool Primary(bool throwException = false)
                         => Primary(_reKey.PrAlphabet, throwException);
                static public bool Primary(string primary, bool throwException = false)
                {
                    if (primary == null || primary == "" || primary.Length < 2)
                    {
                        if (throwException)
                            throw new ArgumentException
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
                                if (throwException)
                                    throw new ArgumentException
                                    (
                                        $"Primary alphabet contains duplicates characters" +
                                        $"\nDuplicate char: {primary[curId]}",
                                        nameof(primary)
                                    );
                                return false;
                            }
                        }
                    }

                    return true;
                }


                public bool External(bool throwException = false)
                         => External(_reKey.ExAlphabet, throwException);
                static public bool External(string external, bool throwException = false)
                {
                    if (external == null || external == "" || external.Length < 2)
                    {
                        if (throwException)
                            throw new ArgumentException
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
                                if (throwException)
                                    throw new ArgumentException
                                    (
                                        $"External alphabet contains duplicates characters" +
                                        $"Duplicate char: {external[curId]}",
                                        nameof(external)
                                    );
                                return false;
                            }
                        }
                    }

                    return true;
                }
            }



            public override bool ForEncryption(string message, bool throwException = false)
            {
                return PartiallyHelper.External(_reKey.ExAlphabet, throwException)
                    && Primary(message, _reKey.PrAlphabet, throwException);
            }
            public override bool ForDecryption(string message, bool throwException = false)
            {
                return PartiallyHelper.Primary(_reKey.PrAlphabet, throwException)
                      && External(message, _reKey.ExAlphabet, throwException);
            }



            public bool Primary(string message, bool throwException = false)
                     => Primary(message, _reKey.PrAlphabet, throwException);
            static public bool Primary(string message, string primary, bool throwException = false)
            {
                if (!PartiallyHelper.Primary(primary, throwException)) return false;

                foreach (char c in message)
                {
                    if (!primary.Contains(c))
                    {
                        if (throwException)
                            throw new ArgumentException
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



            public bool External(string encrypted, bool throwException = false)
                     => External(encrypted, _reKey.ExAlphabet, throwException);
            static public bool External(string encrypted, string external, bool throwException = false)
            {
                if (!PartiallyHelper.External(external, throwException)) return false;

                foreach (char c in encrypted)
                {
                    if (!external.Contains(c))
                    {
                        if (throwException)
                            throw new ArgumentException
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
        }
    }
}