using DragonFiesta.Game.Zone;
using DragonFiesta.Utils.Config.Section.Network;
using System;
using System.Runtime.Serialization;

namespace DragonFiesta.Zone.Game.Zone
{
    public sealed class RemoteZone : IZone
    {
        public byte ID { get; private set; }
        bool IZone.IsConnected { get { return true; } }
        bool IZone.IsReady { get { return true; } }

        public ExternServerInfo NetInfo { get; private set; }

        public int CurrentConnection { get; set; }

        public RemoteZone(byte ID, ExternServerInfo NetInfo)
        {
            this.ID = ID;
            this.NetInfo = NetInfo;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new NotImplementedException();
        }
    }
}