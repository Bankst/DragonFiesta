#region

using DragonFiesta.Providers.Items;

#endregion

namespace DragonFiesta.Game.Stats
{
    public interface iStatsChanger
    {
        StatsHolder Stats { get; }
    }
}