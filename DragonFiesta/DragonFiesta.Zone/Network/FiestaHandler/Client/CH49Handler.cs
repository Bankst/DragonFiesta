using DragonFiesta.Networking.HandlerTypes;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler49Type._Header)]
    public static class CH49Handler
    {
        [PacketHandler(Handler49Type.CMSG_COLLECT_CARDREGIST_REQ)]
        public static void CMSG_CardRegist(ZoneSession sender, FiestaPacket packet)
        {
            //Byte -> InvenSlot
        }

        [PacketHandler(Handler49Type.CMSG_COLLECT_BOOKMARK_REGIST_REQ)]
        public static void CMSG_Bookmark_CardRegist(ZoneSession sender, FiestaPacket packet)
        {
            //UShort -> BookMarkSlot
            //UShort -> CardID
        }

    }
}
