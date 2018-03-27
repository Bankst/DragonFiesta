namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler31Type : HandlerType
    {
        public new const byte _Header = 31;


        //CMSG
        public const ushort CMSG_PRISON_GET_REQ = 6;


        //SMSG
        public const ushort SMSG_PRISON_GET_ACK = 7;


        //NC
        public const ushort NC_PRISON_ADD_GM_REQ = 1;

        public const ushort NC_PRISON_ADD_GM_ACK = 2;

        public const ushort NC_PRISON_OK_CMD = 3;

        public const ushort NC_PRISON_END_REQ = 4;

        public const ushort NC_PRISON_END_ACK = 5;

        public const ushort NC_PRISON_UNDOING_CMD = 8;

        public const ushort NC_PRISON_ALTER_GM_REQ = 9;

        public const ushort NC_PRISON_ALTER_GM_ACK = 10;

        public const ushort NC_DATA_PRISON_ADD_GM_REQ = 11;

        public const ushort NC_DATA_PRISON_ADD_GM_ACK = 12;

        public const ushort NC_DATA_PRISON_UPDATE_MIN_CMD = 13;

        public const ushort NC_DATA_PRISON_GET_REQ = 14;

        public const ushort NC_DATA_PRISON_GET_ACK = 15;

        public const ushort NC_DATA_PRISON_ALTER_GM_REQ = 16;

        public const ushort NC_DATA_PRISON_ALTER_GM_ACK = 17;

        public const ushort NC_ZONE_PRISON_END_CMD = 18;

        public const ushort NC_ZONE_PRISON_GO_REQ = 19;

        public const ushort NC_ZONE_PRISON_GO_ACK = 20;

        public const ushort NC_PRISON_GIVE_UP_REQ = 21;

        public const ushort NC_PRISON_GIVE_UP_FAIL_ACK = 22;

        public const ushort NC_PRISON_ADD_REQ = 23;

        public const ushort NC_PRISON_ADD_ACK = 24;

    }
}