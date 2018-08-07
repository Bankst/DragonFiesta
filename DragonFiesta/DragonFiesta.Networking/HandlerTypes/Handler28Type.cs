namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler28Type : HandlerType
    {
        public new const byte _Header = 28;


        //CMSG
        public const ushort CMSG_CHAR_OPTION_GET_SHORTCUTDATA_REQ = 2;

        public const ushort CMSG_CHAR_OPTION_GET_SHORTCUTSIZE_REQ = 4;

        public const ushort CMSG_CHAR_OPTION_GET_GAME_REQ = 10;

        public const ushort CMSG_CHAR_OPTION_GET_WINDOWPOS_REQ = 12;

        public const ushort CMSG_CHAR_OPTION_GET_KEYMAPPING_REQ = 14;

        public const ushort CMSG_CHAR_OPTION_SET_SHORTCUTDATA_CMD = 16;
        
        public const ushort CMSG_CHAR_OPTION_SET_SHORTCUTSIZE_CMD = 17;       
        
        public const ushort CMSG_CHAR_OPTION_SET_GAME_CMD = 20;

        public const ushort CMSG_CHAR_OPTION_SET_WINDOWPOS_CMD = 21;

        public const ushort CMSG_CHAR_OPTION_SET_KEYMAPPING_CMD = 22;

        public const ushort CMSG_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_REQ = 55;

        public const ushort CMSG_CHAR_OPTION_IMPROVE_SET_KEYMAP_REQ = 57;

        public const ushort CMSG_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_REQ = 59;

        public const ushort CMSG_CHAR_OPTION_IMPROVE_INIT_KEYMAP_REQ = 67;

        public const ushort CMSG_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_REQ = 69;


        //SMSG
        public const ushort SMSG_CHAR_OPTION_GET_SHORTCUTDATA_ACK = 3;

        public const ushort SMSG_CHAR_OPTION_GET_SHORTCUTSIZE_ACK = 5;

        public const ushort SMSG_CHAR_OPTION_GET_GAME_ACK = 11;

        public const ushort SMSG_CHAR_OPTION_GET_WINDOWPOS_ACK = 13;

        public const ushort SMSG_CHAR_OPTION_GET_KEYMAPPING_ACK = 15;

        public const ushort SMSG_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD = 50;

        public const ushort SMSG_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD = 51;

        public const ushort SMSG_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD = 52;

        public const ushort SMSG_CHAR_OPTION_IMPROVE_SET_SHORTCUTDATA_ACK = 56;

        public const ushort SMSG_CHAR_OPTION_IMPROVE_INIT_KEYMAP_ACK = 68;

        public const ushort SMSG_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_ACK = 70;

      
        //NC
        public const ushort NC_CHAR_OPTION_GET_ALL_REQ = 1;

        public const ushort NC_CHAR_OPTION_GET_VIDEO_REQ = 6;

        public const ushort NC_CHAR_OPTION_GET_VIDEO_ACK = 7;

        public const ushort NC_CHAR_OPTION_GET_SOUND_REQ = 8;

        public const ushort NC_CHAR_OPTION_GET_SOUND_ACK = 9;

        public const ushort NC_CHAR_OPTION_SET_VIDEO_CMD = 18;

        public const ushort NC_CHAR_OPTION_SET_SOUND_CMD = 19;

        public const ushort NC_CHAR_OPTION_DB_GET_SHORTCUTDATA_REQ = 23;

        public const ushort NC_CHAR_OPTION_DB_GET_SHORTCUTDATA_ACK = 24;

        public const ushort NC_CHAR_OPTION_DB_GET_SHORTCUTSIZE_REQ = 25;

        public const ushort NC_CHAR_OPTION_DB_GET_SHORTCUTSIZE_ACK = 26;

        public const ushort NC_CHAR_OPTION_DB_GET_VIDEO_REQ = 27;

        public const ushort NC_CHAR_OPTION_DB_GET_VIDEO_ACK = 28;

        public const ushort NC_CHAR_OPTION_DB_GET_SOUND_REQ = 29;

        public const ushort NC_CHAR_OPTION_DB_GET_SOUND_ACK = 30;

        public const ushort NC_CHAR_OPTION_DB_GET_GAME_REQ = 31;

        public const ushort NC_CHAR_OPTION_DB_GET_GAME_ACK = 32;

        public const ushort NC_CHAR_OPTION_DB_GET_WINDOWPOS_REQ = 33;

        public const ushort NC_CHAR_OPTION_DB_GET_WINDOWPOS_ACK = 34;

        public const ushort NC_CHAR_OPTION_DB_GET_KEYMAPPING_REQ = 35;

        public const ushort NC_CHAR_OPTION_DB_GET_KEYMAPPING_ACK = 36;

        public const ushort NC_CHAR_OPTION_DB_SET_SHORTCUTDATA_CMD = 37;

        public const ushort NC_CHAR_OPTION_DB_SET_SHORTCUTSIZE_CMD = 38;

        public const ushort NC_CHAR_OPTION_DB_SET_VIDEO_CMD = 39;

        public const ushort NC_CHAR_OPTION_DB_SET_SOUND_CMD = 40;

        public const ushort NC_CHAR_OPTION_DB_SET_GAME_CMD = 41;

        public const ushort NC_CHAR_OPTION_DB_SET_WINDOWPOS_CMD = 42;

        public const ushort NC_CHAR_OPTION_DB_SET_KEYMAPPING_CMD = 43;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_GET_DATA_TYPE_CMD = 44;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_GET_SHORTCUTDATA_CMD = 45;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_GET_KEYMAP_CMD = 46;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_GET_GAMEOPTION_CMD = 47;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_GET_ETC3_CMD = 48;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_GET_ETC4_CMD = 49;

        public const ushort NC_CHAR_OPTION_IMPROVE_GET_ETC3_CMD = 53;

        public const ushort NC_CHAR_OPTION_IMPROVE_GET_ETC4_CMD = 54;

        public const ushort NC_CHAR_OPTION_IMPROVE_SET_KEYMAP_ACK = 58;
    
        public const ushort NC_CHAR_OPTION_IMPROVE_SET_GAMEOPTION_ACK = 60;

        public const ushort NC_CHAR_OPTION_IMPROVE_SET_ETC3_REQ = 61;

        public const ushort NC_CHAR_OPTION_IMPROVE_SET_ETC3_ACK = 62;

        public const ushort NC_CHAR_OPTION_IMPROVE_SET_ETC4_REQ = 63;

        public const ushort NC_CHAR_OPTION_IMPROVE_SET_ETC4_ACK = 64;

        public const ushort NC_CHAR_OPTION_IMPROVE_INIT_SHORTCUTDATA_REQ = 65;

        public const ushort NC_CHAR_OPTION_IMPROVE_INIT_SHORTCUTDATA_ACK = 66;

        public const ushort NC_CHAR_OPTION_IMPROVE_INIT_ETC3_REQ = 71;

        public const ushort NC_CHAR_OPTION_IMPROVE_INIT_ETC3_ACK = 72;

        public const ushort NC_CHAR_OPTION_IMPROVE_INIT_ETC4_REQ = 73;

        public const ushort NC_CHAR_OPTION_IMPROVE_INIT_ETC4_ACK = 74;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_SHORTCUTDATA_REQ = 75;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_SHORTCUTDATA_ACK = 76;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_KEYMAP_REQ = 77;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_KEYMAP_ACK = 78;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_GAMEOPTION_REQ = 79;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_GAMEOPTION_ACK = 80;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_ETC3_REQ = 81;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_ETC3_ACK = 82;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_ETC4_REQ = 83;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_SET_ETC4_ACK = 84;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_SHORTCUTDATA_REQ = 85;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_SHORTCUTDATA_ACK = 86;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_KEYMAP_REQ = 87;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_KEYMAP_ACK = 88;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_GAMEOPTION_REQ = 89;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_GAMEOPTION_ACK = 90;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_ETC3_REQ = 91;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_ETC3_ACK = 92;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_ETC4_REQ = 93;

        public const ushort NC_CHAR_OPTION_IMPROVE_DB_INIT_ETC4_ACK = 94;

        public const ushort NC_CHAR_OPTION_LOGIN_BLOCKDATA_ERR = 95;

        public const ushort NC_CHAR_OPTION_LOGIN_BLOCKDATA_ERR_REQ = 96;      
        
    }
}