using System;
using DragonFiesta.Utils.IO.SHN;

namespace DragonFiesta.Zone.Data.Skills
{
    public sealed class ActiveSkillInfo
    {
        public uint ID { get; }

        public string InxName { get; }

        public string Name { get; }

        public uint Grade { get; }

        public uint Step { get; }

        public uint MaxStep { get; }

        public uint DemandType { get; }

        public string DemandSk { get; }

        public ushort UseItem { get; }

        public uint ItemNumber { get; }

        public uint ItemOption { get; }

        public ushort DemandItem1 { get; }      

        public ushort DemandItem2 { get; }

        public uint SP { get; }

        public uint SPRate { get; }

        public uint HP { get; }

        public uint HPRate { get; }

        public uint LP { get; }

        public uint Range { get; }

        public uint First { get; }

        public uint Last { get; }

        public byte IsMovingSkill { get; }

        public ushort UsableDegree { get; }

        public ushort DirectionRotate { get; }

        public ushort SkillDegree { get; }

        public uint SkillTargetState { get; }

        public TimeSpan CastTime { get; }

        public uint DlyTime { get; }

        public uint DlyGroupNum { get; }

        public uint DlyTimeGroup { get; }

        public uint MinWC { get; }

        public uint MinWCRate { get; }

        public uint MaxWC { get; }

        public uint MaxWCRate { get; }

        public uint MinMA { get; }

        public uint MinMARate { get; }

        public uint MaxMA { get; }

        public uint MaxMARate { get; }

        public uint AC { get; }

        public uint MR{ get; }

        public uint Area { get; }

        public uint TargetNumber { get; }

        public uint UseClass { get; }

        public string StaNameA { get; }

        public uint StaStrengthA { get; }

        public uint StaSucRateA { get; }

        public string StaNameB { get; }

        public uint StaStrengthB { get; }

        public uint StaSucRateB { get; }

        public string StaNameC { get; }

        public uint StaStrengthC { get; }
        
        public uint StaSucRateC { get; }

        public string StaNameD { get; }

        public uint StaStrengthD { get; }

        public uint StaSucRateD { get; }

        public uint NIMPT { get; }

        public uint NT0 { get; }

        public uint NT1 { get; }

        public uint NT2 { get; }

        public uint NT3 { get; }

        public uint EffectType { get; }

        public uint SpecialIndexA { get; }

        public uint SpecialValueA { get; }

        public uint SpecialIndexB { get; }

        public uint SpecialValueB { get; }

        public uint SpecialIndexC { get; }

        public uint SpecialValueC { get; }

        public uint SpecialIndexD { get; }

        public uint SpecialValueD { get; }

        public uint SpecialIndexE { get; }

        public uint SpecialValueE { get; }

        public string SkillClassifierA { get; }

        public string SkillClassifierB { get; }

        public string SkillClassifierC { get; }

        public byte CannotInside { get; }

        public byte DemandSoul { get; }

        public ushort HitID { get; }


