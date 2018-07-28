using DragonFiesta.Networking.Network;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Zone;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Zone;
using System.Net.Sockets;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.World.InternNetwork
{
    public class InternZoneSession : InternSession
    {
        public ZoneServer Zone { get; set; }

        public bool IsAuthenticated { get { return (Zone != null); } }

        public InternZoneSession(Socket mSocket) : base(mSocket)
        {
           OnDispose += ClearSession;
        }

        private void ClearSession(object sender, SessionEventArgs e)
        {
            if (IsAuthenticated)
            {
                ZoneMethods.SendZoneStopt(Zone.ID);

                MapManager.StopMapsByZoneId(Zone.ID);

                Zone.IsReady = false;

                GameLog.Write(GameLogLevel.Internal, $"Zone disconnected (ID: {Zone.ID}");
                ServerMain.InternalInstance.Title.Update();
            }

            InternZoneSessionManager.Instance.RemoveSession(BaseStateInfo.SessionId);
            ServerMain.InternalInstance.Title.Update();
        }

        public override void SendMessage(IMessage pMessage, bool AddCallBack = true)
        {
            base.SendMessage(pMessage, AddCallBack);
        }

        protected override void DisposeInternal()
        {
            Zone?.Dispose();
            Zone = null;

            base.DisposeInternal();
        }
    }
}