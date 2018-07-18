using DragonFiesta.Login.Network;
using System;
using System.Net;

namespace DragonFiesta.Login.ServerConsole
{
    [ConsoleCommandCategory("IP")]
    public class CONSOLE_IP_HANDLER
    {
        [ConsoleCommand("Unblock")]
        public static bool CMD_IP_UNBLOCK(string[] Params)
        {
            if (Params.Length <= 0)
            {
                CommandLog.Write(CommandLogLevel.InvalidParameters, "Invalid Command use IP Block <IPAddress>");
                return true;
            }

            if (!IPAddress.TryParse(Params[0], out var ip))
            {
                CommandLog.Write(CommandLogLevel.InvalidParameters, $"{ip} Is Not Valid IP Address");
                return true;
            }

            if (!IPBlockManager.GetIPBlockByIP(Params[0], out var e))
            {
                CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, $"IP {e.IP} is not blocked");
                return true;
            }

	        if (!IPBlockManager.RemoveIPBlock(Params[0])) return true;
	        CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Removed IPBlock {0} Successfully!", Params[0]);
	        return true;

        }

        [ConsoleCommand("Block")]
        public static bool CMD_IP_HANDLER(string[] Params)
        {
            if (Params.Length <= 0)
            {
                CommandLog.Write(CommandLogLevel.InvalidParameters, "Invalid Command use IP Block <IPAddress> <Reason>");
                return true;
            }

            if (!IPAddress.TryParse(Params[0], out var ip))
            {
                CommandLog.Write(CommandLogLevel.InvalidParameters, $"{ip} Is Not Valid IP Address");
                return true;
            }

	        var reason = Params.Length == 1 ? "Banned by Console No Reason" : string.Join(" ", 2, Params.Length);

            if (IPBlockManager.GetIPBlockByIP(Params[0], out var e))
            {
                CommandLog.Write(CommandLogLevel.InvalidParameters, $"IP {e.IP} is Already Blocked");
                return true;
            }

	        if (!IPBlockManager.BlockIP(Params[0], DateTime.Now, reason)) return false;
	        CommandLog.Write(CommandLogLevel.Execute, $"Blocked IP {Params[0]} Successfully!");
	        return true;

        }
    }
}