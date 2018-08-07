
using DragonFiesta.Utils.ServerTask;
using System;
using System.Collections.Concurrent;

namespace DragonFiesta.Login.Game.Transfer
{
    [ServerModule(ServerType.Login, InitializationStage.Logic)]
    public class LoginTransferManager
    {
        private static ExpireGuidManager Transfers { get; set; }

        private static ConcurrentDictionary<Guid, LoginServerTransfer> TransfersByGuid;

        [InitializerMethod]
        public static bool InitialTransferManager()
        {
            Transfers = new ExpireGuidManager((int)ServerTaskTimes.SERVER_LOGIN_TRANSFER_CHECK);
            TransfersByGuid = new ConcurrentDictionary<Guid, LoginServerTransfer>();

            ThreadPool.AddUpdateAbleServer(Transfers);

            return true;
        }

        public static bool AddTransfer(LoginServerTransfer ServerTransfer)
        {
            if (TransfersByGuid.TryAdd(ServerTransfer.Id, ServerTransfer))
            {
                Transfers.AddObject(ServerTransfer);
                return true;
            }
            return false;
        }

        public static bool FinishTransfer(Guid Id, out LoginServerTransfer LoginTransfer)
        {
            if (TransfersByGuid.TryRemove(Id, out LoginTransfer))
            {
                Transfers.RemoveObject(LoginTransfer.Id);
                return true;
            }

            return false;
        }
    }
}