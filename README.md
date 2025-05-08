# Jabr
This is an Indie-software tool - **encryptor/decryptor.**

UI is currently only on russian, however the program **DOES support english** (also almost all UTF-8 characters in the latest version) inputs.

This is **no longer a small program** that I have been working on, it _quickly_ turned into one one my favourite projects that I have been developing alone **since August 2024.**

It is capable of **encrypting and decrypting messages** with almost any **UTF-8** caracters.


Fell free to ask questions here, or at my email: **_questions.gyroscopic@gmail.com_**

Feel free to use the algorithm/program/code, 
just **please add a dedication/credit/github link to my original work :D**




# _Why is this program important?_
- Because in Jabr, the encryption and decryption processes happen via **my own algorithm** (called RE for short)
- Which almost has millitary grade security
- The cipher parameters, and encrypted messages are super compact
- And you won't find them anywhere else as of now.


  
# **Rules:**

### **Currently the algorithms uses** an Shift-code, and an alphabet

**The alphabet must follow these rules:**
 -     Must include all the UTF-8 characters from the message including dots, spaces etc.
 -     Every symbol must only be used once in the alphabet
 -     It may also contain other UTF-8 characters, if the user wants so (also used only once)
 -     Must be at least 2 symbols long or the message wont be encrypted
 **The Shift-key must follow these rules:** 
 -     Must be a natural number
 -     Larger than 0
 -     Samller than the alphabet length



# **How the ciphers work**
I won't really go into precise details right now but basically **here is how RE3 and lower algorithms**:
-      You go through the message and replace the message character by the alphabets[Encryption Index]
-      For the FIRST letter from the original message:
     -      You fing the original character (at position CurrentPosition) the index it is stored in the alphabet
     -      You add the shift code to the found index
     -      You add the character at the new possition to the encrypted message:
            Encrypted += Alphabet[Alphabet.IndexOf(Message[CurrentPosition]) + ShiftCode];
-      For the REMAINING letters in the original message:
     -      You fing the original character (at position CurrentPosition) the index it is stored in the alphabet
     -      You add the previous characters index in the alphabet
     -      You add the character at the new possition to the encrypted message:
            Encrypted += Alphabet[Alphabet.IndexOf(Message[CurrentPosition]) + Alphabet.IndexOf(Message[CurrentPosition - 1])];
        
 **RE4 have the shift code more randomly shuffle all the index's** (not only the first one) so it matters a lot more and is significantly more safe than RE3 and lower

 _When talking about almost millitary grade security I mean the RE4 algorithm_



 # **How the shortcuts work: (only relevant to v1.4.2)**
 -    You need to navigate to the settings to make sure the shortcut recognition is turned on
 -    Then you navigate back to the main menu
 -    Instead of entering the number representing the task you want to do ('2' - encrypt, '3' - decrypt)
      Start by entering one the special shortcut symbols: '+' for encoding, '-' for decoding
 -    Then enter 1 number representing the version of the cipher you want to use for the crypting process ('1' - RE1, '2' - RE2, '3' - RE3, '4' - RE4)
 -    After that start entering the message that you are going to encrypt/decrypt
 -    When you are done entering your message, use a spliter "::" to help the program distinguish the message from the alphabet
 -    Enter the alphabet after you have enterent the spliter
 -    Enter the splitter "::" a secong time
 -    Enter the shift for the alphabet
 -    Press enter for the shortcut to execute

     
## **Notes for shortcuts:**
- The shortcut must be one command
- You will get an shortcut read error if one or more of the parameters doesn't follow the rules


## **Shortcut examples:**

### **Example 1:**

**Input:** 
-     +4qwerty::qwertyuiop::2
**Output:**
-     erroqi 
      // The shortcut was successfully recognised and executed


### **Example 2:**

**Input:**
-     -3helloworld::qwertyuiop:2
**Output:**
-     Error while reading shortcut
      // The alphabet doesn't contain all the characters from the message


### **Example 3:**

**Input:**
-     +5aboba::leobang::3
**Output:**
-     Error while reading shortcut
      // The cipher version is invalid, RE5 is currently in the works


### **Example 4:**

**Input:**
-     =2qwerty::qwertyuiop::2
**Output:**
-     Error while reading user input
      // '=' is not a shortcut key


 _**This is only relevant to v1.4.2, the other versions don't support shortcuts yet**_





