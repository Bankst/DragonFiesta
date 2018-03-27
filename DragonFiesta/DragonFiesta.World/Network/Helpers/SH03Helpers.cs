using DragonFiesta.Networking.HandlerTypes;

namespace DragonFiesta.World.Network.Helpers
{
    public class SH03Helpers
    {
        public static void SendVerfiryError(WorldSession pSession, ushort err)
        {
            using (var mPacket = new FiestaPacket(Handler03Type._Header, Handler03Type.SMSG_USER_LOGINWORLDFAIL_ACK))
            {
                mPacket.Write<ushort>((ushort)err);
                pSession.SendPacket(mPacket);
            }
        }
    }
}