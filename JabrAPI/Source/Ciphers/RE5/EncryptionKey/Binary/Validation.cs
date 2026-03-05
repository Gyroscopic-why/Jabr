using System;
using System.Collections.Generic;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class BinaryKey : IBinaryKey
    {
        public bool IsPrimaryValid(List<Byte> message, bool throwException = false)
            => IsPrimaryValid(message, _primaryAlphabet, throwException);
        static public bool IsPrimaryValid(List<Byte> message, List<Byte> primary, bool throwException = false)
        {
            if (!IsPrimaryPartiallyValid(primary, throwException)) return false;

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

        public bool IsPrimaryPartiallyValid(bool throwException = false)
            => IsPrimaryPartiallyValid(_primaryAlphabet, throwException);
        static public bool IsPrimaryPartiallyValid(List<Byte> primary, bool throwException = false)
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



        public bool IsExternalValid(List<Byte> encrypted, bool throwException = false)
            => IsExternalValid(encrypted, _externalAlphabet, throwException);
        static public bool IsExternalValid(List<Byte> encrypted, List<Byte> external, bool throwException = false)
        {
            if (!IsExternalPartiallyValid(external, throwException)) return false;

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

        public bool IsExternalPartiallyValid(bool throwException = false)
            => IsExternalPartiallyValid(_externalAlphabet, throwException);
        static public bool IsExternalPartiallyValid(List<Byte> external, bool throwException = false)
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
}