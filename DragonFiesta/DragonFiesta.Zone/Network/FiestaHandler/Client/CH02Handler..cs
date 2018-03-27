using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Core;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler02Type._Header)]
    public static class CH02Handler
    {
        [PacketHandler(Handler02Type.CMSG_MISC_HEARTBEAT_ACK)]
        public static void CMSG_PONG(ZoneSession sender, FiestaPacket packet)
        {
            sender.GameStates.HasPong = true;
            sender.GameStates.Lag = ServerMain.InternalInstance.CurrentTime.Time.Subtract(sender.GameStates.LastPing);
        }
    }
}