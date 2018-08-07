using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps.Object;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DragonFiesta.Zone.Game.Maps.Types
{
    public class ZoneServerMap : SectorMap, IMap, ISerializable
    {
        private ConcurrentDictionary<ushort, MapObject> ObjectsByID;

        private List<ZoneCharacter> Characters;

        public ZoneServerMap(FieldInfo mInfo) : base(mInfo)
        {
            ObjectsByID = new ConcurrentDictionary<ushort, MapObject>();
            Characters = new List<ZoneCharacter>();
        }

        public void Save()
        {
        }

        protected override void DisposeInternal()
        {

            base.DisposeInternal();

            Characters.Clear();
            Characters = null;

            ObjectsByID.Clear();
            ObjectsByID = null;

        }
    }
}