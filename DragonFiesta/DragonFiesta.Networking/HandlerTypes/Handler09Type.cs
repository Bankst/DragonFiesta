namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler09Type : HandlerType
    {
        public new const byte _Header = 9;


        //CMSG
        public const ushort CMSG_BAT_TARGETTING_REQ = 1;

        public const ushort CMSG_BAT_UNTARGET_REQ = 8;

        public const ushort CMSG_BAT_BASHSTART_CMD = 43;

        public const ushort CMSG_BAT_BASHSTOP_CMD = 50;

        public const ushort CMSG_BAT_SKILLBASH_OBJ_CAST_REQ = 64;

        public const ushort CMSG_BAT_SKILLBASH_FLD_CAST_REQ = 65;



        //SMSG
        public const ushort SMSG_BAT_TARGETINFO_CMD = 2;

        public const ushort SMSG_BAT_EXPGAIN_CMD = 11;

        public const ushort SMSG_BAT_LEVELUP_CMD = 12;

        public const ushort SMSG_BAT_SOMEONELEVELUP_CMD = 13;

        public const ushort SMSG_BAT_HPCHANGE_CMD = 14;

        public const ushort SMSG_BAT_SPCHANGE_CMD = 15;

        public const ushort SMSG_BAT_ABSTATERESET_CMD = 40;

        public const ushort SMSG_BAT_ABSTATEINFORM_CMD = 41;

        public const ushort SMSG_BAT_ABSTATEINFORM_NOEFFECT_CMD = 42;

        public const ushort SMSG_BAT_SKILLBASH_CAST_SUC_ACK = 53;

        public const ushort SMSG_BAT_SWING_START_CMD = 71;

        public const ushort SMSG_BAT_SWING_DAMAGE_CMD = 72;
        
        public const ushort SMSG_BAT_PKINPKFIELD_WMS_CMD = 74;

        //public const ushort SMSG_BAT_REALLYKILL_CMD = 74;        
        
        public const ushort SMSG_BAT_SKILLBASH_HIT_OBJ_START_CMD = 78;

        public const ushort SMSG_BAT_SOMEONESKILLBASH_HIT_OBJ_START_CMD = 79;        
        
        public const ushort SMSG_BAT_SOMEONESKILLBASH_HIT_FLD_START_CMD = 81;

        public const ushort SMSG_BAT_SKILLBASH_HIT_DAMAGE_CMD = 82;        
        
        public const ushort SMSG_BAT_SKILLBASH_HIT_BLAST_CMD = 87;
        
        public const ushort SMSG_BAT_LPCHANGE_CMD = 95;

        
        //NC
        public const ushort NC_BAT_HIT_REQ = 3;

        public const ushort NC_BAT_SOMEONEDAMAGED_SMALL_CMD = 4;

        public const ushort NC_BAT_SOMEONEDAMAGED_LARGE_CMD = 5;

        public const ushort NC_BAT_SOMEONEDEAD_SMALL_CMD = 6;

        public const ushort NC_BAT_SOMEONEDEAD_LARGE_CMD = 7;

        public const ushort NC_BAT_SKILLENCHANT_REQ = 9;

        public const ushort NC_BAT_SOMEONESKILLENCHANT_REQ = 10;

        public const ushort NC_BAT_APCHANGE_CMD = 16;

        public const ushort NC_BAT_EXPLOST_CMD = 17;

        public const ushort NC_BAT_SMASH_REQ = 18;

        public const ushort NC_BAT_SMASH_HIT_ACK = 19;

        public const ushort NC_BAT_SMASH_HITTED_ACK = 20;

        public const ushort NC_BAT_SMASH_MISS_ACK = 21;

        public const ushort NC_BAT_SOMEONESMASH_DAMAGED_CMD = 22;

        public const ushort NC_BAT_SOMEONESMASH_DEAD_CMD = 23;

        public const ushort NC_BAT_SKILLCAST_REQ = 24;

        public const ushort NC_BAT_SKILLCAST_FAIL_ACK = 25;

        public const ushort NC_BAT_SKILLCAST_SUC_ACK = 26;

        public const ushort NC_BAT_SOMEONESKILLCAST_CMD = 27;

        public const ushort NC_BAT_SKILLCASTABORT_CMD = 28;

        public const ushort NC_BAT_SOMEONESKILLCASTCUT_CMD = 29;

        public const ushort NC_BAT_SKILLCASTCUT_CMD = 30;

        public const ushort NC_BAT_SKILLSMASH_CMD = 31;

        public const ushort NC_BAT_SKILLSMASH_HIT_CMD = 32;

        public const ushort NC_BAT_SKILLSMASH_HITTED_CMD = 33;

        public const ushort NC_BAT_SKILLSMASH_MISS_CMD = 34;

        public const ushort NC_BAT_SKILLSMASH_ENCHANT_CMD = 35;

        public const ushort NC_BAT_SOMEONESKILLSMASH_DAMAGED_CMD = 36;

        public const ushort NC_BAT_SOMEONESKILLSMASH_DEAD_CMD = 37;

        public const ushort NC_BAT_SOMEONESKILLSMASH_ENCHANT_CMD = 38;

        public const ushort NC_BAT_ABSTATESET_CMD = 39;

        public const ushort NC_BAT_BASH_HIT_CMD = 44;

        public const ushort NC_BAT_BASH_HITTED_CMD = 45;

        public const ushort NC_BAT_SOMEONEBASH_HIT_CMD = 46;

        public const ushort NC_BAT_BASH_MISS_CMD = 47;

        public const ushort NC_BAT_BASH_MISSED_CMD = 48;

        public const ushort NC_BAT_SOMEONEBASH_MISS_CMD = 49;

        public const ushort NC_BAT_SKILLBASH_CAST_REQ = 51;

        public const ushort NC_BAT_SKILLBASH_CAST_FAIL_ACK = 52;

        public const ushort NC_BAT_SOMEONESKILLBASH_CAST_CMD = 54;

        public const ushort NC_BAT_SKILLBASH_CASTABORT_CMD = 55;

        public const ushort NC_BAT_SOMEONESKILLBASH_CASTCUT_CMD = 56;

        public const ushort NC_BAT_SKILLBASH_HIT_CMD = 57;

        public const ushort NC_BAT_SKILLBASH_HITTED_CMD = 58;

        public const ushort NC_BAT_SOMEONESKILLBASH_HIT_CMD = 59;

        public const ushort NC_BAT_DOTDAMAGE_CMD = 60;

        public const ushort NC_BAT_CEASE_FIRE_CMD = 61;

        public const ushort NC_BAT_ASSIST_REQ = 62;

        public const ushort NC_BAT_ASSIST_ACK = 63;

        public const ushort NC_BAT_SOMEONESKILLBASH_OBJ_CAST_CMD = 66;

        public const ushort NC_BAT_SOMEONESKILLBASH_FLD_CAST_CMD = 67;

        public const ushort NC_BAT_SKILLBASH_CASTABORT_REQ = 68;

        public const ushort NC_BAT_SKILLBASH_CASTABORT_ACK = 69;

        public const ushort NC_BAT_FAMEGAIN_CMD = 70;

        public const ushort NC_BAT_SOMEONESWING_DAMAGE_CMD = 73;

        public const ushort NC_BAT_PKINPKFIELD_CLIENT_CMD = 75;

        public const ushort NC_BAT_MOBSLAYER_CMD = 76;

        public const ushort NC_BAT_TARGETCHANGE_CMD = 77;

        public const ushort NC_BAT_SKILLBASH_HIT_FLD_START_CMD = 80;

        public const ushort NC_BAT_SOULCOLLECT_CMD = 83;

        public const ushort NC_BAT_ABSTATE_ERASE_REQ = 84;

        public const ushort NC_BAT_ABSTATE_ERASE_ACK = 85;

        public const ushort NC_BAT_SUMEONESKILLCUT_CMD = 86;

        public const ushort NC_BAT_WORLD_MOB_KILL_ANNOUNCE_CMD = 88;

        public const ushort NC_BAT_CLIENT_MOB_KILL_ANNOUNCE_CMD = 89;

        public const ushort NC_BAT_AREADOTDAMAGE_CMD = 90;

        public const ushort NC_BAT_REFLECTIONDAMAGE_CMD = 91;

        public const ushort NC_BAT_TOGGLESKILL_ON_CMD = 92;

        public const ushort NC_BAT_TOGGLESKILL_OFF_CMD = 93;

        public const ushort NC_BAT_SKILLBLAST_LIGHTNINGWAVE_CMD = 94;
        
    }
}