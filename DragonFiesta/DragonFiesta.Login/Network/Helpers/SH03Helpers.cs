using DragonFiesta.Networking.HandlerTypes;

namespace DragonFiesta.Login.Network.Helpers
{
    public class SH03Helpers
    {
        public static void SendLoginError(LoginSession pSession, LoginGameError Error)
        {
            using (var packet = new FiestaPacket(Handler03Type._Header, Handler03Type.SMSG_USER_LOGINFAIL_ACK))
            {
                packet.Write<ushort>((ushort)Error);

                pSession.SendPacket(packet);
            }
        }
    }
}