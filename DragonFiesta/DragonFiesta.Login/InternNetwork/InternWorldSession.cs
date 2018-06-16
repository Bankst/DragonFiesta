using DragonFiesta.Login.Core;
using DragonFiesta.Login.Game.Worlds;
using DragonFiesta.Messages.Message.Auth;
using DragonFiesta.Networking.Network;
using System.Net.Sockets;

namespace DragonFiesta.Login.InternNetwork
{
    public class InternWorldSession : InternSession
    {
        public World World { get; set; }

        public bool HasWorld { get => World != null; }

        public InternWorldSession(Socket mSocket) : base(mSocket)
        {

            OnDispose += ClearSession;
        }

        private void ClearSession(object sender, SessionEventArgs e)
        {
            if (HasWorld)
            {
                EngineLog.Write(EngineLogLevel.Info, "Disconnect World {0}", World.Info.WorldID);
            }

            InternWorldSessionManager.Instance.RemoveSession(BaseStateInfo.SessionId);

            ServerMain.InternalInstance.Title.Update();
        }

        protected override void HandleMessage(IMessage pMessage)
        {
            if (!SessionStateInfo.Authenticated && !(pMessage is AuthenticatedWorld))
                Dispose();
            else
                base.HandleMessage(pMessage);
        }

        protected override void DisposeInternal()
        {
            World.Dispose();
            World = null;

            base.DisposeInternal();
        }
    }
}