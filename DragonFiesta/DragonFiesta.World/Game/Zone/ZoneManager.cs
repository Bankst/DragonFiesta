using DragonFiesta.Game.Zone;
using DragonFiesta.World.Config;
using DragonFiesta.World.InternNetwork;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.World.Game.Zone
{
    [ServerModule(ServerType.World, InitializationStage.InternNetwork)]
    public class ZoneManager
    {
        private static ConcurrentDictionary<byte, ZoneServer> ZonesByID;

        private static List<IZone> ZoneList;

        [InitializerMethod]
        public static bool InitialZoneManager()
        {
            ZonesByID = new ConcurrentDictionary<byte, ZoneServer>();
            ZoneList = new List<IZone>();

            for (byte i = 0; i < WorldConfiguration.Instance.InternalServerInfo.MaxConnection; i++)
            {
                var zone = new ZoneServer(i, null);
                ZonesByID.TryAdd(i, zone);
                ZoneList.Add(zone);
            }

            GameLog.Write(GameLogLevel.Startup, "Awaiting {0} zones.{1}", ZoneList.Count, Environment.NewLine);
            return true;
        }

        public static int CountOfSessions() => ZoneList == null ? 0 : ZoneList.Count(m => m.IsConnected);

        public static bool GetZoneByID(byte ID, out ZoneServer Zone) => ZonesByID.TryGetValue(ID, out Zone);

        public static List<IZone> FindAllActiveZone() => ZoneList?.FindAll(m => m.IsConnected);

   
        public static void Broadcast(IMessage Message, params byte[] Exclude) =>
            ZoneServerAction((Zone) =>
            {


                if (Exclude.Contains(Zone.ID))
                    return;


                if (Message is IExpectAnAnswer &&
                InternWorldHandlerStore.Instance.IsCallbackContains(Message.Id))
                {
                    Zone.Send(Message, false);
                    return;
                }

                Zone.Send(Message);
            });

      

        public static void ZoneServerAction(Action<ZoneServer> Action, Predicate<ZoneServer> Match) =>
            ZoneServerAction((client) =>
            {
                if (Match.Invoke(client))
                {
                    Action.Invoke(client);
                }
            });

        public static void ZoneServerAction(Action<ZoneServer> Action)
        {
            for (int i = 0; i < ZoneList.Count; i++)
            {
                try
                {
                    Action.Invoke(ZoneList[i] as ZoneServer);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
  

    }
}