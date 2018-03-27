using DragonFiesta.Messages.Zone.Note;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Network;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using System;

namespace DragonFiesta.Zone.Game.Chat
{
    public static class ZoneChat
    {
        public static void CharacterNote(ZoneCharacter Character, string Message)
        {
            var Note = new CharacterNote
            {
                Id = Guid.NewGuid(),
                CharacterId = Character.Info.CharacterID,
                NotText = Message,
                DestZone = Character.Map.ZoneId,
            };
            InternWorldConnector.Instance.SendMessage(Note);
        }

        public static void LocalZoneNote(string Message)
        {
            ZoneSessionManager.Instance.ClientAction((ZoneSession) =>
            {
                if (ZoneSession.Ingame)
                {
                    SH08Handler.SendNotice(ZoneSession, Message);
                }
            });
        }
        public static void ServerNote(string Message)
        {
            var ServerNote = new ServerNote
            {
                Id = Guid.NewGuid(),
                Text = Message,
            };
            InternWorldConnector.Instance.SendMessage(ServerNote);
        }

        public static void MapNote(IMap Map, string Message)
        {
            var NoteToMap = new MapNote
            {
                Id = Guid.NewGuid(),
                InstanceId = (Map is IInstanceMap) ? ((Map as IInstanceMap).InstanceId) : (ushort)0,
                MapId = Map.MapId,
                NoteText = Message,
            };
            InternWorldConnector.Instance.SendMessage(NoteToMap);

        }
    }
}