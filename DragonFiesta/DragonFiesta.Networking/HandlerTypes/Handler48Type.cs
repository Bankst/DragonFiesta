namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler48Type : HandlerType
    {
        public new const byte _Header = 48;

        //CMSG
        public const ushort CMSG_Unk = 1;

        public const ushort CMSG_Unk2 = 8;

	    public const ushort CMSG_Unk3 = 16;

	    public const ushort CMSG_Unk4 = 17;

		//SMSG
		public const ushort SMSG_Unk = 2;

        public const ushort SMSG_Unk2 = 9;
    }
}
