using DragonFiesta.Game.Objects;
using DragonFiesta.Login.Game.Worlds;
using DragonFiesta.Login.Network;
using DragonFiesta.Login.Network.InternHandler.Server;
using DragonFiesta.Messages.Transfer;
using System;
using System.Collections.Concurrent;

namespace DragonFiesta.Login.Game.Transfer
{
    [ServerModule(ServerType.Login, InitializationStage.Data)]
    public class AccountTransferManager
    {
        public static AccountTransferManager Instance { get; set; }
        private ExpireAbleManager TransferManager { get; set; }

        private ConcurrentDictionary<Guid, AccountTransfer> mTransfersGuid;

        public AccountTransferManager()
        {
            mTransfersGuid = new ConcurrentDictionary<Guid, AccountTransfer>();
            TransferManager = new ExpireAbleManager((int)ServerTaskTimes.ACCOUNT_TRANSFER_CHECK);
        }

        [InitializerMethod]
        public static bool Initialsize()
        {
            try
            {
                Instance = new AccountTransferManager();

                EngineLog.Write(EngineLogLevel.Startup, "AccountTransferManager initiasize");

                return true;

            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Failed Start WorldTransferManager", ex);
                return false;
            }
        }
        public bool AddTransfer(AccountTransfer mTransfer)
        {
            if (mTransfersGuid.TryAdd(mTransfer.TransferID, mTransfer))
            {
                TransferManager.AddObject(mTransfer);
                return true;

            }
            return false;
        }

        public bool AddTransfer(World pWorld, LoginSession pSession)
        {
            AccountTransfer mTransfer = new AccountTransfer
            {
                TransferID = Guid.NewGuid(),
                pSession = pSession,
                WorldId = pWorld.WorldID,
                ClientIP = pSession.Connection.GetClientInfo(),
                Region = pSession.Region,
            };

            if (AddTransfer(mTransfer))
            {
                NewAccountTransfer mTransferMessage = new NewAccountTransfer((int)MessageRequestTimeOuts.LOGIN_TRANSFER_ACCOUNT)
                {
                    Id = mTransfer.TransferID,
                    pAccount = pSession.UserAccount,
                    WorldID = pWorld.WorldID,
                    ClientIP = pSession.Connection.GetIP(),
                    Callback = ServerTransferMethods.TransferResponseHandler,
                    Region = pSession.Region,
                };

                pSession.IsTransfering = true;
                pWorld.SendMessage(mTransferMessage);

                return true;
            }

            return false;
        }

        public  bool FinishTransfer(Guid mGui,out AccountTransfer Transfer)
        {
            if (mTransfersGuid.TryRemove(mGui, out Transfer))
            {
                TransferManager.RemoveObject(Transfer);

                return true;
            }

            return false;
        }
    }
}
