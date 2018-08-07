using DragonFiesta.Providers.Maps;

namespace DragonFiesta.Zone.Game.Maps.Types
{
    public class NormalMap : LocalMap
    {
        public NormalMap(FieldInfo mInfo) : base(mInfo)
        {
        }
        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}