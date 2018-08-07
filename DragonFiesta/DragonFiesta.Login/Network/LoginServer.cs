using DragonFiesta.Networking.Network;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace DragonFiesta.Login.Network
{
    public class LoginListener : FiestaServer<LoginSession>
    {
        public LoginListener(ClientRegion Region, int port) : base(Region, port)
        {
        }
        public override async Task DoWork(Socket client)
        {
            var mSession = new LoginSession(Region, client);
            if (!LoginSessionManager.Instance.AllowConnect(mSession))
                await Task.Factory.StartNew(() => mSession.Dispose());
            else
                await Task.Factory.StartNew(() => mSession.Start());
        }
    }
}