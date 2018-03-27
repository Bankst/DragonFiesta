using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Maps.Object;
using DragonFiesta.Zone.Game.NPC;

namespace DragonFiesta.Zone.Game.Maps
{
    public class NPCObjectSelection : LivingObjectSelectionBase
    {
        private NPCBase NPC;

        public NPCObjectSelection(ILivingObject Owner) : base(Owner)
        {
            NPC = (Owner as NPCBase);
        }

        public override void Dispose()
        {
            NPC = null;
            base.Dispose();
        }
    }
}