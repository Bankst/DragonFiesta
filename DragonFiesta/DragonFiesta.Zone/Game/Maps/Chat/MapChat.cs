using DragonFiesta.Utils.Config;
using DragonFiesta.Zone.Network;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using DragonFiesta.Zone.Game.Maps.Types;

namespace DragonFiesta.Zone.Game.Maps.Chat
{
    public class MapChat : MapChatBase
    {
        public MapChat(LocalMap Map) :
            base(Map, ChatConfiguration.Instance.ChatNormalSettings)
        {
        }

        public sealed override void BroadcastMessage(ZoneSession Session, string Message)
        {
            if (Session.Ingame)
            {
                Map.CharacterAction((character) =>
                {
                    SH08Handler.SendChatMessage(character.Session, Session.Character, Message, false);
                }, true);
            }
        }
    }
}