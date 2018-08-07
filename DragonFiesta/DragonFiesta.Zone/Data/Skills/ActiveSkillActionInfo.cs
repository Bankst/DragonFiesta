namespace DragonFiesta.Zone.Data.Skills
{
    public sealed class ActiveSkillActionInfo
    {
        public ActiveSkillActionType Type { get; private set; }
        public uint Value { get; private set; }

        public ActiveSkillActionInfo(uint Type, uint Value)
        {
            this.Type = (ActiveSkillActionType)Type;
            this.Value = Value;
        }
    }
}