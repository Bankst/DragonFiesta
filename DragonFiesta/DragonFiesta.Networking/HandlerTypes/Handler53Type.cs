namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler53Type : HandlerType
    {
        public new const byte _Header = 53;

        //NC
        public const ushort NC_PET_SET_TENDENCY_DB_REQ = 1;

        public const ushort NC_PET_SET_NAME_DB_REQ = 3;

        public const ushort NC_PET_SET_NAME_DB_ACK = 4;

        public const ushort NC_PET_ASK_NEW_NAME_REQ = 5;

        public const ushort NC_PET_ASK_NEW_NAME_ACK = 6;

        public const ushort NC_PET_SET_NAME_REQ = 7;

        public const ushort NC_PET_SET_NAME_ACK = 8;

        public const ushort NC_PET_SET_NAME_CANCEL_REQ = 9;

        public const ushort NC_PET_SET_NAME_CANCEL_ACK = 10;

        public const ushort NC_PET_LOAD_INFO_DB_REQ = 11;

        public const ushort NC_PET_LOAD_INFO_DB_ACK = 12;

        public const ushort NC_PET_CREATE_DB_REQ = 13;

        public const ushort NC_PET_CREATE_DB_ACK = 14;

        public const ushort NC_PET_REMOVE_DB_REQ = 15;

        public const ushort NC_PET_REMOVE_DB_ACK = 16;

        public const ushort NC_PET_USE_ITEM_FAIL_ACK = 17;

        public const ushort NC_PET_LINK_RESUMMON_CMD = 18;

        public const ushort NC_PET_SET_NAME_CMD = 19;
    }
}
