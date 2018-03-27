using DragonFiesta.Zone.Data.NPC;

namespace DragonFiesta.Zone.Game.NPC
{
    public class QuestNPC : ItemNPCBase
    {
        public QuestNPC(NPCInfo info) : base(info)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}