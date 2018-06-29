using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Network;
using DragonFiesta.Networking.Network.Session;

namespace DragonFiesta.Networking.Helpers
{
    public class _SH02Helpers
    {
        public static void SendPing<TSession>(FiestaSession<TSession> mSession) where TSession : FiestaSession<TSession>
        {
            using (var packet = new FiestaPacket(Handler02Type._Header, Handler02Type.SMSG_MISC_HEARTBEAT_REQ))
            {
                mSession.SendPacket(packet);
            }
        }

        public static void SendPong<TSession>(FiestaSession<TSession> mSession)
            where TSession : FiestaSession<TSession>
        {
            using (var packet = new FiestaPacket(Handler02Type._Header, Handler02Type.SMSG_GAMETIME_ACK))
            {
                packet.Write<byte>(mSession.GameStates.LastPing.Hour);
                packet.Write<byte>(mSession.GameStates.LastPing.Minute);
                packet.Write<byte>(mSession.GameStates.LastPing.Second);
                mSession.SendPacket(packet);
            }
        }
    }
}