using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Network;

namespace DragonFiesta.Networking.Helpers
{
    public class _SH03Helpers
    {
        public static void SendDublicateLogin<TSession>(FiestaSession<TSession> pSession) 
            where TSession : FiestaSession<TSession>
        {
            using (var packet = new FiestaPacket(Handler03Type._Header, Handler03Type.SMSG_USER_CONNECTCUT_CMD))
            {
                packet.Write<ushort>(1667);
                pSession.SendPacket(packet);
            }
        }
    }
}