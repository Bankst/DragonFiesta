namespace DragonFiesta.Networking.HandlerTypes
{   
    public sealed class Handler17Type : HandlerType
    {
        public new const byte _Header = 17;

        //CMSG
        public const ushort CMSG_QUEST_SCRIPT_CMD_ACK = 2;

        //SMSG
        public const ushort SMSG_QUEST_SCRIPT_CMD_REQ = 1;

        public const ushort SMSG_QUEST_RESET_TIME_CLIENT_CMD = 30;



        //NC
        public const ushort NC_QUEST_DB_SET_INFO_REQ = 3;

        public const ushort NC_QUEST_DB_SET_INFO_ACK = 4;

        public const ushort NC_QUEST_DB_CLEAR_REQ = 5;

        public const ushort NC_QUEST_DB_CLEAR_ACK = 6;

        public const ushort NC_QUEST_GIVE_UP_REQ = 7;

        public const ushort NC_QUEST_GIVE_UP_ACK = 8;

        public const ushort NC_QUEST_DB_GIVE_UP_REQ = 9;

        public const ushort NC_QUEST_DB_GIVE_UP_ACK = 10;

        public const ushort NC_QUEST_CLIENT_SCENARIO_DONE_REQ = 11;

        public const ushort NC_QUEST_CLIENT_SCENARIO_DONE_ACK = 12;

        public const ushort NC_QUEST_NOTIFY_MOB_KILL_CMD = 13;

        public const ushort NC_QUEST_SCENARIO_RUN_CMD = 14;

        public const ushort NC_QUEST_SELECT_START_REQ = 15;

        public const ushort NC_QUEST_SELECT_START_ACK = 16;

        public const ushort NC_QUEST_REWARD_SELECT_ITEM_INDEX_CMD = 17;

        public const ushort NC_QUEST_REWARD_NEED_SELECT_ITEM_CMD = 18;

        public const ushort NC_QUEST_ERR = 19;

        public const ushort NC_QUEST_START_REQ = 20;

        public const ushort NC_QUEST_START_ACK = 21;

        public const ushort NC_QUEST_READ_REQ = 22;

        public const ushort NC_QUEST_READ_ACK = 23;

        public const ushort NC_QUEST_DB_READ_REQ = 24;

        public const ushort NC_QUEST_DB_READ_ACK = 25;

        public const ushort NC_QUEST_DB_DONE_REQ = 26;

        public const ushort NC_QUEST_DB_DONE_ACK = 27;

        public const ushort NC_QUEST_RESET_TIME_CMD = 28;

        public const ushort NC_QUEST_RESET_TIME_ZONE_CMD = 29;

        public const ushort NC_QUEST_JOBDUNGEON_FIND_RNG = 31;

        public const ushort NC_QUEST_JOBDUNGEON_LINK_FAIL_CMD = 32;         
        
    }
}