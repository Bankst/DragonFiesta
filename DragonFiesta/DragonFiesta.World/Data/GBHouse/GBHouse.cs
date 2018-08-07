using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.World.Data.GBHouse
{
    public class GBHouse
    {
        public uint GB_GameMoney { get; }

        public uint GB_ExchangeTax { get; }

        public byte GB_ResetTimeHour { get; }

        public byte GB_ResetTimeMin { get; }

        public byte GB_ResetTimeSec { get; }

        public GBHouse(SHNResult pResult, int i)
        {
            GB_GameMoney = pResult.Read<uint>(i, "GB_GameMoney");
            GB_ExchangeTax = pResult.Read<uint>(i, "GB_ExchangeTax");
            GB_ResetTimeHour = pResult.Read<byte>(i, "GB_ResetTimeHour");
            GB_ResetTimeMin = pResult.Read<byte>(i, "GB_ResetTimeMin");
            GB_ResetTimeSec = pResult.Read<byte>(i, "GB_ResetTimeSec");
        }
    }
}
