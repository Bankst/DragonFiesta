namespace DragonFiesta.Networking.HandlerTypes
{
    public sealed class Handler14Type : HandlerType
    {
        public new const byte _Header = 14;


        //CMSG
        public const ushort CMSG_PARTY_JOIN_REQ = 2;

        public const ushort CMSG_PARTY_JOINPROPOSE_ALLOW_ACK = 4;

        public const ushort CMSG_PARTY_JOINPROPOSE_REJECT_ACK = 5;

        public const ushort CMSG_PARTY_LEAVE_REQ = 10;

        public const ushort CMSG_PARTY_KICKOFF_REQ = 20;
        
        public const ushort CMSG_PARTY_CHANGEMASTER_REQ = 40;        
        
        public const ushort CMSG_PARTY_MEMBERINFOREQ_CMD = 72;
        
        public const ushort CMSG_PARTY_ITEM_LOOTING_SET = 75;

        public const ushort CMSG_PARTY_FINDER_LIST_REQ = 84;
        
        
        
        //SMSG
        public const ushort SMSG_PARTY_JOINPROPOSE_REQ = 3;

        public const ushort SMSG_PARTY_JOIN_ACK = 7;

        public const ushort SMSG_PARTY_MEMBER_LIST_CMD = 9;

        public const ushort SMSG_PARTY_LEAVE_ACK = 11;

        public const ushort SMSG_PARTY_KICKOFF_ACK = 21;

        public const ushort SMSG_PARTY_DISMISS_ACK = 30;

        public const ushort SMSG_PARTY_CHANGEMASTER_ACK = 41;       
        
        public const ushort SMSG_PARTY_MEMBERINFORM_CMD = 50;

        public const ushort SMSG_PARTY_MEMBERCLASS_CMD = 51;       
        
        public const ushort SMSG_PARTY_MEMBERLOCATION_CMD = 73;
        
        public const ushort SMSG_PARTY_ITEM_LOOTING_SET = 75;        
        
        public const ushort SMSG_PARTY_ITEM_LOOTING_CMD = 76;
        
        
        //NC
        public const ushort NC_PARTY_FUNDAMENTAL_CMD = 1;

        public const ushort NC_PARTY_JOINPROPOSE_TIMEOUT_ACK = 6;

        public const ushort NC_PARTY_JOIN_CMD = 8;

        public const ushort NC_PARTY_LEAVE_CMD = 12;

        public const ushort NC_PARTY_KICKOFF_CMD = 22;

        public const ushort NC_PARTY_DISMISS_CMD = 31;

        public const ushort NC_PARTY_CHANGEMASTER_CMD = 42;

        public const ushort NC_PARTY_LOGIN_CMD = 60;

        public const ushort NC_PARTY_LOGININFO_CMD = 61;

        public const ushort NC_PARTY_LOGOUT_CMD = 70;

        public const ushort NC_PARTY_LOGOUTINFO_CMD = 71;

        public const ushort NC_PARTY_MEMBERMAPOUT = 74;

        public const ushort NC_PARTY_ITEM_LOOTING_ZONE_CMD = 77;

        public const ushort NC_PARTY_MEMBERINFORM_REQ = 78;

        public const ushort NC_PARTY_MEMBERINFORM_ACK = 79;

        public const ushort NC_PARTY_FINDER_ADD_REQ = 80;

        public const ushort NC_PARTY_FINDER_ADD_ACK = 81;

        public const ushort NC_PARTY_FINDER_DELETE_REQ = 82;

        public const ushort NC_PARTY_FINDER_DELETE_ACK = 83;

        public const ushort NC_PARTY_FINDER_LIST_ACK = 85;

        public const ushort NC_PARTY_FINDER_DELETE_YOUR_MSG_CMD = 86;

        public const ushort NC_PARTY_SET_LOOTER_REQ = 90;

        public const ushort NC_PARTY_SET_LOOTER_ACK = 91;

        public const ushort NC_PARTY_SET_LOOTER_CMD = 92;

        public const ushort NC_PARTY_SET_LOOTER_BROAD_CMD = 93;

        public const ushort NC_PARTY_ZONE_SET_LOOTER_CMD = 94;

        public const ushort NC_PARTY_ITEM_JOIN_LOOTING_CMD = 95;

        public const ushort NC_PARTY_ZONE_JOIN_CMD = 96;

        public const ushort NC_PARTY_ZONE_LEAVE_CMD = 97;
        
    }
}