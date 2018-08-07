using DragonFiesta.Login.Config;
using DragonFiesta.Networking.Network.Managers;

namespace DragonFiesta.Login.Network
{
    [ServerModule(ServerType.Login, InitializationStage.InternNetwork)]
    public class LoginSessionManager : AccountSessionManager<LoginSession>
    {
        public static new LoginSessionManager Instance
        {
            get => _Instance as LoginSessionManager;
            set => _Instance = value;
        }

        public LoginSessionManager(ushort MaxSessions) : base(MaxSessions)
        {
        }

        [InitializerMethod]
        public static bool Initialized()
        {
            Instance = new LoginSessionManager(LoginConfiguration.Instance.GameServerInfo.MaxConnection);
            return true;
        }
    }
}