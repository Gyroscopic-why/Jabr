using System;


using JabrAPI.Template;



namespace JabrAPI.RE5
{
    public partial class EncryptionKey : IEncryptionKey
    {
        public bool IsPrimaryValid(string message, bool throwException = false)
                 => IsPrimaryValid(message, _primaryAlphabet, throwException);
        static public bool IsPrimaryValid(string message, string primary, bool throwException = false)
        {
            if (!IsPrimaryPartiallyValid(primary, throwException)) return false;

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

        public bool IsPrimaryPartiallyValid(bool throwException = false)
                 => IsPrimaryPartiallyValid(_primaryAlphabet, throwException);
        static public bool IsPrimaryPartiallyValid(string primary, bool throwException = false)
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



        public bool IsExternalValid(string encrypted, bool throwException = false)
                 => IsExternalValid(encrypted, _externalAlphabet, throwException);
        static public bool IsExternalValid(string encrypted, string external, bool throwException = false)
        {
            if (!IsExternalPartiallyValid(external, throwException)) return false;

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

        public bool IsExternalPartiallyValid(bool throwException = false)
                 => IsExternalPartiallyValid(_externalAlphabet, throwException);
        static public bool IsExternalPartiallyValid(string external, bool throwException = false)
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
}