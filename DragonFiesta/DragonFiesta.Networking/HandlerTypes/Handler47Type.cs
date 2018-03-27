namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler47Type : HandlerType
    {
        public new const byte _Header = 47;

        //SMSG
        public const ushort SMSG_GAMBLE_EXCHANGECOIN_CHANGE_CMD = 5;


        //NC
        public const ushort NC_GAMBLE_ZONE_PREVMAPNAME_CMD = 1;

        public const ushort NC_GAMBLE_GAMBLEHOUSE_EXIT_REQ = 2;

        public const ushort NC_GAMBLE_GAMBLEHOUSE_EXIT_ACK = 3;

        public const ushort NC_GAMBLE_COIN_CHANGE_CMD = 4;

        public const ushort NC_GAMBLE_COIN_DB_ADD_REQ = 6;

        public const ushort NC_GAMBLE_COIN_DB_ADD_ACK = 7;

        public const ushort NC_GAMBLE_EXCHANGEDCOIN_INIT_CMD = 9;

        public const ushort NC_GAMBLE_EXCHANGEDCOIN_DB_INFO_REQ = 10;

        public const ushort NC_GAMBLE_EXCHANGEDCOIN_DB_INFO_ACK = 11;

        public const ushort NC_GAMBLE_COIN_EXCHANGEMACHINE_UI_OPEN_CMD = 12;

        public const ushort NC_GAMBLE_COIN_VIPCARD_UI_OPEN_REQ = 13;

        public const ushort NC_GAMBLE_COIN_VIPCARD_UI_OPEN_ACK = 14;

        public const ushort NC_GAMBLE_COIN_BUY_REQ = 15;

        public const ushort NC_GAMBLE_COIN_BUY_ACK = 16;

        public const ushort NC_GAMBLE_COIN_DB_BUY_REQ = 17;

        public const ushort NC_GAMBLE_COIN_DB_BUY_ACK = 18;

        public const ushort NC_GAMBLE_COIN_SELL_REQ = 19;

        public const ushort NC_GAMBLE_COIN_SELL_ACK = 20;

        public const ushort NC_GAMBLE_COIN_DB_SELL_REQ = 21;

        public const ushort NC_GAMBLE_COIN_DB_SELL_ACK = 22;

        public const ushort NC_GAMBLE_TYPE_AND_WHERE_STAND_REQ = 23;

        public const ushort NC_GAMBLE_TYPE_AND_WHERE_STAND_ACK = 24;

        public const ushort NC_GAMBLE_COIN_DB_USE_COINITEM_REQ = 25;

        public const ushort NC_GAMBLE_COIN_DB_USE_COINITEM_ACK = 26;

        public const ushort NC_GAMBLE_COIN_USE_COINITEM_MESSAGE_CMD = 27;

        public const ushort NC_GAMBLE_ENTER_PLAYER_DIRECT_CMD = 28;

        public const ushort NC_GAMBLE_WORLD_PREVMAPNAME_CMD = 29;

        public const ushort NC_GAMBLE_PLAYERACT_CMD = 30;

        public const ushort NC_GAMBLE_DICE_TAISAI_GAME_JOIN_REQ = 100;

        public const ushort NC_GAMBLE_DICE_TAISAI_GAME_JOIN_ACK = 101;

        public const ushort NC_GAMBLE_DICE_TAISAI_GAME_JOIN_CMD = 102;

        public const ushort NC_GAMBLE_DICE_TAISAI_GAME_BETTING_INFO_CMD = 103;

        public const ushort NC_GAMBLE_DICE_TAISAI_GAME_LEAVE_REQ = 104;

        public const ushort NC_GAMBLE_DICE_TAISAI_GAME_LEAVE_ACK = 105;

        public const ushort NC_GAMBLE_DICE_TAISAI_GAME_LEAVE_CMD = 106;

        public const ushort NC_GAMBLE_DICE_TAISAI_ALL_RANK_REQ = 107;

        public const ushort NC_GAMBLE_DICE_TAISAI_DB_ALL_RANK_REQ = 108;

        public const ushort NC_GAMBLE_DICE_TAISAI_DB_ALL_RANK_ACK = 109;

        public const ushort NC_GAMBLE_DICE_TAISAI_ALL_RANK_ACK = 110;

        public const ushort NC_GAMBLE_DICE_TAISAI_CURR_RANK_REQ = 111;

        public const ushort NC_GAMBLE_DICE_TAISAI_CURR_RANK_ACK = 112;

        public const ushort NC_GAMBLE_DICE_TAISAI_BETTING_REQ = 113;

        public const ushort NC_GAMBLE_DICE_TAISAI_BETTING_ACK = 114;

        public const ushort NC_GAMBLE_DICE_TAISAI_BETTING_CMD = 115;

        public const ushort NC_GAMBLE_DICE_TAISAI_BETTING_CANCEL_REQ = 116;

        public const ushort NC_GAMBLE_DICE_TAISAI_BETTING_CANCEL_ACK = 117;

        public const ushort NC_GAMBLE_DICE_TAISAI_BETTING_CANCEL_CMD = 118;

        public const ushort NC_GAMBLE_DICE_TAISAI_DICE_ROLL_CMD = 119;

        public const ushort NC_GAMBLE_DICE_TAISAI_DB_DICE_ROLL_RESULT_REQ = 120;

        public const ushort NC_GAMBLE_DICE_TAISAI_DB_DICE_ROLL_RESULT_ACK = 121;

        public const ushort NC_GAMBLE_DICE_TAISAI_DB_DICE_ROLL_RESULT_LOG_CMD = 122;

        public const ushort NC_GAMBLE_DICE_TAISAI_DICE_ROLL_RESULT_CMD = 123;

        public const ushort NC_GAMBLE_DICE_TAISAI_DICE_ROLL_RESULT_EMOTION_CMD = 124;

        public const ushort NC_GAMBLE_DICE_TAISAI_LARGE_AMOUNT_REQ = 125;

        public const ushort NC_GAMBLE_DICE_TAISAI_LARGE_AMOUNT_ACK = 126;

        public const ushort NC_GAMBLE_DICE_TAISAI_LARGE_AMOUNT_CMD = 127;

        public const ushort NC_GAMBLE_DICE_TAISAI_BET_START_CMD = 128;

        public const ushort NC_GAMBLE_DICE_TAISAI_TIMER_CMD = 129;

        public const ushort NC_GAMBLE_SLOTMACHINE_GAME_JOIN_REQ = 200;

        public const ushort NC_GAMBLE_SLOTMACHINE_GAME_JOIN_ACK = 201;

        public const ushort NC_GAMBLE_SLOTMACHINE_GAME_LEAVE_REQ = 202;

        public const ushort NC_GAMBLE_SLOTMACHINE_GAME_LEAVE_ACK = 203;

        public const ushort NC_GAMBLE_SLOTMACHINE_START_REQ = 204;

        public const ushort NC_GAMBLE_SLOTMACHINE_START_ACK = 205;

        public const ushort NC_GAMBLE_SLOTMACHINE_STOPBUTTON_REQ = 206;

        public const ushort NC_GAMBLE_SLOTMACHINE_STOPBUTTON_ACK = 207;

        public const ushort NC_GAMBLE_SLOTMACHINE_WHEELSTOP_REQ = 208;

        public const ushort NC_GAMBLE_SLOTMACHINE_WHEELSTOP_ACK = 209;

        public const ushort NC_GAMBLE_SLOTMACHINE_STAND_UP_CMD = 210;

        public const ushort NC_GAMBLE_SLOTMACHINE_DB_RESULT_REQ = 211;

        public const ushort NC_GAMBLE_SLOTMACHINE_DB_RESULT_ACK = 212;

        public const ushort NC_GAMBLE_SLOTMACHINE_DB_GAMEINFO_REQ = 213;

        public const ushort NC_GAMBLE_SLOTMACHINE_DB_GAMEINFO_ACK = 214;

        public const ushort NC_GAMBLE_SLOTMACHINE_JACKPOTINFO_CMD = 216;

        public const ushort NC_GAMBLE_SLOTMACHINE_JACKPOTRANKING_REQ = 217;

        public const ushort NC_GAMBLE_SLOTMACHINE_JACKPOTRANKING_ACK = 218;

        public const ushort NC_GAMBLE_SLOTMACHINE_WINRANKING_REQ = 219;

        public const ushort NC_GAMBLE_SLOTMACHINE_WINRANKING_ACK = 220;

        public const ushort NC_GAMBLE_SLOTMACHINE_SOMEONE_GET_JACKPOT_CMD = 221;
    }
}
