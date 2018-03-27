using DragonFiesta.Networking.Network.Session;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Accounts;
using System.Net.Sockets;

namespace DragonFiesta.World.Network
{
    public sealed class WorldSession : AccountSession<WorldSession>
    {
        public CharacterCollection CharacterList { get; set; }

        public WorldCharacter Character { get; set; }

        public bool IsCharacterLoggetIn() => (Character != null && Character.LoginInfo.IsOnline);

 

        public bool Ingame => (Character != null &&
            !GameStates.IsTransfering
            && GameStates.Authenticatet
            && GameStates.IsReady);

        public WorldSession(ClientRegion mRegio, Socket mSocket) : base(mRegio, mSocket)
        {
            CharacterList = new CharacterCollection(this);
           OnDispose += ClearSession;
        }

        private void ClearSession(object sender, SessionEventArgs e)
        {
            if (AccountIsLoggedIn)
            {
                UserAccount.IsOnline = false;

                AccountMethods.SendUpdateAccountStates(UserAccount);
            }

            if (IsCharacterLoggetIn())
            {
                WorldCharacterManager.Instance.LogCharacterOut(Character, true);
            }
        }

        protected override void DisposeInternal()
        {

            Character = null;

            base.DisposeInternal();
        }
    }
}