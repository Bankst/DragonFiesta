using DragonFiesta.Game.Transfer;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Maps;

namespace DragonFiesta.World.Game.Transfer
{
    public class WorldMapTransfer : ServerTransferBase
    {
        public WorldCharacter Character { get; set; }

        public WorldServerMap Map { get; set; }


        public override void OnExpire(GameTime Now)
        {
            if (WorldServerTransferManager.FinishTransfer(Character.Info.CharacterID, out WorldMapTransfer end))
            {
                GameLog.Write(GameLogLevel.Warning, "MapTransfer timed out for CharacterId {0}", Character.Info.CharacterID);

                if (end.Character.IsConnected)
                {

                    end.Character.Session.Dispose();
                }
            }
        }

        ~WorldMapTransfer()
        {
            Dispose();
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();

            Character = null;
            Map = null;
        }
    }
}
