using DragonFiesta.Utils.ServerTask;
using System.Collections.Concurrent;

namespace DragonFiesta.World.Game.Transfer
{
    [GameServerModule(ServerType.World, GameInitalStage.Logic)]
    public class WorldServerTransferManager
    {
        private static ConcurrentDictionary<int, WorldServerTransfer> TransfersByAccountId;
        private static ConcurrentDictionary<int, WorldMapTransfer> MapTransfersByCharacterId;

        private static ExpireAbleManager TransferObjects;

        [InitializerMethod]
        public static bool InitialWorldTransferServer()
        {
            TransfersByAccountId = new ConcurrentDictionary<int, WorldServerTransfer>();
            MapTransfersByCharacterId = new ConcurrentDictionary<int, WorldMapTransfer>();

            TransferObjects = new ExpireAbleManager((int)ServerTaskTimes.SERVER_WORLD_TRANSFER_CHECK);

            ThreadPool.AddUpdateAbleServer(TransferObjects);

            return true;
        }


        public static bool AddTransfer(WorldMapTransfer MapTransfer)
        {
            if (MapTransfersByCharacterId.TryAdd(MapTransfer.Character.Info.CharacterID, MapTransfer))
            {
                TransferObjects.AddObject(MapTransfer);
                return true;
            }
            return false;
        }

        public static bool AddTransfer(WorldServerTransfer ServerTransfer)
        {
            if (TransfersByAccountId.TryAdd(ServerTransfer.Account.ID, ServerTransfer))
            {
                TransferObjects.AddObject(ServerTransfer);
                return true;
            }
            return false;
        }

        public static bool FinishTransfer(int CharacterID, out WorldMapTransfer MapTransfer)
        {
            if (MapTransfersByCharacterId.TryRemove(CharacterID, out MapTransfer))
            {
                TransferObjects.RemoveObject(MapTransfer);
                return true;
            }

            return false;
        }
        public static bool FinishTransfer(int AccountId, out WorldServerTransfer WorldTransfer)
        {
            if (TransfersByAccountId.TryRemove(AccountId, out WorldTransfer))
            {
                TransferObjects.RemoveObject(WorldTransfer);
                return true;
            }

            return false;
        }
    }
}