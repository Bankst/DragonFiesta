using DragonFiesta.Login.Core;
using DragonFiesta.Login.Game.Accounts;
using DragonFiesta.Networking.Network.Session;
using System.Net.Sockets;

namespace DragonFiesta.Login.Network
{
    public class LoginSession : AccountSession<LoginSession>
    {
        public LoginSession(ClientRegion mRegion, Socket mSocket)
            : base(mRegion, mSocket)
        {
            OnDisconnect += ClearSession;
        }

        private static void ClearSession(object sender, SessionDisconnectArgs e)
        {
            if (e.Session is LoginSession Session)
            {
                if (Session.AccountIsLoggedIn)
                {
                    if (!Session.GameStates.IsTransferring)
                    {
                        Session.UserAccount.IsOnline = false;
                        AccountManager.UpdateAccountState(Session.UserAccount);
                    }
                }
            }

            ServerMain.InternalInstance.Title.Update();
        }
    }
}