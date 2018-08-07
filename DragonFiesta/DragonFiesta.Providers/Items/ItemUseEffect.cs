namespace DragonFiesta.Providers.Items
{
    public sealed class ItemUseEffect
    {
        public ItemUseEffectType Type { get; private set; }
        public ushort Value { get; private set; }

        public ItemUseEffect(uint Type, ushort Value)
        {
            this.Type = (ItemUseEffectType)Type;
            this.Value = Value;
        }
    }
}