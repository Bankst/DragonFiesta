using DragonFiesta.Game.Accounts;
using DragonFiesta.Login.Game.Accounts;
using DragonFiesta.Login.InternNetwork;
using DragonFiesta.Messages.Accounts;
using DragonFiesta.Messages.Login.Accounts;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Login.Network.InternHandler.Client
{
    public class InternAccountHandler
    {
        [InternMessageHandler(typeof(Account_RequestById))]
        public static void HandleAccoun_Request(Account_RequestById mAccountMSG, InternWorldSession pSession)
        {

            Account DBAccount = null;
            if (AccountManager.GetAccountByID(mAccountMSG.AccountId, out DBAccount))
            {
                GameLog.Write(GameLogLevel.Debug, "Requesting Account {0}", DBAccount.Name);
            }

            pSession.SendMessage(new Account_Response
            {

                Id = mAccountMSG.Id,
                Account = DBAccount
            }, false);
        }

        [InternMessageHandler(typeof(Account_RequestByName))]
        public static void HandleAccoun_Request(Account_RequestByName mAccountMSG, InternWorldSession pSession)
        {

            Account DBAccount = null;
            if (AccountManager.GetAccountByName(mAccountMSG.AccountName, out DBAccount))
            {
                GameLog.Write(GameLogLevel.Debug, "Requesting Account {0}", DBAccount.Name);
            }

            pSession.SendMessage(new Account_Response
            {

                Id = mAccountMSG.Id,
                Account = DBAccount
            }, false);
        }
        [InternMessageHandler(typeof(UpdateAccountState))]
        public static void HandleUpdateAccountState(UpdateAccountState mAccountMSG, InternWorldSession pSession)
        {
            if (!AccountManager.GetAccountByID(mAccountMSG.pAccount.ID, out Account DBAccount))
                return;

            AccountManager.UpdateAccountState(mAccountMSG.pAccount);
        }

        [InternMessageHandler(typeof(AccountUpdate))]
        public static void HandleAccountUpdate(AccountUpdate mAccountMSG, InternWorldSession pSession)
        {
            if (!AccountManager.GetAccountByID(mAccountMSG.Account.ID, out Account DBAccount))
                return;


            AccountManager.UpdateAccount(mAccountMSG.Account);

            pSession.SendMessage(mAccountMSG);
        }
    }
}