


namespace JabrAPI.Template
{
    abstract public class IValidateHelper()
    {
        abstract public bool ForEncryption(string message, bool throwException = false);
        abstract public bool ForDecryption(string message, bool throwException = false);
    }
}