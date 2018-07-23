﻿using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Game.NPC
{
	public class SkillNPC : ItemNPCBase
	{
		public SkillNPC(NPCInfo info) : base(info)
		{
		}

		public override void OpenMenu(ZoneCharacter Character)
		{
		}

        protected override FiestaPacket CreateItemListPacket()
        {
            var packet = new FiestaPacket(Handler07Type._Header, Handler07Type.SMSG_BRIEFINFO_CHANGEWEAPON_CMD);

            return packet;
        }
    }
}