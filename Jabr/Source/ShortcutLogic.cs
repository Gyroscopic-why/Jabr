using System;
using System.Collections.Generic;


using static Jabr.GlobalSettings;
using static Jabr.GlobalVariables;
using static Jabr.CryptingLogic;
using static Jabr.ParametersLogicIO;
using System.Linq;



namespace Jabr
{
    internal class ShortcutLogic
    {

        static public void CheckForShortcut(string userInput)
        {
            if (userInput.Length > 8 && (userInput[0] == '+' || userInput[0] == '-'))
            {   // Check for crypting type ("+" encryption,  "-" decryption) for the initial shortcut recognition

                gShortcutError = 2;

                if (userInput[1] == '3' || userInput[1] == '4')
                {   // Check for cipher version, currently supported: RE3, RE4

                    Int32[] jointPositions = { userInput.Length - 1, 0, 0 };
                    // Find the posID of the joints "::" used in shortcuts

                    for (var i = 1; i < 3; i++)
                    {
                        jointPositions[i] = GetJointPosition(userInput, jointPositions[i - 1] - 1);
                        if (jointPositions[i] == -1)
                        {
                            gShortcutError = i + 2;
                            i++;
                        }
                    }

                    if (gShortcutError == 2) ReadShortcut(userInput, NormalizeJointArrayForReading(jointPositions));
                }
                else gShortcutError = 7;
            }
        }
            //  Basic checking for a shortcut existence
            //  If a shortcut is possible - read and parse it

        static public void ReadShortcut(string shortcut, Int32[] jointPos)
        {
            string expAlphabet = shortcut.Substring(jointPos[0] + 2, jointPos[1] - jointPos[0] - 2);
            string temp = shortcut.Substring(jointPos[1] + 2);  //  expected shifts


            if (ParseShiftsFromShortcut(temp, expAlphabet.Length, out List<Int32> expShifts))
            {
                temp = shortcut.Substring(2, jointPos[0] - 2);  //  reuse for storing the message

                //  true = simplified (without extra questions to the user)
                if (CheckAlphabet(expAlphabet, temp, true))
                {
                    if (shortcut[0] == '+')
                         Encrypt(byte.Parse(shortcut[1].ToString()), gShowInfo,
                             temp, expAlphabet, expShifts);
                    else Decrypt(byte.Parse(shortcut[1].ToString()), gShowInfo,
                             temp, expAlphabet, expShifts);

                    gShortcutError = 0;  //  = successfull shortcut parsing and processing
                }
            }
        }
            //  Main logic function for reading and parsing a shortcut
            //  If successfully finds a shortcut - executes it

        static public Int32 GetJointPosition(string tempString, Int32 startID)
        {
            Int32 position = -1;

            for (var i = startID; i > 1 && position == -1; i--)
            {
                if (tempString[i] == ':' && tempString[i - 1] == ':') position = i - 1;
            }

            return position;
        }
            //  Getting the joint possitions in the shortcut (used for parsing)

        static public Int32[] NormalizeJointArrayForReading(Int32[] jointPos)
        {
            Int32[] normalizedJoints = new Int32[2];
            normalizedJoints[0] = jointPos[2];
            normalizedJoints[1] = jointPos[1];
            return normalizedJoints;
        }
        //  Normalizing the joint positions array for easier reading

        static public bool ParseShiftsFromShortcut(string parseInput, Int32 maxValue, out List<Int32> shifts)
        {
            shifts = new List<Int32>();
            Int32 expShCount = parseInput.Count(c => c == ',') + 1;
            Int32 prevShiftsEndId = 0, nextShiftEndId;

            for (var i = 0; i < expShCount; i++)
            {
                if (prevShiftsEndId + 1 > parseInput.Length) break;

                nextShiftEndId = parseInput.IndexOf(',', prevShiftsEndId);
                if (nextShiftEndId == -1) nextShiftEndId = parseInput.Length;

                if (Int32.TryParse
                    (
                        parseInput.Substring
                        (
                            prevShiftsEndId, nextShiftEndId - prevShiftsEndId
                        ),
                        out Int32 buffer
                ))
                {
                    if (buffer > 0 && buffer < maxValue) shifts.Add(buffer);
                    else
                    {
                        gShortcutError = 6;
                        return false;
                    }
                }
                else
                {
                    gShortcutError = 5;
                    return false;
                }

                prevShiftsEndId = nextShiftEndId + 1;
            }

            return true;
        }
    }
}