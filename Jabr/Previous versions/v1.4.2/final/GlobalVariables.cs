using System;


namespace Jabr
{
    internal class GlobalVariables
    {

            //  Static random number generator object
        static public Random gRandom = new Random();


            //  Storing the input, and result
            //  for the ecnryption/decryption process
            //  
            //  Example (if we are encrypting):
            //   - gDecrypted - input of the original message
            //   - gEncrypted - result of the encryption process
            //
            //  And vise-versa for the dercyption process
        static public string gEncrypted = "", gDecrypted = "";


            //  Storing the alphabet key
            //  for the encryption/decryption processes
        static public string gAlphabet;


            //  String the current program version
        static public readonly string gProgramVersion = "v1.4.2";
        

            //  Stroring the character shift from the alphabet
            //  for the encryption/decryption processes
        static public int gShift;


            //  Shortcut errors:     0 - shortcut was succesfull
            //                       1 - no signs of shortcuts was found in the user input
            //                       2 - error while reading shortcut
            //
            //  I will add more errors in the future
            //  (split the "2" option into multiple more detailed exceptions)
        static public byte gShortcutError = 1;
    }
}