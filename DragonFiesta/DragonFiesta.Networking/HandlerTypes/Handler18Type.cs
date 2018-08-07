namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler18Type : HandlerType
    {
        public new const byte _Header = 18;


        //SMSG
        public const ushort SMSG_SKILL_SKILL_LEARNSUC_CMD = 4;

        public const ushort SMSG_SKILL_EMPOWPOINT_CMD = 16;

        public const ushort SMSG_SKILL_COOLTIME_CMD = 33;


        //NC
        public const ushort NC_SKILL_SKILLTEACH_REQ = 1;

        public const ushort NC_SKILL_SKILLTEACHSUC_ACK = 2;

        public const ushort NC_SKILL_SKILLTEACHFAIL_ACK = 3;

        public const ushort NC_SKILL_SKILL_LEARNFAIL_CMD = 5;

        public const ushort NC_SKILL_SETABSTATE_CMD = 6;

        public const ushort NC_SKILL_SOMEONESETABSTATE_CMD = 7;

        public const ushort NC_SKILL_RESETABSTATE_CMD = 8;

        public const ushort NC_SKILL_SOMEONERESETABSTATE_CMD = 9;

        public const ushort NC_SKILL_EMPOW_RESET_REQ = 10;

        public const ushort NC_SKILL_EMPOW_RESET_DB_REQ = 11;

        public const ushort NC_SKILL_EMPOW_RESET_DB_FAIL_ACK = 12;

        public const ushort NC_SKILL_EMPOW_RESET_DB_SUC_ACK = 13;

        public const ushort NC_SKILL_EMPOW_RESET_FAIL_ACK = 14;

        public const ushort NC_SKILL_EMPOW_RESET_SUC_ACK = 15;

        public const ushort NC_SKILL_EMPOWALLOC_REQ = 17;

        public const ushort NC_SKILL_EMPOWALLOC_DB_REQ = 18;

        public const ushort NC_SKILL_EMPOWALLOC_DB_ACK = 19;

        public const ushort NC_SKILL_EMPOWALLOC_ACK = 20;

        public const ushort NC_SKILL_SKILLEXP_CLIENT_CMD = 29;

        public const ushort NC_SKILL_REVIVE_CMD = 30;

        public const ushort NC_SKILL_SOMEONEREVIVE_CMD = 31;

        public const ushort NC_SKILL_PASSIVESKILL_LEARN_CMD = 32;

        public const ushort NC_SKILL_PRODUCTFIELD_REQ = 34;

        public const ushort NC_SKILL_PRODUCTFIELD_ACK = 35;

        public const ushort NC_SKILL_UNLEARN_REQ = 36;

        public const ushort NC_SKILL_ERASE_REQ = 37;

        public const ushort NC_SKILL_ERASE_ACK = 38;

        public const ushort NC_SKILL_UNLEARN_ACK = 39;

        public const ushort NC_SKILL_WARP_CMD = 40;

        public const ushort NC_SKILL_SOMEONEREVAVALTOME_CMD = 41;

        public const ushort NC_SKILL_REPLYREVIVE_CMD = 42;

        public const ushort NC_SKILL_REPLYREVIVEFAIL_CMD = 43;

        public const ushort NC_SKILL_ITEMACTIONCOOLTIME_CMD = 44;

        public const ushort NC_SKILL_JUMP_CMD = 45;
         
    }
}