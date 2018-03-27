using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Helpers;

namespace DragonFiesta.World.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler02Type._Header)]
    public sealed class CH02Handler
    {
        [PacketHandler(Handler02Type.CMSG_MISC_HEARTBEAT_ACK)]
        public static void CMSG_MISC_HEARTBEAT_ACK(WorldSession sender, FiestaPacket packet)
        {
            sender.GameStates.HasPong = true;
            sender.GameStates.Lag = ServerMain.InternalInstance.CurrentTime.Time.Subtract(sender.GameStates.LastPing);
        }

        [PacketHandler(Handler02Type.CMSG_GAMETIME_REQ)]
        public static void CMSG_PING(WorldSession sender, FiestaPacket packet)
        {
            _SH02Helpers.SendPong(sender);
        }
    }
}