using DragonFiesta.Networking;

namespace DragonFiesta.Zone.Network
{
    [ServerModule(ServerType.Zone, InitializationStage.Sync)]
    public class ZonePingManager :
            PingManager<ZoneSession>
    {

        public ZonePingManager() : base()
        {

        }

        [InitializerMethod]
        public static bool InitialPingManager()
        {
            Instance = new ZonePingManager();

            ServerTaskManager.AddObject(Instance);

            return true;
        }
    }
}
