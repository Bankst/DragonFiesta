using DragonFiesta.Networking.Network;
using DragonFiesta.Zone.Game.Character;
using System.Net.Sockets;
using DragonFiesta.Networking.Network.Session;

namespace DragonFiesta.Zone.Network
{
    public sealed class ZoneSession : FiestaSession<ZoneSession>
    {
        public bool IsAuthenticatet => (Character != null && GameStates.Authenticated);

        public bool IsLoggingOut { get; set; }

        public ZoneCharacter Character { get; set; }

        public bool IsCharacterLoggetIn() => (Character != null && Character.LoginInfo.IsOnline);

        public bool Ingame => (Character != null &&
            !GameStates.IsTransferring
            && GameStates.Authenticated
            && GameStates.IsReady);

        public ZoneSession(ClientRegion mRegio, Socket mSocket) : base(mRegio, mSocket)
        {
            OnDisconnect += ZoneSession_OnDisconnect;
        }

        private void ZoneSession_OnDisconnect(object sender, SessionDisconnectArgs e)
        {
            if (IsAuthenticatet &&
                !GameStates.IsTransferring)
            {
                ZoneCharacterManager.Instance.LogCharacterOut(Character, true);
            }
        }

        protected override void DisposeInternal()
        {
            Character?.Dispose();
            Character = null;

            base.DisposeInternal();
        }
    }
}