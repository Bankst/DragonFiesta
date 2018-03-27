namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler35Type : HandlerType
    {
        public new const byte _Header = 35;


        //NC
        public const ushort NC_MINIHOUSE_ACTIV_REQ = 1;

        public const ushort NC_MINIHOUSE_ACTIV_ACK = 2;

        public const ushort NC_MINIHOUSE_BUILDING_REQ = 3;

        public const ushort NC_MINIHOUSE_BUILDING_ACK = 4;

        public const ushort NC_MINIHOUSE_BUILDING_CMD = 5;

        public const ushort NC_MINIHOUSE_FUNICHERINVEN_CMD = 8;

        public const ushort NC_MINIHOUSE_FUNICHER_CMD = 9;

        public const ushort NC_MINIHOUSE_LOGINCOMPLETE_REQ = 10;

        public const ushort NC_MINIHOUSE_LOGINCOMPLETE_ACK = 11;

        public const ushort NC_MINIHOUSE_LOGINCOMPLETE_CMD = 12;

        public const ushort NC_MINIHOUSE_LOGOUTCOMPLETE_CMD = 13;

        public const ushort NC_MINIHOUSE_VISITREADY_REQ = 14;

        public const ushort NC_MINIHOUSE_VISITREADY_ACK = 15;

        public const ushort NC_MINIHOUSE_VISIT_REQ = 16;

        public const ushort NC_MINIHOUSE_VISIT_ACK = 17;

        public const ushort NC_MINIHOUSE_KICKOUT_REQ = 18;

        public const ushort NC_MINIHOUSE_KICKOUT_ACK = 19;

        public const ushort NC_MINIHOUSE_KICKOUT_CMD = 20;

        public const ushort NC_MINIHOUSE_KICKOUTCANCEL_REQ = 21;

        public const ushort NC_MINIHOUSE_KICKOUTCANCEL_ACK = 22;

        public const ushort NC_MINIHOUSE_KICKOUTCANCEL_CMD = 23;

        public const ushort NC_MINIHOUSE_EXIT_REQ = 24;

        public const ushort NC_MINIHOUSE_EXIT_ACK = 25;

        public const ushort NC_MINIHOUSE_ARRANGEMODE_REQ = 26;

        public const ushort NC_MINIHOUSE_ARRANGEMODE_ACK = 27;

        public const ushort NC_MINIHOUSE_ARRANGEMODE_CMD = 28;

        public const ushort NC_MINIHOUSE_FUNITUREINFOCOMPLETE_REQ = 29;

        public const ushort NC_MINIHOUSE_FUNITUREINFOCOMPLETE_ACK = 30;

        public const ushort NC_MINIHOUSE_REARRANGE_REQ = 31;

        public const ushort NC_MINIHOUSE_REARRANGE_ACK = 32;

        public const ushort NC_MINIHOUSE_REARRANGE_CMD = 33;

        public const ushort NC_MINIHOUSE_CREATE_FURNITURE_REQ = 34;

        public const ushort NC_MINIHOUSE_CREATE_FURNITURE_ACK = 35;

        public const ushort NC_MINIHOUSE_CREATE_FURNITURE_CMD = 36;

        public const ushort NC_MINIHOUSE_DELETE_FURNITURE_REQ = 37;

        public const ushort NC_MINIHOUSE_DELETE_FURNITURE_ACK = 38;

        public const ushort NC_MINIHOUSE_DELETE_FURNITURE_CMD = 39;

        public const ushort NC_MINIHOUSE_COMPULSIONMOVETO_REQ = 40;

        public const ushort NC_MINIHOUSE_COMPULSIONMOVETO_ACK = 41;

        public const ushort NC_MINIHOUSE_COMPULSIONMOVETO_CMD = 42;

        public const ushort NC_MINIHOUSE_MODIFY_PASSWORD_REQ = 43;

        public const ushort NC_MINIHOUSE_MODIFY_PASSWORD_ACK = 44;

        public const ushort NC_MINIHOUSE_MODIFY_PASSWORD_CMD = 45;

        public const ushort NC_MINIHOUSE_MODIFY_TITLE_REQ = 46;

        public const ushort NC_MINIHOUSE_MODIFY_TITLE_ACK = 47;

        public const ushort NC_MINIHOUSE_MODIFY_TITLE_CMD = 48;

        public const ushort NC_MINIHOUSE_MODIFY_OUTSIDE_TITLE_CMD = 49;

        public const ushort NC_MINIHOUSE_MODIFY_MAXENTERNUM_REQ = 50;

        public const ushort NC_MINIHOUSE_MODIFY_MAXENTERNUM_ACK = 51;

        public const ushort NC_MINIHOUSE_MODIFY_MAXENTERNUM_CMD = 52;

        public const ushort NC_MINIHOUSE_MODIFY_OWNERBLOG_REQ = 53;

        public const ushort NC_MINIHOUSE_MODIFY_OWNERBLOG_ACK = 54;

        public const ushort NC_MINIHOUSE_OWNERBLOG_REQ = 55;

        public const ushort NC_MINIHOUSE_OWNERBLOG_ACK = 56;

        public const ushort NC_MINIHOUSE_DB_OWNERBLOG_GET_REQ = 57;

        public const ushort NC_MINIHOUSE_DB_OWNERBLOG_GET_ACK = 58;

        public const ushort NC_MINIHOUSE_DB_OWNERBLOG_SET_REQ = 59;

        public const ushort NC_MINIHOUSE_DB_OWNERBLOG_SET_ACK = 60;

        public const ushort NC_MINIHOUSE_CHAR_ACTION_REQ = 61;

        public const ushort NC_MINIHOUSE_CHAR_ACTION_ACK = 62;

        public const ushort NC_MINIHOUSE_CHAR_ACTION_CMD = 63;

        public const ushort NC_MINIHOUSE_FURNITURE_EFFECT_REQ = 64;

        public const ushort NC_MINIHOUSE_FURNITURE_EFFECT_ACK = 65;

        public const ushort NC_MINIHOUSE_FURNITURE_EFFECT_CMD = 66;

        public const ushort NC_MINIHOUSE_MODIFY_ITEM_INFO_OPEN_REQ = 67;

        public const ushort NC_MINIHOUSE_MODIFY_ITEM_INFO_OPEN_ACK = 68;

        public const ushort NC_MINIHOUSE_MODIFY_ITEM_INFO_OPEN_CMD = 69;

        public const ushort NC_MINIHOUSE_MODIFY_NOTIFY_REQ = 72;

        public const ushort NC_MINIHOUSE_MODIFY_NOTIFY_ACK = 73;

        public const ushort NC_MINIHOUSE_MODIFY_NOTIFY_CMD = 74;

        public const ushort NC_MINIHOUSE_PORTAL_LIST_CMD = 75;

        public const ushort NC_MINIHOUSE_PORTAL_ADD_CMD = 76;

        public const ushort NC_MINIHOUSE_PORTAL_DEL_REQ = 77;

        public const ushort NC_MINIHOUSE_PORTAL_DEL_ACK = 78;

        public const ushort NC_MINIHOUSE_PORTAL_DEL_CMD = 79;

        public const ushort NC_MINIHOUSE_PORTAL_OPEN_REQ = 80;

        public const ushort NC_MINIHOUSE_PORTAL_OPEN_ACK = 81;

        public const ushort NC_MINIHOUSE_PORTAL_CLOSE_REQ = 83;

        public const ushort NC_MINIHOUSE_PORTAL_CLOSE_ACK = 84;

        public const ushort NC_MINIHOUSE_PORTAL_EFFECT_REQ = 86;

        public const ushort NC_MINIHOUSE_PORTAL_EFFECT_ACK = 87;

        public const ushort NC_MINIHOUSE_PORTAL_EFFECT_CMD = 88;

        public const ushort NC_MINIHOUSE_DB_PORTAL_LIST_REQ = 89;

        public const ushort NC_MINIHOUSE_DB_PORTAL_LIST_ACK = 90;

        public const ushort NC_MINIHOUSE_DB_VISITER_COUNT_REQ = 91;

        public const ushort NC_MINIHOUSE_DB_VISITER_COUNT_ACK = 92;

        public const ushort NC_MINIHOUSE_DB_PORTAL_ADD_REQ = 93;

        public const ushort NC_MINIHOUSE_DB_PORTAL_ADD_ACK = 94;

        public const ushort NC_MINIHOUSE_DB_PORTAL_DEL_REQ = 95;

        public const ushort NC_MINIHOUSE_DB_PORTAL_DEL_ACK = 96;

        public const ushort NC_MINIHOUSE_FURNITURE_ENDURE_CMD = 97;

        public const ushort NC_MINIHOUSE_FURNITURE_EMOTION_REQ = 98;

        public const ushort NC_MINIHOUSE_FURNITURE_EMOTION_ACK = 99;

        public const ushort NC_MINIHOUSE_FURNITURE_EMOTION_CMD = 100;

        public const ushort NC_MINIHOUSE_FURNITURE_EMOTION_CANCEL_REQ = 101;

        public const ushort NC_MINIHOUSE_FURNITURE_EMOTION_CANCEL_ACK = 102;

        public const ushort NC_MINIHOUSE_FURNITURE_EMOTION_CANCEL_CMD = 103;

    }
}
