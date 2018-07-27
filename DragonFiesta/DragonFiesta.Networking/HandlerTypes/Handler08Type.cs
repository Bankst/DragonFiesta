namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler08Type : HandlerType
    {
        public new const byte _Header = 8;


        //CMSG
        public const ushort CMSG_ACT_CHAT_REQ = 1;

        public const ushort CMSG_ACT_NPCCLICK_CMD = 10;

        public const ushort CMSG_ACT_ENDOFTRADE_CMD = 11;

        public const ushort CMSG_ACT_WHISPER_REQ = 12;

        public const ushort CMSG_ACT_STOP_REQ = 18;

        public const ushort CMSG_ACT_PARTYCHAT_REQ = 20;

        public const ushort CMSG_ACT_MOVEWALK_CMD = 23;

        public const ushort CMSG_ACT_MOVERUN_CMD = 25;

        public const ushort CMSG_ACT_NPCMENUOPEN_ACK = 29;

        public const ushort CMSG_ACT_SHOUT_CMD = 30;

        public const ushort CMSG_ACT_JUMP_CMD = 36;

        public const ushort CMSG_ACT_PITCHTENT_REQ = 39;

        public const ushort CMSG_ACT_FOLDTENT_REQ = 42;



        //SMSG
        public const ushort SMSG_ACT_SOMEONECHAT_CMD = 2;

        public const ushort SMSG_ACT_SOMEONECHANGEMODE_CMD = 9;

        public const ushort SMSG_ACT_SOMEONEWHISPER_CMD = 13;

        public const ushort SMSG_ACT_WHISPERFAIL_ACK = 14;

        public const ushort SMSG_ACT_WHISPERSUCCESS_ACK = 15;

        public const ushort SMSG_ACT_NOTICE_CMD = 17;

        public const ushort SMSG_ACT_SOMEONESTOP_CMD = 19;

        public const ushort SMSG_ACT_PARTYCHAT_CMD = 21;

        public const ushort SMSG_ACT_SOMEONEMOVEWALK_CMD = 24;

        public const ushort SMSG_ACT_SOMEONEMOVERUN_CMD = 26;

        public const ushort SMSG_ACT_MOVEFAIL_CMD = 27;

        public const ushort SMSG_ACT_NPCMENUOPEN_REQ = 28;

        public const ushort SMSG_ACT_SOMEONESHOUT_CMD = 31;

        public const ushort SMSG_ACT_SOMEONEJUMP_CMD = 37;

        public const ushort SMSG_ACT_PITCHTENT_ACK = 40;

        public const ushort SMSG_ACT_SOMEONEPITCHTENT_CMD = 41;

        public const ushort SMSG_ACT_FOLDTENT_ACK = 43;

        public const ushort SMSG_ACT_SOMEONEFOLDTENT_CMD = 44;

        public const ushort SMSG_ACT_SOMEONEPRODUCE_CAST_CMD = 56;

        public const ushort SMSG_ACT_SOMEONEPRODUCE_CASTCUT_CMD = 58;

        public const ushort SMSG_ACT_SOMEONEPRODUCE_MAKE_CMD = 60;

        public const ushort SMSG_ACT_RIDE_ON_CMD = 63;

        public const ushort SMSG_ACT_SOMEONERIDE_ON_CMD = 64;

        public const ushort SMSG_ACT_RIDE_OFF_CMD = 66;

        public const ushort SMSG_ACT_SOMEONERIDE_OFF_CMD = 67;

        public const ushort SMSG_ACT_RIDE_HUNGRY_CMD = 70;

        public const ushort SMSG_ACT_CREATECASTBAR = 71;

        public const ushort SMSG_ACT_REINFORCE_STOP_CMD = 74;


        //NC 
        public const ushort NC_ACT_WALK_REQ = 3;

        public const ushort NC_ACT_SOMEONEWALK_CMD = 4;

        public const ushort NC_ACT_RUN_REQ = 5;

        public const ushort NC_ACT_SOMEONERUN_CMD = 6;

        public const ushort NC_ACT_MOVEFAIL_ACK = 7;

        public const ushort NC_ACT_CHANGEMODE_REQ = 8;

        public const ushort NC_ACT_NOTICE_REQ = 16;

        public const ushort NC_ACT_PARTYCHAT_ACK = 22;

        public const ushort NC_ACT_EMOTICON_CMD = 32;

        public const ushort NC_ACT_SOMEONEEMOTICON_CMD = 33;

        public const ushort NC_ACT_EMOTICONSTOP_CMD = 34;

        public const ushort NC_ACT_SOMEONEEMOTICONSTOP_CMD = 35;

        public const ushort NC_ACT_SOMEONESPEEDCHANGE_CMD = 38;

        public const ushort NC_ACT_GATHERSTART_REQ = 45;

        public const ushort NC_ACT_GATHERSTART_ACK = 46;

        public const ushort NC_ACT_SOMEONEGATHERSTART_CMD = 47;

        public const ushort NC_ACT_GATHERCANCEL_CMD = 48;

        public const ushort NC_ACT_SOMEONEGATHERCANCEL_CMD = 49;

        public const ushort NC_ACT_GATHERCOMPLETE_REQ = 50;

        public const ushort NC_ACT_GATHERCOMPLETE_ACK = 51;

        public const ushort NC_ACT_SOMEONEGATHERCOMPLETE_CMD = 52;

        public const ushort NC_ACT_PRODUCE_CAST_REQ = 53;

        public const ushort NC_ACT_PRODUCE_CAST_FAIL_ACK = 54;

        public const ushort NC_ACT_PRODUCE_CAST_SUC_ACK = 55;

        public const ushort NC_ACT_PRODUCE_CASTABORT_CMD = 57;

        public const ushort NC_ACT_PRODUCE_MAKE_CMD = 59;

        public const ushort NC_ACT_REINFORCE_FOLDTENT_CMD = 61;

        public const ushort NC_ACT_MOVESPEED_CMD = 62;

        public const ushort NC_ACT_RIDE_FAIL_CMD = 65;

        public const ushort NC_ACT_RIDE_FEEDING_REQ = 68;

        public const ushort NC_ACT_RIDE_FEEDING_ACK = 69;

        public const ushort NC_ACT_CANCELCASTBAR = 72;

        public const ushort NC_ACT_REINFORCE_RUN_CMD = 73;

        public const ushort NC_ACT_ROAR_REQ = 75;

        public const ushort NC_ACT_ROAR_ACK = 76;

        public const ushort NC_ACT_REINFORCE_WALK_CMD = 77;

        public const ushort NC_ACT_WEDDING_PROPOSE_WORD_REQ = 78;

        public const ushort NC_ACT_WEDDING_PROPOSEREQ_REQ = 79;

        public const ushort NC_ACT_WEDDING_PROPOSEACK_REQ = 80;

        public const ushort NC_ACT_WEDDING_PROPOSEACK_ACK = 81;

        public const ushort NC_ACT_WEDDING_PROPOSEREQ_ACK = 82;

        public const ushort NC_ACT_WEDDING_HALL_RESERV_REQ = 83;

        public const ushort NC_ACT_WEDDING_HALL_RESERV_ACK = 84;

        public const ushort NC_ACT_WEDDING_COUPLE_ENTRANCE_RNG = 85;

        public const ushort NC_ACT_WEDDING_HALL_GUEST_ENTER_READY_REQ = 86;

        public const ushort NC_ACT_WEDDING_HALL_GUEST_ENTER_READY_ACK = 87;

        public const ushort NC_ACT_WEDDING_HALL_GUEST_ENTER_REQ = 88;

        public const ushort NC_ACT_WEDDING_HALL_GUEST_ENTER_ACK = 89;

        public const ushort NC_ACT_WEDDING_SOMEONE = 90;

        public const ushort NC_ACT_WEDDING_AGREEMENT_DIVORCE_REQ = 91;

        public const ushort NC_ACT_WEDDING_COMPULSORY_DIVORCE_REQ = 92;

        public const ushort NC_ACT_WEDDING_DIVORCE_REQ_ACK = 93;

        public const ushort NC_ACT_WEDDING_DIVORCE_REFUSE_CMD = 94;

        public const ushort NC_ACT_ACTIONBYITEM_REQ = 103;

        public const ushort NC_ACT_ACTIONBYITEM_ACK = 104;

        public const ushort NC_ACT_REINFORCERELOC_CMD = 105;

        public const ushort NC_ACT_REINFORCEMOVEBYPATH_CMD = 106;

        public const ushort NC_ACT_SETITEMHEALEFFECT = 111;

        public const ushort NC_ACT_AUTO_WAY_FINDING_USE_GATE_REQ = 112;

        public const ushort NC_ACT_AUTO_WAY_FINDING_USE_GATE_ACK = 113;

        public const ushort NC_ACT_NPC_ACTION_CMD = 114;

        public const ushort NC_ACT_NPC_MENU_CMD = 115;

        public const ushort NC_ACT_ANIMATION_START_CMD = 116;

        public const ushort NC_ACT_ANIMATION_STOP_CMD = 117;

        public const ushort NC_ACT_ANIMATION_LEVEL_CHANGE_CMD = 118;

        public const ushort NC_ACT_EFFECT_MESSAGE_CMD = 119;

        public const ushort NC_ACT_PLAY_SOUND_CMD = 120;

        public const ushort NC_ACT_SCRIPT_MSG_CMD = 121;

        public const ushort NC_ACT_OBJECT_SOUND_CMD = 122;

        public const ushort NC_ACT_OBJECT_EFFECT_CMD = 123;

        public const ushort NC_ACT_EVENT_CODE_ACTION_CMD = 124;

        public const ushort NC_ACT_SCRIPT_MSG_WORLD_CMD = 125;

        public const ushort NC_ACT_SHOW_CINEMATIC = 130;

        public const ushort NC_ACT_END_CINEMATIC = 131;

    }
}