using DragonFiesta.Utils.Core;

namespace DragonFiesta.Game.ServerConsole.Command
{
    public class CONSOLE_PACKET_HANDLER
    {
        [ConsoleCommandCategory("Packet")]
        public sealed class CONSOLE_SERVER_HANDLER
        {
            [ConsoleCommand("Dump")]
            public static bool CMD_Dump(string[] Params)
            {
                if (ServerMainDebug.DumpPacket)
                    ServerMainDebug.DumpPacket = false;
                else
                    ServerMainDebug.DumpPacket = true;

                CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Dump Packets {0} ", ServerMainDebug.DumpPacket);
                return true;
            }

            [ConsoleCommand("show")]
            public static bool CMD_SHOW(string[] Params)
            {
                if (ServerMainDebug.DebugPackets)
                    ServerMainDebug.DebugPackets = false;
                else
                    ServerMainDebug.DebugPackets = true;

                CommandLog.WriteConsoleLine(CommandLogLevel.Execute, "Debug Packets {0} ", ServerMainDebug.DebugPackets);
                return true;
            }
        }
    }
}