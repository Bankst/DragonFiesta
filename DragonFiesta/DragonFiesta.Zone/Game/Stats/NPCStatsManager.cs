using DragonFiesta.Game.Stats;
using DragonFiesta.Zone.Game.NPC;

namespace DragonFiesta.Zone.Game.Stats
{
    public class NPCStatsManager : StatsManager
    {
        public NPCBase NPC { get; private set; }

        public override StatsHolder BaseStats
        {
            get { return NPC.Info.MobInfo.Stats; }
        }

        public NPCStatsManager(NPCBase NPC)
        {
            this.NPC = NPC;
        }

        protected override void DisposeInternal()
        {
            NPC = null;
        }
    }
}