using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Game.Zone;

namespace DragonFiesta.Zone.Game.Maps.Types
{
    public class RemoteMap : ZoneServerMap
    {
        public RemoteZone Zone { get; private set; }

        public RemoteMap(RemoteZone Zone, FieldInfo mInfo) : base(mInfo)
        {
            this.Zone = Zone;
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();

            Zone = null;
        }
    }
}