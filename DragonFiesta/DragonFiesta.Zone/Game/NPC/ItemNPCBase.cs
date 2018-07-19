using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Game.NPC
{
    public abstract class ItemNPCBase : NPCBase
    {
        private FiestaPacket ItemListPacket;

        protected abstract FiestaPacket CreateItemListPacket();

        public ItemNPCBase(NPCInfo Info) : base(Info)
        {

        }

        public override void OpenMenu(ZoneCharacter Character)
        {
            Character.Session.SendPacket(ItemListPacket);
        }

        protected override void DisposeInternal()
        {
            ItemListPacket.Dispose();
            ItemListPacket = null;

            base.DisposeInternal();
        }
    }
}