namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler05Type : HandlerType
    {
        public new const byte _Header = 5;


        //CMSG
        public const ushort CMSG_AVATAR_CREATE_REQ = 1;

        public const ushort CMSG_AVATAR_ERASE_REQ = 7;

        public const ushort CMSG_AVATAR_RENAME_REQ = 15;


        //SMSG
        public const ushort SMSG_AVATAR_CREATEFAIL_ACK = 4;

        public const ushort SMSG_AVATAR_CREATESUCC_ACK = 6;
        
        public const ushort SMSG_AVATAR_ERASEFAIL_ACK = 10;        
 
        public const ushort SMSG_AVATAR_ERASESUCC_ACK = 12;        

        public const ushort SMSG_AVATAR_RENAME_ACK = 16;


        //NC
        public const ushort NC_AVATAR_CREATEDATA_REQ = 2;

        public const ushort NC_AVATAR_CREATEDATAFAIL_ACK = 3;

        public const ushort NC_AVATAR_CREATEDATASUC_ACK = 5;

        public const ushort NC_AVATAR_ERASEDATA_REQ = 8;

        public const ushort NC_AVATAR_ERASEDATAFAIL_ACK = 9;

        public const ushort NC_AVATAR_ERASEDATASUC_ACK = 11;

        public const ushort NC_USP_USER_CHARACTER_INSERT = 13;

        public const ushort NC_USP_USER_CHARACTER_DELETE = 14;

        public const ushort NC_AVATAR_RENAME_DB_REQ = 17;

        public const ushort NC_AVATAR_RENAME_DB_ACK = 18;

        public const ushort NC_AVATAR_GUILD_DATA_REQ = 19;

        public const ushort NC_AVATAR_GUILD_DATA_ACK = 20;

        public const ushort NC_AVATAR_GUILD_MEMBER_DATA_REQ = 21;

        public const ushort NC_AVATAR_GUILD_MEMBER_DATA_ACK = 22;        

    }
}