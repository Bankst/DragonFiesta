using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Game.Zone;

namespace DragonFiesta.Zone.Game.Maps.Types
{
    public class RemoteInstanceMap : RemoteMap, IInstanceMap
    {
        private ushort _InstanceId = 0;
        public ushort InstanceId { get => _InstanceId; }

        public RemoteInstanceMap(int InstanceId, RemoteZone Zone, FieldInfo mInfo) : base(Zone, mInfo)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}