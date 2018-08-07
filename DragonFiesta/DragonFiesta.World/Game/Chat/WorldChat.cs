using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Zone;
using DragonFiesta.World.Network.FiestaHandler.Server;

namespace DragonFiesta.World.Game.Chat
{
    public static class WorldChat
    {
        public static void MapAnnounce(WorldServerMap Map, string Message)
        {
            Map.CharacterAction((character) =>
            {
                SH25Handler.SendAnnounce(character.Session, Message);
            }, true);
        }

        public static void ServerAnnounce(string Message)
        {
            foreach (var Zone in ZoneManager.FindAllActiveZone())
            {
                ZoneServerAnnounce((Zone as ZoneServer), Message);
            }
        }

        public static void ZoneServerAnnounce(ZoneServer Server, string Message)
        {
            foreach (var Map in Server.MapList)
            {
                MapAnnounce(Map, Message);
            }
        }
    }
}