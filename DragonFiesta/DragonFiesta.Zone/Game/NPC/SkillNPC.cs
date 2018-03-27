using DragonFiesta.Zone.Data.NPC;

namespace DragonFiesta.Zone.Game.NPC
{
    public class SkillNPC : ItemNPCBase
    {
        public SkillNPC(NPCInfo info) : base(info)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}
