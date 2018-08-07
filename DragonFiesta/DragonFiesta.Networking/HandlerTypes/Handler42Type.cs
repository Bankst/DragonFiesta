namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler42Type : HandlerType
    {

        public new const byte _Header = 42;

        //CMSG
        public const ushort CMSG_CHAT_RESTRICT_ADD_REQ = 3;

        public const ushort CMSG_CHAT_RESTRICT_DEL_REQ = 7;

        public const ushort CMSG_CHAT_RESTRICT_DEL_ALL_REQ = 11;


        //SMSG
        public const ushort SMSG_CHAT_RESTRICT_LIST_CMD = 2;

        public const ushort SMSG_CHAT_RESTRICT_ADD_ACK = 6;

        public const ushort SMSG_CHAT_RESTRICT_DEL_ACK = 10;

        public const ushort SMSG_CHAT_RESTRICT_DEL_ALL_ACK = 14;


        //NC
        public const ushort NC_CHAT_RESTRICT_DB_LIST_CMD = 1;

        public const ushort NC_CHAT_RESTRICT_DB_ADD_REQ = 4;

        public const ushort NC_CHAT_RESTRICT_DB_ADD_ACK = 5;

        public const ushort NC_CHAT_RESTRICT_DB_DEL_REQ = 8;

        public const ushort NC_CHAT_RESTRICT_DB_DEL_ACK = 9;

        public const ushort NC_CHAT_RESTRICT_DB_DEL_ALL_REQ = 12;

        public const ushort NC_CHAT_RESTRICT_DB_DEL_ALL_ACK = 13;    

    }
}