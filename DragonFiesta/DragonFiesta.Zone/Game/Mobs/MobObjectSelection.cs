using DragonFiesta.Zone.Game.Maps.Object;
using DragonFiesta.Zone.Game.Maps.Interface;

namespace DragonFiesta.Zone.Game.Mobs
{
    public class MobObjectSelection : LivingObjectSelectionBase
    {
        private Mob mob;

        public MobObjectSelection(ILivingObject Owner) : base(Owner)
        {
            mob = (Owner as Mob);
        }

        public override void Dispose()
        {
            mob = null;
            base.Dispose();
        }
    }
}