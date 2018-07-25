namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler29Type : HandlerType
    {
        public new const byte _Header = 29;


        //CMSG
        public const ushort CMSG_GUILD_MAKE_REQ = 5;
        
        public const ushort CMSG_GUILD_MEMBER_INVITE_REQ = 9;        
              
        public const ushort CMSG_GUILD_MEMBER_JOIN_ACK = 12;
        
        public const ushort CMSG_GUILD_MEMBER_VANISH_REQ = 14; 
        
        public const ushort CMSG_GUILD_NOTIFY_REQ = 16;

        public const ushort CMSG_GUILD_MEMBER_GRADE_REQ = 22;
        
        public const ushort CMSG_GUILD_MEMBER_LEAVE_REQ = 28;

        public const ushort CMSG_GUILD_MEMBER_LEAVE_ACK = 29;        
        
        public const ushort CMSG_GUILD_CHAT_REQ = 115;        
        
        public const ushort CMSG_GUILD_NAME_REQ = 118;

        public const ushort CMSG_ITEMDB_CLOSE_GUILD_STORAGE_CMD = 160;

        public const ushort CMSG_GUILD_MY_GUILD_TOKEN_INFO_REQ = 190;     
                
        
        //SMSG
        public const ushort SMSG_GUILD_MEMBER_INVITE_ACK = 10;

        public const ushort SMSG_GUILD_MEMBER_JOIN_REQ = 11;
        
        public const ushort SMSG_GUILD_MEMBER_VANISH_ACK = 15;

        public const ushort SMSG_GUILD_NOTIFY_ACK = 17;        
        
        public const ushort SMSG_GUILD_GRADE_REQ = 20;        
        
        public const ushort SMSG_GUILD_MEMBER_GRADE_ACK = 23; 
        
        public const ushort SMSG_GUILD_MEMBER_LIST_ACK = 27;
        
        public const ushort SMSG_GUILD_MEMBER_LEAVE_ACK = 29;        
        
        public const ushort SMSG_GUILD_WAR_LIST_ACK = 39;        
        
        public const ushort SMSG_GUILD_NOTIFY_CMD = 45;        
        
        public const ushort SMSG_GUILD_MEMBER_JOIN_CMD = 54;

        public const ushort SMSG_GUILD_MEMBER_VANISH_CMD = 55;

        public const ushort SMSG_GUILD_MEMBER_LEAVE_CMD = 56;

        public const ushort SMSG_GUILD_MEMBER_GRADE_CMD = 57;        
        
        public const ushort SMSG_GUILD_MEMBER_LOGON_CMD = 61;

        public const ushort SMSG_GUILD_MEMBER_LOGOFF_CMD = 62;        
        
        public const ushort SMSG_GUILD_CHAT_CMD = 116;        
        
        public const ushort SMSG_GUILD_NAME_ACK = 119;          
        
        public const ushort SMSG_GUILD_MY_GUILD_TOKEN_INFO_ACK = 191;

        public const ushort SMSG_GUILD_MY_GUILD_TOURNAMENT_MATCH_TIME_ACK = 196;


        //NC
        public const ushort NC_GUILD_INFO_REQ = 1;

        public const ushort NC_GUILD_INFO_ACK = 2;

        public const ushort NC_GUILD_LIST_REQ = 3;

        public const ushort NC_GUILD_LIST_ACK = 4;

        public const ushort NC_GUILD_MAKE_ACK = 6;

        public const ushort NC_GUILD_DISMISS_REQ = 7;

        public const ushort NC_GUILD_DISMISS_ACK = 8;

        public const ushort NC_GUILD_MEMBER_JOIN_ERR = 13;

        public const ushort NC_GUILD_MONEY_SUB_REQ = 18;

        public const ushort NC_GUILD_MONEY_SUB_ACK = 19;

        public const ushort NC_GUILD_GRADE_ACK = 21;

        public const ushort NC_GUILD_MY_GUILD_INFO_REQ = 24;

        public const ushort NC_GUILD_MY_GUILD_INFO_ACK = 25;

        public const ushort NC_GUILD_MEMBER_LIST_REQ = 26;

        public const ushort NC_GUILD_MONEY_ADD_REQ = 30;

        public const ushort NC_GUILD_MONEY_ADD_ACK = 31;

        public const ushort NC_GUILD_MEMBER_INTRO_REQ = 32;

        public const ushort NC_GUILD_MEMBER_INTRO_ACK = 33;

        public const ushort NC_GUILD_WAR_REQ = 34;

        public const ushort NC_GUILD_WAR_ACK = 35;

        public const ushort NC_GUILD_WAR_ABLE_LIST_REQ = 36;

        public const ushort NC_GUILD_WAR_ABLE_LIST_ACK = 37;

        public const ushort NC_GUILD_WAR_LIST_REQ = 38;

        public const ushort NC_GUILD_WAR_SCORE_REQ = 40;

        public const ushort NC_GUILD_WAR_SCORE_ACK = 41;

        public const ushort NC_GUILD_WAR_SCORE_CMD = 42;

        public const ushort NC_GUILD_MONEY_ADD_CMD = 43;

        public const ushort NC_GUILD_MONEY_SUB_CMD = 44;

        public const ushort NC_GUILD_GRADE_CMD = 46;

        public const ushort NC_GUILD_DISMISS_CMD = 47;

        public const ushort NC_GUILD_DELETE_CMD = 48;

        public const ushort NC_GUILD_WAR_DECLARATION_CMD = 49;

        public const ushort NC_GUILD_WAR_TARGET_CMD = 50;

        public const ushort NC_GUILD_WAR_START_CMD = 51;

        public const ushort NC_GUILD_WAR_END_CMD = 52;

        public const ushort NC_GUILD_WAR_COOLDOWN_DONE_CMD = 53;

        public const ushort NC_GUILD_MEMBER_FLAGS_CMD = 58;

        public const ushort NC_GUILD_MEMBER_EXP_RATIO_CMD = 59;

        public const ushort NC_GUILD_MEMBER_INTRO_CMD = 60;

        public const ushort NC_GUILD_MEMBER_PARTY_CMD = 63;

        public const ushort NC_GUILD_MEMBER_LEVEL_CMD = 64;

        public const ushort NC_GUILD_MEMBER_MAP_CMD = 65;

        public const ushort NC_GUILD_MEMBER_CLASS_CMD = 66;

        public const ushort NC_GUILD_ZONE_WAR_START_CMD = 67;

        public const ushort NC_GUILD_ZONE_WAR_END_CMD = 68;

        public const ushort NC_GUILD_ZONE_WAR_KILL_CMD = 69;

        public const ushort NC_GUILD_ZONE_LIST_REQ = 70;

        public const ushort NC_GUILD_ZONE_LIST_ACK = 71;

        public const ushort NC_GUILD_ZONE_MAKE_CMD = 72;

        public const ushort NC_GUILD_ZONE_DELETE_CMD = 73;

        public const ushort NC_GUILD_ZONE_MEMBER_JOIN_CMD = 74;

        public const ushort NC_GUILD_ZONE_MEMBER_LEAVE_CMD = 75;

        public const ushort NC_GUILD_ZONE_MONEY_CMD = 76;

        public const ushort NC_GUILD_ZONE_TYPE_CMD = 77;

        public const ushort NC_GUILD_ZONE_GRADE_CMD = 78;

        public const ushort NC_GUILD_ZONE_FAME_CMD = 79;

        public const ushort NC_GUILD_ZONE_STONE_LEVEL_CMD = 80;

        public const ushort NC_GUILD_ZONE_EXP_CMD = 81;

        public const ushort NC_GUILD_DB_REQ = 82;

        public const ushort NC_GUILD_DB_ACK = 83;

        public const ushort NC_GUILD_DB_MEMBER_REQ = 84;

        public const ushort NC_GUILD_DB_MEMBER_ACK = 85;

        public const ushort NC_GUILD_DB_ALL_REQ = 86;

        public const ushort NC_GUILD_DB_ALL_ACK = 87;

        public const ushort NC_GUILD_DB_MAKE_REQ = 88;

        public const ushort NC_GUILD_DB_MAKE_ACK = 89;

        public const ushort NC_GUILD_DB_DELETE_REQ = 90;

        public const ushort NC_GUILD_DB_DELETE_ACK = 91;

        public const ushort NC_GUILD_DB_DISMISS_REQ = 92;

        public const ushort NC_GUILD_DB_DISMISS_ACK = 93;

        public const ushort NC_GUILD_DB_MONEY_ADD_REQ = 94;

        public const ushort NC_GUILD_DB_MONEY_ADD_ACK = 95;

        public const ushort NC_GUILD_DB_MONEY_SUB_REQ = 96;

        public const ushort NC_GUILD_DB_MONEY_SUB_ACK = 97;

        public const ushort NC_GUILD_DB_NOTIFY_REQ = 98;

        public const ushort NC_GUILD_DB_NOTIFY_ACK = 99;

        public const ushort NC_GUILD_DB_INTRO_REQ = 100;

        public const ushort NC_GUILD_DB_INTRO_ACK = 101;

        public const ushort NC_GUILD_DB_WAR_REQ = 102;

        public const ushort NC_GUILD_DB_WAR_ACK = 103;

        public const ushort NC_GUILD_DB_RESULT_WRITE_CMD = 104;

        public const ushort NC_GUILD_DB_MEMBER_JOIN_REQ = 105;

        public const ushort NC_GUILD_DB_MEMBER_JOIN_ACK = 106;

        public const ushort NC_GUILD_DB_MEMBER_LEAVE_REQ = 107;

        public const ushort NC_GUILD_DB_MEMBER_LEAVE_ACK = 108;

        public const ushort NC_GUILD_DB_MEMBER_INTRO_REQ = 109;

        public const ushort NC_GUILD_DB_MEMBER_INTRO_ACK = 110;

        public const ushort NC_GUILD_DB_MEMBER_GRADE_REQ = 111;

        public const ushort NC_GUILD_DB_MEMBER_GRADE_ACK = 112;

        public const ushort NC_GUILD_DB_MEMBER_VANISH_REQ = 113;

        public const ushort NC_GUILD_DB_MEMBER_VANISH_ACK = 114;

        public const ushort NC_GUILD_CHAT_ACK = 117;

        public const ushort NC_GUILD_GUILDWARCONFIRM_REQ = 120;

        public const ushort NC_GUILD_GUILDWARCONFIRM_ACK = 121;

        public const ushort NC_GUILD_TOURNAMENT_JOIN_REQ = 122;

        public const ushort NC_GUILD_TOURNAMENT_JOIN_ACK = 123;

        public const ushort NC_GUILD_TOURNAMENT_LEAVE_REQ = 124;

        public const ushort NC_GUILD_TOURNAMENT_LEAVE_ACK = 125;

        public const ushort NC_GUILD_TOURNAMENT_LIST_REQ = 126;

        public const ushort NC_GUILD_TOURNAMENT_LIST_ACK = 127;

        public const ushort NC_GUILD_TOURNAMENT_TYPE_CMD = 128;

        public const ushort NC_GUILD_TOURNAMENT_START_CMD = 129;

        public const ushort NC_GUILD_TOURNAMENT_END_CMD = 130;

        public const ushort NC_GUILD_TOURNAMENTSTART_CMD = 131;

        public const ushort NC_GUILD_TOURNAMENTSTOP_CMD = 132;

        public const ushort NC_GUILD_TOURNAMENT_DB_GET_REQ = 133;

        public const ushort NC_GUILD_TOURNAMENT_DB_GET_ACK = 134;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_REQ = 135;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_ACK = 136;

        public const ushort NC_GUILD_TOURNAMENT_DB_JOIN_REQ = 137;

        public const ushort NC_GUILD_TOURNAMENT_DB_JOIN_ACK = 138;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_TYPE_REQ = 139;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_TYPE_ACK = 140;

        public const ushort NC_GUILD_MOBGUILD_CMD = 141;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_RESULT_REQ = 142;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_RESULT_ACK = 143;

        public const ushort NC_GUILD_TOURNAMENT_RESULT_CMD = 144;

        public const ushort NC_GUILD_STORAGEOPEN_REQ = 145;

        public const ushort NC_GUILD_STORAGEOPEN_ACK = 146;

        public const ushort NC_GUILD_STORAGEWITHDRAW_RNG = 147;

        public const ushort NC_GUILD_STORAGEWITHDRAW_CMD = 148;

        public const ushort NC_GUILD_GUILDWARSTATUS_REQ = 149;

        public const ushort NC_GUILD_GUILDWARSTATUS_ACK = 150;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_MATCH_REQ = 151;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_MATCH_ACK = 152;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_MATCH_TIME_REQ = 153;

        public const ushort NC_GUILD_TOURNAMENT_DB_SET_MATCH_TIME_ACK = 154;

        public const ushort NC_GUILD_TOURNAMENT_RECALL_ENTER_CMD = 155;

        public const ushort NC_GUILD_TOURNAMENT_RECALL_ENTER_REQ = 156;

        public const ushort NC_GUILD_TOURNAMENT_RECALL_ENTER_ACK = 157;

        public const ushort NC_GUILD_STORAGE_WITHDRAW_GRADE_REQ = 158;

        public const ushort NC_GUILD_STORAGE_WITHDRAW_GRADE_ACK = 159;

        public const ushort NC_GUILD_GRADE_GROWTH_REQ = 161;

        public const ushort NC_GUILD_GRADE_GROWTH_ACK = 162;

        public const ushort NC_GUILD_GRADE_GROWTH_DATA_REQ = 163;

        public const ushort NC_GUILD_GRADE_GROWTH_DATA_ACK = 164;

        public const ushort NC_GUILD_GRADE_GROWTH_ZONE_REQ = 165;

        public const ushort NC_GUILD_GRADE_GROWTH_ZONE_ACK = 166;

        public const ushort NC_GUILD_DB_GRADE_GROWTH_REQ = 167;

        public const ushort NC_GUILD_DB_GRADE_GROWTH_ACK = 168;

        public const ushort NC_GUILD_DATA_CHANGE_CMD = 169;

        public const ushort NC_GUILD_TOURNAMENT_DB_REWARD_CREATE_REQ = 170;

        public const ushort NC_GUILD_TOURNAMENT_DB_REWARD_CREATE_ACK = 171;

        public const ushort NC_GUILD_RENAME_REQ = 172;

        public const ushort NC_GUILD_RENAME_ACK = 173;

        public const ushort NC_GUILD_RENAME_CMD = 174;

        public const ushort NC_GUILD_WORLD_RENAME_REQ = 175;

        public const ushort NC_GUILD_WORLD_RENAME_ACK = 176;

        public const ushort NC_GUILD_WORLD_RENAME_CMD = 177;

        public const ushort NC_GUILD_DB_RENAME_REQ = 178;

        public const ushort NC_GUILD_DB_RENAME_ACK = 179;

        public const ushort NC_GUILD_RETYPE_REQ = 180;

        public const ushort NC_GUILD_RETYPE_ACK = 181;

        public const ushort NC_GUILD_RETYPE_CMD = 182;

        public const ushort NC_GUILD_WORLD_RETYPE_REQ = 183;

        public const ushort NC_GUILD_WORLD_RETYPE_ACK = 184;

        public const ushort NC_GUILD_WORLD_RETYPE_CMD = 185;

        public const ushort NC_GUILD_DB_RETYPE_REQ = 186;

        public const ushort NC_GUILD_DB_RETYPE_ACK = 187;

        public const ushort NC_GUILD_DB_TOKEN_ALL_REQ = 188;

        public const ushort NC_GUILD_DB_TOKEN_ALL_ACK = 189;

        public const ushort NC_GUILD_ZONE_USE_GUILD_TOKEN_CMD = 192;

        public const ushort NC_GUILD_WORLD_USE_GUILD_TOKEN_CMD = 193;

        public const ushort NC_GUILD_WORLD_SET_GUILD_TOKEN_CMD = 194;

        public const ushort NC_GUILD_MY_GUILD_TOURNAMENT_MATCH_TIME_REQ = 195;

        public const ushort NC_GUILD_TOURNAMENT_LAST_WINNER_CMD = 197;

        public const ushort NC_GUILD_TOURNAMENT_ZONE_MEMBERGRADE_REQ = 198;

        public const ushort NC_GUILD_TOURNAMENT_ZONE_MEMBERGRADE_ACK = 199;

        public const ushort NC_GUILD_TOURNAMENT_ZONE_FIGHTER_ENTER_CMD = 200;

        public const ushort NC_GUILD_TOURNAMENT_ZONE_FIGHTER_OUT_CMD = 201;

        public const ushort NC_GUILD_TOURNAMENT_FLAGCAPTURE_REQ = 202;

        public const ushort NC_GUILD_TOURNAMENT_FLAGCAPTURE_ACK = 203;

        public const ushort NC_GUILD_TOURNAMENT_FLAGCAPTURE_CMD = 204;

        public const ushort NC_GUILD_TOURNAMENT_FLAGCAPTURE_RESULT_CMD = 205;

        public const ushort NC_GUILD_TOURNAMENT_SKILLPOINT_CMD = 206;

        public const ushort NC_GUILD_TOURNAMENT_USESKILL_REQ = 207;

        public const ushort NC_GUILD_TOURNAMENT_USESKILL_ACK = 208;

        public const ushort NC_GUILD_TOURNAMENT_USESKILL_CMD = 209;

        public const ushort NC_GUILD_TOURNAMENT_DICEGAME_START_CMD = 210;

        public const ushort NC_GUILD_TOURNAMENT_DICEGAME_THROW_REQ = 211;

        public const ushort NC_GUILD_TOURNAMENT_DICEGAME_THROW_ACK = 212;

        public const ushort NC_GUILD_TOURNAMENT_DICEGAME_THROW_CMD = 213;

        public const ushort NC_GUILD_TOURNAMENT_DICEGAME_BEFORE_END_TIME_MSG_CMD = 214;

        public const ushort NC_GUILD_TOURNAMENT_OBSERVER_ENTER_REQ = 215;

        public const ushort NC_GUILD_TOURNAMENT_OBSERVER_ENTER_ACK = 216;

        public const ushort NC_GUILD_TOURNAMENT_ZONE_OBSERVER_ENTER_REQ = 217;

        public const ushort NC_GUILD_TOURNAMENT_ZONE_OBSERVER_ENTER_ACK = 218;

        public const ushort NC_GUILD_TOURNAMENT_OBSERVER_OUT_REQ = 219;

        public const ushort NC_GUILD_TOURNAMENT_OBSERVER_OUT_ACK = 220;

        public const ushort NC_GUILD_TOURNAMENT_SCORE_CMD = 221;

        public const ushort NC_GUILD_TOURNAMENT_PLAYERDIEMSG_CMD = 222;

        public const ushort NC_GUILD_TOURNAMENT_PLAYERKILLMSG_CMD = 223;

        public const ushort NC_GUILD_TOURNAMENT_STARTMSG_CMD = 224;

        public const ushort NC_GUILD_TOURNAMENT_MANAGERUSERMSG_CMD = 225;

        public const ushort NC_GUILD_TOURNAMENTSTOPMSG_CMD = 226;

        public const ushort NC_GUILD_TOURNAMENT_ZONE_JOIN_NEW_REQ = 227;

        public const ushort NC_GUILD_TOURNAMENT_ZONE_JOIN_NEW_ACK = 228;

        public const ushort NC_GUILD_TOURNAMENT_DB_JOIN_NEW_REQ = 229;

        public const ushort NC_GUILD_TOURNAMENT_DB_JOIN_NEW_ACK = 230;

        public const ushort NC_GUILD_TOURNAMENT_DB_FINAL_SELECTION_REQ = 231;

        public const ushort NC_GUILD_TOURNAMENT_DB_FINAL_SELECTION_ACK = 232;

        public const ushort NC_GUILD_TOURNAMENT_JOIN_LIST_REQ = 233;

        public const ushort NC_GUILD_TOURNAMENT_JOIN_LIST_ACK = 234;

        public const ushort NC_GUILD_TOURNAMENT_DB_JOIN_LIST_REQ = 235;

        public const ushort NC_GUILD_TOURNAMENT_DB_JOIN_LIST_ACK = 236;

        public const ushort NC_GUILD_TOURNAMENT_NOTIFY_CMD = 237;

        public const ushort NC_GUILD_DB_DELETE_CANCEL_REQ = 238;

        public const ushort NC_GUILD_DB_DELETE_CANCEL_ACK = 239;

        public const ushort NC_GUILD_TOURNAMENT_ITEM_PICK_CMD = 240;

        public const ushort NC_GUILD_TOURNAMENT_ITEM_EFFECT_CMD = 241;

        public const ushort NC_GUILD_TOURNAMENT_ITEM_FLAG_CMD = 242;

        public const ushort NC_GUILD_TOURNAMENT_ITEM_SCAN_CMD = 243;

        public const ushort NC_GUILD_TOURNAMENT_GOLD_REFUND_ZONE_CMD = 244;

        public const ushort NC_GUILD_TOURNAMENT_GOLD_REFUND_CMD = 245;

        public const ushort NC_GUILD_TOURNAMENT_ENTER_LIST_DB_GET_REQ = 246;

        public const ushort NC_GUILD_TOURNAMENT_ENTER_LIST_DB_GET_ACK = 247;

        public const ushort NC_GUILD_TOURNAMENT_REFUND_NOTICE_CMD = 248;

        public const ushort NC_GUILD_TOURNAMENT_REFUND_NOTICE_DB_SET_REQ = 249;

        public const ushort NC_GUILD_TOURNAMENT_REFUND_NOTICE_DB_SET_ACK = 250;

        public const ushort NC_GUILD_HISTORY_LIST_REQ = 253;

        public const ushort NC_GUILD_HISTORY_LIST_ACK = 254;

        public const ushort NC_GUILD_HISTORY_DB_LIST_REQ = 255;

        public const ushort NC_GUILD_HISTORY_DB_LIST_ACK = 256;

        public const ushort NC_GUILD_EMBLEM_CHECK_AVAILABILITY_REQ = 257;

        public const ushort NC_GUILD_EMBLEM_CHECK_AVAILABILITY_ACK = 258;

        public const ushort NC_GUILD_EMBLEM_INFO_DB_REQ = 259;

        public const ushort NC_GUILD_EMBLEM_INFO_DB_ACK = 260;

        public const ushort NC_GUILD_EMBLEM_SAVE_REQ = 261;

        public const ushort NC_GUILD_EMBLEM_SAVE_ACK = 262;

        public const ushort NC_GUILD_EMBLEM_SAVE_DB_REQ = 263;

        public const ushort NC_GUILD_EMBLEM_SAVE_DB_ACK = 264;

        public const ushort NC_GUILD_EMBLEM_OFF_MSG_CMD = 265;

        public const ushort NC_GUILD_EMBLEM_INFO_CMD = 266;

        public const ushort NC_GUILD_EMBLEM_INFO_NOTICE_CMD = 267;

        public const ushort NC_GUILD_EMBLEM_LEVELUP_CMD = 268;

        public const ushort NC_GUILD_EMBLEM_STATE_DB_REQ = 269;

        public const ushort NC_GUILD_EMBLEM_STATE_DB_ACK = 270;
         
    }
}