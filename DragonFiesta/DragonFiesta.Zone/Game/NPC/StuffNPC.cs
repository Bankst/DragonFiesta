using DragonFiesta.Zone.Data.NPC;

namespace DragonFiesta.Zone.Game.NPC
{
    public class StuffNPC : ItemNPCBase
    {
        public StuffNPC(NPCInfo info) : base(info)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}