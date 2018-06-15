using DragonFiesta.Messages.Login.Transfer;
using DragonFiesta.Networking.Helpers;
using DragonFiesta.World.Config;
using DragonFiesta.World.Game.Transfer;
using DragonFiesta.World.Network;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Transfer
{
    public class WorldTransferHandler
    {
        [InternMessageHandler(typeof(AddWorldServerTransfer))]
        public static void HandledAdWorldServerTransfer(AddWorldServerTransfer mTransferMessage, InternLoginConnector pSession)
        {
            if (!WorldSessionManager.Instance.GetAccount(mTransferMessage.Account.ID, out WorldSession mWorldSession))
            {
                if (WorldConfiguration.Instance.WorldID == mTransferMessage.WorldId &&
                    WorldServerTransferManager.AddTransfer(new WorldServerTransfer
                    {
                        Account = mTransferMessage.Account,
                        IP = mTransferMessage.IP,
                        WorldId = mTransferMessage.WorldId,
                    }))
                {
                    mTransferMessage.Added = true;
                    pSession.SendMessage(mTransferMessage);
                }
            }
            else
            {
                mWorldSession.Character = null;
                _SH03Helpers.SendDuplicateLogin(mWorldSession);
                mWorldSession.Dispose();
            }
        }
    }
}