using DragonFiesta.Game.Stats;
using DragonFiesta.Providers.Items;
using DragonFiesta.Zone.Game.NPC;

namespace DragonFiesta.Zone.Game.Stats
{
    public class NPCStatsManager : StatsManager
    {
        public NPCBase NPC { get; private set; }

        public override StatsHolder BaseStats => NPC.Info.MobInfo.Stats;

	    public NPCStatsManager(NPCBase npc)
        {
            this.NPC = npc;
        }

        protected override void DisposeInternal()
        {
            NPC = null;
        }
    }
}