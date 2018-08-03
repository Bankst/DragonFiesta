using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.NPCSchedule
{
    public class NPCSchedule
    {
        public string Mob_Inx { get; }

        public ushort NS_Year { get; }

        public byte NS_Month { get; }

        public byte NS_Day { get; }

        public byte NS_Hour { get; }

        public byte NS_Minute { get; }

        public ushort NS_CycleHour { get; }

        public byte NS_LifeHour { get; }

        public byte NS_IsMsg { get; }

        public NPCSchedule(SHNResult pResult, int i)
        {
            Mob_Inx = pResult.Read<string>(i, "Mob_Inx");
            NS_Year = pResult.Read<ushort>(i, "NS_Year");
            NS_Month = pResult.Read<byte>(i, "NS_Month");
            NS_Day = pResult.Read<byte>(i, "NS_Day");
            NS_Hour = pResult.Read<byte>(i, "NS_Hour");
            NS_Minute = pResult.Read<byte>(i, "NS_Minute");
            NS_CycleHour = pResult.Read<ushort>(i, "NS_CycleHour");
            NS_LifeHour = pResult.Read<byte>(i, "NS_LifeHour");
            NS_IsMsg = pResult.Read<byte>(i, "NS_IsMsg");
        }
    }
}
