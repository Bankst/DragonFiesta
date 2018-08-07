using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.GuildTournament
{
    public class GuildTournament
    {
        public byte IsActive { get; }

        public byte Weeks { get; }

        public byte Hour { get; }

        public byte Minute { get; }

        public byte TermHour { get; }

        public byte TermMinute { get; }

        public ushort MatchCycleMin { get; }

        public ushort ExploerTimeMin { get; }

        public ushort WaitPlayTimeSec { get; }

        public ushort PlayTime { get; }

        public ushort Deadline { get; }

        public ushort MatchDelay { get; }

        public ushort Match_161 { get; }

        public ushort Match_162 { get; }

        public ushort Match_8 { get; }

        public ushort Match_4 { get; }

        public ushort Match_2 { get; }

        public GuildTournament(SHNResult pResult, int i)
        {
            IsActive = pResult.Read<byte>(i, "isActive");
            Weeks = pResult.Read<byte>(i, "Weeks");
            Hour = pResult.Read<byte>(i, "Hour");
            Minute = pResult.Read<byte>(i, "Minute");
            TermHour = pResult.Read<byte>(i, "TermHour");
            TermMinute = pResult.Read<byte>(i, "TermMinute");
            MatchCycleMin = pResult.Read<ushort>(i, "MatchCycleMin");
            ExploerTimeMin = pResult.Read<ushort>(i, "ExploerTimeMin");
            WaitPlayTimeSec = pResult.Read<ushort>(i, "WaitPlayTimeSec");
            PlayTime = pResult.Read<ushort>(i, "PlayTime");
            Deadline = pResult.Read<ushort>(i, "Deadline");
            MatchDelay = pResult.Read<ushort>(i, "MatchDelay");
            Match_161 = pResult.Read<ushort>(i, "Match_161");
            Match_162 = pResult.Read<ushort>(i, "Match_162");
            Match_8 = pResult.Read<ushort>(i, "Match_8");
            Match_4 = pResult.Read<ushort>(i, "Match_4");
            Match_2 = pResult.Read<ushort>(i, "Match_2");
        }
    }
}
