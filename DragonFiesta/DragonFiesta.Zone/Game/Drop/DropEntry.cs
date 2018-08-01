using DragonFiesta.Zone.Game.Maps.Object;

namespace DragonFiesta.Zone.Game.Maps
{
    public class DropEntry : MapObject
    {
        public override MapObjectType Type => MapObjectType.Drop;

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}