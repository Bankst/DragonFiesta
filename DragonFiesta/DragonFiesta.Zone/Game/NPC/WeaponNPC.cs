using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Networking.HandlerTypes;

namespace DragonFiesta.Zone.Game.NPC
{
    public sealed class WeaponNPC : ItemNPCBase
    {
        public WeaponNPC(NPCInfo info) : base(info)
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