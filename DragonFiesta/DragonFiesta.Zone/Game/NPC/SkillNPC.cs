using DragonFiesta.Providers.Text;
using DragonFiesta.Zone.Data.Menu;
using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Chat;

namespace DragonFiesta.Zone.Game.NPC
{
	public class SkillNPC : ItemNPCBase
	{
		public SkillNPC(NPCInfo info) : base(info)
		{
		}

		public override void OpenMenu(ZoneCharacter Character)
		{
			//bruh

		}


		protected override void DisposeInternal()
		{
			base.DisposeInternal();
		}
	}
}