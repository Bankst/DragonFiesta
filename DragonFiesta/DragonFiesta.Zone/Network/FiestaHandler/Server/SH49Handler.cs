using DragonFiesta.Networking.HandlerTypes;

namespace DragonFiesta.Zone.Network.FiestaHandler.Server
{
    public static class SH49Handler
    {
        public static void CardRegistAck(ZoneSession session)
        {
            using (var packet = new FiestaPacket(Handler49Type._Header, Handler49Type.SMSG_COLLECT_CARDREGIST_ACK))
            {
                packet.Write<byte>(0); //IvenSlot
                packet.Write<ushort>(0); //CardID
                packet.Write<byte>(0); //Star
                packet.Write<uint>(0); //Serial
                packet.Write<ushort>(ZoneGameErrors.Unk); //ErrorCode
                session.SendPacket(packet);
            }
        }

        public static void BookMarkRegist(ZoneSession session)
        {
            using (var packet = new FiestaPacket(Handler49Type._Header, Handler49Type.SMSG_COLLECT_BOOKMARK_REGIST_ACK))
            {
                packet.Write<ushort>(0); //BookMarkSlot
                packet.Write<ushort>(0); //CardID
                packet.Write<ushort>(ZoneGameErrors.Unk1); // ErrorCode
                session.SendPacket(packet);
            }
        }
    }
}
