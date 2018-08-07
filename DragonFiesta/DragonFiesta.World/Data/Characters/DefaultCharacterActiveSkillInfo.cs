namespace DragonFiesta.World.Data.Characters
{
    public sealed class DefaultCharacterActiveSkillInfo
    {
        public ushort SkillId { get; private set; }

        public DefaultCharacterActiveSkillInfo(ushort SkillId)
        {
            this.SkillId = SkillId;
        }
    }
}