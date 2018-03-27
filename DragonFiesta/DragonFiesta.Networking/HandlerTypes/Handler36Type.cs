namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler36Type  : HandlerType
    {
        public new const byte _Header = 36;


        //NC
        public const ushort NC_CHARGED_SETBUFF_CMD = 1;

        public const ushort NC_CHARGED_RESETBUFF_CMD = 2;

        public const ushort NC_CHARGED_BUFFSTART_CMD = 3;

        public const ushort NC_CHARGED_BUFFTERMINATE_CMD = 4;

        public const ushort NC_CHARGED_STAT_INITIALIZE_DB_REQ = 5;

        public const ushort NC_CHARGED_SKILLEMPOW_INITIALIZE_DB_REQ = 6;

        public const ushort NC_CHARGED_STAT_INITIALIZE_DB_SUC_ACK = 7;

        public const ushort NC_CHARGED_SKILLEMPOW_INITIALIZE_DB_SUC_ACK = 8;

        public const ushort NC_CHARGED_STAT_INITIALIZE_DB_FAIL_ACK = 9;

        public const ushort NC_CHARGED_SKILLEMPOW_INITIALIZE_DB_FAIL_ACK = 10;

        public const ushort NC_CHARGED_DELETEWEAPONTITLE_CMD = 11;

        public const ushort NC_CHARGED_STAT_INITIALIZE_SUC_CMD = 12;

        public const ushort NC_CHARGED_SKILLEMPOW_INITIALIZE_SUC_CMD = 13;

        public const ushort NC_CHARGED_STAT_INITIALIZE_FAIL_CMD = 14;

        public const ushort NC_CHARGED_SKILLEMPOW_INITIALIZE_FAIL_CMD = 15;

        public const ushort NC_CHARGED_BOOTHSLOTSIZE_CMD = 16;

    }
}
