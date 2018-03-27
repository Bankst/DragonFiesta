namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler11Type : HandlerType
    {
        public new const byte _Header = 11;


        //NC
        public const ushort NC_PATCH_LAUNCHER_VERSION_REQ = 1;

        public const ushort NC_PATCH_LAUNCHER_VERSION_ACK = 2;

        public const ushort NC_PATCH_NOTICE_REQ = 3;

        public const ushort NC_PATCH_NOTICE_ACK = 4;

        public const ushort NC_PATCH_CLIENT_VERSION_REQ = 5;

        public const ushort NC_PATCH_CLIENT_VERSION_ACK = 6;

        public const ushort NC_PATCH_SERVER_ALLOC_REQ = 7;

        public const ushort NC_PATCH_SERVER_ALLOC_ACK = 8;

        public const ushort NC_PATCH_INFO_REQ = 9;

        public const ushort NC_PATCH_INFO_ACK = 10;

        public const ushort NC_PATCH_INFO_DATA_REQ = 11;

        public const ushort NC_PATCH_INFO_DATA_ACK = 12;

        public const ushort NC_PATCH_FILE_INFO_REQ = 13;

        public const ushort NC_PATCH_FILE_INFO_ACK = 14;

        public const ushort NC_PATCH_FILE_DATA_REQ = 15;

        public const ushort NC_PATCH_FILE_DATA_ACK = 16;

        public const ushort NC_PATCH_CLOSE_REQ = 17;

        public const ushort NC_PATCH_STATUS_SET_REQ = 18;

        public const ushort NC_PATCH_NOTICE_SET_REQ = 19;

        public const ushort NC_PATCH_INFO_VERIFY_REQ = 20;

        public const ushort NC_PATCH_INFO_VERIFY_ACK = 21;

        public const ushort NC_PATCH_DATA_SERVER_READY_CMD = 22;

        public const ushort NC_PATCH_DATA_SERVER_USER_COUNT_CMD = 23;
    }
}
