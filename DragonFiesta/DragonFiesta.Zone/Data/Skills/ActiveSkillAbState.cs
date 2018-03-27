using DragonFiesta.Zone.Data.Buffs;

namespace DragonFiesta.Zone.Data.Skills
{
    public sealed class ActiveSkillAbState
    {
        public AbStateInfo AbStateInfo { get; private set; }
        public uint Strength { get; private set; }
        public uint SuccessRate { get; private set; }

        public ActiveSkillAbState(AbStateInfo AbStateInfo, uint Strength, uint SuccessRate)
        {
            this.AbStateInfo = AbStateInfo;
            this.Strength = Strength;
            this.SuccessRate = SuccessRate;
        }
    }
}