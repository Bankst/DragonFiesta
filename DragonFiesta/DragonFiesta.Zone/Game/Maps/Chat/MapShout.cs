using DragonFiesta.Utils.Config;
using DragonFiesta.Zone.Network;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using DragonFiesta.Zone.Game.Maps.Types;

namespace DragonFiesta.Zone.Game.Maps.Chat
{
    public class MapShout : MapChatBase
    {
        public MapShout(LocalMap Map) : base(Map, ChatConfiguration.Instance.ShoutChatSettings)
        {
        }

        public sealed override void BroadcastMessage(ZoneSession Session, string Message)
        {
            if (Session.Ingame)
            {
                Map.CharacterAction((character) =>
                {
                    SH08Handler.SendChatMessage(character.Session, Session.Character, Message, true);
                }, true);
            }
        }
    }
}