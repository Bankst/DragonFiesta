using DragonFiesta.Game.CommandAccess;
using DragonFiesta.Messages.Command;
using DragonFiesta.Messages.Zone.Note;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;
using DragonFiesta.Zone.Game.Maps;
using DragonFiesta.Zone.Game.Maps.Object;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.Network.FiestaHandler.Server;

namespace DragonFiesta.Zone.InternNetwork.InternHandler.Client.Note
{
    public sealed class ZoneChatHandler
    {
        [InternMessageHandler(typeof(GameCommandToServer))]
        public static void HandleGameCommandToServer(GameCommandToServer Respone, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetLoggedInCharacterByCharacterID(Respone.CharacterId, out ZoneCharacter Character))
            {
                CommandLog.Write(CommandLogLevel.Error, "Internal Command Error Character {0} not found!!", Respone.CharacterId);
                return;
            }

            if (!GameCommandManager.GetGameCommand(Respone.Category, Respone.Command, out GameCommand Command))
            {
                CommandLog.Write(CommandLogLevel.Error, "Internal Command Error ZoneCommand {0} {1} on Wolrd Not found!!", Respone.Category, Respone.Command);
                return;
            }
            string CommandString = $"{Respone.Category} {Respone.Command} {string.Join(" ", Respone.Args) }";
            if (Command.Method != null)
            {
                if ((bool)Command.Method.Invoke(Character, new object[] { Character, Respone.Args }))
                {
                    CommandLog.Write(CommandLogLevel.Execute, "Character {0} Execute Command {1}", Respone.CharacterId, CommandString);
                    return;
                }
            }
            else
            {
                ZoneChat.CharacterNote(Character, $"Command {CommandString } Not Found!");
            }
        }

        [InternMessageHandler(typeof(CharacterNote))]
        public static void HandleNoteToCharacter(CharacterNote MapMessage, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetCharacterByCharacterID(MapMessage.CharacterId, out ZoneCharacter Character, out CharacterErrors res))
            {
                return;
            }
            SH08Handler.SendNotice(Character.Session, MapMessage.NotText);
        }

        [InternMessageHandler(typeof(MapNote))]
        public static void HandleMapNote(MapNote MapMessage, InternWorldConnector pSession)
        {
            if (MapManager.GetMap(MapMessage.MapId, MapMessage.InstanceId, out ZoneServerMap Map))
            {
                if (Map is LocalMap LocalMap)
                {
                    LocalMap.CharacterAction((character) =>
                     {
                         SH08Handler.SendNotice(character.Session, MapMessage.NoteText);
                     }, true);
                }
            }
        }

        [InternMessageHandler(typeof(ServerNote))]
        public static void HandleServerNote(ServerNote MapMessage, InternWorldConnector pSession)
        {
            ZoneChat.LocalZoneNote(MapMessage.Text);
        }
    }
}