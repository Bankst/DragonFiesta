using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Game.NPC
{
    public abstract class ItemNPCBase : NPCBase
    {
        private FiestaPacket ItemListPacket;

        protected abstract FiestaPacket CreateItemListPacket();

	    protected ItemNPCBase(NPCInfo Info) : base(Info)
	    {
		    //ItemListPacket = CreateItemListPacket();
	    }

        public override void OpenMenu(ZoneCharacter Character)
        {
	        ItemListPacket = CreateItemListPacket();
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