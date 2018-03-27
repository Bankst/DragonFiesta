using DragonFiesta.Game.ServerConsole.Handler;
using System.Linq;

namespace DragonFiesta.Utils.Module.ServerConsole.Command
{
    [ConsoleCommandCategory("Server")]
    public sealed class CONSOLE_SERVER_HANDLER
    {
        [ConsoleCommand("Performance")]
        public static bool CMD_PERFORMANCE(string[] Params)
        {
            CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Now Tasks Perfomance {0} Per Seconds", ThreadPool.PerfomanceCount);
            return true;
        }
    }
}