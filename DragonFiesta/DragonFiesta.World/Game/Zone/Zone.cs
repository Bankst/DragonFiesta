using DragonFiesta.Game.Zone;
using DragonFiesta.Utils.Config.Section.Network;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.InternNetwork;
using System;
using System.Runtime.Serialization;

namespace DragonFiesta.World.Game.Zone
{
    [Serializable]
    public class ZoneServer : IZone, IDisposable
    {
        public byte ID { get; private set; }

        public ServerInfo NetInfo { get; set; }

        public bool IsFull => (CurrentConnection >= NetInfo.MaxConnection);

        public SecureCollection<WorldServerMap> MapList;

        public bool IsConnected { get => (Session != null); }

        private InternZoneSession Session;

        public bool IsReady { get { return IsConnected && _IsReady; } set { _IsReady = value; } }

        private bool _IsReady;

        bool IZone.IsReady { get { return IsReady; } }

        private object ThreadLocker;

        public int CurrentConnection { get; set; }

        public ZoneServer(byte ID, ServerInfo NetInfo)
        {
            this.ID = ID;
            this.NetInfo = NetInfo;
            ThreadLocker = new object();
            MapList = new SecureCollection<WorldServerMap>();
        }

        public void Dispose()
        {
            MapList.Clear();
            IsReady = false;
            Session = null;
        }

        public void SetClient(InternZoneSession NewSession)
        {
            lock (ThreadLocker)
            {
                IsReady = false;
                Session = NewSession;
                Session.Zone = this;
            }
            GameLog.Write(GameLogLevel.Internal, "Zone connected. Assigned ID '{0}'.", ID);
        }

        public void Send(IMessage msg, bool AddCallBack = true)
        {
            if (IsConnected)
            {
                Session.SendMessage(msg, AddCallBack);
            }
        }

        protected ZoneServer(SerializationInfo info, StreamingContext context)
        {
            ID = info.GetByte("ID");
            NetInfo = (ServerInfo)info.GetValue("NetInfo", typeof(ServerInfo));
            _IsReady = info.GetBoolean("IsReady");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", ID);
            info.AddValue("NetInfo", this.NetInfo);
            info.AddValue("IsReady", IsReady);
        }
    }
}