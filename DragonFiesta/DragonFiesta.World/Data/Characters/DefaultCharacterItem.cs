using DragonFiesta.Providers.Items;

namespace DragonFiesta.World.Data.Characters
{
    public sealed class DefaultCharacterItem
    {
        /// <summary>
        /// The ItemInfo which belongs to this default item.
        /// </summary>
        public ItemBaseInfo ItemInfo { get; private set; }

        public InventoryType pFlags { get; set; }

        //public ConcurrentDictionary<ItemOptionIndex, long> Options { get; private set; }

        public byte Upgrades { get; private set; }

        /// <summary>
        /// The amount of this item.
        /// </summary>
        public byte Amount { get; private set; }

        public DefaultCharacterItem(ItemBaseInfo ItemInfo, SQLResult pRes, int i)
        {
            //   Options = new ConcurrentDictionary<ItemOptionIndex, long>();

            this.ItemInfo = ItemInfo;
            Amount = pRes.Read<byte>(i, "Amount");
            pFlags = (InventoryType)pRes.Read<byte>(i, "Inventory");
            Upgrades = pRes.Read<byte>(i, "Upgrades");
        }
    }
}