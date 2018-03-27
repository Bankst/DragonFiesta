using DragonFiesta.Networking.Network;
using DragonFiesta.World.Config;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DragonFiesta.World.InternNetwork
{
    [ServerModule(ServerType.World, InitializationStage.InternNetwork)]
    public class InternZoneServer : InternServer
    {
        public static InternZoneServer Instance { get; private set; }

        //Accepting ZoneClient
        public InternZoneServer(int port) : base(port)
        {
        }

        public override async Task DoWork(Socket mSocket)
        {


            var mSession = new InternZoneSession(mSocket);

            if (!InternZoneSessionManager.Instance.AddSession(mSession))
            {
                GameLog.Write(GameLogLevel.Internal, "Access Denied No More InternZoneConnection Awaiting");
                mSocket.Close();
            }

            await Task.Factory.StartNew(mSession.StartRecv);
        }

        [InitializerMethod]
        public static bool Initialized()
        {
            try
            {
                if (WorldConfiguration.Instance.InternalServerInfo.MaxConnection <= 0)
                    throw new StartupException("Invalid Max InternConnection Please Check you Config");

                Instance = new InternZoneServer(WorldConfiguration.Instance.InternalServerInfo.ListeningPort);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}