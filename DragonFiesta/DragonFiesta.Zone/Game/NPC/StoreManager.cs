using DragonFiesta.Zone.Data.NPC;

namespace DragonFiesta.Zone.Game.NPC
{
    public class StoreManager : ItemNPCBase
    {
        public StoreManager(NPCInfo info) : base(info)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}