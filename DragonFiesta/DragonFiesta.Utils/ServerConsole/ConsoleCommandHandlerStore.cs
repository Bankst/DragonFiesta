using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonFiesta.Utils.ServerConsole
{
    [ServerModule(ServerType.Zone, InitializationStage.PreData)]
    [ServerModule(ServerType.World, InitializationStage.PreData)]
    [ServerModule(ServerType.Login, InitializationStage.PreData)]
    public class ConsoleCommandHandlerStore
    {
        //Category Command Function
        private static Dictionary<string, Dictionary<string, MethodInfo>> CategoryConsoleCommands;

        [InitializerMethod]
        public static bool Initialize()
        {
            try
            {
                CategoryConsoleCommands = Reflector.GiveCategoryConsoleMethods();
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Failed to Initialize Console Commands {0}", ex.ToString());
                return false;
            }
            return true;
        }

        public static bool GetCommandFromCategory(
            string CategoryName,
            string CommandText,
            out MethodInfo pMethod)
        {
            pMethod = null;
            if (!CategoryConsoleCommands.TryGetValue(CategoryName.ToUpper(), out Dictionary<string, MethodInfo> _Category)) return false;
            if (!_Category.TryGetValue(CommandText.ToUpper(), out pMethod)) return false;

            return true;
        }

        public static bool InvokeConsoleCommand(string input, params string[] args)
        {
            string cmdText = args[0].ToUpper();
            if (CategoryConsoleCommands.ContainsKey(cmdText))
            {
                if (args.Length >= 2)
                {
                    if (GetCommandFromCategory(cmdText, args[1], out MethodInfo pInfo))
                    {
                        return (bool)pInfo.Invoke(null, new object[] { args.Skip(2).ToArray() });
                    }
                }
            }
            return false;
        }
    }
}