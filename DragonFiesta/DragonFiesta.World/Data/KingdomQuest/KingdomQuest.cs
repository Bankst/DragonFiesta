using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.KingdomQuest
{
    public class KingdomQuest
    {
        public short ID { get; }

        public string Title { get; }

        public ushort LimitTime { get; }

        public byte ST_Year { get; }

        public byte ST_Month { get; }

        public byte ST_Day { get; }

        public byte ST_Hour { get; }

        public byte ST_Minute { get; }

        public byte ST_Second { get; }

        public ushort StartWaitTime { get; }

        public byte NextStartMode { get; }

        public ushort NextStartDeleyMin { get; }

        public byte RepeatMode { get; }

        public ushort RepeatCount { get; }

        public byte MinLevel { get; }

        public byte MaxLevel { get; }

        public ushort MinPlayers { get; }

        public ushort MaxPlayers { get; }

        public byte PlayerRepeatMode { get; }

        public ushort PlayerRepeatCount { get; }

        public byte PlayerRevivalCount { get; }

        public ushort DemandQuest { get; }

        public ushort DemandItem { get; }

        public byte DemandMobKill { get; }

        public uint RewardIndex { get; }

        public short MapLink { get; }

        public string SciptLanguage { get; }

        public string InitValue { get; }

        public uint UseClass { get; }

        public sbyte DemandGender { get; }

        public KingdomQuest(SHNResult pResult, int i)
        {

        }
    }
}
