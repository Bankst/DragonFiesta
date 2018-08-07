namespace DragonFiesta.Zone.Data.Buffs
{
    public sealed class SubAbStateAction
    {
        public SubAbStateActionType Type { get; private set; }
        public uint Value { get; private set; }

        public SubAbStateAction(uint Type, uint Value)
        {
            this.Type = (SubAbStateActionType)Type;
            this.Value = Value;
        }
    }
}