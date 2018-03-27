using DragonFiesta.Networking.HandlerTypes;

namespace DragonFiesta.World.Network.FiestaHandler.Server
{
    public class SH25Handler
    {
        public static void SendAnnounce(WorldSession Session, string Message)
        {
            using (var Packet = new FiestaPacket(Handler25Type._Header, Handler25Type.SMSG_ANNOUNCE_Z2W_CMD))
            {
                Packet.Write<byte>(1);
                Packet.Write<byte>(Message.Length);
                Packet.WriteString(Message, Message.Length);
                Session.SendPacket(Packet);
            }
        }
    }
}