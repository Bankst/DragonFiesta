namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler03Type : HandlerType
    {
        public new const byte _Header = 3;
    
        //CMSG
        public const ushort CMSG_USER_XTRAP_REQ = 4;

        public const ushort CMSG_USER_WORLDSELECT_REQ = 11;

        public const ushort CMSG_USER_LOGINWORLD_REQ = 15;

        public const ushort CMSG_USER_NORMALLOGOUT_CMD = 24;

        public const ushort CMSG_USER_WORLD_STATUS_REQ = 27;

        public const ushort CMSG_USER_AVATAR_LIST_REQ = 31;

        public const ushort CMSG_USER_WILL_WORLD_SELECT_REQ = 51;

        public const ushort CMSG_USER_LOGIN_WITH_OTP_REQ = 55;

        public const ushort CMSG_USER_GER_LOGIN_REQ = 56;

        public const ushort CMSG_USER_US_LOGIN_REQ = 90;

        public const ushort CMSG_USER_CLIENT_VERSION_CHECK_REQ = 101;


        //SMSG
        public const ushort SMSG_USER_XTRAP_ACK = 5;

        public const ushort SMSG_USER_LOGINFAIL_ACK = 9;

        public const ushort SMSG_USER_LOGIN_ACK = 10;

        public const ushort SMSG_USER_WORLDSELECT_ACK = 12;

        public const ushort SMSG_USER_LOGINWORLD_ACK = 20;

        public const ushort SMSG_USER_LOGINWORLDFAIL_ACK = 21;

        public const ushort SMSG_USER_CONNECTCUT_CMD = 23;

        public const ushort SMSG_USER_WORLD_STATUS_ACK = 28;

        public const ushort SMSG_USER_WILL_WORLD_SELECT_ACK = 52;

        public const ushort SMSG_USER_CLIENT_WRONGVERSION_CHECK_ACK = 102;

        public const ushort SMSG_USER_CLIENT_RIGHTVERSION_CHECK_ACK = 103;  


        //NC

        public const ushort NC_USER_LOGIN_REQ = 6;

        public const ushort NC_USER_PASSWORD_CHECK_REQ = 7;

        public const ushort NC_USER_PASSWORD_CHECK_ACK = 8;

        public const ushort NC_USER_WILLLOGIN_REQ = 13;

        public const ushort NC_USER_WILLLOGIN_ACK = 14;

        public const ushort NC_USER_LOGIN_DB = 16;

        public const ushort NC_USER_LOGOUT_DB = 17;

        public const ushort NC_USER_AVATARINFO_REQ = 18;

        public const ushort NC_USER_AVATARINFO_ACK = 19;

        public const ushort NC_USER_KICKOFFFROMWORLD_CMD = 22;

        public const ushort NC_USER_CONNECTCUT2ZONE_CMD = 25;

        public const ushort NC_USER_CONNECTCUT2WORLDMANAGER_CMD = 26;
        
        public const ushort NC_USER_LOGIN_NETMARBLE_REQ = 29;

        public const ushort NC_USER_LOGIN_NETMARBLE_DB_REQ = 30;

        public const ushort NC_USER_LOGIN_OUTSPARK_REQ = 32;

        public const ushort NC_USER_TEENAGER_CMD = 37;

        public const ushort NC_USER_TEENAGER_REQ = 38;

        public const ushort NC_USER_TEENAGER_ACK = 39;

        public const ushort NC_USER_TEENAGER_SET_CMD = 40;

        public const ushort NC_USER_TEENAGER_REMAIN_MIN_CMD = 41;

        public const ushort NC_USER_IS_IP_BLOCK_REQ = 42;

        public const ushort NC_USER_IS_IP_BLOCK_ACK = 43;

        public const ushort NC_USER_POSSIBLE_NEW_CONNECT_CMD = 49;

        public const ushort NC_USER_USE_BEAUTY_SHOP_CMD = 50;

        public const ushort NC_USER_CREATE_OTP_REQ = 53;

        public const ushort NC_USER_CREATE_OTP_ACK = 54;

        public const ushort NC_USER_GER_PASSWORD_CHECK_REQ = 57;

        public const ushort NC_USER_GER_PASSWORD_CHECK_ACK = 58;

        public const ushort NC_USER_GER_IS_IP_BLOCK_REQ = 59;

        public const ushort NC_USER_GER_IS_IP_BLOCK_ACK = 60;

        public const ushort NC_USER_TW_LOGIN_REQ = 61;

        public const ushort NC_USER_TW_PASSWORD_CHECK_REQ = 62;

        public const ushort NC_USER_TW_PASSWORD_CHECK_ACK = 63;

        public const ushort NC_USER_TW_IS_IP_BLOCK_REQ = 64;

        public const ushort NC_USER_TW_IS_IP_BLOCK_ACK = 65;

        public const ushort NC_USER_JP_LOGIN_REQ = 74;

        public const ushort NC_USER_JP_PASSWORD_CHECK_REQ = 75;

        public const ushort NC_USER_JP_PASSWORD_CHECK_ACK = 76;

        public const ushort NC_USER_JP_IS_IP_BLOCK_REQ = 77;

        public const ushort NC_USER_JP_IS_IP_BLOCK_ACK = 78;

        public const ushort NC_USER_CH_LOGIN_REQ = 82;

        public const ushort NC_USER_CH_PASSWORD_CHECK_REQ = 83;

        public const ushort NC_USER_CH_PASSWORD_CHECK_ACK = 84;

        public const ushort NC_USER_CH_IS_IP_BLOCK_REQ = 85;

        public const ushort NC_USER_CH_IS_IP_BLOCK_ACK = 86;

        public const ushort NC_USER_TUTORIAL_CAN_SKIP_CMD = 95;

        public const ushort NC_USER_RETURN_CHECK_REQ = 96;

        public const ushort NC_USER_RETURN_CHECK_ACK = 97;

        public const ushort NC_USER_LOGOUT_LAST_DAY_REQ = 98;

        public const ushort NC_USER_LOGOUT_LAST_DAY_ACK = 99;

        public const ushort NC_USER_SET_RETURN_CMD = 100;

    }
}