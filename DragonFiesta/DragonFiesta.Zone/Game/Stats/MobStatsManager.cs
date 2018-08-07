using DragonFiesta.Game.Stats;
using DragonFiesta.Providers.Items;
using DragonFiesta.Zone.Game.Mobs;

namespace DragonFiesta.Zone.Game.Stats
{
    public sealed class MobStatsManager : StatsManager
    {
        public Mob Mob { get; private set; }
        public override StatsHolder BaseStats => Mob.Info.Stats;

        public MobStatsManager(Mob mob)
        {
            this.Mob = mob;
        }

        protected override void DisposeInternal()
        {
            Mob = null;
        }
    }
}