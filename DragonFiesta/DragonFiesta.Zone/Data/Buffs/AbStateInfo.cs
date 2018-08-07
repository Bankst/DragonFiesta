using System.Collections.Concurrent;
using System.Collections.Generic;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Buffs
{
    public sealed class AbStateInfo
    {
        public ushort ID { get; private set; }
        public uint AbStateIndex { get; private set; }
        public byte Grade { get; private set; }
        public List<AbStateInfo> PartyStates { get; private set; }
        public double PartyRange { get; private set; }
        public ConcurrentDictionary<uint, SubAbStateInfo> SubAbStates { get; private set; }
        public bool IsSave { get; private set; }
        public string mnin { get; set; }
        public AbStateInfo MainAbState { get; set; }

        public AbStateInfo(SQLResult pResult, int rIndex)
        {
            ID = pResult.Read<ushort>(rIndex, "ID");
            AbStateIndex = pResult.Read<uint>(rIndex, "AbStateIndex");
            Grade = pResult.Read<byte>(rIndex, "StateGrade");
            PartyStates = new List<AbStateInfo>();
            PartyRange = pResult.Read<uint>(rIndex, "PartyRange");
            SubAbStates = new ConcurrentDictionary<uint, SubAbStateInfo>();
            IsSave = pResult.Read<bool>(rIndex, "IsSave");
        }
    }
}