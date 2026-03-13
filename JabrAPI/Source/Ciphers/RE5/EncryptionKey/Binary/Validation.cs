using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
        override public ValidateHelper IsValid => _validationHelper;

        public class ValidateHelper : IBinaryValidateHelper
        {
            private readonly BinaryKey _binKey;
            private readonly PartiallyHelper _partiallyHelper;



            internal ValidateHelper(BinaryKey reKey)
            {
                _binKey = reKey;
                _partiallyHelper = new(reKey);
            }



            public PartiallyHelper Partially => _partiallyHelper;
            public class PartiallyHelper
            {
                private readonly BinaryKey _binKey;

                internal PartiallyHelper(BinaryKey reKey)
                {
                    _binKey = reKey;
                }



                public bool Primary(out Exception? exception)
                    => Primary(_binKey.PrAlphabet, out exception);
                static public bool Primary(List<Byte> primary, out Exception? exception)
                {
                    if (primary == null || primary.Count < 2 || primary.Count > 256)
                    {
                        exception = new ArgumentException
                        (
                            "Primary alphabet is not set, too short or too long",
                            nameof(primary)
                        );
                        return false;
                    }
                    for (var curId = 0; curId < primary.Count; curId++)
                    {
                        for (var id2 = curId + 1; id2 < primary.Count; id2++)
                        {
                            if (primary[curId] == primary[id2])
                            {
                                exception = new ArgumentException
                                (
                                    $"Primary alphabet contains duplicates characters" +
                                    $"\nDuplicate byte: {primary[curId]}",
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
                    => Primary(_binKey.PrAlphabet, throwExceptions);
                static public bool Primary(List<Byte> primary, bool throwExceptions = false)
                {
                    bool isValid = Primary(primary, out Exception? exception);
                    if (!isValid && throwExceptions) throw exception!;
                    return isValid;
                }



                public bool External(out Exception? exception)
                    => External(_binKey.ExAlphabet, out exception);
                static public bool External(List<Byte> external, out Exception? exception)
                {
                    if (external == null || external.Count < 2 || external.Count > 256)
                    {
                        exception = new ArgumentException
                        (
                            "External alphabet is not set, too short or too long",
                            nameof(external)
                        );
                        return false;
                    }
                    for (var curId = 0; curId < external.Count; curId++)
                    {
                        for (var id2 = curId + 1; id2 < external.Count; id2++)
                        {
                            if (external[curId] == external[id2])
                            {
                                exception = new ArgumentException
                                (
                                    "External alphabet contains duplicates characters" +
                                    $"Duplicate byte: {external[curId]}",
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
                    => External(_binKey.ExAlphabet, throwExceptions);
                static public bool External(List<Byte> external, bool throwExceptions = false)
                {
                    bool isValid = External(external, out Exception? exception);
                    if (!isValid && throwExceptions) throw exception!;
                    return isValid;
                }
            }



            public override bool ForEncryption(List<Byte> message, out Exception? exception)
            {
                return PartiallyHelper.External(_binKey.ExAlphabet, out exception)
                            && Primary(message, _binKey.PrAlphabet, out exception);
            }
            public override bool ForDecryption(List<Byte> message, out Exception? exception)
            {
                return PartiallyHelper.Primary(_binKey.PrAlphabet, out exception)
                          && External(message, _binKey.ExAlphabet, out exception);
            }

            public override bool ForEncryption(List<Byte> message, bool throwExceptions = false)
            {
                return PartiallyHelper.External(_binKey.ExAlphabet, throwExceptions)
                            && Primary(message, _binKey.PrAlphabet, throwExceptions);
            }
            public override bool ForDecryption(List<Byte> message, bool throwExceptions = false)
            {
                return PartiallyHelper.Primary(_binKey.PrAlphabet, throwExceptions)
                          && External(message, _binKey.ExAlphabet, throwExceptions);
            }



            public bool Primary(List<Byte> message, out Exception? exception)
                => Primary(message, _binKey.PrAlphabet, out exception);
            static public bool Primary(List<Byte> message, List<Byte> primary, out Exception? exception)
            {
                if (!PartiallyHelper.Primary(primary, out exception)) return false;

                foreach (Byte b in message)
                {
                    if (!primary.Contains(b))
                    {
                        exception = new ArgumentException
                        (
                            $"Message contains bytes not present in the primary alphabet" +
                            $"\nMissing byte: {b}",
                            nameof(primary)
                        );
                        return false;
                    }
                }
                return true;
            }

            public bool Primary(List<Byte> message, bool throwExceptions = false)
                => Primary(message, _binKey.PrAlphabet, throwExceptions);
            static public bool Primary(List<Byte> message, List<Byte> primary, bool throwExceptions = false)
            {
                bool isValid = Primary(message, primary, out Exception? exception);
                if (!isValid && throwExceptions) throw exception!;
                return isValid;
            }



            public bool External(List<Byte> encrypted, out Exception? exception)
                => External(encrypted, _binKey.ExAlphabet, out exception);
            static public bool External(List<Byte> encrypted, List<Byte> external, out Exception? exception)
            {
                if (!PartiallyHelper.External(external, out exception)) return false;

                foreach (Byte b in encrypted)
                {
                    if (!external.Contains(b))
                    {
                        exception = new ArgumentException
                        (
                            $"Message contains bytes not present in the external alphabet" +
                            $"\nMissing byte: {b}",
                            nameof(external)
                        );
                        return false;
                    }
                }
                return true;
            }

            public bool External(List<Byte> encrypted, bool throwExceptions = false)
                => External(encrypted, _binKey.ExAlphabet, throwExceptions);
            static public bool External(List<Byte> encrypted, List<Byte> external, bool throwExceptions = false)
            {
                bool isValid = External(encrypted, external, out Exception? exception);
                if (!isValid && throwExceptions) throw exception!;
                return isValid;
            }
        }
    }
}