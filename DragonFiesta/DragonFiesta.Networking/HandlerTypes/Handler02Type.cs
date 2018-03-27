namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler02Type : HandlerType
    {
        public new const byte _Header = 2;


        //CMSG
        public const ushort CMSG_MISC_HEARTBEAT_ACK = 5;

        public const ushort CMSG_GAMETIME_REQ = 13;


        //SMSG
        public const ushort SMSG_MISC_HEARTBEAT_REQ = 4;

        public const ushort SMSG_MISC_SEED_ACK = 7;

        public const ushort SMSG_GAMETIME_ACK = 14;

        public const ushort SMSG_MISC_RESTMINUTE_CMD = 16;

        public const ushort SMSG_MISC_PINGTEST_TOOL_WM_CLIENT_ZONE_DB = 19;

        public const ushort SMSG_MISC_GET_CHAT_BLOCK_SPAM_FILTER_CMD = 69;

        public const ushort SMSG_CHAT_BLOCK = 72;

        public const ushort SMSG_SERVER_TIME_NOTIFY_CMD = 73;


        //NC
        public const ushort NC_MISC_S2SCONNECTION_RDY = 1;

        public const ushort NC_MISC_S2SCONNECTION_REQ = 2;

        public const ushort NC_MISC_S2SCONNECTION_ACK = 3;

        public const ushort NC_MISC_SEED_REQ = 6;

        public const ushort NC_MISC_USER_COUNT_CMD = 8;

        public const ushort NC_MISC_CONNECTER_REQ = 9;

        public const ushort NC_MISC_CONNECTER_ACK = 10;

        public const ushort NC_DELIVER_WM_LOGIN_ACDB_CMD = 11;

        public const ushort NC_MISC_DELIVER_WM_LOGIN_ALDB_CMD = 12;

        public const ushort NC_MISC_CLIENT_DEBUG_MSG_CMD = 15;

        public const ushort NC_MISC_ZONERINGLINKTEST_RNG = 17;

        public const ushort NC_MISC_PINGTEST_CLIENT_ZONE_DB = 18;

        public const ushort NC_MISC_PINGTEST_TOOL_WM_DB = 20;

        public const ushort NC_MISC_PINGTEST_TOOL_WM_ZONE_DB = 21;

        public const ushort NC_MISC_PINGTEST_TOOL_WM_ZONE = 22;

        public const ushort NC_MISC_HIDE_EXCEPT_ME_ON_CMD = 23;

        public const ushort NC_MISC_HIDE_EXCEPT_ME_OFF_CMD = 24;

        public const ushort NC_MISC_APEX_SERVER_DATA_CMD = 25;

        public const ushort NC_MISC_APEX_CLIENT_DATA_CMD = 26;

        public const ushort NC_MISC_WEB_KEY_REQ = 27;

        public const ushort NC_MISC_WEB_KEY_ACK = 28;

        public const ushort NC_MISC_WEB_DB_KEY_REQ = 29;

        public const ushort NC_MISC_WEB_DB_KEY_ACK = 30;

        public const ushort NC_MISC_CHAR_LOGOFF_STATISTICS = 31;

        public const ushort NC_MISC_EVENT_HIT3_ADD_CASH = 32;

        public const ushort NC_MISC_TIMEFROMWORLD_CMD = 33;

        public const ushort NC_MISC_START_THE_BOOM_CMD = 35;

        public const ushort NC_MISC_WHSHANDLEFIX_CMD = 36;

        public const ushort NC_MISC_WHSHANDLEREPAIR_CMD = 37;

        public const ushort NC_MISC_XTRAP2_SERVER_DATA_CMD = 38;

        public const ushort NC_MISC_XTRAP2_CLIENT_DATA_CMD = 39;

        public const ushort NC_MISC_XTRAP2_OPTOOL_READ_CODEMAP_REQ = 40;

        public const ushort NC_MISC_XTRAP2_OPTOOL_READ_CODEMAP_ACK = 41;

        public const ushort NC_MISC_CONNECTFROMWHERE_REQ = 42;

        public const ushort NC_MISC_CONNECTFROMWHERE_ACK = 43;

        public const ushort NC_MISC_EVENT_L20_DB_REQ = 44;

        public const ushort NC_MISC_EVENT_L20_DB_ACK = 45;

        public const ushort NC_MISC_EVENT_L20_CMD = 46;

        public const ushort NC_MISC_CONNECTFROMWHERE_DB_REQ = 47;

        public const ushort NC_MISC_CONNECTFROMWHERE_DB_ACK = 48;

        //public const ushort NC_MISC_SERVERPARAMETER_REQ = 47;

        //public const ushort NC_MISC_SERVERPARAMETER_ACK = 48;

        public const ushort NC_MISC_CS_REQ = 49;

        public const ushort NC_MISC_CS_ACK = 50;

        public const ushort NC_MISC_CS_CLOSE = 51;

        public const ushort NC_MISC_HACK_SCAN_STORE_CMD = 52;

        public const ushort NC_MISC_HACK_SCAN_STORE_DB_CMD = 53;

        public const ushort NC_MISC_APEX_CLIENT_CHCSTART_CMD = 54;

        public const ushort NC_MISC_EVENT_DONE_MUNSANG_Z2WM = 55;

        public const ushort NC_MISC_EVENT_DONE_MUNSANG_WM2ACC = 56;

        public const ushort NC_MISC_EVENT_DONE_MUNSANG_ACC2WM = 57;

        public const ushort NC_MISC_EVENT_DONE_MUNSANG_WM2Z = 58;

        public const ushort NC_MISC_EVENT_DONE_MUNSANG_Z2CLI = 59;

        public const ushort NC_MISC_GM_CHAT_COLOR_REQ = 61;

        public const ushort NC_MISC_CLIENT_LOADING_BUG_DETECT_CMD = 64;

        public const ushort NC_MISC_DB_CLIENT_LOADING_BUG_DETECT_CMD = 65;

        public const ushort NC_MISC_MISCERROR_CMD = 66;

        public const ushort NC_MISC_GET_CHAT_BLOCK_SPAM_FILTER_DB_CMD = 68;

        public const ushort NC_MISC_SET_CHAT_BLOCK_SPAM_FILTER_CMD = 70;

        public const ushort NC_MISC_SET_CHAT_BLOCK_SPAM_FILTER_DB_CMD = 71;

        public const ushort NC_MISC_SPAMMER_REPORT_REQ = 95;

        public const ushort NC_MISC_SPAMMER_REPORT_ACK = 96;

        public const ushort NC_MISC_SPAMMER_CHAT_BAN_REQ = 97;

        public const ushort NC_MISC_SPAMMER_CHAT_BAN_ACK = 98;

        public const ushort NC_MISC_SPAMMER_SET_DB_CHAT_BAN_REQ = 99;

        public const ushort NC_MISC_SPAMMER_SET_DB_CHAT_BAN_ACK = 100;

        public const ushort NC_MISC_SPAMMER_ZONE_CHAT_BAN_CMD = 101;

        public const ushort NC_MISC_SPAMMER_RELEASE_CHAT_BAN_REQ = 102;

        public const ushort NC_MISC_SPAMMER_RELEASE_CHAT_BAN_ACK = 103;

        public const ushort NC_MISC_SPAMMER_RELEASE_CHAT_BAN_DB_REQ = 104;

        public const ushort NC_MISC_SPAMMER_RELEASE_CHAT_BAN_DB_ACK = 105;

        public const ushort NC_MISC_EVENTNPC_STANDSTART_ZONE_CMD = 106;

        public const ushort NC_MISC_EVENTNPC_STANDEND_ZONE_CMD = 107;

        public const ushort NC_MISC_EVENTNPC_STANDSTART_CLIENT_CMD = 108;

        public const ushort NC_MISC_EVENTNPC_STANDEND_CLIENT_CMD = 109;

        public const ushort NC_MISC_ITEMSHOP_URL_REQ = 110;

        public const ushort NC_MISC_ITEMSHOP_URL_ACK = 111;

        public const ushort NC_MISC_ITEMSHOP_URL_DB_REQ = 112;

        public const ushort NC_MISC_ITEMSHOP_URL_DB_ACK = 113;

    }
}