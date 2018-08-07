namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler52Type : HandlerType
    {
        public new const byte _Header = 52;


        //NC
        public const ushort NC_EVENT_GET_ALL_EVENT_INFO_REQ = 1;

        public const ushort NC_EVENT_GET_ALL_EVENT_INFO_ACK = 2;

        public const ushort NC_EVENT_ADD_EVENT_REQ = 3;

        public const ushort NC_EVENT_ADD_EVENT_ACK = 4;

        public const ushort NC_EVENT_UPDATE_EVENT_REQ = 5;

        public const ushort NC_EVENT_UPDATE_EVENT_ACK = 6;

        public const ushort NC_EVENT_DEL_EVENT_REQ = 7;

        public const ushort NC_EVENT_DEL_EVENT_ACK = 8;

        public const ushort NC_EVENT_SET_ALL_READY_REQ = 9;

        public const ushort NC_EVENT_SET_ALL_READY_ACK = 10;

        public const ushort NC_EVENT_ADD_UPDATE_EVENT_CMD = 11;

        public const ushort NC_EVENT_DEL_EVENT_CMD = 12;
    }
}
