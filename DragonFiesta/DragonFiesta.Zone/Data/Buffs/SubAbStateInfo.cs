using System;
using System.Collections.Generic;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Buffs
{
    public sealed class SubAbStateInfo
    {
        public ushort ID { get; private set; }

        public uint Strength { get; private set; }
        public byte Type { get; private set; }
        public byte SubType { get; private set; }
        public TimeSpan KeepTime { get; private set; }
        public List<SubAbStateAction> Actions { get; private set; }

        public SubAbStateInfo(ushort ID, uint Strength, byte Type, byte SubType, TimeSpan KeepTime, params SubAbStateAction[] Actions)
        {
            this.ID = ID;
            this.Strength = Strength;
            this.Type = Type;
            this.SubType = SubType;
            this.KeepTime = KeepTime;
            this.Actions = new List<SubAbStateAction>(Actions);
        }

        public SubAbStateInfo(SQLResult result, int rIndex)
        {
            ID = result.Read<ushort>(rIndex, "ID");
            Strength = result.Read<uint>(rIndex, "Strenght");
            Type = result.Read<byte>(rIndex, "Type");
            SubType = result.Read<byte>(rIndex, "SubType");
            KeepTime = TimeSpan.FromMilliseconds(result.Read<uint>(rIndex, "KeepTime"));
            Actions = new List<SubAbStateAction>();

            for (int i = 0; i < 4; i++)
            {
                var letter = StringHelper.Characters_Upper[i];
                uint actionIndex = result.Read<uint>(rIndex, "ActionIndex" + letter),
                     actionValue = result.Read<uint>(rIndex, "ActionArg" + letter);
                if (actionIndex == 0)
                    continue;
                Actions.Add(new SubAbStateAction(actionIndex, actionValue));
            }
        }
    }
}