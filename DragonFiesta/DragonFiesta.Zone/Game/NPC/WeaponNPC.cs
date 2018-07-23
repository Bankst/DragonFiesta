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
            var packet = new FiestaPacket(Handler15Type._Header, Handler15Type.SMSG_MENU_SHOPOPENTABLE_WEAPON_CMD);

            packet.Write<ushort>(Info.Items.Count);
            packet.Write<ushort>(MapObjectId);

            for (int i = 0; i < Info.Items.Count; i++)
            {
                var item = Info.Items[i];

                packet.Write<byte>(item.Slot);
                packet.Write<ushort>(item.Info.ID);
            }

            return packet;
        }
    }
}