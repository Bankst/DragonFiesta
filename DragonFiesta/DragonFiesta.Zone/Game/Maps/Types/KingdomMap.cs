using DragonFiesta.Providers.Maps;

namespace DragonFiesta.Zone.Game.Maps.Types
{
    public class KingdomMap : InstanceMap
    {
        public KingdomMap(ushort InstanceId, FieldInfo mInfo) : base(InstanceId, mInfo)
        {
        }
        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}