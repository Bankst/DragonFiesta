using DragonFiesta.Networking.Network;
using DragonFiesta.World.Config;

namespace DragonFiesta.World.InternNetwork
{
    [ServerModule(ServerType.World, InitializationStage.PreData)]
    public class InternZoneSessionManager : InternSessionManagerBase<InternZoneSession>
    {
      

        public InternZoneSessionManager(ushort MaxSessions) : base(MaxSessions)
        {
        }

        [InitializerMethod]
        public static bool Initalized()
        {
            Instance = new InternZoneSessionManager(WorldConfiguration.Instance.InternalServerInfo.MaxConnection);
            return true;
        }
    }
}