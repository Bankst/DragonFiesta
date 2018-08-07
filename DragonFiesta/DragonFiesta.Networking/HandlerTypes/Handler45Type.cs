namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler45Type : HandlerType
    {
        public new const byte _Header = 45;
        
        //NC
        public const ushort NC_USER_CONNECTION_SET_ACCLOG_UP_CMD = 1;

        public const ushort NC_USER_CONNECTION_SET_WORLD_DOWN_CMD = 2;

        public const ushort NC_USER_CONNECTION_GET_LOGIN_USER_REQ = 3;

        public const ushort NC_USER_CONNECTION_GET_LOGIN_USER_ACK = 4;

        public const ushort NC_USER_CONNECTION_SET_USER_WORLD_LOGIN_CMD = 5;

        public const ushort NC_USER_CONNECTION_SET_USER_WORLD_LOGOUT_CMD = 6;

        public const ushort NC_USER_CONNECTION_CHANGE_CHAR_ID_REQ = 9;

        public const ushort NC_USER_CONNECTION_DB_CHANGE_CHAR_ID_REQ = 10;

        public const ushort NC_USER_CONNECTION_DB_CHANGE_CHAR_ID_ACK = 11;

        public const ushort NC_USER_CONNECTION_CHANGE_CHAR_ID_ACK = 12;

        public const ushort NC_USER_CONNECTION_CHANGE_CHAR_ID_CMD = 13;

        public const ushort NC_USER_CONNECTION_CHANGE_CHAR_ID_ACADEMY_MASTER_CMD = 14;

        public const ushort NC_USER_CONNECTION_ZONE_CHANGE_CHAR_ID_CMD = 15;

        public const ushort NC_USER_CONNECTION_CHANGE_CHAR_ID_ITEM_USE_REQ = 16;

        public const ushort NC_USER_CONNECTION_DB_CHANGE_CHAR_ID_ITEM_USE_REQ = 17;

        public const ushort NC_USER_CONNECTION_DB_CHANGE_CHAR_ID_ITEM_USE_ACK = 18;

        public const ushort NC_USER_CONNECTION_CHANGE_CHAR_ID_ITEM_USE_ACK = 19;

    }
}
