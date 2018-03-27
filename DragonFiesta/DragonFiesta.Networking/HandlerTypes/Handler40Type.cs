namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler40Type : HandlerType
    {
        public new const byte _Header = 40;

        //NC
        public const ushort NC_PROMOTION_USER_REQ = 1;

        public const ushort NC_PROMOTION_USER_ACK = 2;

        public const ushort NC_PROMOTION_USER_CMD = 3;

        public const ushort NC_PROMOTION_DB_REWARD_REQ = 4;

        public const ushort NC_PROMOTION_DB_REWARD_ACK = 5;

        public const ushort NC_PROMOTION_REWARD_ITEM_CMD = 6;        

    }
}
