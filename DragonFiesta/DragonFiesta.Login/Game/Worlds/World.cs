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

        public bool IsConnected { get { return (Session != null && Session.IsReady); } }

        public bool IsReady { get { return IsConnected && _IsReady; } set { _IsReady = value; } }

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


                int total = OnlinePlayers * 100 / ConnectionInfo.MaxPlayers;

                if (total < 25)
                {
                    return WorldStatus.Low;
                }
                else if (total < 50)
                {
                    return WorldStatus.Medium;
                }
                else if (total >= 100)
                {
                    return WorldStatus.Full;
                }

                return WorldStatus.Hight;
            }
        }

        private bool _IsReady;

        public InternWorldSession Session { get; set; }

        public World(WorldInfo Info)
        {
            this.Info = Info;
        }

        public void WriteInfo(FiestaPacket pPacket)
        {
            pPacket.Write<byte>(Info.WorldID);
            pPacket.WriteString(Info.WorldName, 16);
            pPacket.Write<byte>(Status == WorldStatus.Reserved ? WorldStatus.Maintenance : Status == WorldStatus.Full ? WorldStatus.Hight : Status);
        }

        public void SendMessage(IMessage pMessage, bool AddCallback = true)
        {
            Session.SendMessage(pMessage);
        }

        public void Dispose()
        {

            ConnectionInfo = null;
            Session = null;
            _IsReady = false;
        }
    }
}