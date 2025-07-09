

using static Jabr.GlobalSettings;
using static Jabr.GlobalVariables;
using static Jabr.CryptingLogic;
using static Jabr.ParametersLogicIO;


namespace Jabr
{
    internal class ShortcutLogic
    {

        static public void CheckForShortcut(string userInput)
        {
            if (userInput.Length > 8 && (userInput[0] == '+' || userInput[0] == '-'))
            {   // Check for crypting type ("+" encryption,  "-" decryption) for the initial shortcut recognition

                gShortcutError = 2;
                // !!! Only needed for error output in my GetTask function,
                // you can easily delete this in your code if you dont have such a system
                //
                // gShortcutError = 2  means that we found an attempt to use a shortcut
                // So we asume the shortcut recognition failed, so we prepare the error message
                // If the shortcut parse is succesfull, we will reasign the gShortcutError value to 0
                //
                // I do the asigning here for optimisation to the error output
                // However, in the future I will split it to get more details why the parsing failed

                if (userInput[1] == '1' || userInput[1] == '2' || userInput[1] == '3' || userInput[1] == '4')
                {   // Check for cipher version, currently supported: RE1, RE2, RE3, RE4

                    int[] curJointPos = new int[2]; // Find the posID of the joints "::" used in shortcuts

                    curJointPos[0] = GetJointPosition(userInput, userInput.Length - 2);
                    //Try to find the first joint pos

                    if (curJointPos[0] != -1)
                    {   // If the joint was found

                        curJointPos[1] = GetJointPosition(userInput, curJointPos[0] - 1);
                        //Try to find the second joint

                        if (curJointPos[1] != -1)
                        {   // If the second joint was found
                            ReadShortcut(userInput, curJointPos); // Try to find the shortcut
                        }
                    }
                }
            }
        }
            //  Basic checking for a shortcut existence
            //  If a shortcut is possible - read and parse it

        static public void ReadShortcut(string shortcut, int[] jointPos)
        {
            // Note that the joints are stored backwards int the array (jp[0] = joint 2, jp[1] = joint 1)


                //  Expected alphabet from the shortcut
            string expAlphabet = "";


                //  temp will temporary hold the expected shift, then the message
            string temp = ""; 


                //  Expected shift parameter from the shortcut
            int expShift;



            for (int i = jointPos[1] + 2; i < jointPos[0]; i++)
                expAlphabet += shortcut[i]; //  Get the alphabet between joint 1 && 2


            for (int i = jointPos[0] + 2; i < shortcut.Length; i++)
                temp += shortcut[i];        //  Get the shift  after the second joint



            if (int.TryParse(temp, out expShift)) 
            {
                //  Check if the shift is valid (is an integer)

                
                //  Check that the shift is valid
                if (expShift < expAlphabet.Length && expShift > 0) 
                {   
                    //  RE validation is:
                    //  when the shift for the alphabet is more than 0,
                    //  and less than the alphabet length


                    temp = ""; // The message will be stored in "str temp" rather than another string for optimisation

                    for (int i = 2; i < jointPos[1]; i++)
                    {
                        temp += shortcut[i]; // Get the message
                    }

                    if (CheckAlphabet(expAlphabet, temp, true)) //true = simplified (without extra questions to the user)
                    {   //If the alphabet is valid, then use the shortcut

                        if (shortcut[0] == '+') Encrypt(byte.Parse(shortcut[1].ToString()), gShowInfo, temp, gEncrypted, expAlphabet, expShift);
                        else Decrypt(byte.Parse(shortcut[1].ToString()), gShowInfo, gDecrypted, temp, expAlphabet, expShift);
                        


                        gShortcutError = 0;
                        //  !!!!! Only needed for error output in my GetTask function,
                        //  you can easily delete this in your code if you dont have such a system
                        //
                        //  gShortcutError = 0, means that the parsing of a shortcut was succesfull,
                        //  therefore no errors was detected
                    }
                }
            }
        }
            //  Main logic function for reading and parsing a shortcut
            //  If successfully finds a shortcut - executes it

        static public int GetJointPosition(string tempString, int startID)
        {
            int position = -1; // Propose the joint doesn't exist

            for (int i = startID; i > 1 && position == -1; i--)
            {   // start searching for the joint "::" from the end
                if (tempString[i] == ':' && tempString[i - 1] == ':') position = i - 1;
            }   // if the joint "::" was found, return the posID of the first ':' char

            return position;
        }
            //  Getting the joint possitions in the shortcut (used for parsing)

    }
}