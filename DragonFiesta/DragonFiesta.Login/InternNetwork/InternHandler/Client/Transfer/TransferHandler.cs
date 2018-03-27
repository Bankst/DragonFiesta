using DragonFiesta.Game.Accounts;
using DragonFiesta.Login.Game.Accounts;
using DragonFiesta.Login.Game.Transfer;
using DragonFiesta.Messages.World.Transfer;

namespace DragonFiesta.Login.InternNetwork.InternHandler.Client.Transfer
{
    public class TransferHandler
    {
        [InternMessageHandler(typeof(AddLoginServerTransfer))]
        public static void HandleAddLoginServerTransfer(AddLoginServerTransfer Request, InternWorldSession pSession)
        {
            if (AccountManager.GetAccountByID(Request.AccountId, out Account mAccount))
            {
                if (LoginTransferManager.AddTransfer(new LoginServerTransfer(Request.IP, Request.Id, mAccount)))
                {
                    Request.Added = true;
                }
            }

            pSession.SendMessage(Request);
        }
    }
}