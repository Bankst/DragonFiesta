using DragonFiesta.Utils.Config.Section.Chat;
using DragonFiesta.Zone.Game.Chat;
using DragonFiesta.Zone.Network;
using DragonFiesta.Zone.Game.Maps.Types;

namespace DragonFiesta.Zone.Game.Maps.Chat
{
    public abstract class MapChatBase : ChatBase
    {
        public LocalMap Map { get; private set; }

        public MapChatBase(LocalMap Map, ChatSection Config) : base(Config)
        {
            this.Map = Map;
        }

        public override void BroadcastMessage(ZoneSession Session, string Message)
        {
            if (Session.Ingame)
            {
                Map.CharacterAction((character) =>
                {
                    //TODO SEND PACKET,....
                }, true);
            }
        }
    }
}