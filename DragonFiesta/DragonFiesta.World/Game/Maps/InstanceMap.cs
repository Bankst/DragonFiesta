using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Zone;
using System;
using System.Runtime.Serialization;

namespace DragonFiesta.World.Game.Maps
{
    [Serializable]
    public class InstanceMap : WorldServerMap, IInstanceMap
    {
        private ushort _InstanceId = 0;

        public ushort InstanceId => _InstanceId;

        public InstanceMap(ZoneServer Server, FieldInfo mInfo, ushort InstanceId) : base(Server, mInfo)
        {
            _InstanceId = InstanceId;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_InstanceId", InstanceId);
            base.GetObjectData(info, context);
        }

        protected InstanceMap(SerializationInfo Sinfo, StreamingContext context) : base(Sinfo, context)
        {
            _InstanceId = Sinfo.GetUInt16("_InstanceId");
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();


        }
    }
}