using DragonFiesta.Game.Accounts;
using DragonFiesta.Messages.Accounts;
using DragonFiesta.Messages.Login.Accounts;
using System;

namespace DragonFiesta.World.InternNetwork.InternHandler.Server.Accounts
{
    public class AccountMethods
    {

        public static void SendAccountRequestById(int AccountId,
            Action<IMessage> CallBack)
        {
            Account_RequestById Ban = new Account_RequestById
            {
                Id = Guid.NewGuid(),
                AccountId = AccountId,
                Callback = CallBack,
            };
            InternLoginConnector.Instance.SendMessage(Ban);
        }
        public static void SendAccountRequestByName(
            string AccountName,
            Action<IMessage> CallBack)
        {
            Account_RequestByName Ban = new Account_RequestByName
            {
                Id = Guid.NewGuid(),
                AccountName = AccountName,
                Callback = CallBack,
            };
            InternLoginConnector.Instance.SendMessage(Ban);
        }

        public static void SendUpdateAccountStates(Account mAccount)
        {
            UpdateAccountState StateMessage = new UpdateAccountState()
            {
                Id = Guid.NewGuid(),
                pAccount = mAccount,
            };
            InternLoginConnector.Instance.SendMessage(StateMessage);
        }

        public static void SendUpdateAccount(Account mAccount,Action<IMessage> ResultCallBack)
        {
            AccountUpdate UpdateAccount = new AccountUpdate()
            {
                Id = Guid.NewGuid(),
                Account = mAccount,
                 Callback = ResultCallBack,
            };
            InternLoginConnector.Instance.SendMessage(UpdateAccount);
        }
    }
}