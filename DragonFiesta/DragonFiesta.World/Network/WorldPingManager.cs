using DragonFiesta.Networking;

namespace DragonFiesta.World.Network
{
    [GameServerModule(ServerType.World, GameInitalStage.Sync)]
    public class WorldPingManager : PingManager<WorldSession>
    {

        WorldPingManager() : base()
        {

        }

        [InitializerMethod]
        public static bool InitialPingManager()
        {
            Instance = new WorldPingManager();

            ServerTaskManager.AddObject(Instance);

            return true;
        }
    }
}
