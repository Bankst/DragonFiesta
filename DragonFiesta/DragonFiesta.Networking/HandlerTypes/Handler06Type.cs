namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler06Type : HandlerType
    {
        public new const byte _Header = 6;


        //CMSG
        public const ushort CMSG_MAP_LOGIN_REQ = 1;

        public const ushort CMSG_MAP_LOGINCOMPLETE_CMD = 3;

        public const ushort CMSG_MAP_TOWNPORTAL_REQ = 26;


        //SMSG
        public const ushort SMSG_MAP_LOGIN_ACK = 2;

        public const ushort SMSG_MAP_LOGINFAIL_ACK = 4;

        public const ushort SMSG_MAP_LOGOUT_CMD = 5;

        public const ushort SMSG_MAP_LINK_REQ = 6;

        public const ushort SMSG_MAP_LINKSAME_CMD = 9;

        public const ushort SMSG_MAP_LINKOTHER_CMD = 10;

        public const ushort SMSG_MAP_TOWNPORTAL_ACK = 27;

        public const ushort SMSG_MAP_LINKEND_CLIENT_CMD = 42;


        //NC
        public const ushort NC_MAP_LINKCANCEL_ACK = 7;

        public const ushort NC_MAP_LINKALLOW_ACK = 8;

        public const ushort NC_MAP_LINKSTART_CMD = 11;

        public const ushort NC_MAP_LINKEND_CMD = 12;

        public const ushort NC_MAP_LINKRESERVE_REQ = 13;

        public const ushort NC_MAP_LINKRESERVE_ACK = 14;

        public const ushort NC_MAP_REGIST_CMD = 15;

        public const ushort NC_MAP_EXPBONUS_RNG = 16;

        public const ushort NC_MAP_ITEMBONUS_RNG = 17;

        public const ushort NC_MAP_FREEPKZONE_ON_CMD = 18;

        public const ushort NC_MAP_FREEPKZONE_OFF_CMD = 19;

        public const ushort NC_MAP_WING_SAVE_REQ = 20;

        public const ushort NC_MAP_WING_SAVE_ACK = 21;

        public const ushort NC_MAP_WING_FLY_REQ = 22;

        public const ushort NC_MAP_WING_FLY_ACK = 23;

        public const ushort NC_MAP_PARTYBATTLEZONE_ON_CMD = 24;

        public const ushort NC_MAP_PARTYBATTLEZONE_OFF_CMD = 25;

        public const ushort NC_MAP_TONORMALCOORD_CMD = 28;

        public const ushort NC_MAP_LINK_FAIL_CMD = 29;

        public const ushort NC_MAP_MULTY_LINK_CMD = 31;

        public const ushort NC_MAP_MULTY_LINK_SELECT_REQ = 31;

        public const ushort NC_MAP_MULTY_LINK_SELECT_ACK = 32;

        public const ushort NC_MAP_FIELD_ATTRIBUTE_CMD = 39;

        public const ushort NC_MAP_KQTEAMBATTLEZONE_ON_CMD = 40;

        public const ushort NC_MAP_KQTEAMBATTLEZONE_OFF_CMD = 41;

        public const ushort NC_MAP_CAN_USE_REVIVEITEM_CMD = 43;

        public const ushort NC_MAP_INDUN_LEVEL_VIEW_CMD = 44;       

    }
}