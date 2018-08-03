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
            ID = pResult.Read<short>(i, "ID");
            Title = pResult.Read<string>(i, "Title");
            LimitTime = pResult.Read<ushort>(i, "LimitTime");
            ST_Year = pResult.Read<byte>(i, "ST_Year");
            ST_Month = pResult.Read<byte>(i, "ST_Month");
            ST_Day = pResult.Read<byte>(i, "ST_Day");
            ST_Hour = pResult.Read<byte>(i, "ST_Hour");
            ST_Minute = pResult.Read<byte>(i, "ST_Minute");
            ST_Second = pResult.Read<byte>(i, "ST_Second");
            StartWaitTime = pResult.Read<ushort>(i, "StartWaitTime");
            NextStartMode = pResult.Read<byte>(i, "NextStartMode");
            NextStartDeleyMin = pResult.Read<ushort>(i, "NextStartDeleyMin");
            RepeatMode = pResult.Read<byte>(i, "RepeatMode");
            RepeatCount = pResult.Read<ushort>(i, "RepeatCount");
            MinLevel = pResult.Read<byte>(i, "MinLevel");
            MaxLevel = pResult.Read<byte>(i, "MaxLevel");
            MinPlayers = pResult.Read<byte>(i, "MinPlayers");
            MaxPlayers = pResult.Read<byte>(i, "MaxPlayers");
            PlayerRepeatMode = pResult.Read<byte>(i, "PlayerRepeatMode");
            PlayerRepeatCount = pResult.Read<ushort>(i, "PlayerRepeatCount");
            PlayerRevivalCount = pResult.Read<byte>(i, "PlayerRevivalCount");
            DemandQuest = pResult.Read<ushort>(i, "DemandQuest");
            DemandItem = pResult.Read<ushort>(i, "DemandItem");
            DemandMobKill = pResult.Read<byte>(i, "DemandMobKill");
            RewardIndex = pResult.Read<uint>(i, "RewardIndex");
            MapLink = pResult.Read<short>(i, "MapLink");
            SciptLanguage = pResult.Read<string>(i, "SciptLanguage");
            InitValue = pResult.Read<string>(i, "InitValue");
            UseClass = pResult.Read<uint>(i, "UseClass");
            DemandGender = pResult.Read<sbyte>(i, "DemandGender");
        }
    }
}
