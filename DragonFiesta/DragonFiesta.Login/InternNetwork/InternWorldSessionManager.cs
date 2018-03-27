using DragonFiesta.Networking.Network;

namespace DragonFiesta.Login.InternNetwork
{
    [ServerModule(ServerType.Login, InitializationStage.PreData)]
    public class InternWorldSessionManager : InternSessionManagerBase<InternWorldSession>
    {
        public InternWorldSessionManager(ushort MaxSessions) : base(MaxSessions)
        {
        }

        [InitializerMethod]
        public static bool Initialize()
        {
            Instance = new InternWorldSessionManager(ushort.MaxValue);
            return true;
        }
    }
}