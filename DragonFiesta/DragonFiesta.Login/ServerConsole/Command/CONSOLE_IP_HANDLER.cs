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

            if (!IPAddress.TryParse(Params[0], out IPAddress IP))
            {
                CommandLog.Write(CommandLogLevel.InvalidParameters, "Is Not Valid IP Address");
                return true;
            }

            if (!IPBlockManager.GetIPBlockByIP(Params[0], out IPBlockEntry E))
            {
                CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "IP {0} is not blocked");
                return true;
            }

            if (IPBlockManager.RemoveIPBlock(Params[0]))
            {
                CommandLog.WriteConsoleLine(CommandLogLevel.InvalidParameters, "Removed IPBlock {0} Success!", Params[0]);
                return true;
            }

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

            if (!IPAddress.TryParse(Params[0], out IPAddress IP))
            {
                CommandLog.Write(CommandLogLevel.InvalidParameters, "Is Not Valid IP Address");
                return true;
            }

            string Reason = string.Empty;
            if (Params.Length == 1)
                Reason = "Banned by Console No Reason";
            else
                Reason = String.Join(" ", 2, Params.Length);

            if (IPBlockManager.GetIPBlockByIP(Params[0], out IPBlockEntry E))
            {
                CommandLog.Write(CommandLogLevel.InvalidParameters, "IP {0} is Already Blocked");
                return true;
            }
            if (IPBlockManager.BlockIP(Params[0], DateTime.Now, Reason))
            {
                CommandLog.Write(CommandLogLevel.Execute, "Blocked IP {0} Success!", Params[0]);
                return true;
            }

            return false;
        }
    }
}