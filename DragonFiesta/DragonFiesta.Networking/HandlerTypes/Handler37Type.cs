namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler37Type : HandlerType
    {
        public new const byte _Header = 37;


        //CMSG
        public const ushort CMSG_HOLY_PROMISE_SET_UP_CONFIRM_ACK = 5;

        public const ushort CMSG_HOLY_PROMISE_DEL_UP_REQ = 6;

        public const ushort CMSG_HOLY_PROMISE_DEL_DOWN_REQ = 10;

        public const ushort CMSG_HOLY_PROMISE_GET_CEN_REWARD_REQ = 60;

        public const ushort CMSG_HOLY_PROMISE_WITHDRAW_CEN_REWARD_REQ = 64;


        //SMSG
        public const ushort SMSG_HOLY_PROMISE_SET_UP_ACK = 2;

        public const ushort SMSG_HOLY_PROMISE_SET_UP_CONFIRM_ING = 3;

        public const ushort SMSG_HOLY_PROMISE_SET_UP_CONFIRM_REQ = 4;

        public const ushort SMSG_HOLY_PROMISE_DEL_DOWN_ACK = 11;

        public const ushort SMSG_HOLY_PROMISE_LIST_CMD = 20;

        public const ushort SMSG_HOLY_PROMISE_ADD_CMD = 21;

        public const ushort SMSG_HOLY_PROMISE_LOGIN_CMD = 22;

        public const ushort SMSG_HOLY_PROMISE_LOGOUT_CMD = 23;

        public const ushort SMSG_HOLY_PROMISE_DEL_CMD = 24;

        public const ushort SMSG_HOLY_PROMISE_LEVEL_CMD = 25;

        public const ushort SMSG_HOLY_PROMISE_REWARD_ITEM_CMD = 26;

        public const ushort SMSG_HOLY_PROMISE_GET_CEN_REWARD_ACK = 61;

        public const ushort SMSG_HOLY_PROMISE_WITHDRAW_CEN_REWARD_ACK = 65;

        public const ushort SMSG_HOLY_PROMISE_CLIENT_GET_REMAIN_MONEY_CMD = 69;


        //NC
        public const ushort NC_HOLY_PROMISE_SET_UP_REQ = 1;

        public const ushort NC_HOLY_PROMISE_DEL_UP_ACK = 7;

        public const ushort NC_HOLY_PROMISE_REWARD_MONEY_CMD = 27;

        public const ushort NC_HOLY_PROMISE_DB_SET_UP_REQ = 30;

        public const ushort NC_HOLY_PROMISE_DB_SET_UP_ACK = 31;

        public const ushort NC_HOLY_PROMISE_DB_DEL_UP_REQ = 32;

        public const ushort NC_HOLY_PROMISE_DB_DEL_UP_ACK = 33;

        public const ushort NC_HOLY_PROMISE_DB_DEL_DOWN_REQ = 34;

        public const ushort NC_HOLY_PROMISE_DB_DEL_DOWN_ACK = 35;

        public const ushort NC_HOLY_PROMISE_DB_DEL_CHAR_REQ = 36;

        public const ushort NC_HOLY_PROMISE_DB_DEL_CHAR_ACK = 37;

        public const ushort NC_HOLY_PROMISE_DB_GET_UP_REQ = 38;

        public const ushort NC_HOLY_PROMISE_DB_GET_UP_ACK = 39;

        public const ushort NC_HOLY_PROMISE_DB_GET_MEMBER_REQ = 40;

        public const ushort NC_HOLY_PROMISE_DB_GET_MEMBER_ACK = 41;

        public const ushort NC_HOLY_PROMISE_DB_REWARD_REQ = 42;

        public const ushort NC_HOLY_PROMISE_DB_REWARD_ACK = 43;

        public const ushort NC_HOLY_PROMISE_DB_SET_DATE_REQ = 44;

        public const ushort NC_HOLY_PROMISE_DB_SET_DATE_ACK = 45;

        public const ushort NC_HOLY_PROMISE_MY_UP_ZONE = 50;

        public const ushort NC_HOLY_PROMISE_USE_MONEY_ZONE = 51;

        public const ushort NC_HOLY_PROMISE_MYUPPER_REQ = 52;

        public const ushort NC_HOLY_PROMISE_MYUPPER_ACK = 53;

        public const ushort NC_HOLY_PROMISE_CENTRANSFER_RNG = 54;

        public const ushort NC_HOLY_PROMISE_DB_GET_CEN_REWARD_REQ = 62;

        public const ushort NC_HOLY_PROMISE_DB_GET_CEN_REWARD_ACK = 63;

        public const ushort NC_HOLY_PROMISE_DB_WITHDRAW_CEN_REWARD_REQ = 66;

        public const ushort NC_HOLY_PROMISE_DB_WITHDRAW_CEN_REWARD_ACK = 67;

        public const ushort NC_HOLY_PROMISE_DB_GET_REMAIN_MONEY_CMD = 68;

    }
}