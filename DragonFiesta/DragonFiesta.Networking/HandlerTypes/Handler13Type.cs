namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler13Type : HandlerType
    {
        public new const byte _Header = 13;


        //NC
        public const ushort NC_ITEMDB_ADMINCREATE_REQ = 1;

        public const ushort NC_ITEMDB_ADMINCREATESUC_ACK = 2;

        public const ushort NC_ITEMDB_ADMINCREATEFAIL_ACK = 3;

        public const ushort NC_ITEMDB_QUESTALL_REQ = 4;

        public const ushort NC_ITEMDB_QUESTALLSUC_ACK = 5;

        public const ushort NC_ITEMDB_QUESTALLFAIL_ACK = 6;

        public const ushort NC_ITEMDB_QUESTLOT_REQ = 7;

        public const ushort NC_ITEMDB_QUESTLOTSUC_ACK = 8;

        public const ushort NC_ITEMDB_QUESTLOTFAIL_ACK = 9;

        public const ushort NC_ITEMDB_BUYALL_REQ = 10;

        public const ushort NC_ITEMDB_BUYALLSUC_ACK = 11;

        public const ushort NC_ITEMDB_BUYALLFAIL_ACK = 12;

        public const ushort NC_ITEMDB_BUYLOT_REQ = 13;

        public const ushort NC_ITEMDB_BUYLOTSUC_ACK = 14;

        public const ushort NC_ITEMDB_BUYLOTFAIL_ACK = 15;

        public const ushort NC_ITEMDB_EQUIP_REQ = 16;

        public const ushort NC_ITEMDB_EQUIPSUC_ACK = 17;

        public const ushort NC_ITEMDB_EQUIPFAIL_ACK = 18;

        public const ushort NC_ITEMDB_UNEQUIP_REQ = 19;

        public const ushort NC_ITEMDB_UNEQUIPSUC_ACK = 20;

        public const ushort NC_ITEMDB_UNEQUIPFAIL_ACK = 21;

        public const ushort NC_ITEMDB_DROPALL_REQ = 22;

        public const ushort NC_ITEMDB_DROPALLSUC_ACK = 23;

        public const ushort NC_ITEMDB_DROPALLFAIL_ACK = 24;

        public const ushort NC_ITEMDB_DROPLOT_REQ = 25;

        public const ushort NC_ITEMDB_DROPLOTSUC_ACK = 26;

        public const ushort NC_ITEMDB_DROPLOTFAIL_ACK = 27;

        public const ushort NC_ITEMDB_PICKALL_REQ = 28;

        public const ushort NC_ITEMDB_PICKALLSUC_ACK = 29;

        public const ushort NC_ITEMDB_PICKALLFAIL_ACK = 30;

        public const ushort NC_ITEMDB_PICKMERGE_REQ = 31;

        public const ushort NC_ITEMDB_PICKMERGESUC_ACK = 32;

        public const ushort NC_ITEMDB_PICKMERGEFAIL_ACK = 33;

        public const ushort NC_ITEMDB_SELLALL_REQ = 34;

        public const ushort NC_ITEMDB_SELLALLSUC_ACK = 35;

        public const ushort NC_ITEMDB_SELLALLFAIL_ACK = 36;

        public const ushort NC_ITEMDB_SELLLOT_REQ = 37;

        public const ushort NC_ITEMDB_SELLLOTSUC_ACK = 38;

        public const ushort NC_ITEMDB_SELLLOTFAIL_ACK = 39;

        public const ushort NC_ITEMDB_RELOC_REQ = 40;

        public const ushort NC_ITEMDB_RELOCSUC_ACK = 41;

        public const ushort NC_ITEMDB_RELOCFAIL_ACK = 42;

        public const ushort NC_ITEMDB_EXCHANGE_REQ = 43;

        public const ushort NC_ITEMDB_EXCHANGESUC_ACK = 44;

        public const ushort NC_ITEMDB_EXCHANGEFAIL_ACK = 45;

        public const ushort NC_ITEMDB_MERGE_REQ = 46;

        public const ushort NC_ITEMDB_MERGESUC_ACK = 47;

        public const ushort NC_ITEMDB_MERGEFAIL_ACK = 48;

        public const ushort NC_ITEMDB_SPLIT_N_MERGE_REQ = 49;

        public const ushort NC_ITEMDB_SPLIT_N_MERGESUC_ACK = 50;

        public const ushort NC_ITEMDB_SPLIT_N_MERGEFAIL_ACK = 51;

        public const ushort NC_ITEMDB_SPLIT_REQ = 52;

        public const ushort NC_ITEMDB_SPLITSUC_ACK = 53;

        public const ushort NC_ITEMDB_SPLITFAIL_ACK = 54;

        public const ushort NC_ITEMDB_MOB_DROP_CMD = 55;

        public const ushort NC_ITEMDB_PICKMONEY_REQ = 56;

        public const ushort NC_ITEMDB_PICKMONEYSUC_ACK = 57;

        public const ushort NC_ITEMDB_PICKMONEYFAIL_ACK = 58;

        public const ushort NC_ITEMDB_ITEMTRADE_REQ = 59;

        public const ushort NC_ITEMDB_ITEMTRADEFAIL_ACK = 60;

        public const ushort NC_ITEMDB_ITEMTRADESUC_ACK = 61;

        public const ushort NC_ITEMDB_USELOT_REQ = 62;

        public const ushort NC_ITEMDB_USEALL_REQ = 63;

        public const ushort NC_ITEMDB_USE_ACK = 64;

        public const ushort NC_ITEMDB_SOULSTONEBUY_REQ = 65;

        public const ushort NC_ITEMDB_SOULSTONEBUYSUC_ACK = 66;

        public const ushort NC_ITEMDB_SOULSTONEBUYFAIL_ACK = 67;

        public const ushort NC_ITEMDB_OPENSTORAGE_REQ = 68;

        public const ushort NC_ITEMDB_OPENSTORAGE_FAIL_ACK = 69;

        public const ushort NC_ITEMDB_OPENSTORAGE_ACK = 70;

        public const ushort NC_ITEMDB_UPGRADE_REQ = 71;

        public const ushort NC_ITEMDB_UPGRADE_ACK = 72;

        public const ushort NC_ITEMDB_ITEMCHANGE_REQ = 73;

        public const ushort NC_ITEMDB_ITEMCHANGE_ACK = 74;

        public const ushort NC_ITEMDB_ITEMTOTALINFORM_REQ = 75;

        public const ushort NC_ITEMDB_ITEMTOTALINFORM_ACK = 76;

        public const ushort NC_ITEMDB_CREATEITEMLIST_REQ = 77;

        public const ushort NC_ITEMDB_CREATEITEMLISTSUC_ACK = 78;

        public const ushort NC_ITEMDB_CREATEITEMLISTFAIL_ACK = 79;

        public const ushort NC_ITEMDB_GETFROMCHEST_REQ = 80;

        public const ushort NC_ITEMDB_GETFROMCHESTSUC_ACK = 81;

        public const ushort NC_ITEMDB_GETFROMCHESTFAIL_ACK = 82;

        public const ushort NC_ITEMDB_BOOTHTRADE_ALL_REQ = 83;

        public const ushort NC_ITEMDB_BOOTHTRADE_MERGE_REQ = 84;

        public const ushort NC_ITEMDB_BOOTHTRADE_LOT_REQ = 85;

        public const ushort NC_ITEMDB_BOOTHTRADE_ACK = 86;

        public const ushort NC_ITEMDB_PRODUCE_REQ = 87;

        public const ushort NC_ITEMDB_PRODUCE_ACK = 88;

        public const ushort NC_ITEMDB_DESTROY_REQ = 89;

        public const ushort NC_ITEMDB_DESTROY_ACK = 90;

        public const ushort NC_ITEMDB_QUESTREWARD_REQ = 91;

        public const ushort NC_ITEMDB_QUESTREWARD_ACK = 92;

        public const ushort NC_ITEMDB_QUESTITEMGET_REQ = 93;

        public const ushort NC_ITEMDB_QUESTITEMGET_ACK = 94;

        public const ushort NC_ITEMDB_DEPOSIT_REQ = 95;

        public const ushort NC_ITEMDB_DEPOSIT_ACK = 96;

        public const ushort NC_ITEMDB_WITHDRAW_REQ = 97;

        public const ushort NC_ITEMDB_WITHDRAW_ACK = 98;

        public const ushort NC_ITEMDB_CHARGED_LIST_REQ = 99;

        public const ushort NC_ITEMDB_CHARGED_LIST_ACK = 100;

        public const ushort NC_ITEMDB_CHARGED_WITHDRAW_REQ = 101;

        public const ushort NC_ITEMDB_CHARGED_WITHDRAW_ACK = 102;

        public const ushort NC_ITEMDB_CREATEMUSHROOM_REQ = 103;

        public const ushort NC_ITEMDB_CREATEMUSHROOMSUC_ACK = 104;

        public const ushort NC_ITEMDB_CREATEMUSHROOMFAIL_ACK = 105;

        public const ushort NC_ITEMDB_ITEMBREAK_REQ = 106;

        public const ushort NC_ITEMDB_ITEMBREAKSUC_ACK = 107;

        public const ushort NC_ITEMDB_ITEMBREAKFAIL_ACK = 108;

        public const ushort NC_ITEMDB_CHESTITEM_REQ = 109;

        public const ushort NC_ITEMDB_CHESTITEM_ACK = 110;

        public const ushort NC_ITEMDB_GUILD_TOURNAMENT_REWARD_CREATE_REQ = 111;

        public const ushort NC_ITEMDB_GUILD_TOURNAMENT_REWARD_CREATE_ACK = 112;

        public const ushort NC_ITEMDB_OPEN_GUILD_STORAGE_REQ = 113;

        public const ushort NC_ITEMDB_OPEN_GUILD_STORAGE_FAIL_ACK = 114;

        public const ushort NC_ITEMDB_OPEN_GUILD_STORAGE_ACK = 115;

        public const ushort NC_ITEMDB_GUILD_STORAGE_WITHDRAW_REQ = 116;

        public const ushort NC_ITEMDB_GUILD_STORAGE_WITHDRAW_ACK = 117;

        public const ushort NC_ITEMDB_DISMANTLE_REQ = 118;

        public const ushort NC_ITEMDB_DISMANTLE_ACK = 119;

        public const ushort NC_ITEMDB_INC_DEC_MONEY_REQ = 122;

        public const ushort NC_ITEMDB_INC_DEC_MONEYSUC_ACK = 123;

        public const ushort NC_ITEMDB_INC_DEC_MONEYFAIL_ACK = 124;

        public const ushort NC_ITEMDB_MINIHOUSE_EFFECT_DEMANDGOOD_REQ = 125;

        public const ushort NC_ITEMDB_MINIHOUSE_EFFECT_DEMANDGOOD_ACK = 126;

        public const ushort NC_ITEMDB_REINFORCEUNEQUIP_REQ = 127;

        public const ushort NC_ITEMDB_REINFORCEUNEQUIPSUC_ACK = 128;

        public const ushort NC_ITEMDB_REINFORCEUNEQUIPFAIL_ACK = 129;

        public const ushort NC_ITEMDB_GUILD_ACADEMY_REWARD_STORAGE_OPEN_REQ = 130;

        public const ushort NC_ITEMDB_GUILD_ACADEMY_REWARD_STORAGE_OPEN_FAIL_ACK = 131;

        public const ushort NC_ITEMDB_GUILD_ACADEMY_REWARD_STORAGE_OPEN_ACK = 132;

        public const ushort NC_ITEMDB_GUILD_ACADEMY_REWARD_STORAGE_WITHDRAW_REQ = 133;

        public const ushort NC_ITEMDB_GUILD_ACADEMY_REWARD_STORAGE_WITHDRAW_ACK = 134;

        public const ushort NC_ITEMDB_GUILD_ACADEMY_REWARD_STORAGE_DEPOSIT_REQ = 135;

        public const ushort NC_ITEMDB_GUILD_ACADEMY_REWARD_STORAGE_DEPOSIT_ACK = 136;

        public const ushort NC_ITEMDB_MINIHOUSE_PORTAL_EFFECT_DEMANDGOOD_REQ = 137;

        public const ushort NC_ITEMDB_MINIHOUSE_PORTAL_EFFECT_DEMANDGOOD_ACK = 138;

        public const ushort NC_ITEMDB_FURNITURE_ENDURE_REQ = 139;

        public const ushort NC_ITEMDB_FURNITURE_ENDURE_ACK = 140;

        public const ushort NC_ITEMDB_WEAPONENDURE_CHARGE_REQ = 141;

        public const ushort NC_ITEMDB_WEAPONENDURE_CHARGE_ACK = 142;

        public const ushort NC_ITEMDB_WEAPONENDURESET_CMD = 143;

        public const ushort NC_ITEMDB_GUILD_TOKEN_BUYALL_REQ = 144;

        public const ushort NC_ITEMDB_GUILD_TOKEN_BUYALLSUC_ACK = 145;

        public const ushort NC_ITEMDB_GUILD_TOKEN_BUYALLFAIL_ACK = 146;

        public const ushort NC_ITEMDB_GUILD_TOKEN_BUYLOT_REQ = 147;

        public const ushort NC_ITEMDB_GUILD_TOKEN_BUYLOTSUC_ACK = 148;

        public const ushort NC_ITEMDB_GUILD_TOKEN_BUYLOTFAIL_ACK = 149;

        public const ushort NC_ITEMDB_ENCHANT_ADD_GEM_REQ = 150;

        public const ushort NC_ITEMDB_ENCHANT_ADD_GEM_ACK = 151;

        public const ushort NC_ITEMDB_ENCHANT_REMOVE_GEM_REQ = 152;

        public const ushort NC_ITEMDB_ENCHANT_REMOVE_GEM_ACK = 153;

        public const ushort NC_ITEMDB_ENCHANT_ADD_NEW_SOCKET_REQ = 154;

        public const ushort NC_ITEMDB_ENCHANT_ADD_NEW_SOCKET_ACK = 155;

        public const ushort NC_ITEMDB_ENCHANT_SET_GEM_LOT_REQ = 156;

        public const ushort NC_ITEMDB_ENCHANT_SET_GEM_LOT_ACK = 157;

        public const ushort NC_ITEMDB_MYSTERY_VAULT_MAKEITEM_REQ = 158;

        public const ushort NC_ITEMDB_MYSTERY_VAULT_MAKEITEM_ACK = 159;

        public const ushort NC_ITEMDB_BUYCAPSULE_REQ = 160;

        public const ushort NC_ITEMDB_BUYCAPSULE_ACK = 161;

        public const ushort NC_ITEMDB_CAPSULEITEM_REQ = 162;

        public const ushort NC_ITEMDB_CAPSULEITEM_ACK = 163;

        public const ushort NC_ITEMDB_GETFROMCAPSULE_REQ = 164;

        public const ushort NC_ITEMDB_GETFROMCAPSULE_ACK = 165;

        public const ushort NC_ITEMDB_EQUIP_BELONGED_CANCEL_USE_REQ = 166;

        public const ushort NC_ITEMDB_EQUIP_BELONGED_CANCEL_USE_ACK = 167;

        public const ushort NC_ITEMDB_ITEMINFO_REQ = 168;

        public const ushort NC_ITEMDB_ITEMINFO_ACK = 169;

        public const ushort NC_ITEMDB_ITEMREBUILD_REQ = 170;

        public const ushort NC_ITEMDB_ITEMREBUILD_ACK = 171;

        public const ushort NC_ITEMDB_MOVER_UPGRADE_REQ = 172;

        public const ushort NC_ITEMDB_MOVER_UPGRADE_ACK = 173;

        public const ushort NC_ITEMDB_MOVER_RAREMOVER_REQ = 174;

        public const ushort NC_ITEMDB_MOVER_RAREMOVER_ACK = 175;

        public const ushort NC_ITEMDB_ITEMMONEY_BUYALL_REQ = 176;

        public const ushort NC_ITEMDB_ITEMMONEY_BUYLOT_REQ = 177;

        public const ushort NC_ITEMDB_ITEMMONEY_BUY_ACK = 178;

        public const ushort NC_ITEMDB_RANDOMOPTION_CHANGE_REQ = 179;

        public const ushort NC_ITEMDB_RANDOMOPTION_CHANGE_ACK = 180;

        public const ushort NC_ITEMDB_CHAT_COLOR_CHANGE_REQ = 181;

        public const ushort NC_ITEMDB_CHAT_COLOR_CHANGE_ACK = 182;

        public const ushort NC_ITEMDB_TERMEXTEND_REQ = 183;

        public const ushort NC_ITEMDB_TERMEXTEND_ACK = 184;

        public const ushort NC_ITEMDB_REPURCHASE_ALL_REQ = 185;

        public const ushort NC_ITEMDB_REPURCHASE_ALL_ACK = 186;

        public const ushort NC_ITEMDB_REPURCHASE_LOT_REQ = 187;

        public const ushort NC_ITEMDB_REPURCHASE_LOT_ACK = 188;

        public const ushort NC_ITEMDB_CLASS_CHANGE_REQ = 189;

        public const ushort NC_ITEMDB_CLASS_CHANGE_ACK = 190;

        public const ushort NC_ITEMDB_UES_FRIEND_POINT_REQ = 191;

        public const ushort NC_ITEMDB_UES_FRIEND_POINT_ACK = 192;

        public const ushort NC_ITEMDB_RANDOMOPTION_CHANGE_CONSUME_AND_COUNTING_REQ = 193;

        public const ushort NC_ITEMDB_RANDOMOPTION_CHANGE_CONSUME_AND_COUNTING_ACK = 194;

        public const ushort NC_ITEMDB_SHIELDENDURE_CHARGE_REQ = 198;

        public const ushort NC_ITEMDB_SHIELDENDURE_CHARGE_ACK = 199;

        public const ushort NC_ITEMDB_SHIELDENDURESET_CMD = 200;

        public const ushort NC_ITEMDB_MAPLINK_ITEM_CONSUME_REQ = 201;

        public const ushort NC_ITEMDB_MAPLINK_ITEM_CONSUME_ACK = 202;

        public const ushort NC_ITEMDB_MIX_ITEM_REQ = 203;

        public const ushort NC_ITEMDB_MIX_ITEM_ACK = 204;

        public const ushort NC_ITEMDB_RANDOMOPTION_RECOVER_COUNT_LIMIT_REQ = 205;

        public const ushort NC_ITEMDB_RANDOMOPTION_RECOVER_COUNT_LIMIT_ACK = 206;

        public const ushort NC_ITEMDB_RESET_SCROLL_LINK_MAP_INFO_CMD = 207;

        public const ushort NC_ITEMDB_NEW_UPGRADE_REQ = 209;

        public const ushort NC_ITEMDB_NEW_UPGRADE_ACK = 210;

        public const ushort NC_ITEMDB_BRACELET_UPGRADE_REQ = 211;

        public const ushort NC_ITEMDB_BRACELET_UPGRADE_ACK = 212;
    }
}
