using DragonFiesta.Providers.Items;

namespace DragonFiesta.Zone.Data.NPC
{
    public sealed class NPCItem
    {
        public byte Slot { get; private set; }
        public ItemBaseInfo Info { get; private set; }
        public NPCItem(byte Slot, ItemBaseInfo Info)
        {
            this.Slot = Slot;
            this.Info = Info;
        }
    }
}