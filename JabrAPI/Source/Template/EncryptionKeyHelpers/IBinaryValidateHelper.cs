using System;
using System.Collections.Generic;



namespace JabrAPI.Template
{
    abstract public class IBinaryValidateHelper()
    {
        abstract public bool ForEncryption(List<Byte> message, bool throwException = false);
        abstract public bool ForDecryption(List<Byte> message, bool throwException = false);
    }
}