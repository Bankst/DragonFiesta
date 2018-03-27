using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.ServerConsole.Server;
using System.Linq;

namespace DragonFiesta.World.Game.Command
{
    [GameServerModule(ServerType.World, GameInitalStage.Map)]
    [GameCommandCategory("World")]
    public class SERVER_COMMAND_HANDLER
    {
        [WorldCommand("Perform")]
        public static bool Perform(WorldCharacter Sender, string[] Parameters)
        {
            ZoneChat.CharacterNote(Sender, $"World Has Been {ThreadPool.PerfomanceCount} Tasks/Calls Per Seconds");
            return true;
        }
        [WorldCommand("Break")]
        public static bool shutdown_Break(WorldCharacter Sender, string[] Params)
        {
            string Reason = null;
            if (Params.Length > 0)
            {

                Reason = string.Join(" ", Params.ToArray());
            }

            if(!WorldShutdownHandler.IsInitialized())
            {
                ZoneChat.CharacterNote(Sender, "First initial Shutdown bevor use this command");
                return true;
            }

            WorldShutdownHandler.UnInitial(Reason);

            return true;
        }
        [WorldCommand("Shutdown")]
        public static bool Shutdown(WorldCharacter Sender, string[] Params)
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

            if (WorldShutdownHandler.IsInitialized())
                WorldShutdownHandler.Update(Seconds, Reason);
            else
                WorldShutdownHandler.Initialize(Seconds, Reason);


            return true;
        }


        

    }
}
