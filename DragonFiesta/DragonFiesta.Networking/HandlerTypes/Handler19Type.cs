namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler19Typ : HandlerType
    {
        public new const byte _Header = 19;


        //CMSG
        public const ushort CMSG_TRADE_PROPOSE_REQ = 1;

        public const ushort CMSG_TRADE_PROPOSE_ASKNO_ACK = 3;        
        
        public const ushort CMSG_TRADE_PROPOSEYES_ACK = 6;
        
        public const ushort CMSG_TRADE_CANCEL_REQ = 10;

        public const ushort CMSG_TRADE_UPBOARD_REQ = 13;
        
        public const ushort CMSG_TRADE_DOWNBOARD_REQ = 17;

        public const ushort CMSG_TRADE_CENBOARDING_REQ = 21;        
        
        public const ushort CMSG_TRADE_BOARDLOCK_REQ = 25;
        
        public const ushort CMSG_TRADE_DECIDE_REQ = 31;


        
        //SMSG
        public const ushort SMSG_TRADE_PROPOSE_ASK_REQ = 2;

        public const ushort SMSG_TRADE_PROPOSENO_ACK = 4;

        public const ushort SMSG_TRADE_START_CMD = 9;

        public const ushort SMSG_TRADE_CANCEL_CMD = 12;

        public const ushort SMSG_TRADE_UPBOARD_ACK = 15;

        public const ushort SMSG_TRADE_OPPOSITUPBOARD_CMD = 16;

        public const ushort SMSG_TRADE_DOWNBOARD_ACK = 19;

        public const ushort SMSG_TRADE_OPPOSITDOWNBOARD_CMD = 20;

        public const ushort SMSG_TRADE_OPPOSITCENBOARDING_CMD = 24;

        public const ushort SMSG_TRADE_BOARDLOCK_ACK = 27;

        public const ushort SMSG_TRADE_OPPOSITBOARDLOCK_CMD = 28;

        public const ushort SMSG_TRADE_DECIDE_ACK = 33;

        public const ushort SMSG_TRADE_OPPOSITDECIDE_CMD = 34;

        public const ushort SMSG_TRADE_TRADECOMPLETE_CMD = 36;


        //NC
        public const ushort NC_TRADE_PROPOSE_ASKYES_ACK = 5;

        public const ushort NC_TRADE_PROPOSE_CANCEL_CMD = 7;

        public const ushort NC_TRADE_PROPOSE_CANCELED_CMD = 8;

        public const ushort NC_TRADE_CANCEL_ACK = 11;

        public const ushort NC_TRADE_UPBOARDFAIL_ACK = 14;

        public const ushort NC_TRADE_DOWNBOARDFAIL_ACK = 18;

        public const ushort NC_TRADE_CENBOARDINGFAIL_ACK = 22;

        public const ushort NC_TRADE_CENBOARDING_ACK = 23;

        public const ushort NC_TRADE_BOARDLOCKFAIL_ACK = 26;

        public const ushort NC_TRADE_BOARDUNLOCK_CMD = 29;

        public const ushort NC_TRADE_OPPOSITBOARDUNLOCK_CMD = 30;

        public const ushort NC_TRADE_DECIDEFAIL_ACK = 32;

        public const ushort NC_TRADE_TRADEFAIL_CMD = 35;
        
    }
}