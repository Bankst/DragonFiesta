using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.AttendReward
{
    public class AttendSchedule
    {
        public ushort AS_StartYear { get; }

        public byte AS_StartMonth { get; }

        public byte AS_StartDay { get; }

        public byte AS_StartHour { get; }

        public byte AS_StartMinute { get; }

        public ushort AS_JoinKeepTime { get; }

        public ushort AS_CkeckTerm { get; }

        public AttendSchedule(SHNResult pResult, int i)
        {
            AS_StartYear = pResult.Read<ushort>(i, "AS_StartYear");
            AS_StartMonth = pResult.Read<byte>(i, "AS_StartMonth");
            AS_StartDay = pResult.Read<byte>(i, "AS_StartDay");
            AS_StartHour = pResult.Read<byte>(i, "AS_StartHour");
            AS_StartMinute = pResult.Read<byte>(i, "AS_StartMinute");
            AS_JoinKeepTime = pResult.Read<ushort>(i, "AS_JoinKeepTime");
            AS_CkeckTerm = pResult.Read<ushort>(i, "AS_CkeckTerm");
        }
    }
}
