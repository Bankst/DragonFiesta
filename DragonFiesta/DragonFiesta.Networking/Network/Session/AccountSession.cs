using DragonFiesta.Game.Accounts;
using DragonFiesta.Networking.Network.Managers;
using System.Net.Sockets;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Networking.Network.Session
{
    public class AccountSession<TSession> : FiestaSession<TSession>
            where TSession : FiestaSession<TSession>
    {

        public bool AccountIsLoggedIn => (UserAccount != null);

	    public Account UserAccount { get; set; }

        public AccountSession(ClientRegion mRegion, Socket mSocket) :
            base(mRegion, mSocket)
        {
            OnDisconnect += AccountSession_OnDisconnect;
        }

        private void AccountSession_OnDisconnect(object sender, SessionDisconnectArgs e)
        {
	        if (!AccountIsLoggedIn) return;
	        if (AccountSessionManager<TSession>.Instance.RemoveAccount(UserAccount.ID, out TSession Session))
	        {
		        GameLog.Write(GameLogLevel.Debug, "Account {0} Disconnected", UserAccount.Name);
	        }
        }
        protected override void DisposeInternal()
        {
            UserAccount = null;

            base.DisposeInternal();
        }
    }
}
