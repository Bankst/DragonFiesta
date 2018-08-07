namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler21Type : HandlerType
    {
        public new const byte _Header = 21;

        
        //CMSG
        public const ushort CMSG_FRIEND_SET_REQ = 1;

        public const ushort CMSG_FRIEND_SET_CONFIRM_ACK = 4;

        public const ushort CMSG_FRIEND_DEL_REQ = 5;

        public const ushort CMSG_FRIEND_POINT_REQ = 29;

        public const ushort CMSG_FRIEND_FIND_FRIENDS_REQ = 31;

        public const ushort CMSG_FRIEND_UES_FRIEND_POINT_REQ = 33;


        //SMSG
        public const ushort SMSG_FRIEND_SET_ACK = 2;

        public const ushort SMSG_FRIEND_SET_CONFIRM_REQ = 3;

        public const ushort SMSG_FRIEND_DEL_ACK = 6;

        public const ushort SMSG_FRIEND_LIST_CMD = 7;

        public const ushort SMSG_FRIEND_ADD_CMD = 8;

        public const ushort SMSG_FRIEND_LOGIN_CMD = 9;

        public const ushort SMSG_FRIEND_LOGOUT_CMD = 10;

        public const ushort SMSG_FRIEND_REFUSE_CMD = 11;

        public const ushort SMSG_FRIEND_DEL_CMD = 12;

        public const ushort SMSG_FRIEND_MAP_CMD = 13;

        public const ushort SMSG_FRIEND_PARTY_CMD = 14;

        public const ushort SMSG_FRIEND_LEVEL_CMD = 15;

        public const ushort SMSG_FRIEND_CLASS_CHANGE_CMD = 24;

        public const ushort SMSG_FRIEND_POINT_ACK = 30;

        public const ushort SMSG_FRIEND_FIND_FRIENDS_ACK = 32;

        public const ushort SMSG_FRIEND_UES_FRIEND_POINT_ACK = 34;

        public const ushort SMSG_FRIEND_GET_DIFF_FRIEND_POINT_CMD = 37;

        
        //NC
        public const ushort NC_FRIEND_DB_SET_REQ = 16;

        public const ushort NC_FRIEND_DB_SET_ACK = 17;

        public const ushort NC_FRIEND_DB_DEL_REQ = 18;

        public const ushort NC_FRIEND_DB_DEL_ACK = 19;

        public const ushort NC_FRIEND_DB_GET_REQ = 20;

        public const ushort NC_FRIEND_DB_GET_ACK = 21;

        public const ushort NC_FRIEND_DB_POINT_CMD = 28;  
     
        public const ushort NC_FRIEND_SOMEONE_GET_SPECIALITEM_ZONE_CMD = 35;

        public const ushort NC_FRIEND_SOMEONE_GET_SPECIALITEM_WORLD_CMD = 36;

    }
}