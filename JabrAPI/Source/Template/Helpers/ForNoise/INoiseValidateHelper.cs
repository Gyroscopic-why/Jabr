


namespace JabrAPI.Template
{
    abstract public class INoiseValidateHelper()
    {
        abstract public bool ForAdding  (string message, bool   throwException = false);
        abstract public bool ForAdding  (IEncryptionKey  reKey, string message,  bool throwException = false);
        abstract public bool ForRemoving(IEncryptionKey  reKey, string message,  bool throwException = false);
    }
}