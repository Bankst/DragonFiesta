using DragonFiesta.Login.Config;
using DragonFiesta.Login.Network;

namespace DragonFiesta.Login.Game.Authentication
{
    public class LoginServer
    {
        public ClientRegion Region { get; private set; }
        public string ListeningIP { get; private set; }
        public int ListenPorts { get; private set; }

        private LoginListener mListener;

        public LoginServer(SQLResult pRes, int i)
        {
            Region = (ClientRegion)pRes.Read<byte>(i, "Region");
            ListeningIP = pRes.Read<string>(i, "ListenerIP");
            ListenPorts = pRes.Read<int>(i, "Port");
        }

        public bool Start()
        {
            try
            {
                if (LoginConfiguration.Instance.GameServerInfo.MaxConnection <= 0)
                    throw new StartupException("Invalid Max GameConnection Please Check you Config");

                mListener = new LoginListener(Region, ListenPorts);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Stop()
        {
            mListener.Stop();
        }

        ~LoginServer()
        {
            mListener = null;
            ListeningIP = null;
        }
    }
}