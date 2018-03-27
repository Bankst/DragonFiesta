namespace DragonFiesta.Providers.Maps
{
    public class FieldLvCondition
    {
        public ushort MapId { get; set; }
        public ushort MinLevel { get; set; }
        public ushort MaxLevel { get; set; }

        public FieldLvCondition(SQLResult pResult, int i)
        {
            MapId = pResult.Read<ushort>(i, "MapId");
            MinLevel = pResult.Read<ushort>(i, "MinLevel");
            MaxLevel = pResult.Read<ushort>(i, "MaxLevel");
        }

        public bool IsLevelInRange(byte Level)
        {
            return Level > MinLevel && Level < MaxLevel;
        }
    }
}