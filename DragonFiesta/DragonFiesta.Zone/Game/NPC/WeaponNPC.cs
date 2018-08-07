using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Networking.HandlerTypes;

namespace DragonFiesta.Zone.Game.NPC
{
    public sealed class WeaponNPC : ItemNPCBase
    {
        public WeaponNPC(NPCInfo info) : base(info)
        {

        }

        protected override FiestaPacket CreateItemListPacket()
        {
            var packet = new FiestaPacket(Handler15Type._Header, Handler15Type.SMSG_MENU_SHOPOPENTABLE_WEAPON_CMD);

            packet.Write<ushort>(Info.Items.Count);
            packet.Write<ushort>(MapObjectId);

            foreach (var item in Info.Items)
            {
	            packet.Write<byte>(item.Slot);
	            packet.Write<ushort>(item.Info.ID);
            }

            return packet;
        }
    }
}