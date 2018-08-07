namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler38Type : HandlerType
    {

        public new const byte _Header = 38;

        //CMSG       
        public const ushort CMSG_GUILD_ACADEMY_MY_GUILD_ACADEMY_INFO_REQ = 7;  
        
        public const ushort CMSG_GUILD_ACADEMY_MEMBER_LIST_REQ = 13;

        public const ushort CMSG_GUILD_ACADEMY_MEMBER_JOIN_REQ = 17;

        public const ushort CMSG_GUILD_ACADEMY_MEMBER_VANISH_REQ = 22;

        public const ushort CMSG_GUILD_ACADEMY_MEMBER_LEAVE_REQ = 27;       

        public const ushort CMSG_GUILD_ACADEMY_MASTER_TELEPORT_REQ = 31;        

        public const ushort CMSG_GUILD_ACADEMY_MEMBER_CHAT_BAN_REQ = 33;

        public const ushort CMSG_GUILD_ACADEMY_NOTIFY_REQ = 36;

        public const ushort CMSG_GUILD_ACADEMY_MEMBER_GUILD_INVITE_REQ = 41;

        public const ushort CMSG_GUILD_ACADEMY_MEMBER_GUILD_JOIN_ACK = 44;

        public const ushort CMSG_GUILD_ACADEMY_CHAT_REQ = 104;


        //SMSG       
        public const ushort SMSG_GUILD_ACADEMY_MY_GUILD_ACADEMY_INFO_ACK = 8;        
        
        public const ushort SMSG_GUILD_ACADEMY_MEMBER_LIST_ACK = 14;    
        
        public const ushort SMSG_GUILD_ACADEMY_MEMBER_JOIN_ACK = 18;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_JOIN_CMD = 19;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_VANISH_ACK = 23;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_VANISH_CMD = 24;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_LEAVE_ACK = 28;

        public const ushort SMSG_GUILD_ACADEMY_NOTIFY_ACK = 37;

        public const ushort SMSG_GUILD_ACADEMY_NOTIFY_CMD = 38;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_GUILD_JOIN_REQ = 43;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_GUILD_JOIN_CMD = 46;

        public const ushort SMSG_GUILD_ACADEMY_DB_GET_REWARD_MONEY_ACK = 52;

        public const ushort SMSG_GUILD_ACADEMY_SET_REWARD_ITEM_REQ = 61;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_LEAVE_CMD = 96;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_LOGON_CMD = 97;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_LOGOFF_CMD = 98;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_MAP_CMD = 99;        

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_LEVEL_CMD = 102;

        public const ushort SMSG_GUILD_ACADEMY_MEMBER_CLASS_CMD = 103;

        public const ushort SMSG_GUILD_ACADEMY_CHAT_CMD = 105;

        public const ushort SMSG_GUILD_ACADEMY_CHAT_ACK = 106;


        //NC
        public const ushort NC_GUILD_ACADEMY_START_DB_ALL_REQ = 1;

        public const ushort NC_GUILD_ACADEMY_START_DB_ALL_ACK = 2;

        public const ushort NC_GUILD_ACADEMY_START_DB_GET_RANKING_LIST_REQ = 3;

        public const ushort NC_GUILD_ACADEMY_START_DB_GET_RANKING_LIST_ACK = 4;

        public const ushort NC_GUILD_ACADEMY_START_DB_RANK_BALANCE_REQ = 5;

        public const ushort NC_GUILD_ACADEMY_START_DB_RANK_BALANCE_ACK = 6;

        public const ushort NC_GUILD_ACADEMY_MY_ACADEMY_RANK_INFO_REQ = 9;

        public const ushort NC_GUILD_ACADEMY_MY_ACADEMY_RANK_INFO_ACK = 10;

        public const ushort NC_GUILD_ACADEMY_LIST_REQ = 11;

        public const ushort NC_GUILD_ACADEMY_LIST_ACK = 12;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_REQ = 15;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_ACK = 16;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_JOIN_REQ = 20;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_JOIN_ACK = 21;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_VANISH_REQ = 25;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_VANISH_ACK = 26;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_LEAVE_REQ = 29;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_LEAVE_ACK = 30;

        public const ushort NC_GUILD_ACADEMY_MASTER_TELEPORT_ACK = 32;

        public const ushort NC_GUILD_ACADEMY_MEMBER_CHAT_BAN_ACK = 34;

        public const ushort NC_GUILD_ACADEMY_MEMBER_CHAT_BAN_CMD = 35;

        public const ushort NC_GUILD_ACADEMY_DB_NOTIFY_REQ = 39;

        public const ushort NC_GUILD_ACADEMY_DB_NOTIFY_ACK = 40;

        public const ushort NC_GUILD_ACADEMY_MEMBER_GUILD_INVITE_ACK = 42;

        public const ushort NC_GUILD_ACADEMY_MEMBER_GUILD_JOIN_ERR = 45;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_GUILD_JOIN_REQ = 47;

        public const ushort NC_GUILD_ACADEMY_DB_MEMBER_GUILD_JOIN_ACK = 48;

        public const ushort NC_GUILD_ACADEMY_GET_REWARD_MONEY_REQ = 49;

        public const ushort NC_GUILD_ACADEMY_GET_REWARD_MONEY_ACK = 50;

        public const ushort NC_GUILD_ACADEMY_DB_GET_REWARD_MONEY_REQ = 51;

        public const ushort NC_GUILD_ACADEMY_GET_REWARD_ITEM_REQ = 53;

        public const ushort NC_GUILD_ACADEMY_GET_REWARD_ITEM_ACK = 54;

        public const ushort NC_GUILD_ACADEMY_DB_GET_REWARD_ITEM_REQ = 55;

        public const ushort NC_GUILD_ACADEMY_DB_GET_REWARD_ITEM_ACK = 56;

        public const ushort NC_GUILD_ACADEMY_SET_REWARD_MONEY_REQ = 57;

        public const ushort NC_GUILD_ACADEMY_SET_REWARD_MONEY_ACK = 58;

        public const ushort NC_GUILD_ACADEMY_DB_SET_REWARD_MONEY_REQ = 59;

        public const ushort NC_GUILD_ACADEMY_DB_SET_REWARD_MONEY_ACK = 60;

        public const ushort NC_GUILD_ACADEMY_SET_REWARD_ITEM_ACK = 62;

        public const ushort NC_GUILD_ACADEMY_DB_SET_REWARD_ITEM_REQ = 63;

        public const ushort NC_GUILD_ACADEMY_DB_SET_REWARD_ITEM_ACK = 64;

        public const ushort NC_GUILD_ACADEMY_CLEAR_REWARD_MONEY_REQ = 65;

        public const ushort NC_GUILD_ACADEMY_CLEAR_REWARD_MONEY_ACK = 66;

        public const ushort NC_GUILD_ACADEMY_DB_CLEAR_REWARD_MONEY_REQ = 67;

        public const ushort NC_GUILD_ACADEMY_DB_CLEAR_REWARD_MONEY_ACK = 68;

        public const ushort NC_GUILD_ACADEMY_CLEAR_REWARD_ITEM_REQ = 69;

        public const ushort NC_GUILD_ACADEMY_CLEAR_REWARD_ITEM_ACK = 70;

        public const ushort NC_GUILD_ACADEMY_DB_CLEAR_REWARD_ITEM_REQ = 71;

        public const ushort NC_GUILD_ACADEMY_DB_CLEAR_REWARD_ITEM_ACK = 72;

        public const ushort NC_GUILD_ACADEMY_GET_GRADUATE_REQ = 73;

        public const ushort NC_GUILD_ACADEMY_GET_GRADUATE_ACK = 74;

        public const ushort NC_GUILD_ACADEMY_DB_GET_GRADUATE_REQ = 75;

        public const ushort NC_GUILD_ACADEMY_DB_GET_GRADUATE_ACK = 76;

        public const ushort NC_GUILD_ACADEMY_GET_RANKING_LIST_REQ = 77;

        public const ushort NC_GUILD_ACADEMY_GET_RANKING_LIST_ACK = 78;

        public const ushort NC_GUILD_ACADEMY_DB_GET_RANKING_LIST_REQ = 79;

        public const ushort NC_GUILD_ACADEMY_DB_GET_RANKING_LIST_ACK = 80;

        public const ushort NC_GUILD_ACADEMY_DB_GRADUATE_JOIN_REQ = 81;

        public const ushort NC_GUILD_ACADEMY_DB_GRADUATE_JOIN_ACK = 82;

        public const ushort NC_GUILD_ACADEMY_DB_GRADUATE_JOIN_CMD = 83;

        public const ushort NC_GUILD_ACADEMY_DB_LEVEL_UP_REQ = 84;

        public const ushort NC_GUILD_ACADEMY_DB_LEVEL_UP_ACK = 85;

        public const ushort NC_GUILD_ACADEMY_DB_LEVEL_UP_CMD = 86;

        public const ushort NC_GUILD_ACADEMY_DB_RANK_BALANCE_REQ = 87;

        public const ushort NC_GUILD_ACADEMY_DB_RANK_BALANCE_ACK = 88;

        public const ushort NC_GUILD_ACADEMY_SET_MASTER_REQ = 89;

        public const ushort NC_GUILD_ACADEMY_SET_MASTER_ACK = 90;

        public const ushort NC_GUILD_ACADEMY_SET_MASTER_CMD = 91;

        public const ushort NC_GUILD_ACADEMY_DB_SET_MASTER_REQ = 92;

        public const ushort NC_GUILD_ACADEMY_DB_SET_MASTER_ACK = 93;

        public const ushort NC_GUILD_ACADEMY_DISMISS_CMD = 94;

        public const ushort NC_GUILD_ACADEMY_DELETE_CMD = 95;

        public const ushort NC_GUILD_ACADEMY_MEMBER_INTRO_CMD = 100;

        public const ushort NC_GUILD_ACADEMY_MEMBER_PARTY_CMD = 101;

        public const ushort NC_GUILD_ACADEMY_DB_ACADEMY_REWARD_REQ = 107;

        public const ushort NC_GUILD_ACADEMY_DB_ACADEMY_REWARD_ACK = 108;

        public const ushort NC_GUILD_ACADEMY_REWARD_STORAGE_OPEN_REQ = 109;

        public const ushort NC_GUILD_ACADEMY_REWARD_STORAGE_OPEN_ACK = 110;

        public const ushort NC_GUILD_ACADEMY_REWARD_STORAGE_CLOSE_CMD = 111;

        public const ushort NC_GUILD_ACADEMY_REWARD_STORAGE_WITHDRAW_GRADE_REQ = 112;

        public const ushort NC_GUILD_ACADEMY_REWARD_STORAGE_WITHDRAW_GRADE_ACK = 113;

        public const ushort NC_GUILD_ACADEMY_REWARD_STORAGE_WITHDRAW_RNG = 114;

        public const ushort NC_GUILD_ACADEMY_REWARD_STORAGE_WITHDRAW_CMD = 115;

        public const ushort NC_GUILD_ACADEMY_REWARD_STORAGE_DEPOSIT_RNG = 116;

        public const ushort NC_GUILD_ACADEMY_REWARD_STORAGE_DEPOSIT_CMD = 117;

        public const ushort NC_GUILD_ACADEMY_ZONE_MEMBER_JOIN_CMD = 118;

        public const ushort NC_GUILD_ACADEMY_ZONE_MEMBER_LEAVE_CMD = 119;

        public const ushort NC_GUILD_ACADEMY_ZONE_MASTER_TELEPORT_CMD = 120;

        public const ushort NC_GUILD_ACADEMY_ZONE_MEMBER_GUILD_JOIN_CMD = 121;

        public const ushort NC_GUILD_ACADEMY_ZONE_GUILD_BUFF_CMD = 122;

        public const ushort NC_GUILD_ACADEMY_ZONE_GUILD_ACADEMY_MASTER_BUFF_CMD = 123;

        public const ushort NC_GUILD_ACADEMY_WAR_INFO_LIST_CMD = 124;

        public const ushort NC_GUILD_ACADEMY_WAR_START_CMD = 125;

        public const ushort NC_GUILD_ACADEMY_WAR_END_CMD = 126;

        public const ushort NC_GUILD_ACADEMY_DB_SET_MASTER_BY_LEAVE_REQ = 127;

        public const ushort NC_GUILD_ACADEMY_DB_SET_MASTER_BY_LEAVE_ACK = 128;

        public const ushort NC_GUILD_ACADEMY_SET_MASTER_BY_LEAVE_CMD = 129;

        public const ushort NC_GUILD_ACADEMY_REWARDSTORAGE_ITEM_INFO_ZONE_RNG = 130;

        public const ushort NC_GUILD_ACADEMY_REWARDSTORAGE_REWARD_ZONE_RNG = 131;

        public const ushort NC_GUILD_ACADEMY_REWARDSTORAGE_GRADE_INFO_CMD = 132;

        public const ushort NC_GUILD_ACADEMY_MEMBER_DB_CHAT_BAN_REQ = 133;

        public const ushort NC_GUILD_ACADEMY_MEMBER_DB_CHAT_BAN_ACK = 134;

        public const ushort NC_GUILD_ACADEMY_MEMBER_DB_SAVE_CHAT_BAN_TIME_CMD = 135;

        public const ushort NC_GUILD_ACADEMY_MEMBER_CHAT_BAN_CANCEL_REQ = 138;

        public const ushort NC_GUILD_ACADEMY_MEMBER_CHAT_BAN_CANCEL_ACK = 139;

        public const ushort NC_GUILD_ACADEMY_MEMBER_CHAT_BAN_CANCEL_CMD = 140;

        public const ushort NC_GUILD_ACADEMY_MEMBER_DB_CHAT_BAN_CANCEL_REQ = 141;

        public const ushort NC_GUILD_ACADEMY_MEMBER_DB_CHAT_BAN_CANCEL_ACK = 142;

        public const ushort NC_GUILD_ACADEMY_HISTORY_LIST_REQ = 143;

        public const ushort NC_GUILD_ACADEMY_HISTORY_LIST_ACK = 144;

        public const ushort NC_GUILD_ACADEMY_HISTORY_DB_LIST_REQ = 145;

        public const ushort NC_GUILD_ACADEMY_HISTORY_DB_LIST_ACK = 146;

    }
}