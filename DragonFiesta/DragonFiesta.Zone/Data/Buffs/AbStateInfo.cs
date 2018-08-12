using System.Collections.Concurrent;
using System.Collections.Generic;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.Buffs
{
    public sealed class AbStateInfo
    {
        public ushort ID { get; }

        public string InxName { get; }

        public uint AbStateIndex { get; }

        public uint KeepTimeRatio { get; }

        public byte KeepTimePower { get; }

        public byte StateGrade { get; }










        public AbStateInfo(SHNResult pResult, int row)
        {
            ID = pResult.Read<ushort>(row, "ID");
            InxName = pResult.Read<string>(row, "InxName");
            AbStateIndex = pResult.Read<uint>(row, "AbStateIndex");
            KeepTimeRatio = pResult.Read<uint>(row, "KeepTimeRatio");
            KeepTimePower = pResult.Read<byte>(row, "KeepTimePower");
            StateGrade = pResult.Read<byte>(row, "StateGrade");
        }
    }
}