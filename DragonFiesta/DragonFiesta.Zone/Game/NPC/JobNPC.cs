using DragonFiesta.Zone.Data.NPC;

namespace DragonFiesta.Zone.Game.NPC
{
    public class JobNPC : ItemNPCBase
    {
        public JobNPC(NPCInfo info) : base(info)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}