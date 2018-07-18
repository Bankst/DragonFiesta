using DragonFiesta.Game.CommandAccess;
using DragonFiesta.Messages.Command;
using DragonFiesta.Messages.Zone.Note;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Zone;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Chat
{
    public class ZoneChatHandler
    {
        [InternMessageHandler(typeof(GameCommandToServer))]
        public static void HandleGameCommandToServer(GameCommandToServer response, InternZoneSession pSession)
        {
            if (!WorldCharacterManager.Instance.GetLoggedInCharacterByCharacterID(response.CharacterId, out var character))
            {
                CommandLog.Write(CommandLogLevel.Error, "Internal Command Error Character {0} not found!!", response.CharacterId);
                return;
            }

            if (!GameCommandManager.GetGameCommand(response.Category, response.Command, out var command))
            {
                CommandLog.Write(CommandLogLevel.Error, "Internal Command Error WorldCommand {0} {1} on World Not found!!", response.Category, response.Command);
                return;
            }
            var commandString = $"{response.Category} {response.Command} {string.Join(" ", response.Args) }";
            if (command.Method != null)
            {
	            if (!(bool) command.Method.Invoke(character, new object[] {character, response.Args})) return;
	            CommandLog.Write(CommandLogLevel.Execute, "Character {0} Execute Command {1}", response.CharacterId, commandString);
            }
            else
            {
                ZoneChat.CharacterNote(character, $"Command {commandString } Not Found!");
            }
        }

        [InternMessageHandler(typeof(ServerNote))]
        public static void HandleServerNote(ServerNote response, InternZoneSession pSession)
        {
            foreach (var zone in ZoneManager.FindAllActiveZone())
            {
                ZoneChat.ZoneServerNote((zone as ZoneServer), response.Text);
            }
        }

        [InternMessageHandler(typeof(MapNote))]
        public static void HandleMapNote(MapNote response, InternZoneSession pSession)
        {
            if (MapManager.GetMap(response.MapId, response.InstanceId, out var map))
            {
                map.Zone.Send(response);
            }

        }
    }
}