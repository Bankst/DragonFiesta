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
        public static void HandleGameCommandToServer(GameCommandToServer Respone, InternZoneSession pSession)
        {
            if (!WorldCharacterManager.Instance.GetLoggedInCharacterByCharacterID(Respone.CharacterId, out WorldCharacter Character))
            {
                CommandLog.Write(CommandLogLevel.Error, "Internal Command Error Character {0} not found!!", Respone.CharacterId);
                return;
            }

            if (!GameCommandManager.GetGameCommand(Respone.Category, Respone.Command, out GameCommand Command))
            {
                CommandLog.Write(CommandLogLevel.Error, "Internal Command Error WorldCommand {0} {1} on World Not found!!", Respone.Category, Respone.Command);
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

        [InternMessageHandler(typeof(ServerNote))]
        public static void HandleServerNote(ServerNote Respone, InternZoneSession pSession)
        {
            foreach (var Zone in ZoneManager.FindAllActiveZone())
            {
                ZoneChat.ZoneServerNote((Zone as ZoneServer), Respone.Text);
            }
        }

        [InternMessageHandler(typeof(MapNote))]
        public static void HandleMapNote(MapNote Respone, InternZoneSession pSession)
        {
            if (MapManager.GetMap(Respone.MapId, Respone.InstanceId, out WorldServerMap Map))
            {
                Map.Zone.Send(Respone);
            }

        }
    }
}