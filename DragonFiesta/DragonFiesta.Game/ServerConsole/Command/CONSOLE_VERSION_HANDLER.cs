#region

using DragonFiesta.Utils.Core;

#endregion

namespace DragonFiesta.Game.ServerConsole.Command
{
    [ConsoleCommandCategory("Version")]
    public static class CONSOLE_VERSION_HANDLER
    {
        [ConsoleCommand("Trigger")]
        public static bool CMD_TRIGGER_VERSION(string[] Params)
        {
            if (ServerMainDebug.TriggerVersion)
            {
                ServerMainDebug.TriggerVersion = false;
                CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Versions TriggerMode Off");
            }
            else
            {
                ServerMainDebug.TriggerVersion = true;
                CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Versions TriggerMode On Wil be next Triggert next Version Into Database");
            }
            return true;
        }
    }
}