using DragonFiesta.Messages.Zone.Note;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Zone;
using System;

namespace DragonFiesta.World.Game.Chat
{
    public static class ZoneChat
    {
        public static void CharacterNote(WorldCharacter pChar, string Message)
        {
            pChar.Map.Zone.Send(new CharacterNote
            {
                Id = Guid.NewGuid(),
                CharacterId = pChar.Info.CharacterID,
                DestZone = pChar.Map.ZoneId,
                NotText = Message,
            });
        }

        public static void ZoneServerNote(ZoneServer Server, string Message)
        {
            ZoneManager.Broadcast(new ServerNote
            {
                Id = Guid.NewGuid(),
                Text = Message,
            });
        }

        public static void ChatMessageToAll(string Message)
        {
            //Need for Chat Gms or not
        }
    }
}