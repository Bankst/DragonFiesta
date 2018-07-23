#region

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonFiesta.Game.CommandAccess;

#endregion

namespace DragonFiesta.Game.Command
{
    [GameServerModule(ServerType.World, GameInitalStage.Command)]
    [GameServerModule(ServerType.Zone, GameInitalStage.Command)]
    public class GameCommandStore
    {
        [InitializerMethod]
        public static bool InitialStore()
        {
            LoadReflectionCommands();
            return true;
        }

        private static void LoadReflectionCommands()
        {
            IEnumerable<Pair<string, Pair<List<Attributes.GameCommandAttribute>, MethodInfo>>> methods = from t in Reflector.Global.GetTypesWithAttribute<GameCommandCategory>()
                                                                                                         from m in Reflector.GetMethodsFromTypeWithAttributes<Attributes.GameCommandAttribute>(t.Item2)
                                                                                                         where m != null
                                                                                                         select new Pair<string, Pair<List<Attributes.GameCommandAttribute>, MethodInfo>>(t.Item1.Category, m);

            foreach (var Atr in methods)
            {
                string CmdCategory = Atr.Item1.ToUpper();

                if (!GameCommandManager.CommandsByCategory.TryGetValue(CmdCategory, out ConcurrentDictionary<string, GameCommand> CommandList))
                {
                    DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find Category in Database : {0}", CmdCategory);
                    continue;
                }

                foreach (var cmd in Atr.Item2.Item1)
                {
                    if (!GameCommandManager.CommandsByCategory[CmdCategory].TryGetValue(cmd.Command.ToUpper(), out GameCommand Command))
                    {
                        DatabaseLog.Write(DatabaseLogLevel.Warning, "Can't find Command : {0} Category : {1} Database", cmd.Command, CmdCategory);
                        continue;
                    }

                    Command.Method = Atr.Item2.Item2;//Settings Method to Command
                    Command.Type = cmd.CmdType;
                }
            }
        }
    }
}