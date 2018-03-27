using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Zone;

namespace DragonFiesta.World.Game.Maps
{
    public class NormalMap : WorldServerMap
    {
        public NormalMap(ZoneServer Server, FieldInfo mInfo) : base(Server, mInfo)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}