using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Game.NPC
{
    public class SoulStoneNPC : NPCBase
    {
        public SoulStoneNPC(NPCInfo info) : base(info)
        {
        }

        public sealed override void OpenMenu(ZoneCharacter character)
        {
	        using (var packet = new FiestaPacket(Handler15Type._Header, Handler15Type.SMSG_MENU_SHOPOPENSOULSTONE_CMD))
	        {
				packet.Write<uint>(character.Info.LevelParameter.StoneHP);
				packet.Write<uint>(character.Info.MaxHPStones);
		        packet.Write<uint>(character.Info.LevelParameter.PriceHPStone);

		        packet.Write<uint>(character.Info.LevelParameter.StoneSP);
		        packet.Write<uint>(character.Info.MaxSPStones);
		        packet.Write<uint>(character.Info.LevelParameter.PriceSPStone);

				character.Session.SendPacket(packet);
			}
        }

        protected sealed override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}