namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler51Type : HandlerType
    {
        public new const byte _Header = 51;

        //SMSG
        public const ushort SMSG_MOVER_SOMEONE_RIDE_ON_CMD = 4;

        public const ushort SMSG_MOVER_SOMEONE_RIDE_OFF_CMD = 8;


        //NC
        public const ushort NC_MOVER_RIDE_ON_REQ = 1;

        public const ushort NC_MOVER_RIDE_ON_CMD = 2;

        public const ushort NC_MOVER_RIDE_ON_FAIL_CMD = 3;

        public const ushort NC_MOVER_RIDE_OFF_REQ = 5;

        public const ushort NC_MOVER_RIDE_OFF_CMD = 6;

        public const ushort NC_MOVER_RIDE_OFF_FAIL_CMD = 7;

        public const ushort NC_MOVER_FEEDING_ERROR_CMD = 9;

        public const ushort NC_MOVER_HUNGRY_CMD = 10;

        public const ushort NC_MOVER_LINKDATA_WORLD_CMD = 11;

        public const ushort NC_MOVER_LINKDATA_ZONE_CMD = 12;

        public const ushort NC_MOVER_MOVESPEED_CMD = 13;

        public const ushort NC_MOVER_SKILLBASH_OBJ_CAST_REQ = 14;

        public const ushort NC_MOVER_SKILLBASH_FLD_CAST_REQ = 15;

    }
}
