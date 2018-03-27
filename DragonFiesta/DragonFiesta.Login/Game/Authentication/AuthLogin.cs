using DragonFiesta.Game.Accounts;
using DragonFiesta.Login.Core;
using DragonFiesta.Login.Game.Accounts;
using DragonFiesta.Login.Network;
using DragonFiesta.Login.Network.InternHandler.Server;
using DragonFiesta.Utils.Core;
using DragonFiesta.Utils.ServerTask;
using System;
using System.Threading;

namespace DragonFiesta.Login.Game.Authentication
{
    public class AuthLogin : iExpireAble
    {
        public static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(2);

        private DateTime ExpireTime;
        DateTime iExpireAble.ExpireTime { get { return ExpireTime; } }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;

        private LoginGameError Error { get; set; } = LoginGameError.UNKOWN_ERROR;

        private ushort SessionId { get; set; }

        public AuthLogin(ushort SessionId)
        {
            ExpireTime = ServerMainBase.InternalInstance.CurrentTime.Time.Add(DefaultTimeout);
        }

        public LoginGameError AuthAccount(LoginSession pClient, string username, string Password)
        {
            Error = LoginGameError.UNKOWN_ERROR;

            if (!AccountManager.GetAccountByName(username, out Account account))
            {
                return LoginGameError.INVALID_ID_OR_PW;
            }

            if (!LoginSessionManager.Instance.AddAccount(account.ID, pClient))
            {
                return LoginGameError.LOGIN_FAILED;
            }
            else if (account.IsOnline)
            {
                ServerAccountMethods.SendDublicateLogin(account.ID);
                return LoginGameError.LOGIN_FAILED;
            }

            if (!account.Password.Equals(Password))
            {
                return LoginGameError.INVALID_ID_OR_PW;
            }

            if (!account.IsActivated)
            {
                return LoginGameError.AGREEMENT_MISSING;
            }

            if (account.IsBanned && account.BanTime <= 0)//is now banned also block login
            {
                return LoginGameError.BLOCKED;
            }
            else if (!account.IsBanned && account.BanTime >= 0)//ban is expiret also unblock full
            {
                account.BanTime = 0;
                account.BanDate = DateTime.Now;
                AccountManager.UpdateAccount(account);
            }


            account.LastLogin = ServerMain.InternalInstance.CurrentTime.Time;
            pClient.UserAccount = account;
            account.LastIP = pClient.GetIP();
            account.IsOnline = true;
            AccountManager.UpdateAccountState(account);

            return LoginGameError.None; //Auth OK :)
        }

        public void OnExpire(GameTime Now)
        {
            if (LoginManager.Instance.TryGetLogin(SessionId, out AuthLogin Login))
            {
                Dispose();
            }
        }

        public void Update(GameTime Now)
        {
        }

        public void Dispose() => Interlocked.CompareExchange(ref IsDisposedInt, 1, 0);

        protected void DisposeInternal()
        {
        }
    }
}