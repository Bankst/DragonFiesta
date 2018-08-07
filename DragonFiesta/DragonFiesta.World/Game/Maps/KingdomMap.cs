using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Zone;

namespace DragonFiesta.World.Game.Maps
{
    public class KingdomMap : InstanceMap
    {
        public KingdomMap(ZoneServer Server, FieldInfo mInfo, ushort InstanceId) : base(Server, mInfo, InstanceId)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();

        }
    }
}