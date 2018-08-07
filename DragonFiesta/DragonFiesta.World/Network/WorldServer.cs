using DragonFiesta.Networking.Network;
using DragonFiesta.World.Config;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DragonFiesta.World.Network
{
    [GameServerModule(ServerType.World, GameInitalStage.Network)]
    public sealed class WorldServer : FiestaServer<WorldSession>
    {
        public static WorldServer Instance { get; set; }

        public WorldServer(ClientRegion Regio, int port) : base(Regio, port)
        {
        }

        public override async Task DoWork(Socket client)
        {
            var mSession = new WorldSession(Region, client);

            if (!WorldSessionManager.Instance.AllowConnect(mSession))
                await Task.Factory.StartNew(() => mSession.Dispose());
            else
                await Task.Factory.StartNew(() => mSession.Start());
        }


        [InitializerMethod]
        public static bool InitializeAsync()
        {
            try
            {
                if (WorldConfiguration.Instance.ServerInfo.MaxConnection <= 0)
                    throw new StartupException("Invalid Max GameConnection Please Check you Config");

                Instance = new WorldServer(WorldConfiguration.Instance.ServerRegion,
                    WorldConfiguration.Instance.ServerInfo.ListeningPort);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}