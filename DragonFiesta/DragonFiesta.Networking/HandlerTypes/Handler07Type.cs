namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler07Type : HandlerType
    {
        public new const byte _Header = 7;

        //CMSG
        public const ushort CMSG_BRIEFINFO_INFORM_CMD = 1;


        //SMSG
        public const ushort SMSG_BRIEFINFO_UNEQUIP_CMD = 4;

        public const ushort SMSG_BRIEFINFO_CHANGEWEAPON_CMD = 5;

        public const ushort SMSG_BRIEFINFO_LOGINCHARACTER_CMD = 6;

        public const ushort SMSG_BRIEFINFO_CHARACTER_CMD = 7;

        public const ushort SMSG_BRIEFINFO_REGENMOB_CMD = 8;

        public const ushort SMSG_BRIEFINFO_MOB_CMD = 9;

        public const ushort SMSG_BRIEFINFO_DROPEDITEM_CMD = 10;

        public const ushort SMSG_BRIEFINFO_ITEMONFIELD_CMD = 11;       

        public const ushort SMSG_BRIEFINFO_BRIEFINFODELETE_CMD = 14;

        public const ushort SMSG_BRIEFINFO_ABSTATE_CHANGE_CMD = 24;


        //NC
        public const ushort NC_BRIEFINFO_CHANGEDECORATE_CMD = 2;

        public const ushort NC_BRIEFINFO_CHANGEUPGRADE_CMD = 3;

        public const ushort NC_BRIEFINFO_MAGICFIELDSPREAD_CMD = 12;

        public const ushort NC_BRIEFINFO_MAGICFIELDINFO_CMD = 13;

        public const ushort NC_BRIEFINFO_BUILDDOOR_CMD = 15;

        public const ushort NC_BRIEFINFO_DOOR_CMD = 16;

        public const ushort NC_BRIEFINFO_EFFECTBLAST_CMD = 17;

        public const ushort NC_BRIEFINFO_EFFECT_CMD = 18;

        public const ushort NC_BRIEFINFO_MINIHOUSEBUILD_CMD = 19;

        public const ushort NC_BRIEFINFO_MINIHOUSE_CMD = 20;

        public const ushort NC_BRIEFINFO_PLAYER_LIST_INFO_APPEAR_CMD = 22;

        public const ushort NC_BRIEFINFO_ABSTATE_CHANGE_LIST_CMD = 25;

        public const ushort NC_BRIEFINFO_REGENMOVER_CMD = 26;

        public const ushort NC_BRIEFINFO_MOVER_CMD = 27;

        public const ushort NC_BRIEFINFO_REGENPET_CMD = 28;

        public const ushort NC_BRIEFINFO_PET_CMD = 29;

    }
}