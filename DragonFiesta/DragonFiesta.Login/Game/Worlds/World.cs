using DragonFiesta.Game.Worlds;
using DragonFiesta.Login.Data;
using DragonFiesta.Login.InternNetwork;

namespace DragonFiesta.Login.Game.Worlds
{
    public class World
    {

        public int OnlinePlayers { private get; set; }
        public WorldConnectionInfo ConnectionInfo { get; set; }
        public WorldInfo Info { get; private set; }

        public bool IsConnected => (Session != null && Session.IsReady);

	    public bool IsReady { get => IsConnected && _isReady;
		    set => _isReady = value;
	    }

        public WorldStatus Status
        {
            get
            {
                if (Session == null)
                    return WorldStatus.Offline;

                if (Session != null && !Session.IsConnected)
                    return WorldStatus.EmptyServer;

                if (!IsReady)
                    return WorldStatus.Maintenance;

                if (Info.IsTestServer)
                    return WorldStatus.Reserved;

                var total = OnlinePlayers * 100 / ConnectionInfo.MaxPlayers;

                if (total < 25)
                {
                    return WorldStatus.Low;
                }

	            if (total < 50)
	            {
		            return WorldStatus.Medium;
	            }

	            return total >= 100 ? WorldStatus.Full : WorldStatus.High;
            }
        }

        private bool _isReady;

        public InternWorldSession Session { get; set; }

        public World(WorldInfo info)
        {
            this.Info = info;
        }

        public void WriteInfo(FiestaPacket pPacket)
        {
            pPacket.Write<byte>(Info.WorldID);
            pPacket.WriteString(Info.WorldName, 16);
            pPacket.Write<byte>(Status == WorldStatus.Reserved ? WorldStatus.Maintenance : Status == WorldStatus.Full ? WorldStatus.High : Status);
        }

        public void SendMessage(IMessage pMessage, bool addCallback = true)
        {
            Session.SendMessage(pMessage);
        }

        public void Dispose()
        {

            ConnectionInfo = null;
            Session = null;
            _isReady = false;
        }
    }
}