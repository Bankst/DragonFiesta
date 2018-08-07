using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;

namespace DragonFiesta.Zone.Game.NPC
{
    public class GBDice : NPCBase
    {
        public GBDice(NPCInfo Info) : base(Info)
        {
        }

        public override sealed void OpenMenu(ZoneCharacter Character)
        {
            // throw new NotImplementedException();
        }
        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}