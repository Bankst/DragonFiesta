namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler50Type : HandlerType
    {
        public new const byte _Header = 50;


        //NC
        public const ushort NC_SYSLOG_ACCOUNT_LOGIN_SUCCESS = 1;

        public const ushort NC_SYSLOG_ACCOUNT_LOGIN_FAILURE = 2;

        public const ushort NC_SYSLOG_ACCOUNT_LOGOUT = 3;

        public const ushort NC_SYSLOG_CHAR_CREATED = 4;

        public const ushort NC_SYSLOG_CHAR_DELETED = 5;

        public const ushort NC_SYSLOG_CHAR_ENTER_GAME = 6;

        public const ushort NC_SYSLOG_CHAR_LEAVE_GAME = 7;

        public const ushort NC_SYSLOG_CHAR_LEVEL_UP = 8;

        public const ushort NC_SYSLOG_CHAR_ZONE_TRANSITION = 9;

        public const ushort NC_SYSLOG_CHAR_DEATH = 10;

        public const ushort NC_SYSLOG_CHAR_VICTORY = 11;

        public const ushort NC_SYSLOG_CHAR_LOOT = 12;

        public const ushort NC_SYSLOG_CHAR_ITEM_BUY = 13;

        public const ushort NC_SYSLOG_CHAR_ITEM_SELL = 14;

        public const ushort NC_SYSLOG_CHAR_QUEST_STARTED = 15;

        public const ushort NC_SYSLOG_CHAR_QUEST_FINISHED = 16;

        public const ushort NC_SYSLOG_SERVER_CCU = 17;

        public const ushort NC_SYSLOG_ZONE_FRAME = 18;

        public const ushort NC_SYSLOG_CHAR_ITEMMONEY_BUY = 20;

        public const ushort NC_SYSLOG_CHAR_ITEM_REBUY = 21;

        public const ushort NC_GAMIGO_NEW_TUTORIAL_STORE_STEP = 23;
    }
}
