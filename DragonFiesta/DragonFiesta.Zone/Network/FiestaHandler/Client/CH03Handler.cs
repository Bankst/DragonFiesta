using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.InternNetwork.InternHandler.Server.Character;

namespace DragonFiesta.Zone.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler03Type._Header)]
    public static class CH03Handler
    {
        public static void CMSG_CONNECTION_CLOSE(ZoneSession sender, FiestaPacket packet)
        {

            if (!sender.Ingame ||
                sender.IsDisposed
                || !packet.Read(out bool BackToCharacterList))
            {
                sender.Dispose();
                return;
            }

            if (BackToCharacterList)
            {
                CharacterMethods.SendCharacterLoggedOut(sender.Character, true);
            }
        }
    }
}