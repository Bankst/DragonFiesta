using DragonFiesta.Networking.Network;
using DragonFiesta.Zone.Config;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DragonFiesta.Zone.Network
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Network)]
    public sealed class ZoneServer : FiestaServer<ZoneSession>
    {
        public static ZoneServer Instance { get; set; }

        public ZoneServer(ClientRegion Regio, int port) : base(Regio, port)
        {
        }

        public override async Task DoWork(Socket client)
        {
            var mSession = new ZoneSession(Region, client);

            if (!ZoneSessionManager.Instance.AllowConnect(mSession))
                await Task.Factory.StartNew(() => mSession.Dispose());
            else
                await Task.Factory.StartNew(() => mSession.Start());
        }

        public static bool Start(ClientRegion Region)
        {
            try

            {
                if (ZoneConfiguration.Instance.ZoneServerInfo.MaxConnection <= 0)
                    throw new StartupException("Invalid Max InternConnection Please Check you Config");
                Instance = new ZoneServer(
                    Region,
                    ZoneConfiguration.Instance.ZoneServerInfo.ListeningPort);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Shutdown()
        {
            Instance?.Stop();
        }
    }
}