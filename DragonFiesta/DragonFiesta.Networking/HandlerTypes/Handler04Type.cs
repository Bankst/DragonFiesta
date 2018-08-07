namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler04Type : HandlerType
    {
        public new const byte _Header = 4;


        //CMSG
        public const ushort CMSG_CHAR_LOGIN_REQ = 1;

        public const ushort CMSG_CHAR_REVIVE_REQ = 78;

        public const ushort CMSG_CHAR_STAT_INCPOINT_REQ = 92;

        public const ushort CMSG_CHAR_LOGOUTREADY_CMD = 113;

        public const ushort CMSG_CHAR_LOGOUTCANCEL_CMD = 114;

        public const ushort CMSG_CHAR_NEWBIE_GUIDE_VIEW_SET_CMD = 218;

        public const ushort CMSG_CHAR_TUTORIAL_POPUP_ACK = 273;


        //SMSG
        public const ushort SMSG_CHAR_LOGINFAIL_ACK = 2;

        public const ushort SMSG_CHAR_LOGIN_ACK = 3;

        public const ushort SMSG_CHAR_GUILD_CMD = 18;

        public const ushort SMSG_CHAR_CENCHANGE_CMD = 51;

        public const ushort SMSG_CHAR_CHANGEPARAMCHANGE_CMD = 53;

        public const ushort SMSG_CHAR_CLIENT_BASE_CMD = 56;

        public const ushort SMSG_CHAR_CLIENT_SHAPE_CMD = 57;

        public const ushort SMSG_CHAR_CLIENT_QUEST_DOING_CMD = 58;

        public const ushort SMSG_CHAR_CLIENT_QUEST_DONE_CMD = 59;

        public const ushort SMSG_CHAR_CLIENT_SKILL_CMD = 61;

        public const ushort SMSG_CHAR_CLIENT_PASSIVE_CMD = 62;

        public const ushort SMSG_CHAR_CLIENT_ITEM_CMD = 71;

        public const ushort SMSG_CHAR_CLIENT_GAME_CMD = 72;

        public const ushort SMSG_CHAR_CLIENT_CHARTITLE_CMD = 73;

        public const ushort SMSG_CHAR_CLIENT_CHARGEDBUFF_CMD = 74;

        public const ushort SMSG_CHAR_DEADMENU_CMD = 77;

        public const ushort SMSG_CHAR_REVIVESAME_CMD = 79;

        public const ushort SMSG_CHAR_STAT_REMAINPOINT_CMD = 91;

        public const ushort SMSG_CHAR_STAT_INCPOINTSUC_ACK = 95;

        public const ushort SMSG_CHAR_FAMECHANGE_CMD = 111;

        public const ushort SMSG_CHAR_GUILD_ACADEMY_CMD = 151;

        public const ushort SMSG_CHAR_CLIENT_QUEST_READ_CMD = 206;

        public const ushort SMSG_CHAR_CLIENT_QUEST_REPEAT_CMD = 215;

        public const ushort SMSG_CHAR_CLIENT_NEWBIE_GUIDE_VIEW_LIST_CMD = 217;

        public const ushort SMSG_CHAR_CLIENT_COININFO_CMD = 222;

        public const ushort SMSG_CHAR_CLIENT_CARDCOLLECT_CMD = 228;

        public const ushort SMSG_CHAR_TUTORIAL_POPUP_REQ = 272;

        public const ushort SMSG_CHAR_TUTORIAL_DOING_CMD = 285;

        public const ushort SMSG_CHAR_USEITEM_MINIMON_USE_BROAD_CMD = 300;

        public const ushort SMSG_CHAR_ANI_FILE_CHECK_CMD = 323;


        //NC
        public const ushort NC_CHAR_CHARDATA_REQ = 4;

        public const ushort NC_CHAR_CHARDATA_ACK = 5;

        public const ushort NC_CHAR_CHARDATAFAIL_ACK = 6;

        public const ushort NC_CHAR_BASE_CMD = 7;

        public const ushort NC_CHAR_SHAPE_CMD = 8;

        public const ushort NC_CHAR_QUEST_DOING_CMD = 10;

        public const ushort NC_CHAR_QUEST_DONE_CMD = 11;

        public const ushort NC_CHAR_ABSTATE_CMD = 12;

        public const ushort NC_CHAR_SKILL_CMD = 13;

        public const ushort NC_CHAR_SKILL_PASSIVE_CMD = 14;

        public const ushort NC_CHAR_HOUSE_CMD = 15;

        public const ushort NC_CHAR_FRIEND_CMD = 16;

        public const ushort NC_CHAR_MASPUP_CMD = 17;

        public const ushort NC_CHAR_EMBLEM_CMD = 19;

        public const ushort NC_CHAR_PET_CMD = 20;

        public const ushort NC_CHAR_ARENA_CMD = 21;

        public const ushort NC_CHAR_SEAWAR_CMD = 22;

        public const ushort NC_CHAR_ITEM_CMD = 23;

        public const ushort NC_CHAR_CHESTINFO_CMD = 24;

        public const ushort NC_CHAR_CHARTITLE_CMD = 25;

        public const ushort NC_CHAR_KQMAP_CMD = 26;

        public const ushort NC_CHAR_CHARGEDBUFF_CMD = 27;

        public const ushort NC_CHAR_ZONE_CHARDATA_REQ = 28;

        public const ushort NC_CHAR_ZONE_CHARDATA_ACK = 29;

        public const ushort NC_CHAR_ZONE_CHARDATAFAIL_ACK = 30;

        public const ushort NC_CHAR_ZONE_BASE_CMD = 31;

        public const ushort NC_CHAR_ZONE_SHAPE_CMD = 32;

        public const ushort NC_CHAR_ZONE_QUEST_DOING_CMD = 33;

        public const ushort NC_CHAR_ZONE_QUEST_DONE_CMD = 34;

        public const ushort NC_CHAR_ZONE_ABSTATE_CMD = 35;

        public const ushort NC_CHAR_ZONE_SKILL_CMD = 36;

        public const ushort NC_CHAR_ZONE_PASSIVE_CMD = 37;

        public const ushort NC_CHAR_ZONE_HOUSE_CMD = 38;

        public const ushort NC_CHAR_ZONE_FRIEND_CMD = 39;

        public const ushort NC_CHAR_ZONE_MASPUP_CMD = 40;

        public const ushort NC_CHAR_ZONE_GUILD_CMD = 41;

        public const ushort NC_CHAR_ZONE_EMBLEM_CMD = 42;

        public const ushort NC_CHAR_ZONE_PET_CMD = 43;

        public const ushort NC_CHAR_ZONE_ARENA_CMD = 44;

        public const ushort NC_CHAR_ZONE_SEAWAR_CMD = 45;

        public const ushort NC_CHAR_ZONE_ITEM_CMD = 46;

        public const ushort NC_CHAR_ZONE_CHESTINFO_CMD = 47;

        public const ushort NC_CHAR_ZONE_GAME_CMD = 48;

        public const ushort NC_CHAR_ZONE_CHARTITLE_CMD = 49;

        public const ushort NC_CHAR_ZONE_CHARGEDBUFF_CMD = 50;

        public const ushort NC_CHAR_BASEPARAMCHANGE_CMD = 52;

        public const ushort NC_CHAR_LOGIN_DB = 54;

        public const ushort NC_CHAR_LOGOUT_DB = 55;

        public const ushort NC_CHAR_CLIENT_ABSTATE_CMD = 60;

        public const ushort NC_CHAR_CLIENT_HOUSE_CMD = 63;

        public const ushort NC_CHAR_CLIENT_FRIEND_CMD = 64;

        public const ushort NC_CHAR_CLIENT_MASPUP_CMD = 65;

        public const ushort NC_CHAR_CLIENT_GUILD_CMD = 66;

        public const ushort NC_CHAR_CLIENT_EMBLEM_CMD = 67;

        public const ushort NC_CHAR_CLIENT_PET_CMD = 68;

        public const ushort NC_CHAR_CLIENT_ARENA_CMD = 69;

        public const ushort NC_CHAR_CLIENT_SEAWAR_CMD = 70;

        public const ushort NC_CHAR_REGISTNUMBER_REQ = 75;

        public const ushort NC_CHAR_REGISTNUMBER_ACK = 76;

        public const ushort NC_CHAR_REVIVEOTHER_CMD = 80;

        public const ushort NC_CHAR_PROMOTE_REQ = 85;

        public const ushort NC_CHAR_CLASSCHANGE_REQ = 86;

        public const ushort NC_CHAR_CLASSCHANGE_ACK = 87;

        public const ushort NC_CHAR_CLASSCHANGE_CMD = 88;

        public const ushort NC_CHAR_PROMOTE_ACK = 89;

        public const ushort NC_CHAR_SOMEONEPROMOTE_CMD = 90;

        public const ushort NC_CHAR_STAT_INCPOINT_DB_REQ = 93;

        public const ushort NC_CHAR_STAT_INCPOINTSUC_DB_ACK = 94;

        public const ushort NC_CHAR_STAT_INCPOINTFAIL_DB_ACK = 96;

        public const ushort NC_CHAR_STAT_INCPOINTFAIL_ACK = 97;

        public const ushort NC_CHAR_STAT_DECPOINT_REQ = 98;

        public const ushort NC_CHAR_STAT_DECPOINT_DB_REQ = 99;

        public const ushort NC_CHAR_STAT_DECPOINTSUC_DB_ACK = 100;

        public const ushort NC_CHAR_STAT_DECPOINTSUC_ACK = 101;

        public const ushort NC_CHAR_STAT_DECPOINTFAIL_DB_ACK = 102;

        public const ushort NC_CHAR_STAT_DECPOINTFAIL_ACK = 103;

        public const ushort NC_CHAR_PLAYERSEARCH_RNG = 104;

        public const ushort NC_CHAR_PLAYERFOUND_RNG = 105;

        public const ushort NC_CHAR_PLAYERSUMMON_RNG = 106;

        public const ushort NC_CHAR_KICKPLAYEROUT_RNG = 107;

        public const ushort NC_CHAR_PLAYERBANNED_RNG = 108;

        public const ushort NC_CHAR_SOMEONEGUILDCHANGE_CMD = 110;

        public const ushort NC_CHAR_FAMESAVE_CMD = 112;

        public const ushort NC_CHAR_EXP_CHANGED_CMD = 115;

        public const ushort NC_CHAR_LEVEL_CHANGED_CMD = 116;

        public const ushort NC_CHAR_DATATRANSMISSION_RNG = 117;

        public const ushort NC_CHAR_GET_ITEMLIST_BY_TYPE_REQ = 118;

        public const ushort NC_CHAR_GET_ITEMLIST_BY_TYPE_ACK = 119;

        public const ushort NC_CHAR_SET_STYLE_REQ = 120;

        public const ushort NC_CHAR_SET_STYLE_ACK = 121;

        public const ushort NC_CHAR_SET_STYLE_DB_REQ = 122;

        public const ushort NC_CHAR_SET_STYLE_DB_ACK = 123;

        public const ushort NC_CHAR_SET_STYLE_GET_INFO_REQ = 124;

        public const ushort NC_CHAR_SET_STYLE_GET_INFO_ACK = 125;

        public const ushort NC_CHAR_SET_STYLE_GET_INFO_DB_REQ = 126;

        public const ushort NC_CHAR_SET_STYLE_GET_INFO_DB_ACK = 127;

        public const ushort NC_CHAR_WEDDINGDATA_REQ = 128;

        public const ushort NC_CHAR_WEDDINGDATA_ACK = 129;

        public const ushort NC_CHAR_WEDDINGDATA_CMD = 130;

        public const ushort NC_CHAR_WEDDING_PROPOSE_REQ = 131;

        public const ushort NC_CHAR_WEDDING_PROPOSE_ACK = 132;

        public const ushort NC_CHAR_WEDDING_PROPOSE_CANCEL_REQ = 133;

        public const ushort NC_CHAR_WEDDING_PROPOSE_CANCEL_ACK = 134;

        public const ushort NC_CHAR_WEDDING_ESCAPE_DIVORCE_REQ = 135;

        public const ushort NC_CHAR_WEDDING_ESCAPE_DIVORCE_ACK = 136;

        public const ushort NC_CHAR_WEDDING_DIVORCE_BY_AGREE_REQ = 137;

        public const ushort NC_CHAR_WEDDING_DIVORCE_BY_AGREE_ACK = 138;

        public const ushort NC_CHAR_WEDDING_DIVORCE_BY_FORCE_REQ = 139;

        public const ushort NC_CHAR_WEDDING_DIVORCE_BY_FORCE_ACK = 140;

        public const ushort NC_CHAR_WEDDING_DIVORCE_DO_REQ = 141;

        public const ushort NC_CHAR_WEDDING_DIVORCE_DO_ACK = 142;

        public const ushort NC_CHAR_WEDDING_DIVORCE_CANCEL_REQ = 143;

        public const ushort NC_CHAR_WEDDING_DIVORCE_CANCEL_ACK = 144;

        public const ushort NC_CHAR_WEDDING_DO_REQ = 145;

        public const ushort NC_CHAR_WEDDING_DO_ACK = 146;

        public const ushort NC_CHAR_WEDDING_PARTNER_INFO_REQ = 147;

        public const ushort NC_CHAR_WEDDING_PARTNER_INFO_ACK = 148;

        public const ushort NC_CHAR_WEDDING_PARTNER_INFO_RNG = 149;

        public const ushort NC_CHAR_SOMEONEGUILDACADEMYCHANGE_CMD = 150;

        public const ushort NC_CHAR_GUILD_ACADEMY_ZONE_CMD = 152;

        public const ushort NC_CHAR_GET_ITEMLIST_BY_TYPE_NUM_REQ = 153;

        public const ushort NC_CHAR_GET_ITEMLIST_BY_TYPE_NUM_ACK = 154;

        public const ushort NC_CHAR_WEDDING_PARTNER_SUMMON_RNG = 170;

        public const ushort NC_CHAR_WEDDING_REFRESH_INFO_RNG = 171;

        public const ushort NC_CHAR_PLAYERSEARCH_BY_NORMAL_USER_RNG = 173;

        public const ushort NC_CHAR_PLAYERFOUND_BY_NORMAL_USER_RNG = 174;

        public const ushort NC_CHAR_POLYMORPH_CMD = 175;

        public const ushort NC_CHAR_DEPOLYMORPH_CMD = 176;

        public const ushort NC_CHAR_EMPTY_INSTANCE_DUNGEON_RNG = 177;

        public const ushort NC_CHAR_WEDDING_CANCEL_WEDDING = 178;

        public const ushort NC_CHAR_REGNUM_VARIFICATION_REQ = 179;

        public const ushort NC_CHAR_REGNUM_VARIFICATION_ACK = 180;

        public const ushort NC_CHAR_ZONE_LINK_FROM_CMD = 184;

        public const ushort NC_CHAR_SAVE_LINK_REQ = 185;

        public const ushort NC_CHAR_CLIENT_AUTO_PICK_REQ = 188;

        public const ushort NC_CHAR_CLIENT_AUTO_PICK_ACK = 189;

        public const ushort NC_CHAR_CLIENT_AUTO_PICK_CMD = 190;

        public const ushort NC_CHAR_ZONE_AUTO_PICK_CMD = 193;

        public const ushort NC_CHAR_ADMIN_LEVEL_INFORM_CMD = 198;

        public const ushort NC_CHAR_GET_CHAT_BLOCK_SPAMER_DB_CMD = 202;

        public const ushort NC_CHAR_GET_CHAT_BLOCK_SPAMER_CMD = 203;

        public const ushort NC_CHAR_QUEST_READ_CMD = 204;

        public const ushort NC_CHAR_ZONE_QUEST_READ_CMD = 205;

        public const ushort NC_CHAR_ITEMACTIONCOOLTIME_CMD = 207;

        public const ushort NC_CHAR_ITEMACTIONCOOLTIME_ZONE_CMD = 208;

        public const ushort NC_CHAR_FREESTAT_SET_DB_REQ = 209;

        //public const ushort NC_CHAR_FREESTAT_SET_DB_ACK = 210;

        //public const ushort NC_CHAR_SINGLE_OPTION_CMD = 210;

        public const ushort NC_CHAR_ZONE_SINGLE_OPTION_CMD = 211;

        public const ushort NC_CHAR_MYSTERYVAULT_UI_STATE_CMD = 212;

        public const ushort NC_CHAR_QUEST_REPEAT_CMD = 213;

        public const ushort NC_CHAR_ZONE_QUEST_REPEAT_CMD = 214;

        public const ushort NC_CHAR_NEWBIE_GUIDE_VIEW_LIST_CMD = 216;

        public const ushort NC_CHAR_DB_NEWBIE_GUIDE_VIEW_SET_CMD = 219;

        public const ushort NC_CHAR_COININFO_CMD = 220;

        public const ushort NC_CHAR_ZONE_COININFO_CMD = 221;

        public const ushort NC_CHAR_CHANGEBYCONDITION_PARAM_CMD = 223;

        public const ushort NC_CHAR_CARDCOLLECT_CMD = 226;

        public const ushort NC_CHAR_ZONE_CARDCOLLECT_CMD = 227;

        public const ushort NC_CHAR_CARDCOLLECT_BOOKMARK_CMD = 229;

        public const ushort NC_CHAR_ZONE_CARDCOLLECT_BOOKMARK_CMD = 230;

        public const ushort NC_CHAR_CLIENT_CARDCOLLECT_BOOKMARK_CMD = 231;

        public const ushort NC_CHAR_CARDCOLLECT_REWARD_CMD = 232;

        public const ushort NC_CHAR_ZONE_CARDCOLLECT_REWARD_CMD = 233;

        public const ushort NC_CHAR_CLIENT_CARDCOLLECT_REWARD_CMD = 234;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_LIST_DB_REQ = 235;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_LIST_DB_ACK = 236;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_LIST_CLIENT_CMD = 237;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_CHECK_DB_REQ = 238;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_CHECK_DB_ACK = 239;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_CHECK_CMD = 240;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_REWARD_REQ = 241;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_REWARD_ACK = 242;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_REWARD_DB_REQ = 243;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_REWARD_DB_ACK = 244;

        public const ushort NC_CHAR_EVENT_ATTENDANCE_CHANGE_DAY_CMD = 245;

        public const ushort NC_CHER_EVENT_ATTENDANCE_CHANGE_START_CMD = 246;

        public const ushort NC_CHAR_REBIRTH_CMD = 258;

        public const ushort NC_CHAR_REBIRTH_REJECT_CMD = 259;

        public const ushort NC_CHAR_TUTORIAL_MAKE_ITEM_REQ = 274;

        public const ushort NC_CHAR_TUTORIAL_MAKE_ITEM_ACK = 275;

        public const ushort NC_CHAR_TUTORIAL_INFO_WORLD_CMD = 276;

        public const ushort NC_CHAR_TUTORIAL_INFO_ZONE_CMD = 277;

        public const ushort NC_CHAR_TUTORIAL_STEP_REQ = 278;

        public const ushort NC_CHAR_TUTORIAL_STEP_ACK = 279;

        public const ushort NC_CHAR_TUTORIAL_STEP_SAVE_REQ = 280;

        public const ushort NC_CHAR_TUTORIAL_STEP_SAVE_ACK = 281;

        public const ushort NC_CHAR_TUTORIAL_FREESTAT_INIT_REQ = 282;

        public const ushort NC_CHAR_TUTORIAL_FREESTAT_INIT_ACK = 283;

        public const ushort NC_CHAR_TUTORIAL_STEP_SAVE_CMD = 284;

        public const ushort NC_CHAR_CHAT_COLOR_CMD = 286;

        public const ushort NC_CHAR_ZONE_CHAT_COLOR_CMD = 287;

        public const ushort NC_CHAR_SUPPORT_REWARD_CMD = 288;

        public const ushort NC_CHAR_USEITEM_MINIMON_INFO_CMD = 289;

        public const ushort NC_CHAR_USEITEM_MINIMON_INFO_ZONE_CMD = 290;

        public const ushort NC_CHAR_USEITEM_MINIMON_INFO_CLIENT_CMD = 291;

        public const ushort NC_CHAR_USEITEM_MINIMON_NORMAL_ITEM_ON_REQ = 292;

        public const ushort NC_CHAR_USEITEM_MINIMON_NORMAL_ITEM_ON_ACK = 293;

        public const ushort NC_CHAR_USEITEM_MINIMON_NORMAL_ITEM_OFF_REQ = 294;

        public const ushort NC_CHAR_USEITEM_MINIMON_NORMAL_ITEM_OFF_ACK = 295;

        public const ushort NC_CHAR_USEITEM_MINIMON_CHARGED_ITEM_ON_REQ = 296;

        public const ushort NC_CHAR_USEITEM_MINIMON_CHARGED_ITEM_ON_ACK = 297;

        public const ushort NC_CHAR_USEITEM_MINIMON_CHARGED_ITEM_OFF_REQ = 298;

        public const ushort NC_CHAR_USEITEM_MINIMON_CHARGED_ITEM_OFF_ACK = 299;

        public const ushort NC_CHAR_USEITEM_MINIMON_NOTICE_CMD = 301;

        public const ushort NC_CHAR_CHARGEDBUFF_ERASE_REQ = 302;

        public const ushort NC_CHAR_CHARGEDBUFF_ERASE_ACK = 303;

        public const ushort NC_CHAR_SELL_ITEM_INFO_ZONE_CMD = 304;

        public const ushort NC_GAMIGO_NEW_TUTORIAL_STORE_STEP_REQ = 322;

    }
}
