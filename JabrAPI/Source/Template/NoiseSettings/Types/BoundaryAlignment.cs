namespace JabrAPI.Template
{
    public enum TextOutputBoundaryAlignment
    {
        c32 = 5,
        c64 = 6,
        c128 = 7,
        c256 = 8,
        c512 = 9,

        c1024 = 10,
        c2048 = 11,
        c4096 = 12,
        c8192 = 13,

        c16384 = 14,
        c32768 = 15,
        c65536 = 16,
        c131072 = 17,
        c262144 = 18,
        c524288 = 19,

        c1048576 = 20
    }


    public enum BinaryBoundaryAlignment
    {
        Byte32 = 5,
        Byte64 = 6,
        Byte128 = 7,
        Byte256 = 8,
        Byte512 = 9,

        KByte1 = 10,
        KByte2 = 11,
        KByte4 = 12,
        KByte8 = 13,
        KByte16 = 14,

        KByte32 = 15,
        KByte64 = 16,
        KByte128 = 17,
        KByte256 = 18,
        KByte512 = 19,

        MByte1 = 20
    }
}