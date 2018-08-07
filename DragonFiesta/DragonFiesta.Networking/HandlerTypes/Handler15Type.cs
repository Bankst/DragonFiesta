namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler15Type : HandlerType
    {
        public new const byte _Header = 15;


        //CMSG
        public const ushort CMSG_MENU_SERVERMENU_ACK = 2;


        //SMSG
        public const ushort SMSG_MENU_SERVERMENU_REQ = 1;

		public const ushort SMSG_MENU_SHOPOPENWEAPON_CMD = 3;

		public const ushort SMSG_MENU_SHOPOPENSKILL_CMD = 4;

		public const ushort SMSG_MENU_SHOPOPENSOULSTONE_CMD = 5;

		public const ushort SMSG_MENU_SHOPOPENITEM_CMD = 6;

		public const ushort SMSG_MENU_OPENSTORAGE_FAIL_CMD = 7;

		public const ushort SMSG_MENU_OPENSTORAGE_CMD = 8;

        public const ushort SMSG_MENU_SHOPOPENTABLE_WEAPON_CMD = 9;

        public const ushort SMSG_MENU_SHOPOPENTABLE_SKILL_CMD = 10;

        public const ushort SMSG_MENU_SHOPOPENTABLE_ITEM_CMD = 11;

        public const ushort SMSG_MENU_GUILDMENUOPEN_CMD = 12;        

        
        //NC
        public const ushort NC_MENU_RANDOMOPTION_CMD = 14;

        public const ushort NC_MENU_SERVERMENU_CLOSE_CMD = 15;

        public const ushort NC_MENU_INDUNRANK_CMD = 16; // TODO: unknown function

    }
}