using System;
using System.Data;
using DragonFiesta.Providers.Items;

namespace DragonFiesta.Zone.Data.Mob
{
    public sealed class MobInfo
    {
        public ushort ID { get; private set; }
        public string Index { get; private set; }
        public byte Level { get; private set; }
        public StatsHolder Stats { get; private set; }
        public bool IsNPC { get; private set; }
        public uint Size { get; private set; }

        //MobInfoServer
        public bool DetectEnemy { get; private set; }

        public uint EXP { get; private set; }
        public double EXPRange { get; private set; }
        public double DetectCharacterRange { get; private set; }
        public double FollowCharacterRange { get; private set; }
        public bool BroadcastAtDead { get; private set; }
        public ushort MaxUpdateCounter { get; private set; }

        public MobInfo(SQLResult pResult, int i, DataRow row)
        {
            ID = pResult.Read<ushort>(i, "ID");
            Index = pResult.Read<string>(i, "InxName");
            Level = pResult.Read<byte>(i, "Level");
            Stats = new StatsHolder()
            {
                MaxHP = pResult.Read<int>(i, "MaxHP"),     
                WalkSpeed = pResult.Read<ushort>(i, "WalkSpeed"),
                RunSpeed = pResult.Read<ushort>(i, "RunSpeed"),
            };
            IsNPC = pResult.Read<bool>(i, "IsNPC");
            Size = pResult.Read<uint>(i, "Size");
            LoadServerInfo(row);
        }

        private void LoadServerInfo(DataRow row)
        {
            Stats.WeaponDefense = Convert.ToInt32(row["AC"]);
            Stats.Evasion = Convert.ToInt32(row["TB"]);
            Stats.MagicDefense = Convert.ToInt32(row["MR"]);
            DetectEnemy = (Convert.ToUInt32(row["EnemyDetectType"]) != 2);
            EXP = Convert.ToUInt32(row["MonEXP"]);
            EXPRange = Convert.ToDouble(row["EXPRange"]);
            DetectCharacterRange = Convert.ToDouble(row["DetectCha"]);
            FollowCharacterRange = Convert.ToDouble(row["FollowCha"]);
            Stats.Str = Convert.ToInt32(row["Str"]);
            Stats.Dex = Convert.ToInt32(row["Dex"]);
            Stats.End = Convert.ToInt32(row["Con"]);
            Stats.Int = Convert.ToInt32(row["Int"]);
            Stats.Spr = Convert.ToInt32(row["Men"]);
            Stats.MaxSP = Convert.ToInt32(row["MaxSP"]);
            BroadcastAtDead = (Convert.ToByte(row["BroadAtDead"]) > 0);
        }
    }
}