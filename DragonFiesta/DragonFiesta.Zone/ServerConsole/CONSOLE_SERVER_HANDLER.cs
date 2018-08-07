using DragonFiesta.Zone.ServerConsole.Server;
using System.Linq;

namespace DragonFiesta.Zone.ServerConsole
{
    [ConsoleCommandCategory("Server")]
    public sealed class CONSOLE_SERVER_HANDLER
    {
        [ConsoleCommand("break")]
        public static bool shutdown_break(string[] Params)
        {
            string Reason = null;
            if (Params.Length > 0)
            {

                Reason = string.Join(" ", Params.ToArray());
            }


            return ZoneShutdownHandler.UnInitial(Reason);
        }

        [ConsoleCommand("shutdown")]
        public static bool CMD_SHUTOWN(string[] Params)
        {
            string Reason = "Shutdown by Console";
            int Seconds = 10;
            if (Params.Length > 0)
            {
                if (int.TryParse(Params[0], out Seconds))
                {
                    if (Params.Length > 1)
                    {
                        Reason = StringExtensions.ToString(Params.Skip(1).ToArray());
                    }
                }
            }

            if (ZoneShutdownHandler.IsInitialized())
                ZoneShutdownHandler.Update(Seconds, Reason);
            else
                ZoneShutdownHandler.Initialize(Seconds, Reason);

            return true;
        }
    }
}
