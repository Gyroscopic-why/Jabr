using System;



namespace JabrAPI.Template
{
    abstract public class IValidateHelper()
    {
        abstract public bool ForEncryption(string message, out Exception? exception);
        abstract public bool ForDecryption(string message, out Exception? exception);

        abstract public bool ForEncryption(string message, bool throwExceptions = false);
        abstract public bool ForDecryption(string message, bool throwExceptions = false);
    }
}