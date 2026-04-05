


namespace JabrAPI
{
    public enum BinaryChunkSize
    {
        bTEST = 4,
        Byte256  = 256,
        Byte512  = 512,
        
        KByte1   =  1_024,
        KByte2   =  2_048,
        KByte4   =  4_096,
        KByte8   =  8_192,
        KByte16  = 16_284,

        KByte32  =  32_768,
        KByte64  =  65_536,
        KByte128 = 131_072,
        KByte256 = 262_144,
        KByte512 = 524_288,

        MByte1   = 1_048_576
    }


    public enum TextChunkSize
    {
        c256 = 256,
        c512 = 512,

        c1024 = 1_024,
        c2048 = 2_048,
        c4096 = 4_096,
        c8192 = 8_192,

        c16384  =  16_384,
        c32768  =  32_768,
        c65536  =  65_536,
        c131072 = 131_072,
        c262144 = 262_144,
        c524288 = 524_288,

        c1048576 = 1_048_576
    }
}