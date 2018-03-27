using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Game.NPC
{
    public class SoulStoneNPC : NPCBase
    {
        public SoulStoneNPC(NPCInfo Info) : base(Info)
        {
        }

        public override sealed void OpenMenu(ZoneCharacter Character)
        {
        }
        protected sealed override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}