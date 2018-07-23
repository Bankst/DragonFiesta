using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Data.NPC;

namespace DragonFiesta.Zone.Game.NPC
{
    public class StuffNPC : ItemNPCBase
    {
        public StuffNPC(NPCInfo info) : base(info)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }

        protected override FiestaPacket CreateItemListPacket()
        {
            var packet = new FiestaPacket(Handler07Type._Header, Handler07Type.SMSG_BRIEFINFO_CHANGEWEAPON_CMD);

            return packet;
        }
    }
}