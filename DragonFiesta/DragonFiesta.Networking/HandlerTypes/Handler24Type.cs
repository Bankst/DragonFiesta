namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler24Type : HandlerType
    {
        public new const byte _Header = 24;
        
        
        //NC
        public const ushort NC_CT_SET_CURRENT_REQ = 1;

        public const ushort NC_CT_SET_CURRENT_ACK = 2;

        public const ushort NC_CT_SET_SOMEONECHANGE_CMD = 3;

        public const ushort NC_CT_SET_CMD = 4;

        public const ushort NC_CT_DB_SET_CMD = 5;

        public const ushort NC_CT_CHARTTING_CMD = 6;

        public const ushort NC_CT_ADD_FRIEND_CMD = 7;

        public const ushort NC_CT_SET_CURRENT_DB_CMD = 8;

        public const ushort NC_CT_LUASCRIPT_SET_WORLD_CMD = 9;

        public const ushort NC_CT_LUASCRIPT_SET_ZONE_CMD = 10;         
        
    }
}
