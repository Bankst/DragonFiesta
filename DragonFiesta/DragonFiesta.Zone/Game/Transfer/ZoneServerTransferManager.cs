
using DragonFiesta.Utils.ServerTask;
using System.Collections.Concurrent;

namespace DragonFiesta.Zone.Game.Transfer
{
    [GameServerModule(ServerType.Zone, GameInitalStage.Logic)]
    public class ZoneServerTransferManager
    {
        private static ExpireAbleManager TransferObjects;
        private static ConcurrentDictionary<ushort, ZoneTransfer> TransfersByWorldSessionId;

        [InitializerMethod]
        public static bool InitialZoneTransferServer()
        {
            TransfersByWorldSessionId = new ConcurrentDictionary<ushort, ZoneTransfer>();
            TransferObjects = new ExpireAbleManager((int)ServerTaskTimes.SERVER_ZONE_TRANSFER_CHECK);

            ThreadPool.AddUpdateAbleServer(TransferObjects);

            return true;
        }

        public static bool AddTransfer(ZoneTransfer ServerTransfer)
        {
            if (TransfersByWorldSessionId.TryAdd(ServerTransfer.WorldSessionId, ServerTransfer))
            {
                TransferObjects.AddObject(ServerTransfer);
                return true;
            }
            return false;
        }

        public static bool FinishTransfer(ushort WorldSessionId, out ZoneTransfer WorldTransfer)
        {
            if (TransfersByWorldSessionId.TryRemove(WorldSessionId, out WorldTransfer))
            {
                TransferObjects.RemoveObject(WorldTransfer);
                return true;
            }
            return false;
        }
    }
}