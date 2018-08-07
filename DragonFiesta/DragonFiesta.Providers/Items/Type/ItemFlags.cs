namespace DragonFiesta.Providers.Items
{
    public enum ItemFlags : byte
    {
        GuildItem = 0,
        EquippedItem = 32,
        InventoryItem = 36,
        MiniHouse = 144,
        PremiumItem = 155,
        None = 255,
    }
}