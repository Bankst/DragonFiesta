namespace DragonFiesta.Providers.Items
{
    public enum ItemFlags : byte
    {
        GuildItem = 0x00,
        EquippedItem = 0x20,
        InventoryItem = 0x24,
        MiniHouse = 144,
        PremiumItem = 155,
        None = 0xFF,
    }
}