using DragonFiesta.Login.Config;
using DragonFiesta.Login.Network;
using DragonFiesta.Networking.Network;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DragonFiesta.Login.InternNetwork
{
    [ServerModule(ServerType.Login, InitializationStage.Networking)]
    public class InternWorldServer : InternServer
    {
        private static InternWorldServer Instance { get; set; }

        public InternWorldServer(int port) : base(port)
        {
        }

        public override async Task DoWork(Socket mSocket)
        {
            if (InternWorldSessionManager.Instance.CountOfSessions >=
                LoginConfiguration.Instance.InternServerInfo.MaxConnection) { mSocket.Close(); return; }

            var mSession = new InternWorldSession(mSocket);

            if (!AllowConnect(mSocket) || !InternWorldSessionManager.Instance.AddSession(mSession)) mSocket.Close();

            await Task.Factory.StartNew(mSession.StartRecv);
        }

        protected bool AllowConnect(Socket RemoteSocket) => !IPBlockManager.GetIPBlockByIP(((IPEndPoint)(RemoteSocket.RemoteEndPoint)).Address.ToString(), out IPBlockEntry entry);

        [InitializerMethod]
        public static bool Initialize()
        {
            try
            {
                if (LoginConfiguration.Instance.InternServerInfo.MaxConnection == 0)
                    throw new StartupException("Invalid Max InternConnection Please Check your Config");

                Instance = new InternWorldServer(
                    LoginConfiguration.Instance.InternServerInfo.ListeningPort);

                return true;
            }
            catch
            {
                return false;
            }
        }

        [Shutdown(ShutdownType.Intern)]
        private static void Shutdown()
        {
            Instance?.Stop();
        }
    }
}