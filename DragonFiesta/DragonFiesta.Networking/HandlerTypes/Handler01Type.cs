namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler01Type : HandlerType
    {
        public new const byte _Header = 1;


        //NC
        public const ushort NC_LOG_GAME_ADD = 1;

        public const ushort NC_LOG_GAME_ADD_REQ = 2;

        public const ushort NC_LOG_GAME_ADD_ACK = 3;

        public const ushort NC_LOG_GAME_LOGIN = 10;

        public const ushort NC_LOG_GAME_LOGOUT = 11;

        public const ushort NC_LOG_GAME_LOGIN_SPAWN_APPS = 12;

        public const ushort NC_LOG_GAME_LINK = 13;

        public const ushort NC_LOG_GMAE_LOGOUT_ZONEINFO = 16;

        public const ushort NC_LOG_GAME_CREATE_AVATAR = 17;

        public const ushort NC_LOG_GAME_DELETE_AVATAR = 18;

        public const ushort NC_LOG_GAME_HIT = 19;

        public const ushort NC_LOG_GAME_MOVE = 20;

        public const ushort NC_LOG_GAME_PRISON = 25;

        public const ushort NC_LOG_GAME_PRISON_RELEASE = 26;

        public const ushort NC_LOG_GAME_LEVEL_UP = 30;

        public const ushort NC_LOG_GAME_LEVEL_DOWN = 31;

        public const ushort NC_LOG_GAME_CHANGE_CLASS = 32;

        public const ushort NC_LOG_GAME_PK = 40;

        public const ushort NC_LOG_GAME_PKED = 41;

        public const ushort NC_LOG_GAME_MK = 42;

        public const ushort NC_LOG_GAME_MKED = 43;

        public const ushort NC_LOG_GAME_MK_GETEXP = 44;

        public const ushort NC_LOG_GAME_MK_GETEXPINFIELD = 45;

        public const ushort NC_LOG_GAME_SKILL_LEARN = 50;

        public const ushort NC_LOG_GAME_SKILL_DELETE = 51;

        public const ushort NC_LOG_GAME_SKILL_USE = 52;

        public const ushort NC_LOG_GAME_STATE_SET = 55;

        public const ushort NC_LOG_GAME_STATE_CLEAR = 56;

        public const ushort NC_LOG_GAME_CHARGED_BUFF_SET = 57;

        public const ushort NC_LOG_GAME_CHARGED_BUFF_CLR = 58;

        public const ushort NC_LOG_GAME_QUEST_GET = 60;

        public const ushort NC_LOG_GAME_QUEST_COMPLETE = 61;

        public const ushort NC_LOG_GAME_QUEST_SET_INFO = 62;

        public const ushort NC_LOG_GAME_QUEST_ITEM_GET = 63;

        public const ushort NC_LOG_GAME_QUEST_DELETE = 64;

        public const ushort NC_LOG_GAME_KQ_ENTER = 65;

        public const ushort NC_LOG_GAME_KQ_LEAVE = 66;

        public const ushort NC_LOG_GAME_ITEM_MOB_DROP_RATE = 68;

        public const ushort NC_LOG_GAME_ITEM_BREAK = 69;

        public const ushort NC_LOG_GAME_ITEM_TAKE = 70;

        public const ushort NC_LOG_GAME_ITEM_DROP = 71;

        public const ushort NC_LOG_GAME_ITEM_BUY = 72;

        public const ushort NC_LOG_GAME_ITEM_SELL = 73;

        public const ushort NC_LOG_GAME_ITEM_TRADE = 74;

        public const ushort NC_LOG_GAME_ITEM_USE = 75;

        public const ushort NC_LOG_GAME_ITEM_INVEN_MOVE = 76;

        public const ushort NC_LOG_GAME_ITEM_EQUIP = 77;

        public const ushort NC_LOG_GAME_ITEM_UNEQUIP = 78;

        public const ushort NC_LOG_GAME_ITEM_CREATE = 79;

        public const ushort NC_LOG_GAME_ITEM_STORE_IN = 80;

        public const ushort NC_LOG_GAME_ITEM_STORE_OUT = 81;

        public const ushort NC_LOG_GAME_ITEM_UPGRADE = 82;

        public const ushort NC_LOG_GAME_ITEM_PRODUCT = 83;

        public const ushort NC_LOG_GAME_ITEM_TITLE = 84;

        public const ushort NC_LOG_GAME_ITEM_MOB_DROP = 85;

        public const ushort NC_LOG_GAME_ITEM_MERGE = 86;

        public const ushort NC_LOG_GAME_ITEM_SPLIT = 87;

        public const ushort NC_LOG_GAME_ITEM_SOULSTONEBUY = 88;

        public const ushort NC_LOG_GAME_ITEM_USELOT = 89;

        public const ushort NC_LOG_GAME_ITEM_USEALL = 90;

        public const ushort NC_LOG_GAME_ITEM_BOOTH_BUY = 91;

        public const ushort NC_LOG_GAME_ITEM_BOOTH_SELL = 92;

        public const ushort NC_LOG_GAME_ITEM_PRODUCT_STUFF = 93;

        public const ushort NC_LOG_GAME_MONEY_DEPOSIT = 94;

        public const ushort NC_LOG_GAME_MONEY_WITHDRAW = 95;

        public const ushort NC_LOG_GAME_MONEY_TRADE_INCOME = 96;

        public const ushort NC_LOG_GAME_MONEY_TRADE_OUTGO = 97;

        public const ushort NC_LOG_GAME_MONEY_CHANGE = 98;

        public const ushort NC_LOG_GAME_ENCHANNT = 100;

        public const ushort NC_LOG_GAME_DISENCHANT = 101;

        public const ushort NC_LOG_GAME_PARTY_CREATE = 110;

        public const ushort NC_LOG_GAME_PARTY_JOIN = 111;

        public const ushort NC_LOG_GAME_PARTY_LEAVE = 112;

        public const ushort NC_LOG_GAME_PARTY_CHG_MAS = 113;

        public const ushort NC_LOG_GAME_PARTY_DELETE = 114;

        public const ushort NC_LOG_GAME_PARTY_BANISH = 115;

        public const ushort NC_LOG_GAME_FRIEND_ADD = 120;

        public const ushort NC_LOG_GAME_FRIEND_DELETE = 121;

        public const ushort NC_LOG_GAME_MAS_PUP_ADD = 130;

        public const ushort NC_LOG_GAME_MAS_PUP_DELETE = 131;

        public const ushort NC_LOG_GAME_GUILD_CREATE = 140;

        public const ushort NC_LOG_GAME_GUILD_DELETE = 141;

        public const ushort NC_LOG_GAME_GUILD_TYPE = 142;

        public const ushort NC_LOG_GAME_GUILD_GRADE = 143;

        public const ushort NC_LOG_GAME_GUILD_WAR_DECLARE = 144;

        public const ushort NC_LOG_GAME_GUILD_WAR_ACCEPT = 145;

        public const ushort NC_LOG_GAME_GUILD_WAR_RESULT = 146;

        public const ushort NC_LOG_GAME_GUILD_M_JOIN = 160;

        public const ushort NC_LOG_GAME_GUILD_M_LEAVE = 161;

        public const ushort NC_LOG_GAME_GUILD_M_BANISH = 162;

        public const ushort NC_LOG_GAME_GUILD_M_GRADE = 163;

        public const ushort NC_LOG_GAME_GUILD_K_MONEY_WITHDRAW = 170;

        public const ushort NC_LOG_GAME_GUILD_G_REWARD_MONEY = 171;

        public const ushort NC_LOG_GAME_GUILD_4_REWARD_FAME = 172;

        public const ushort NC_LOG_GAME_GUILD_G_REWARD_EXP = 173;

        public const ushort NC_LOG_GAME_GUILD_4_TOURNAMENT_RESULT = 174;

        public const ushort NC_LOG_GAME_GUILD_G_REWARD_TOKEN = 175;

        public const ushort NC_LOG_GAME_GUILD_G_REWARD_MONEY_DIVISION = 176;

        public const ushort NC_LOG_GAME_CHARGE_WITHDRAW = 190;

        public const ushort NC_LOG_GAME_MINIHOUSE = 200;

        public const ushort NC_LOG_GAME_MINIHOUSE_BUILDING = 201;

        public const ushort NC_LOG_GAME_MINIHOUSE_VISIT = 202;

        public const ushort NC_LOG_GAME_PET = 250;

        public const ushort NC_LOG_GAME_EMBLEM = 270;

        public const ushort NC_LOG_GAME_QUEST_REWARD_EXP = 280;

        public const ushort NC_LOG_GAME_QUEST_REWARD_MONEY = 281;

        public const ushort NC_LOG_GAME_QUEST_REWARD_ITEM = 282;

        public const ushort NC_LOG_GAME_QUEST_REWARD_ABSTATE = 283;

        public const ushort NC_LOG_GAME_QUEST_REWARD_FAME = 284;

        public const ushort NC_LOG_GAME_QUEST_REWARD_PET = 285;

        public const ushort NC_LOG_GAME_QUEST_REWARD_MINIHOUSE = 286;

        public const ushort NC_LOG_GAME_QUEST_REWARD_TITLE = 287;

        public const ushort NC_LOG_GAME_ARENA_PVP = 300;

        public const ushort NC_LOG_GAME_ARENA_FBZ = 310;

        public const ushort NC_LOG_GAME_ARENA_CNG = 320;

        public const ushort NC_LOG_GAME_ARENA_GUILD = 330;

        public const ushort NC_LOG_GAME_ITEM_PUT_ON_BELONGED = 340;

        public const ushort NC_LOG_GAME_SEAWAR = 400;

        public const ushort NC_LOG_REGENLOCATESAVE_CMD = 490;

        public const ushort NC_LOG_GAME_MINIGAME = 500;

        public const ushort NC_LOG_GAME_ITEM_TAKE_INVEN_EXT = 510;

        public const ushort NC_LOG_GAME_ITEM_DROP_INVEN_EXT = 511;

        public const ushort NC_LOG_GAME_ITEM_BUY_INVEN_EXT = 512;

        public const ushort NC_LOG_GAME_ITEM_SELL_INVEN_EXT = 513;

        public const ushort NC_LOG_GAME_ITEM_USE_INVEN_EXT = 514;

        public const ushort NC_LOG_GAME_ITEM_INVEN_MOVE_INVEN_EXT = 515;

        public const ushort NC_LOG_GAME_ITEM_EQUIP_INVEN_EXT = 516;

        public const ushort NC_LOG_GAME_ITEM_UNEQUIP_INVEN_EXT = 517;

        public const ushort NC_LOG_GAME_ITEM_CREATE_INVEN_EXT = 518;

        public const ushort NC_LOG_GAME_ITEM_STORE_IN_INVEN_EXT = 519;

        public const ushort NC_LOG_GAME_ITEM_STORE_OUT_INVEN_EXT = 520;

        public const ushort NC_LOG_GAME_ITEM_PUT_ON_BELONGED_EXT = 521;

        public const ushort NC_LOG_GAME_ITEM_CW_BREAKATZERO = 530;

        public const ushort NC_LOG_WEDDING_PROPOSE_EXE = 600;

        public const ushort NC_LOG_WEDDING_PROPOSE_CANCEL = 601;

        public const ushort NC_LOG_WEDDING_DIVORCE_REQ = 602;

        public const ushort NC_LOG_WEDDING_DIVORCE_EXE = 603;

        public const ushort NC_LOG_WEDDING_DIVORCE_CANCEL = 604;

        public const ushort NC_LOG_WEDDING_HALL_RESERVE = 605;

        public const ushort NC_LOG_WEDDING_HALL_START = 606;

        public const ushort NC_LOG_WEDDING_HALL_CANCEL = 607;

        public const ushort NC_LOG_WEDDING_WEDDING_EXE = 608;

        public const ushort NC_LOG_USER_LOGINFAIL = 800;

        public const ushort NC_LOG_USER_LOGIN = 801;

        public const ushort NC_LOG_USER_LOGOUT = 802;

        public const ushort NC_LOG_GUILD_ACADEMY_JOIN = 850;

        public const ushort NC_LOG_GUILD_ACADEMY_LEAVE = 851;

        public const ushort NC_LOG_GUILD_ACADEMY_VANISH = 852;

        public const ushort NC_LOG_GUILD_ACADEMY_SET_MASTER = 853;

        public const ushort NC_LOG_GUILD_ACADEMY_GRADUATE = 854;

        public const ushort NC_LOG_GUILD_ACADEMY_GUILD_INVITE = 855;

        public const ushort NC_LOG_GUILD_ACADEMY_CHAT_BAN = 856;

        public const ushort NC_LOG_GUILD_ACADEMY_MASTER_TELEPORT = 857;

        public const ushort NC_LOG_GUILD_ACADEMY_SET_REWARD_MONEY = 858;

        public const ushort NC_LOG_GUILD_ACADEMY_SET_REWARD_ITEM = 859;

        public const ushort NC_LOG_GUILD_ACADEMY_CLEAR_REWARD_MONEY = 860;

        public const ushort NC_LOG_GUILD_ACADEMY_CLEAR_REWARD_ITEM = 861;

        public const ushort NC_LOG_GUILD_ACADEMY_REWARD_MONEY = 862;

        public const ushort NC_LOG_GUILD_ACADEMY_REWARD_ITEM = 863;

        public const ushort NC_LOG_GUILD_ACADEMY_REWARD_ITEM_PAY = 864;

        public const ushort NC_LOG_GUILD_ACADEMY_LEVEL_UP = 865;

        public const ushort NC_LOG_GAME_MK2_START = 910;

        public const ushort NC_LOG_GAME_MK2_SUCCESS = 911;

        public const ushort NC_LOG_GAME_MK2_FAIL = 912;

        public const ushort NC_LOG_GAME_MK2_DEAD = 913;

        public const ushort NC_LOG_GAME_MK_DROP_ITEM = 920;

        public const ushort NC_LOG_MOVER_UPGRADE = 922;

        public const ushort NC_LOG_MOVER_RAREMOVER = 923;

        public const ushort NC_LOG_ITEMMONEY_BUY = 924;

        public const ushort NC_LOG_RANDOMOPTION_CHANGE = 925;

        public const ushort NC_LOG_RANDOMOPTION_CHANGE_BEFORE = 926;

        public const ushort NC_LOG_RANDOMOPTION_CHANGE_AFTER = 927;

        public const ushort NC_LOG_RANDOMOPTION_USE_CONSUME_ITEM_TO_TARGET_ITEM = 928;

        public const ushort NC_LOG_UES_FRIEND_POINT = 929;

        public const ushort NC_LOG_CLASS_CHANGE_REQ = 930;

        public const ushort NC_LOG_RANDOMOPTION_RECOVER_COUNT_LIMIT = 933;

        public const ushort NC_LOG_GAME_DATA_TYPE_0 = 1000;

        public const ushort NC_LOG_GAME_DATA_TYPE_1 = 1001;

        public const ushort NC_LOG_GAME_DATA_TYPE_2 = 1002;

        public const ushort NC_LOG_GAME_DATA_TYPE_3 = 1003;

        public const ushort NC_LOG_GAME_DATA_TYPE_4 = 1004;

        public const ushort NC_LOG_GAME_DATA_TYPE_5 = 1005;

        public const ushort NC_LOG_GAME_DATA_TYPE_6 = 1006;

        public const ushort NC_LOG_GAME_DATA_TYPE_7 = 1007;

        public const ushort NC_LOG_GAME_DATA_TYPE_8 = 1008;

        public const ushort NC_LOG_GAME_DATA_TYPE_9 = 1009;

        public const ushort NC_LOG_GAME_DATA_TYPE_A = 1010;

        public const ushort NC_LOG_GAME_DATA_TYPE_B = 1011;

        public const ushort NC_LOG_GAME_DATA_TYPE_C = 1012;

        public const ushort NC_LOG_GAME_DATA_TYPE_D = 1013;

        public const ushort NC_LOG_GAME_DATA_TYPE_E = 1014;

        public const ushort NC_LOG_GAME_DATA_TYPE_F = 1015;

        public const ushort NC_LOG_GAME_DATA_TYPE_G = 1016;

        public const ushort NC_LOG_GAME_DATA_TYPE_H = 1017;

        public const ushort NC_LOG_GAME_DATA_TYPE_I = 1018;

        public const ushort NC_LOG_GAME_DATA_TYPE_J = 1019;

        public const ushort NC_LOG_GAME_DATA_TYPE_K = 1020;

        public const ushort NC_LOG_GAME_DATA_TYPE_L = 1021;
        
    }
}
