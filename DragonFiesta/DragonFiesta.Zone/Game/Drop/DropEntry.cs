using DragonFiesta.Zone.Game.Maps.Object;
using System.Data.SqlClient;

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