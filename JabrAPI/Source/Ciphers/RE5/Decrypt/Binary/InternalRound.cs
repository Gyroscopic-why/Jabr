using System;
using System.Text;
using System.Collections.Generic;


using AVcontrol;



namespace JabrAPI
{
    static public partial class RE5
    {
        static internal partial class Internal
        {
            static public List<Byte> DecryptionRound(
                List<Byte> encryptedChunk,
                List<Byte> prAlphabet, List<Byte> exAlphabet,
                List<Byte> shifts,
                Int32 exLength,
                Int32 maxEncodingLength,
                Int32 realMessageLength,
                ref Int32 decodedId)
            {
                Int32 shCount = shifts.Count, encCurId = 0;
                Int32 parsedEncoding = (Int32)Numsys.ToDecimalFromCustom128
                (
                    encryptedChunk[1..maxEncodingLength],
                    exLength,
                    exAlphabet
                );


                decodedId = exAlphabet.IndexOf(encryptedChunk[0])
                    - decodedId
                    - shifts[0]
                    + parsedEncoding * exLength;


                List<Byte> decrypted = new(realMessageLength);
                decrypted.AddRange(prAlphabet[decodedId]);


                for (var curId = 1; curId < realMessageLength; curId++)
                {
                    encCurId += maxEncodingLength;
                    decodedId = exAlphabet.IndexOf(encryptedChunk[encCurId])
                        - decodedId
                        - shifts[curId % shCount];

                    parsedEncoding = (Int32)Numsys.ToDecimalFromCustom128
                    (
                        encryptedChunk[(encCurId + 1)..(encCurId + maxEncodingLength)],
                        exLength,
                        exAlphabet
                    );

                    decodedId += parsedEncoding * exLength;
                    decrypted.AddRange(prAlphabet[decodedId]);
                }

                return decrypted;
            }
        }
    }
}