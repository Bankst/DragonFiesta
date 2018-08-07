using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Providers.Maps
{
	public class FieldLvCondition
    {
        public string MapId { get; set; }
		public ushort ModeIDLv { get; set; }
        public ushort MinLevel { get; set; }
        public ushort MaxLevel { get; set; }

        public FieldLvCondition(SHNResult pResult, int i)
        {
            MapId = pResult.Read<string>(i, "MapIndex");
	        ModeIDLv = pResult.Read<ushort>(i, "ModeIDLv");
            MinLevel = pResult.Read<ushort>(i, "LvFrom");
            MaxLevel = pResult.Read<ushort>(i, "LvTo");
        }

        public bool IsLevelInRange(byte level)
        {
            return level > MinLevel && level < MaxLevel;
        }
    }
}