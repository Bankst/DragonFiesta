﻿using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Data.NPC;

namespace DragonFiesta.Zone.Game.NPC
{
    public class SkillNPC : ItemNPCBase
	{
		public SkillNPC(NPCInfo info) : base(info)
		{
		}

        protected override FiestaPacket CreateItemListPacket()
        {
            var packet = new FiestaPacket(Handler15Type._Header, Handler15Type.SMSG_MENU_SHOPOPENTABLE_SKILL_CMD);

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