namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler20Type : HandlerType
    {
        public new const byte _Header = 20;

        //CMSG
        public const ushort CMSG_SOULSTONE_HP_BUY_REQ = 1;

        public const ushort CMSG_SOULSTONE_SP_BUY_REQ = 2;        
        
        public const ushort CMSG_SOULSTONE_HP_USE_REQ = 7;        
        
        public const ushort CMSG_SOULSTONE_SP_USE_REQ = 9;        
        
        
        //SMSG
        public const ushort SMSG_SOULSTONE_HP_BUY_ACK = 3;

        public const ushort SMSG_SOULSTONE_SP_BUY_ACK = 4;

        public const ushort SMSG_SOULSTONE_BUYFAIL_ACK = 5;

        public const ushort SMSG_SOULSTONE_USEFAIL_ACK = 6;
        
        public const ushort SMSG_SOULSTONE_HP_USESUC_ACK = 8;

        public const ushort SMSG_SOULSTONE_SP_USESUC_ACK = 10;

        public const ushort SMSG_SOULSTONE_HP_SOMEONEUSE_CMD = 11;

        public const ushort SMSG_SOULSTONE_SP_SOMEONEUSE_CMD = 12;

    }
}