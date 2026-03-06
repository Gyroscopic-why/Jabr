using JabrAPI.Template;
using System;
using System.Collections.Generic;
using static JabrAPI.RE5.EncryptionKey.ValidateHelper;



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
                


                public bool Primary(bool throwException = false)
                    => Primary(_binKey.PrAlphabet, throwException);
                static public bool Primary(List<Byte> primary, bool throwException = false)
                {
                    if (primary == null || primary.Count < 2 || primary.Count > 256)
                    {
                        if (throwException)
                        {
                            throw new ArgumentException
                            (
                                "Primary alphabet is not set, too short or too long",
                                nameof(primary)
                            );
                        }
                        return false;
                    }
                    for (var curId = 0; curId < primary.Count; curId++)
                    {
                        for (var id2 = curId + 1; id2 < primary.Count; id2++)
                        {
                            if (primary[curId] == primary[id2])
                            {
                                if (throwException)
                                {
                                    throw new ArgumentException
                                    (
                                        $"Primary alphabet contains duplicates characters" +
                                        $"\nDuplicate byte: {primary[curId]}",
                                        nameof(primary)
                                    );
                                }
                                return false;
                            }
                        }
                    }

                    return true;
                }


                public bool External(bool throwException = false)
                    => External(_binKey.ExAlphabet, throwException);
                static public bool External(List<Byte> external, bool throwException = false)
                {
                    if (external == null || external.Count < 2 || external.Count > 256)
                    {
                        if (throwException)
                        {
                            throw new ArgumentException
                            (
                                "External alphabet is not set, too short or too long",
                                nameof(external)
                            );
                        }
                        return false;
                    }
                    for (var curId = 0; curId < external.Count; curId++)
                    {
                        for (var id2 = curId + 1; id2 < external.Count; id2++)
                        {
                            if (external[curId] == external[id2])
                            {
                                if (throwException)
                                {
                                    throw new ArgumentException
                                    (
                                        "External alphabet contains duplicates characters" +
                                        $"Duplicate byte: {external[curId]}",
                                        nameof(external)
                                    );
                                }
                                return false;
                            }
                        }
                    }

                    return true;
                }
            }



            public override bool ForEncryption(List<Byte> message, bool throwException = false)
            {
                return PartiallyHelper.External(_binKey.ExAlphabet, throwException)
                    && Primary(message, _binKey.PrAlphabet, throwException);
            }
            public override bool ForDecryption(List<Byte> message, bool throwException = false)
            {
                return PartiallyHelper.Primary(_binKey.PrAlphabet, throwException)
                        && External(message, _binKey.ExAlphabet, throwException);
            }



            public bool Primary(List<Byte> message, bool throwException = false)
                => Primary(message, _binKey.PrAlphabet, throwException);
            static public bool Primary(List<Byte> message, List<Byte> primary, bool throwException = false)
            {
                if (PartiallyHelper.Primary(primary, throwException)) return false;

                foreach (Byte b in message)
                {
                    if (!primary.Contains(b))
                    {
                        if (throwException)
                        {
                            throw new ArgumentException
                            (
                                $"Message contains bytes not present in the primary alphabet" +
                                $"\nMissing byte: {b}",
                                nameof(primary)
                            );
                        }
                        return false;
                    }
                }
                return true;
            }



            public bool External(List<Byte> encrypted, bool throwException = false)
                => External(encrypted, _binKey.ExAlphabet, throwException);
            static public bool External(List<Byte> encrypted, List<Byte> external, bool throwException = false)
            {
                if (!PartiallyHelper.External(external, throwException)) return false;

                foreach (Byte b in encrypted)
                {
                    if (!external.Contains(b))
                    {
                        if (throwException)
                        {
                            throw new ArgumentException
                            (
                                $"Message contains bytes not present in the external alphabet" +
                                $"\nMissing byte: {b}",
                                nameof(external)
                            );
                        }
                        return false;
                    }
                }
                return true;
            }
        }
    }
}