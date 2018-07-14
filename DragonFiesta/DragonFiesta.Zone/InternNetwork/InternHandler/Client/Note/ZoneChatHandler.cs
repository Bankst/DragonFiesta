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
        public static void HandleGameCommandToServer(GameCommandToServer response, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetLoggedInCharacterByCharacterID(response.CharacterId, out var character))
            {
                CommandLog.Write(CommandLogLevel.Error, "Internal Command Error Character {0} not found!!", response.CharacterId);
                return;
            }

            if (!GameCommandManager.GetGameCommand(response.Category, response.Command, out var command))
            {
                CommandLog.Write(CommandLogLevel.Error, "Internal Command Error ZoneCommand {0} {1} on Wolrd Not found!!", response.Category, response.Command);
                return;
            }
            var commandString = $"{response.Category} {response.Command} {string.Join(" ", response.Args) }";
            if (command.Method != null)
            {
	            if (!(bool) command.Method.Invoke(character, new object[] {character, response.Args})) return;
	            CommandLog.Write(CommandLogLevel.Execute, "Character {0} Execute Command {1}", response.CharacterId, commandString);
	            return;
            }

	        ZoneChat.CharacterNote(character, $"Command {commandString} Not Found!");
        }

        [InternMessageHandler(typeof(CharacterNote))]
        public static void HandleNoteToCharacter(CharacterNote mapMessage, InternWorldConnector pSession)
        {
            if (!ZoneCharacterManager.Instance.GetCharacterByCharacterID(mapMessage.CharacterId, out var character, out var res))
            {
                return;
            }
            SH08Handler.SendNotice(character.Session, mapMessage.NotText);
        }

        [InternMessageHandler(typeof(MapNote))]
        public static void HandleMapNote(MapNote mapMessage, InternWorldConnector pSession)
        {
	        if (!MapManager.GetMap(mapMessage.MapId, mapMessage.InstanceId, out var map)) return;
	        if (map is LocalMap localMap)
	        {
		        localMap.CharacterAction((character) =>
		        {
			        SH08Handler.SendNotice(character.Session, mapMessage.NoteText);
		        });
	        }
        }

        [InternMessageHandler(typeof(ServerNote))]
        public static void HandleServerNote(ServerNote mapMessage, InternWorldConnector pSession)
        {
            ZoneChat.LocalZoneNote(mapMessage.Text);
        }
    }
}