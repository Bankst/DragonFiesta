using DragonFiesta.Zone.Data.Buffs;
using DragonFiesta.Providers.Characters;
using System;
using System.Collections.Generic;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.Zone.Data.Skills
{
    public sealed class ActiveSkillInfo
    {
        public ushort ID { get; private set; }

        public uint Step { get; private set; }
        public uint MaxStep { get; private set; }
        public ActiveSkillInfo DemandSkill { get; private set; }
        public WhoEquip WhoEquip { get; private set; } // DemandClass
        public uint SP { get; private set; }
        public uint SPRate { get; private set; }
        public uint HP { get; private set; }
        public uint HPRate { get; private set; }
        public uint Range { get; private set; }
        public bool IsMovingSkill { get; private set; }
        public TimeSpan CastTime { get; private set; }
        public TimeSpan CoolDownTime { get; private set; }

        //we dont use statsholder here because it arent 'real' stats
        public MinMax<uint> WeaponDamage { get; private set; }

        public MinMax<uint> MagicDamage { get; private set; }
        public uint WeaponDefense { get; private set; }
        public uint MagicDefense { get; private set; }
        public uint MaxTargets { get; private set; }
        public List<ActiveSkillAbState> AbStates { get; private set; }
        public ActiveSkillEffectType EffectType { get; private set; }
        public List<ActiveSkillActionInfo> Actions { get; private set; }

        public ActiveSkillInfo(SQLResult pResult, int i)
        {
            ID = pResult.Read<ushort>(i, "ID");
            Step = pResult.Read<uint>(i, "Step");
            MaxStep = pResult.Read<uint>(i, "MaxStep");
            WhoEquip = new WhoEquip(pResult.Read<uint>(i, "DemandClass"));
            SP = pResult.Read<uint>(i, "SP");
            SPRate = pResult.Read<uint>(i, "SPRate");
            HP = pResult.Read<uint>(i, "HP");
            HPRate = pResult.Read<uint>(i, "HPRate");
            Range = pResult.Read<uint>(i, "Range");
            IsMovingSkill = pResult.Read<bool>(i, "IsMovingSkill");
            CastTime = TimeSpan.FromMilliseconds(pResult.Read<uint>(i, "CastTime"));
            CoolDownTime = TimeSpan.FromMilliseconds(pResult.Read<uint>(i, "DlyTime"));
            WeaponDamage = new MinMax<uint>(pResult.Read<uint>(i, "MinWC"), pResult.Read<uint>(i, "MaxWC"));
            MagicDamage = new MinMax<uint>(pResult.Read<uint>(i, "MinMA"), pResult.Read<uint>(i, "MaxMA"));
            WeaponDefense = pResult.Read<uint>(i, "AC");
            MagicDefense = pResult.Read<uint>(i, "MR");
            MaxTargets = pResult.Read<uint>(i, "TargetNumber");
        }

        public bool FinalizeLoad(SQLResult pResult, int i)
        {
            short demenSkID = pResult.Read<short>(i, "DemandSk");
            if (demenSkID != -1 && demenSkID > -1)
            {
                if (!SkillDataProvider.GetActiveSkillInfoByID((ushort)demenSkID, out ActiveSkillInfo demandSk))
                {
                    EngineLog.Write(EngineLogLevel.Warning, "Can't find demand skill with ID '{0}' for skill '{1}'", demenSkID, ID);
                    return false;
                }

                DemandSkill = demandSk;
            }

            AbStates = new List<ActiveSkillAbState>();
            for (int i2 = 0; i2 < 4; i2++)
            {
                var letter = StringHelper.Characters_Upper[i2];
                short abStateID = pResult.Read<short>(i, "StaName" + letter);
                if (abStateID == -1)
                    continue;
                if (!BuffDataProvider.GetAbStateInfoByID((ushort)abStateID, out AbStateInfo abState))
                {
                    EngineLog.Write(EngineLogLevel.Warning, "Can't find abstate with ID '{0}' for skill '{1}'", abStateID, ID);
                    return false;
                }

                AbStates.Add(new ActiveSkillAbState(abState, pResult.Read<uint>(i, "StaStrength" + letter), pResult.Read<uint>(i, "StaSucRate" + letter)));
            }

            EffectType = (ActiveSkillEffectType)pResult.Read<uint>(i, "EffectType");
            Actions = new List<ActiveSkillActionInfo>();
            for (int i3 = 0; i3 < 5; i3++)
            {
                var letter = StringHelper.Characters_Upper[i3];
                var actionIndex = pResult.Read<uint>(i, "SpecialIndex" + letter);
                if (actionIndex < 1)
                    continue;
                Actions.Add(new ActiveSkillActionInfo(actionIndex, pResult.Read<uint>(i, "SpecialValue" + letter)));
            }
            return true;
        }
    }
}