        public ActiveSkillInfo(SHNResult pResult, int i)
        {
            ID = pResult.Read<ushort>(i, "ID");
            InxName = pResult.Read<string>(i, "InxName");
            Name = pResult.Read<string>(i, "Name");
            Grade = pResult.Read<uint>(i, "Grade");
            Step = pResult.Read<uint>(i, "Step");
            MaxStep = pResult.Read<uint>(i, "MaxStep");
            DemandType = pResult.Read<uint>(i, "DemandType");
            DemandSk = pResult.Read<string>(i, "DemandSk");
            UseItem = pResult.Read<ushort>(i, "UseItem");
            ItemNumber = pResult.Read<uint>(i, "ItemNumber");
            ItemOption = pResult.Read<uint>(i, "ItemOption");
            DemandItem1 = pResult.Read<ushort>(i, "DemandItem1");
            DemandItem2 = pResult.Read<ushort>(i, "DemandItem2");
            SP = pResult.Read<uint>(i, "SP");
            SPRate = pResult.Read<uint>(i, "SPRate");
            HP = pResult.Read<uint>(i, "HP");
            HPRate = pResult.Read<uint>(i, "HPRate");
            LP = pResult.Read<uint>(i, "LP");
            Range = pResult.Read<uint>(i, "Range");
            First = pResult.Read<uint>(i, "First");
            Last = pResult.Read<uint>(i, "Last");
            IsMovingSkill = pResult.Read<byte>(i, "IsMovingSkill");
            UsableDegree = pResult.Read<ushort>(i, "UsableDegree");
            DirectionRotate = pResult.Read<ushort>(i, "DirectionRotate");
            SkillDegree = pResult.Read<ushort>(i, "SkillDegree");
            SkillTargetState = pResult.Read<uint>(i, "SkillTargetState");
            CastTime = TimeSpan.FromMilliseconds(pResult.Read<uint>(i, "CastTime"));
            DlyTime = pResult.Read<uint>(i, "DlyTime");
            DlyGroupNum = pResult.Read<uint>(i, "DlyGroupNum");
            DlyTimeGroup = pResult.Read<uint>(i, "DlyTimeGroup");
            MinWC = pResult.Read<uint>(i, "MinWC");
            MinWCRate = pResult.Read<uint>(i, "MinWCRate");
            MaxWC = pResult.Read<uint>(i, "MaxWC");
            MaxWCRate = pResult.Read<uint>(i, "MaxWCRate");
            MinMA = pResult.Read<uint>(i, "MinMA");
            MinMARate = pResult.Read<uint>(i, "MinMARate");
            MaxMA = pResult.Read<uint>(i, "MaxMA");
            MaxMARate = pResult.Read<uint>(i, "MaxMARate");
            AC = pResult.Read<uint>(i, "AC");
            MR = pResult.Read<uint>(i, "MR");
            Area = pResult.Read<uint>(i, "Area");
            TargetNumber = pResult.Read<uint>(i, "TargetNumber");
            UseClass = pResult.Read<uint>(i, "UseClass");
            StaNameA = pResult.Read<string>(i, "StaNameA");
            StaStrengthA = pResult.Read<uint>(i, "StaStrengthA");
            StaSucRateA = pResult.Read<uint>(i, "StaSucRateA");
            StaNameB = pResult.Read<string>(i, "StaNameB");
            StaStrengthB = pResult.Read<uint>(i, "StaStrengthB");
            StaSucRateB = pResult.Read<uint>(i, "StaSucRateB");
            StaNameC = pResult.Read<string>(i, "StaNameC");
            StaStrengthC = pResult.Read<uint>(i, "StaStrengthC");
            StaSucRateC = pResult.Read<uint>(i, "StaSucRateC");
            StaNameD = pResult.Read<string>(i, "StaNameD");
            StaStrengthD = pResult.Read<uint>(i, "StaStrengthD");
            StaSucRateD = pResult.Read<uint>(i, "StaSucRateD");
            NIMPT = pResult.Read<uint>(i, "nIMPT");
            NT0 = pResult.Read<uint>(i, "nT0");
            NT1 = pResult.Read<uint>(i, "nT1");
            NT2 = pResult.Read<uint>(i, "nT2");
            NT3 = pResult.Read<uint>(i, "nT3");
            EffectType = pResult.Read<uint>(i, "EffectType");
            SpecialIndexA = pResult.Read<uint>(i, "SpecialIndexA");
            SpecialValueA = pResult.Read<uint>(i, "SpecialValueA");
            SpecialIndexB = pResult.Read<uint>(i, "SpecialIndexB");
            SpecialValueB = pResult.Read<uint>(i, "SpecialValueB");
            SpecialIndexC = pResult.Read<uint>(i, "SpecialIndexC");
            SpecialValueC = pResult.Read<uint>(i, "SpecialValueC");
            SpecialIndexD = pResult.Read<uint>(i, "SpecialIndexD");
            SpecialValueD = pResult.Read<uint>(i, "SpecialValueD");
            SpecialIndexE = pResult.Read<uint>(i, "SpecialIndexE");
            SpecialValueE = pResult.Read<uint>(i, "SpecialValueE");
            SkillClassifierA = pResult.Read<string>(i, "SkillClassifierA");
            SkillClassifierB = pResult.Read<string>(i, "SkillClassifierB");
            SkillClassifierC = pResult.Read<string>(i, "SkillClassifierC");
            CannotInside = pResult.Read<byte>(i, "CannotInside");
            DemandSoul = pResult.Read<byte>(i, "DemandSoul");
            HitID = pResult.Read<ushort>(i, "HitID");
        }

        #warning Finish ActiveSkill Stuff

        #region SQL
        /*
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
        }*/
        #endregion
    }
}