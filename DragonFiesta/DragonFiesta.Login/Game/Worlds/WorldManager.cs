using DragonFiesta.Login.Data;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace DragonFiesta.Login.Game.Worlds
{
    [ServerModule(ServerType.Login, InitializationStage.Data)]
    public class WorldManager
    {
        #region Property
        public static WorldManager Instance { get; set; }

        private ConcurrentDictionary<byte, World> WorldByIDs;
        public SecureCollection<World> WorldList { get; private set; }

        public WorldManager()
        {
            WorldByIDs = new ConcurrentDictionary<byte, World>();
            WorldList = new SecureCollection<World>();

            LoadWorldListFromDatabase();
        }

        [InitializerMethod]
        public static bool Initialize()
        {
            try
            {
                Instance = new WorldManager();
                return true;
            }
            catch (Exception ex)
            {
                EngineLog.Write(EngineLogLevel.Exception, "Failed to Start WorldManager", ex);
                return false;
            }
        }
        #endregion

        #region GetMethods
        public bool GetWorldByID(byte Id, out World pWorld)
            => WorldByIDs.TryGetValue(Id, out pWorld);

        #endregion

        #region Network

        public void Broadcast(IMessage Message, params byte[] Exclude) =>
       WorldServerAction((World) =>
       {


           if (Exclude.Contains(World.Info.WorldID))
               return;

           World.SendMessage(Message, false);
       });

        public void WorldServerAction(Action<World> Action, Predicate<World> Match) =>
            WorldServerAction((client) =>
            {
                if (Match.Invoke(client))
                {
                    Action.Invoke(client);
                }
            });

        public void WorldServerAction(Action<World> Action)
        {
            foreach (var World in WorldList.Where(mW => mW.IsConnected))
            {
                try
                {
                    Action.Invoke(World);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        #endregion

        #region DataMethods
        public void LoadWorldListFromDatabase()
        {
            try
            {
                SQLResult pResult = DB.Select(DatabaseType.Login, "SELECT * FROM WorldList");
                DatabaseLog.WriteProgressBar(">> Load World Infos");
                using (ProgressBar mBar = new ProgressBar((pResult.Count)))
                {
                    for (int i = 0; i < pResult.Count; i++)
                    {
                        World pWorld = new World(new WorldInfo(pResult, i));

                        if (!WorldByIDs.TryAdd(pWorld.Info.WorldID, pWorld)
                            || !WorldList.Add(pWorld))
                        {
                            DatabaseLog.Write(DatabaseLogLevel.Warning, "Duplicate World {0} Found  ", pWorld.Info);
                        }
                        mBar.Step();
                    }
                    DatabaseLog.WriteProgressBar(">> Loaded {0} Worlds", WorldByIDs.Count);
                }
            }
            catch (Exception ex)
            {
                DatabaseLog.Write(ex, "Failed to Load WorldList");
            }
        }
        #endregion

    }
}