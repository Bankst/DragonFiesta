namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler16Type : HandlerType
    {
        public new const byte _Header = 16;

        //CMSG
        public const ushort CMSG_CHARSAVE_UI_STATE_SAVE_REQ = 37;


        //SMSG
        public const ushort SMSG_CHARSAVE_UI_STATE_SAVE_ACK = 38;


        //NC
        public const ushort NC_CHARSAVE_ALL_REQ = 1;

        public const ushort NC_CHARSAVE_ALL_ACK = 2;

        public const ushort NC_CHARSAVE_LOCATION_CMD = 3;

        public const ushort NC_CHARSAVE_LEVEL_CMD = 4;

        public const ushort NC_CHARSAVE_QUEST_DOING_REQ = 5;

        public const ushort NC_CHARSAVE_QUEST_DOING_ACK = 6;

        public const ushort NC_CHARSAVE_ABSTATE_REQ = 7;

        public const ushort NC_CHARSAVE_ABSTATE_ACK = 8;

        public const ushort NC_CHARSAVE_SKILL_REQ = 9;

        public const ushort NC_CHARSAVE_SKILL_ACK = 10;

        public const ushort NC_CHARSAVE_TITLE_REQ = 11;

        public const ushort NC_CHARSAVE_TITLE_ACK = 12;

        public const ushort NC_CHARSAVE_CHARSTAT_CMD = 13;

        public const ushort NC_CHARSAVE_2WLDMAN_ALL_CMD = 14;

        public const ushort NC_CHARSAVE_2WLDMAN_QUEST_DOING_CMD = 15;

        public const ushort NC_CHARSAVE_2WLDMAN_QUEST_DONE_CMD = 16;

        public const ushort NC_CHARSAVE_2WLDMAN_ABSTATE_CMD = 17;

        public const ushort NC_CHARSAVE_2WLDMAN_SKILL_CMD = 18;

        public const ushort NC_CHARSAVE_2WLDMAN_ITEM_CMD = 19;

        public const ushort NC_CHARSAVE_2WLDMAN_CHESTINFO_CMD = 20;

        public const ushort NC_CHARSAVE_2WLDMAN_GAME_CMD = 21;

        public const ushort NC_CHARSAVE_2WLDMAN_TITLE_CMD = 22;

        public const ushort NC_CHARSAVE_2WLDMAN_MISC_CMD = 23;

        public const ushort NC_CHARSAVE_PKCOUNT_CMD = 24;

        public const ushort NC_CHARSAVE_2WLDMAN_LINK_FROM_CMD = 25;

        public const ushort NC_CHARSAVE_REST_EXP_LAST_EXEC_TIME_SAVE_REQ = 26;

        public const ushort NC_CHARSAVE_REST_EXP_LAST_EXEC_TIME_SAVE_ACK = 27;

        public const ushort NC_CHARSAVE_AUTO_PICK_SAVE_REQ = 28;

        public const ushort NC_CHARSAVE_AUTO_PICK_SAVE_ACK = 29;

        public const ushort NC_CHARSAVE_SET_CHAT_BLOCK_SPAMER_WM_CMD = 33;

        public const ushort NC_CHARSAVE_SET_CHAT_BLOCK_SPAMER_DB_CMD = 34;

        public const ushort NC_CHARSAVE_ITEMACTIONCOOLTIME_REQ = 35;

        public const ushort NC_CHARSAVE_ITEMACTIONCOOLTIME_ACK = 36;

        public const ushort NC_CHARSAVE_DB_UI_STATE_SAVE_REQ = 39;

        public const ushort NC_CHARSAVE_DB_UI_STATE_SAVE_ACK = 40;

        public const ushort NC_CHARSAVE_2WLDMAN_QUEST_READ_CMD = 41;

        public const ushort NC_CHARSAVE_2WLDMAN_SINGLE_OPTION_CMD = 42;

        public const ushort NC_CHARSAVE_2WLDMAN_QUEST_REPEAT_CMD = 43;

        public const ushort NC_CHARSAVE_2WLDMAN_COININFO_CMD = 44;

        public const ushort NC_CHARSAVE_CHAT_COLOR_CMD = 49;

        public const ushort NC_CHARSAVE_2WLDMAN_CHAT_COLOR_CMD = 50;

        public const ushort NC_CHARSAVE_2WLDMAN_PET_LINK_RESUMMON_CMD = 51;

        public const ushort NC_CHARSAVE_2WLDMAN_ITEMACTIONCOOLTIME_CMD = 52;

        public const ushort NC_CHARSAVE_USEITEM_MINIMON_INFO_DB_CMD = 53;

        public const ushort NC_CHARSAVE_USEITEM_MINIMON_INFO_WORLD_CMD = 54;

        public const ushort NC_CHARSAVE_SELL_ITEM_INFO_CMD = 55;

    }
}
