using DragonFiesta.Game.Transfer;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps.Types;

namespace DragonFiesta.Zone.Game.Transfer
{
    public class ZoneTransfer : ServerTransferBase
    {

        public ushort WorldSessionId { get; set; }
        public ZoneCharacter Character { get; set; }

        public ZoneServerMap Map { get; set; }

        public Position SpawnPosition { get; set; }


        public override void OnExpire(GameTime gameTime)
        {
            if (ZoneServerTransferManager.FinishTransfer(WorldSessionId, out ZoneTransfer Transfer))
            {
                if (Transfer.Character.IsConnected)
                {
                    Character.Map = Transfer.Map;
                    Character.Position = Transfer.SpawnPosition;
                    Transfer.Character.Session.Dispose();
                }
            }
        }
        protected override void DisposeInternal()
        {
            Map = null;
            Character = null;
            base.DisposeInternal();

        }
    }
}
