using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;
using System.Linq;
using DragonFiesta.Zone.ServerConsole.Server;

namespace DragonFiesta.Zone.Game.Command
{
    [GameServerModule(ServerType.World, GameInitalStage.Map)]
    [GameCommandCategory("Zone")]
    public class ZONE_COMMAND_HANDLER
    {
        [ZoneCommand("Perform")]
        public static bool Perform(ZoneCharacter Sender, string[] Parameters)
        {
            ZoneChat.CharacterNote(Sender, $"Zone Has Been {ThreadPool.PerfomanceCount} Tasks/Calls Per Seconds");
            return true;
        }

        [ZoneCommand("Break")]
        public static bool shutdown_Break(ZoneCharacter Sender, string[] Params)
        {
            string Reason = null;
            if (Params.Length > 0)
            {

                Reason = string.Join(" ", Params.ToArray());
            }

            if (!ZoneShutdownHandler.IsInitialized())
            {
                ZoneChat.CharacterNote(Sender, "First initial Shutdown bevor use this command");
                return true;
            }

            ZoneShutdownHandler.UnInitial(Reason);

            return true;
        }

        [ZoneCommand("Shutdown")]
        public static bool Shutdown(ZoneCharacter Sender, string[] Params)
        {
            string Reason = "No Reason";
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
