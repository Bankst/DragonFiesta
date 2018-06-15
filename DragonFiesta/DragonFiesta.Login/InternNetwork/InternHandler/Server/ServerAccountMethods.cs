using DragonFiesta.Game.Accounts;
using DragonFiesta.Login.InternNetwork;
using DragonFiesta.Login.InternNetwork.InternHandler.Response.Account;
using DragonFiesta.Messages.Accounts;
using System;

namespace DragonFiesta.Login.Network.InternHandler.Server
{
    public class ServerAccountMethods
    {
        public static void SendAccountUpdate(Account pAccount)
        {
            AccountUpdate Update = new AccountUpdate()
            {
                Id = Guid.NewGuid(),
                Account = pAccount,
            };
            InternWorldSessionManager.Instance.Broadcast(Update);
        }

        public static void SendAccountDelete(Account pAccount)
        {
            AccountDelete mDeleteAccount = new AccountDelete(1000)
            {
                Id = Guid.NewGuid(),
                AccountId = pAccount.ID,
                Callback = Account_Response.DeleteResponse,
            };

            InternWorldSessionManager.Instance.Broadcast(mDeleteAccount);
        }

        public static void SendDuplicateLogin(int AccountId)
        {
            DuplicateLoginFound Message = new DuplicateLoginFound
            {
                Id = Guid.NewGuid(),
                AccountID = AccountId,
            };

            InternWorldSessionManager.Instance.Broadcast(Message);
        }
    }
}