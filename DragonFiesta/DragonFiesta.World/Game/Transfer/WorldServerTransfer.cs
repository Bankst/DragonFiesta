using DragonFiesta.Game.Accounts;
using DragonFiesta.Game.Transfer;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.World.Game.Transfer
{
    public class WorldServerTransfer : ServerTransferBase
    {
        public Account Account { get; set; }

        public string IP { get; set; }

        public byte WorldId { get; set; }

        public override void OnExpire(GameTime gameTime)
        {
            if (WorldServerTransferManager.FinishTransfer(Account.ID, out WorldServerTransfer end))
            {
                GameLog.Write(GameLogLevel.Warning, "WorldTransfer timed out for AccountId {0}", Account.ID);
            }
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();

            Account = null;
        }
    }
}