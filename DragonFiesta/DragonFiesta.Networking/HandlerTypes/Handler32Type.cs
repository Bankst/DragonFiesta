namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler32Type : HandlerType
    {
        public new const byte _Header = 32;


        //NC
        public const ushort NC_REPORT_ADD_REQ = 1;

        public const ushort NC_REPORT_ADD_ACK = 2;

        public const ushort NC_REPORT_CANCEL_REQ = 3;

        public const ushort NC_REPORT_CANCEL_ACK = 4;

        public const ushort NC_REPORT_GET_REQ = 5;

        public const ushort NC_REPORT_GET_ACK = 6;

        public const ushort NC_DATA_REPORT_ADD_REQ = 7;

        public const ushort NC_DATA_REPORT_ADD_ACK = 8;

        public const ushort NC_DATA_REPORT_CANCEL_REQ = 9;

        public const ushort NC_DATA_REPORT_CANCEL_ACK = 10;

        public const ushort NC_DATA_REPORT_GET_REQ = 11;

        public const ushort NC_DATA_REPORT_GET_ACK = 12;

    }
}
