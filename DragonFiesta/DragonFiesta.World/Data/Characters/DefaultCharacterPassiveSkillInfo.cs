namespace DragonFiesta.World.Data.Characters
{
    public class DefaultCharacterPassiveSkillInfo
    {
        public ushort SkillID { get; private set; }

        public DefaultCharacterPassiveSkillInfo(ushort SkillID)
        {
            this.SkillID = SkillID;
        }
    }
}