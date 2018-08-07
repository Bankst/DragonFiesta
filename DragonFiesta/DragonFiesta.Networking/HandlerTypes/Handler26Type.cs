namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler26Type : HandlerType
    {
        public new const byte _Header = 26;

        //CMSG
        public const ushort CMSG_BOOTH_ENTRY_REQ = 7;

        public const ushort CMSG_BOOTH_REFRESH_REQ = 10;

        public const ushort Unk = 24;

        //SMSG
        public const ushort SMSG_BOOTH_SOMEONEOPEN_CMD = 3;

        public const ushort SMSG_BOOTH_ENTRY_SELL_ACK = 8;

        public const ushort SMSG_BOOTH_REFRESH_SELL_ACK = 11;

        public const ushort SMSG_BOOTH_SEARCH_BOOTH_CLOSED_CMD = 23;


        //NC
        public const ushort NC_BOOTH_OPEN_REQ = 1;

        public const ushort NC_BOOTH_OPEN_ACK = 2;

        public const ushort NC_BOOTH_CLOSE_REQ = 4;

        public const ushort NC_BOOTH_CLOSE_ACK = 5;

        public const ushort NC_BOOTH_SOMEONECLOSE_CMD = 6;

        public const ushort NC_BOOTH_ENTRY_BUY_ACK = 9;

        public const ushort NC_BOOTH_REFRESH_BUY_ACK = 12;

        public const ushort NC_BOOTH_ITEMTRADE_REQ = 13;

        public const ushort NC_BOOTH_ITEMTRADE_ACK = 14;

        public const ushort NC_BOOTH_BUYREFRESH_CMD = 15;

        public const ushort NC_BOOTH_INTERIORSTART_REQ = 16;

        public const ushort NC_BOOTH_INTERIORSTART_ACK = 17;

        public const ushort NC_BOOTH_SOMEONEINTERIORSTART_CMD = 18;

        public const ushort NC_BOOTH_SEARCH_ITEM_LIST_CATEGORIZED_REQ = 19;

        public const ushort NC_BOOTH_SEARCH_ITEM_LIST_CATEGORIZED_ACK = 20;

        public const ushort NC_BOOTH_SEARCH_BOOTH_POSITION_REQ = 21;

        public const ushort NC_BOOTH_SEARCH_BOOTH_POSITION_ACK = 22; 
        
    }
}
