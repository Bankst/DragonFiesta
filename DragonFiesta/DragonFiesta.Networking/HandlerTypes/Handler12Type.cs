namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler12Type : HandlerType
    {
        public new const byte _Header = 12;


        //CMSG
        public const ushort CMSG_ITEM_BUY_REQ = 3;        

        public const ushort CMSG_ITEM_SELL_REQ = 6;

        public const ushort CMSG_ITEM_DROP_REQ = 7;

        public const ushort CMSG_ITEM_PICK_REQ = 9;

        public const ushort CMSG_ITEM_RELOC_REQ = 11;

        public const ushort CMSG_ITEM_EQUIP_REQ = 15;        
        
        public const ushort CMSG_ITEM_UNEQUIP_REQ = 18;        
        
        public const ushort CMSG_ITEM_USE_REQ = 21;
        
        public const ushort CMSG_ITEM_UPGRADE_REQ = 23;

        public const ushort CMSG_ITEM_CHARGEDINVENOPEN_REQ = 32;

        public const ushort CMSG_ITEM_REWARDINVENOPEN_REQ = 44;
        

        //SMSG
        public const ushort SMSG_ITEM_CELLCHANGE_CMD = 1;

        public const ushort SMSG_ITEM_EQUIPCHANGE_CMD = 2;

        public const ushort SMSG_ITEM_BUY_ACK = 4;

        public const ushort SMSG_ITEM_PICK_ACK = 10;
        
        public const ushort SMSG_ITEM_EQUIP_ACK = 17;

        public const ushort SMSG_ITEM_UNEQUIP_ACK = 19;        
        
        public const ushort SMSG_ITEM_USE_ACK = 22;        
        
        public const ushort SMSG_ITEM_UPGRADE_ACK = 24;        
        
        public const ushort SMSG_ITEM_USECOMPLETE_CMD = 26;       
        
        public const ushort SMSG_ITEM_CHARGEDINVENOPEN_ACK = 33;        
        
        public const ushort SMSG_ITEM_GUILD_STORAGE_WITHDRAW_ACK = 39;        
        
        public const ushort SMSG_ITEM_REWARDINVENOPEN_ACK = 45;
        
        
        //NC
        public const ushort NC_ITEM_SELL_ACK = 5;

        public const ushort NC_ITEM_DROP_ACK = 8;

        public const ushort NC_ITEM_RELOC_ACK = 12;

        public const ushort NC_ITEM_SPLIT_REQ = 13;

        public const ushort NC_ITEM_SPLIT_ACK = 14;

        public const ushort NC_ITEM_RINGEQUIP_REQ = 16;

        public const ushort NC_ITEM_SOMEONEPICK_CMD = 20;

        public const ushort NC_ITEM_USEABORT_CMD = 25;

        public const ushort NC_ITEM_PICKOTHER_ACK = 27;

        public const ushort NC_ITEM_DEPOSIT_REQ = 28;

        public const ushort NC_ITEM_DEPOSIT_ACK = 29;

        public const ushort NC_ITEM_WITHDRAW_REQ = 30;

        public const ushort NC_ITEM_WITHDRAW_ACK = 31;

        public const ushort NC_ITEM_CHARGED_WITHDRAW_REQ = 34;

        public const ushort NC_ITEM_CHARGED_WITHDRAW_ACK = 35;

        public const ushort NC_ITEM_BREAKSUCCESS_CMD = 36;

        public const ushort NC_ITEM_BREAKFAIL_CMD = 37;

        public const ushort NC_ITEM_GUILD_STORAGE_WITHDRAW_REQ = 38;

        public const ushort NC_ITEM_OPENSTORAGEPAGE_REQ = 40;

        public const ushort NC_ITEM_SOMEONEUSE_CMD = 41;

        public const ushort NC_ITEM_DISMANTLE_REQ = 42;

        public const ushort NC_ITEM_DISMANTLE_ACK = 43;

        public const ushort NC_ITEM_REWARDINVENOPENFAIL_ACK = 46;

        public const ushort NC_ITEM_GUILD_ACADEMY_REWARD_STORAGE_WITHDRAW_REQ = 47;

        public const ushort NC_ITEM_GUILD_ACADEMY_REWARD_STORAGE_WITHDRAW_ACK = 48;

        public const ushort NC_ITEM_GUILD_ACADEMY_REWARD_STORAGE_DEPOSIT_REQ = 49;

        public const ushort NC_ITEM_GUILD_ACADEMY_REWARD_STORAGE_DEPOSIT_ACK = 50;

        public const ushort NC_ITEM_MH_FURNITURE_ENDURE_KIT_USE_REQ = 51;

        public const ushort NC_ITEM_MH_FURNITURE_ENDURE_KIT_USE_ACK = 52;

        public const ushort NC_ITEM_WEAPONENDURE_CHARGE_REQ = 53;

        public const ushort NC_ITEM_WEAPONENDURE_CHARGE_ACK = 54;

        public const ushort NC_ITEM_WEAPONENDURESET_CMD = 55;

        public const ushort NC_ITEM_ITEMBREAK_CMD = 56;

        public const ushort NC_ITEM_REVIVEITEMUSE_CMD = 57;

        public const ushort NC_ITEM_REVIVEITEMUSEFAIL_CMD = 58;

        public const ushort NC_ITEM_DICE_GAME_CMD = 59;

        public const ushort NC_ITEM_DICE_GAME_START_REQ = 60;

        public const ushort NC_ITEM_DICE_GAME_START_ACK = 61;

        public const ushort NC_ITEM_DICE_GAME_START_CMD = 62;

        public const ushort NC_ITEM_DICE_GAME_RESULT_CMD = 63;

        public const ushort NC_ITEM_ENCHANT_ADD_GEM_REQ = 64;

        public const ushort NC_ITEM_ENCHANT_ADD_GEM_ACK = 65;

        public const ushort NC_ITEM_ENCHANT_REMOVE_GEM_REQ = 66;

        public const ushort NC_ITEM_ENCHANT_REMOVE_GEM_ACK = 67;

        public const ushort NC_ITEM_ENCHANT_ADD_NEW_SOCKET_REQ = 68;

        public const ushort NC_ITEM_ENCHANT_ADD_NEW_SOCKET_ACK = 69;

        public const ushort NC_ITEM_ENCHANT_SET_GEM_LOT_CMD = 70;

        public const ushort NC_ITEM_BUY_SUC_ACTION_CMD = 71;

        public const ushort NC_ITEM_EQUIP_BELONGED_CANCEL_USE_REQ = 72;

        public const ushort NC_ITEM_EQUIP_BELONGED_CANCEL_USE_ACK = 73;

        public const ushort NC_ITEM_AUTO_ARRANGE_INVEN_REQ = 74;

        public const ushort NC_ITEM_AUTO_ARRANGE_INVEN_ACK = 75;

        public const ushort NC_ITEM_ACCOUNT_STORAGE_OPEN_CMD = 76;

        public const ushort NC_ITEM_ACCOUNT_STORAGE_CLOSE_CMD = 77;

        public const ushort NC_ITEM_USE_ACTIVESKILL_REQ = 78;

        public const ushort NC_ITEM_USE_ACTIVESKILL_ACK = 79;

        public const ushort NC_ITEM_MINIMON_EQUIP_REQ = 80;

        public const ushort NC_ITEM_MOVER_UPGRADE_REQ = 81;

        public const ushort NC_ITEM_MOVER_UPGRADE_ACK = 82;

        public const ushort NC_ITEM_RANDOMOPTION_CHANGE_REQ = 83;

        public const ushort NC_ITEM_RANDOMOPTION_CHANGE_ACK = 84;

        public const ushort NC_ITEM_CHAT_COLOR_CHANGE_REQ = 85;

        public const ushort NC_ITEM_CHAT_COLOR_CHANGE_ACK = 86;

        public const ushort NC_ITEM_TERMEXTEND_REQ = 87;

        public const ushort NC_ITEM_TERMEXTEND_ACK = 88;

        public const ushort NC_ITEM_REPURCHASE_REQ = 89;

        public const ushort NC_ITEM_REPURCHASE_ACK = 90;

        public const ushort NC_ITEM_SELL_ITEM_LIST_CMD = 91;

        public const ushort NC_ITEM_SELL_ITEM_INSERT_CMD = 92;

        public const ushort NC_ITEM_SELL_ITEM_DELETE_CMD = 93;

        public const ushort NC_ITEM_CLASS_CHANGE_REQ = 94;

        public const ushort NC_ITEM_CLASS_CHANGE_ACK = 95;

        public const ushort NC_ITEM_OPENCLASSCHANGEMENU_CMD = 96;

        public const ushort NC_ITEM_RANDOMOPTION_CHANGE_ACCEPT_REQ = 97;

        public const ushort NC_ITEM_RANDOMOPTION_CHANGE_ACCEPT_ACK = 98;

        public const ushort NC_ITEM_SHIELDENDURE_CHARGE_REQ = 100;

        public const ushort NC_ITEM_SHIELDENDURE_CHARGE_ACK = 101;

        public const ushort NC_ITEM_SHIELDENDURESET_CMD = 102;

        public const ushort NC_ITEM_MAPLINK_SCROLL_REQ = 103;

        public const ushort NC_ITEM_MAPLINK_SCROLL_ACK = 104;

        public const ushort NC_ITEM_MIX_ITEM_REQ = 105;

        public const ushort NC_ITEM_MIX_ITEM_ACK = 106;

        public const ushort NC_ITEM_RANDOMOPTION_RECOVER_COUNT_LIMIT_REQ = 107;

        public const ushort NC_ITEM_RANDOMOPTION_RECOVER_COUNT_LIMIT_ACK = 108;

        public const ushort NC_ITEM_NEW_UPGRADE_REQ = 109;

        public const ushort NC_ITEM_NEW_UPGRADE_ACK = 110;

        public const ushort NC_ITEM_BRACELET_UPGRADE_REQ = 111;

        public const ushort NC_ITEM_BRACELET_UPGRADE_ACK = 112;
        
    }
}