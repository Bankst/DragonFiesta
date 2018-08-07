using System.Collections.Concurrent;

namespace DragonFiesta.Zone.Game.Zone
{
    [ServerModule(ServerType.Zone, InitializationStage.PreData)]
    public class ZoneManager
    {
        private static ConcurrentDictionary<byte, RemoteZone> ZonesById;


        public static void Dispose() => ZonesById.Clear();
        
 

        public static int RemoteZoneCount() => ZonesById == null ? -1 : ZonesById.Count;

        public static bool GetRemoteZoneByID(byte ID, out RemoteZone Zone) => ZonesById.TryGetValue(ID, out Zone);

        public static bool AddRemoteZone(RemoteZone Zone) => ZonesById.TryAdd(Zone.ID, Zone);

        public static bool RemoveRemoteZone(byte Id, out RemoteZone Zone) => ZonesById.TryRemove(Id, out Zone);

        [InitializerMethod]
        public static bool InitializeZoneManager()
        {
            ZonesById = new ConcurrentDictionary<byte, RemoteZone>();

            return true;
        }
    }
}