# Different versions and progress:

## **Gen 1 (first prototypes)**
**Versions:**
- _v1.0_ - Lost in time
- _v1.1_ - Lost in time






## **Gen 2 (26.10.2024)**

- **v1.2 New features:**
    - **Full UI rework**
    - Optimisation
    - Minor bug fixes

**Versions:**
- _v1.2 alpha_ --- unfinished
- _v1.2 beta_  ---- New UI, has dev info
- _v1.2.1_ -------- Improved UI, has dev info, minor bug fixes






## **Gen 3 (26.10.2024)**

- **v1.3 New features:**
    - **Split the encryptiom and decryption part into reusable functions :D**
    - Code refactoring
    - Improved UI
    - Minor bug fixes

**Versions:**
- _v1.3 beta_ --- Unfinished, has dev info
- _v1.3_C_ ------ finished & clean version, has dev info
- _v1.3_O_ ------ optimised version, without dev info, works faster than other Gen 3 variants






## **Gen 4 (22.11.2024 - 11.3.2025)**

- **v1.4   Global update - New features: (22.11.2024)**
    - **New and improved encryption/decryption algorith version with improved security (RE4)**
    - **Ability to change ciphers (RE1,  RE2,  RE3,  and  RE4)** (RE2 is curently only accessible in v1.4.2 beta)
    - **Added switchable settings**
    - **Fixed advanced info display bug about the encryption/decryption process**
    - Improved UI
    - Massive optimisation
    - Fixed spelling mistakes
    
- **For developers/enthusiasts:**
    - **Split the project into 3 files**
    - **Made a test chamber with extra debugging info and simple UI**
    - Cleaned the code
    - Removed old useless libraries




- **v1.4.1   QOL patch - New features: (20.1.2025)**
    - **New settings option: switchable input style for better user experience**
    - **Fixed inconsistent spacing issue**
    - Changed settings UI for future scaleability
    - Improved info output
    - Improved performance
    - Fixed spelling mistakes

- **For developers/enthusiasts:**
    - **Massive code refactoring for easier integrations**
    - Removed old useless code
    - Test chamber code cleaning




**v1.4.2 beta   Medium update:**
- **New features:**
    - **New settings feature: enabling using of shortcuts for fast encrypting/decrypting**
    - **Added shortcut parsing for user inputs in the task selection menu**
    - **Fixed info output**
    - Added primitive error output for parsing shortcuts
    - Improved performance
    - Fixed spelling mistakes
    - Changed the writing in some places


- **For developers/enthusiasts:**
    - **Massive code refactoring for easier integrations (although a bit unfinished)**
    - **Added a lot of comments (also for old code)**
    - Removed old useless code

**Versions:**
- _v1.4_
- _v1.4 TestChamber_
- _v1.4.1_
- _v1.4.1 TestChamber_
- _v1.4.2 beta_


## **Gen 5**
_*Work in progress,_ est. finish date - end of april 2025



# **Expected features (Not exclusively for Gen 5):**
- **Implemented from this list:**
    + (+)  Settings menu ----------------------------------------------------------------- Implemented in v1.4
    + (+)  Other ciphers to chose from in settings (?) ----------------------------------- Implemented in v1.4
    + (+)  Fast base encryption/decryption options (simplified version of the UI) ----- Implemented in v1.4.1
    + (+)  Fast input system (v1.4.2 beta exclusive feature) ----------------------------- Implemented in v1.4.2 beta
      
**Currently in development:**
- Randomly generating codes & cipher parameters
- English UI
- Advanced settings menu
- Hashing for encryption/decryption option
- File encryption/decryption
- Custom cipher options
- Multi-layer encryption/decryption
- Persistent settings
- Better installation guide (after persistent settings)

**"(+)": Currently already implemented in the latest version (v1.4.2 beta)**






## **Not defined future plans:**

- Add multiple encryption/decryption at the same time
- Integrate other ciphers in the program (not RE algorithms)
- Add optional parameter bruteforcing using a bit of known information
- Add graphical interface 
- Enable creating of encryption/decryption .txt files




The program was made for fun and educational purposes, and is not meant to hurt anyone.

**New features will be added as time passes.**
