using DragonFiesta.Networking.Network;

namespace DragonFiesta.Zone.Network
{
    [ServerModule(ServerType.Zone, InitializationStage.InternNetwork)]
    public class ZoneSessionManager : FiestaSessionManagerBase<ZoneSession>
    {
        public ZoneSessionManager(ushort MaxSessions) : base(MaxSessions)
        {


        }

        [InitializerMethod]
        public static bool Initialize()
        {
            try
            {
                Instance = new ZoneSessionManager(ushort.MaxValue);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}