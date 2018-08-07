using DragonFiesta.Providers.Maps;

namespace DragonFiesta.Zone.Game.Maps.Types
{
    public class InstanceMap : LocalMap, IInstanceMap
    {
        private ushort _InstanceId = 0;
        public ushort InstanceId { get => _InstanceId; }

        public InstanceMap(ushort InstanceId, FieldInfo mInfo) : base(mInfo)
        {
            _InstanceId = InstanceId;
        }
        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}