namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler22Type : HandlerType
    {
        public new const byte _Header = 22;


        //CMSG
        public const ushort CMSG_KQ_LIST_REFRESH_REQ = 27;


        //SMSG
        public const ushort SMSG_KQ_LIST_TIME_ACK = 28;

        public const ushort SMSG_KQ_LIST_ADD_ACK = 29;

        public const ushort SMSG_KQ_LIST_DELETE_ACK = 30;

        public const ushort SMSG_KQ_LIST_UPDATE_ACK = 31;

        public const ushort SMSG_KQ_JOINING_ALARM_CMD = 36;

        public const ushort SMSG_KQ_JOINING_ALARM_END_CMD = 37;

        public const ushort SMSG_KQ_JOINING_ALARM_LIST = 38;

        public const ushort SMSG_KQ_VOTE_START_ACK = 40;


        //NC
        public const ushort NC_KQ_LIST_REQ = 1;

        public const ushort NC_KQ_LIST_ACK = 2;

        public const ushort NC_KQ_STATUS_REQ = 3;

        public const ushort NC_KQ_STATUS_ACK = 4;

        public const ushort NC_KQ_JOIN_REQ = 5;

        public const ushort NC_KQ_JOIN_ACK = 6;

        public const ushort NC_KQ_JOIN_CANCEL_REQ = 7;

        public const ushort NC_KQ_JOIN_CANCEL_ACK = 8;

        public const ushort NC_KQ_SCHEDULE_REQ = 9;

        public const ushort NC_KQ_SCHEDULE_ACK = 10;

        public const ushort NC_KQ_NOTIFY_CMD = 11;

        public const ushort NC_KQ_START_CMD = 12;

        public const ushort NC_KQ_W2Z_MAKE_REQ = 13;

        public const ushort NC_KQ_Z2W_MAKE_ACK = 14;

        public const ushort NC_KQ_W2Z_START_CMD = 15;

        public const ushort NC_KQ_Z2W_END_CMD = 16;

        public const ushort NC_KQ_W2Z_DESTROY_CMD = 17;

        public const ushort NC_KQ_COMPLETE_CMD = 18;

        public const ushort NC_KQ_FAIL_CMD = 19;

        public const ushort NC_KQ_SCORE_CMD = 20;

        public const ushort NC_KQ_REWARD_REQ = 21;

        public const ushort NC_KQ_REWARDSUC_ACK = 22;

        public const ushort NC_KQ_REWARDFAIL_ACK = 23;

        public const ushort NC_KQ_RESTDEADNUM_CMD = 24;

        public const ushort NC_KQ_ENTRYRESPONCE_REQ = 25;

        public const ushort NC_KQ_ENTRYRESPONCE_ACK = 26;

        public const ushort NC_KQ_SCORE_SIMPLE_CMD = 33;

        public const ushort NC_KQ_MOBKILLNUMBER_CMD = 34;

        public const ushort NC_KQ_NOREWARD_CMD = 35;

        public const ushort NC_KQ_VOTE_START_REQ = 39;

        public const ushort NC_KQ_VOTE_VOTING_CMD = 41;

        public const ushort NC_KQ_VOTE_VOTING_REQ = 42;

        public const ushort NC_KQ_VOTE_VOTING_ACK = 43;

        public const ushort NC_KQ_VOTE_RESULT_SUC_CMD = 44;

        public const ushort NC_KQ_VOTE_RESULT_FAIL_CMD = 45;

        public const ushort NC_KQ_VOTE_CANCEL_CMD = 46;

        public const ushort NC_KQ_VOTE_BAN_MSG_CMD = 47;

        public const ushort NC_KQ_VOTE_BAN_MSG_LOGOFF_CMD = 48;

        public const ushort NC_KQ_JOIN_LIST_REQ = 49;

        public const ushort NC_KQ_JOIN_LIST_ACK = 50;

        public const ushort NC_KQ_LINK_TO_FORCE_BY_BAN_CMD = 51;

        public const ushort NC_KQ_VOTE_START_CHECK_REQ = 52;

        public const ushort NC_KQ_VOTE_START_CHECK_ACK = 53;

        public const ushort NC_KQ_SCORE_INFO_CMD = 54;

        public const ushort NC_KQ_TEAM_SELECT_REQ = 55;

        public const ushort NC_KQ_TEAM_SELECT_ACK = 56;

        public const ushort NC_KQ_TEAM_SELECT_CMD = 57;

        public const ushort NC_KQ_TEAM_TYPE_CMD = 58;

        public const ushort NC_KQ_PLAYER_DISJOIN_CMD = 59;

        public const ushort NC_KQ_SCORE_BOARD_INFO_CMD = 60;

        public const ushort NC_KQ_WINTER_EVENT_2014_SCORE_CMD = 61;
        
    }
}