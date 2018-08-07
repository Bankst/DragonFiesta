using DragonFiesta.Networking.Network.Managers;

namespace DragonFiesta.World.Network
{
    [ServerModule(ServerType.World, InitializationStage.InternNetwork)]
    public class WorldSessionManager : AccountSessionManager<WorldSession>
    {
        public static new WorldSessionManager Instance
        {
            get => _Instance as WorldSessionManager;
            set => _Instance = value;
        }
        public WorldSessionManager(ushort MaxSessions) : base(MaxSessions)
        {
        }



        [InitializerMethod]
        public static bool Initialize()
        {
            try
            {
                Instance = new WorldSessionManager(ushort.MaxValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}