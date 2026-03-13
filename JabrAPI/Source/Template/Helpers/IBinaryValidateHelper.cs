using System;
using System.Collections.Generic;



namespace JabrAPI.Template
{
    abstract public class IBinaryValidateHelper()
    {
        abstract public bool ForEncryption(List<Byte> message, out Exception? exception);
        abstract public bool ForDecryption(List<Byte> message, out Exception? exception);

        abstract public bool ForEncryption(List<Byte> message, bool throwExceptions = false);
        abstract public bool ForDecryption(List<Byte> message, bool throwExceptions = false);
    }
}