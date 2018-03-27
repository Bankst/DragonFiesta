using System.Collections.Concurrent;
using System.Linq;

namespace DragonFiesta.Game.CommandAccess
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Sync)]
    [ServerModule(ServerType.World, InitializationStage.Logic)]
    public class GameCommandManager
    {
        private static ConcurrentDictionary<byte, GameRole> RolesById { get; set; }

        public static ConcurrentDictionary<string, ConcurrentDictionary<string, GameCommand>> CommandsByCategory { get; private set; }

        public static ConcurrentDictionary<int, GameCommand> CommandsByID { get; private set; }

        [InitializerMethod]
        public static bool OnStart()
        {
            RolesById = new ConcurrentDictionary<byte, GameRole>();
            CommandsByCategory = new ConcurrentDictionary<string, ConcurrentDictionary<string, GameCommand>>();
            CommandsByID = new ConcurrentDictionary<int, GameCommand>();
            LoadCommands();
            LoadRolesFromDatabase();
            LoadCommandsPermission();

            return true;
        }

        private static void LoadCommands()
        {
            DatabaseLog.WriteProgressBar(">> Load GameCommands");

            using (SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM  GameCommands"))
            {
                using (ProgressBar mBar = new ProgressBar(pResult.Count))
                {
                    for (int i = 0; i < pResult.Count; i++)
                    {
                        var Command = new GameCommand(pResult, i);

                        if (!CommandsByID.TryAdd(Command.Id, Command))
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Dublicate CommandId {0} Found!");

                        mBar.Step();
                    }
                    DatabaseLog.WriteProgressBar(">> Loaded {0} GameCommands", CommandsByID.Count);
                }
            }
        }

        private static void LoadCommandsPermission()
        {
            DatabaseLog.WriteProgressBar(">> Load GameCommand_permissions");

            using (SQLResult pResult = DB.Select(DatabaseType.Data, "SELECT * FROM  GameCommand_permissions"))
            {
                using (ProgressBar mBar = new ProgressBar(pResult.Count))
                {
                    for (int i = 0; i < pResult.Count; i++)
                    {
                        mBar.Step();

                        int CommandId = pResult.Read<int>(i, "ID");
                        byte Role = pResult.Read<byte>(i, "RoleId");

                        if (!CommandsByID.TryGetValue(CommandId, out GameCommand Cmd))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Can not find CommandId {0] By Role {1}");
                            continue;
                        }

                        if (!RolesById.TryGetValue(Role, out GameRole GMRole))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Cant not find Role {0} with CommandId {1}", Role, CommandId);
                            continue;
                        }

                        if (!CommandsByCategory.TryGetValue(Cmd.CommandCategory.ToUpper(), out ConcurrentDictionary<string, GameCommand> CmdList))
                            CommandsByCategory.TryAdd(Cmd.CommandCategory.ToUpper(), new ConcurrentDictionary<string, GameCommand>());

                        if (!CommandsByCategory[Cmd.CommandCategory.ToUpper()].TryAdd(Cmd.Command.ToUpper(), Cmd))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Dublicate Commands Found Category : {0} Comand {1}", Cmd.CommandCategory, Cmd.Command);
                            continue;
                        }

                        if (!Cmd.RoleInfo.TryAdd(Role, GMRole))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Dublciate Role Found for Command {0}", Cmd.Id);
                            continue;
                        }
                    }

                    foreach (var cmd in CommandsByID.Values.Where(m => m.RoleInfo.Count == 0)) // command has role now inkoknitors command allowed :D
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "No Role found for Command {0} Category {1}", cmd, cmd.CommandCategory);
                    }

                    DatabaseLog.WriteProgressBar(">> Loaded {0} CommandCategorys ", CommandsByCategory.Count);
                }
            }
        }

        private static void LoadRolesFromDatabase()
        {
            SQLResult Result = DB.Select(DatabaseType.Data, "SELECT * FROM GameCommandRoles");

            DatabaseLog.WriteProgressBar(">> Load  GameCommandRoles");

            using (ProgressBar mBar = new ProgressBar(Result.Count))
            {
                for (int i = 0; i < Result.Count; i++)
                {
                    var Role = new GameRole(Result, i);

                    if (!RolesById.TryAdd(Role.Id, Role))
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Dublicate Role Id {0} found!", Role.Id);

                    mBar.Step();
                }
                DatabaseLog.WriteProgressBar(">> Loaded {0} GameCommandRoles", RolesById.Count);
            }
        }

        public static GameRole GetRole(byte Id) => RolesById.Values.Single(m => m.Id == Id);

        public static bool GetGameCommand(string Category, string Cmd, out GameCommand Command)
        {
            Command = null;
            if (CommandsByCategory.TryGetValue(Category, out ConcurrentDictionary<string, GameCommand> values))
            {
                if (values.TryGetValue(Cmd, out Command))
                    return true;
            }
            return false;
        }
    }
}