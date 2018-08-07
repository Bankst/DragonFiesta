using DragonFiesta.Providers.Maps;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps.Event;
using DragonFiesta.Zone.Game.Mobs;
using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Maps.Chat;
using DragonFiesta.Zone.Game.Maps.Object;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Zone.Game.Maps.Types
{
    public class LocalMap : ZoneServerMap
    {
        public SecureWriteCollection<IMapObject> Objects { get; private set; }

        public event EventHandler<MapObjectEventArgs> OnObjectAdded;

        public event EventHandler<MapObjectEventArgs> OnObjectRemoved;

        public MapChat MapChat { get; private set; }

        public MapShout MapShout { get; private set; }

        private ConcurrentDictionary<ushort, IMapObject> ObjectsByID;
        private List<ZoneCharacter> Characters;

        private ConcurrentQueue<ushort> AvailableObjectIDs;

        private Func<IMapObject, bool> ObjectsAddFunc;
        private Func<IMapObject, bool> ObjectsRemoveFunc;
        private Action ObjectsClearFunc;

        private DisplayManager DisplayManager { get; set; }
        private MobGroupManager GroupSpawn { get; set; }

        private NPCManager NPCSpawn { get; set; }

        public LocalMap(FieldInfo mInfo) : base(mInfo)
        {
        }

        public override bool Start()
        {
            Objects = new SecureWriteCollection<IMapObject>(out ObjectsAddFunc, out ObjectsRemoveFunc, out ObjectsClearFunc);

            ObjectsByID = new ConcurrentDictionary<ushort, IMapObject>();
            Characters = new List<ZoneCharacter>();

            AvailableObjectIDs = new ConcurrentQueue<ushort>();
            for (ushort i = 50000; i <= ushort.MaxValue; i++)
            {
                if (i == ushort.MinValue)
                    break;

                AvailableObjectIDs.Enqueue(i);
            }

            MapChat = new MapChat(this);
            MapShout = new MapShout(this);

            DisplayManager = new DisplayManager(this);
            GroupSpawn = new MobGroupManager(this);
            NPCSpawn = new NPCManager(this);

            if (!base.Start())
                return false;

            if (!GroupSpawn.Start())
                return false;

            if (!NPCSpawn.Start())
                return false;

            ThreadPool.AddUpdateAbleServer(GroupSpawn);
            ThreadPool.AddUpdateAbleServer(NPCSpawn);

            return true;
        }

        public bool GetObjectsInRange(uint X, uint Y, double Distance, out ILivingObject[] Result, int MaxObjects = int.MaxValue)
        {
            Result = null;

            try
            {
                var list = new List<ILivingObject>();

                lock (ThreadLocker)
                {
                    for (int i = 0; i < Objects.Count && list.Count <= MaxObjects; i++)
                    {
                        try
                        {
                            var obj = Objects[i];

                            if (obj.IsDisposed
                                || !(obj is ILivingObject)
                                || Position.GetDistance(X, obj.Position.X, Y, obj.Position.Y) > Distance)
                                continue;

                            list.Add((obj as ILivingObject));
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }

                Result = list.ToArray();

                return true;
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Error getting objects in range (X: {0}, Y: {1}, Distance: {2}):", X, Y, Distance);
                return false;
            }
        }

        public bool GetObjectByID(ushort ID, out IMapObject Object) => ObjectsByID.TryGetValue(ID, out Object);

        public bool AddObject(IMapObject Object, bool AssignID = true)
        {
            try
            {
                if (Object.Type != MapObjectType.NPC)
                    GameLog.Write(GameLogLevel.Debug, "[{0}] Adding object to map. Type: {1}, X: {2}, Y: {3}", Info.MapInfo.Index, Object.Type, Object.Position.X, Object.Position.Y);

                lock (ThreadLocker)
                {
                    //get mapsector
                    if (!CheckPositionInMap(Object.Position))//Check Position
                    {
                        GameLog.Write(GameLogLevel.Warning, "[{0}] Can't Adding Object to map Type: {1}, X: {2}, Y: {3} Position was out of Range ", Info.MapInfo.Index, Object.Type, Object.Position.X, Object.Position.Y);
                        return false;
                    }

                    var sector = GetSectorByPosition(Object.Position);

                    //check if we need to assign an object id
                    if (AssignID)
                    {
                        if (!AvailableObjectIDs.TryDequeue(out ushort ID))
                        {
                            GameLog.Write(GameLogLevel.Warning, "Object ID overflow on map '{0}'.", Info.MapInfo.Index);
                            return false;
                        }

                        Object.MapObjectId = ID;

                        //if (Object.Type != MapObjectType.NPC)
                        //    GameLog.Write(GameLogType.Debug, "[{0}] Assigned ID {1} to object.", FieldInfo.MapInfo.Index, Object.ID);
                    }

                    //if (Object.Type != MapObjectType.NPC)
                    //    GameLog.Write(GameLogType.Debug, "[{0}] Adding object {1} to sector: {2}:{3}", FieldInfo.MapInfo.Index, Object.ID, sector.X, sector.Y);

                    //add to map sector
                    sector.AddObject(Object);

                    //update object
                    Object.MapSector = sector;
                    Object.Map = this;

                    if (ObjectsAddFunc.Invoke(Object)
                        && ObjectsByID.TryAdd(Object.MapObjectId, Object))
                    {
                        switch (Object.Type)
                        {
                            case MapObjectType.Character:
                                Characters.Add((Object as ZoneCharacter));
                                break;
                        }

                        FinalizeObjectAdd(Object, AssignID);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Error adding object to map:");
                return false;
            }
        }

        protected virtual void FinalizeObjectAdd(IMapObject Object, bool IDAssigned)
        {
            if (OnObjectAdded != null)
            {
                var args = new MapObjectEventArgs(Object);

                OnObjectAdded.Invoke(this, args);
            }
        }

        public bool RemoveObject(IMapObject Object, bool FreeID = true)
        {
            try
            {
                lock (ThreadLocker)
                {
                    if (Object.Type != MapObjectType.NPC)
                        GameLog.Write(GameLogLevel.Debug, "[{0}] remove object to map. Type: {1}, X: {2}, Y: {3}", Info.MapInfo.Index, Object.Type, Object.Position.X, Object.Position.Y);

                    if (ObjectsRemoveFunc.Invoke(Object)
                        && ObjectsByID.TryRemove(Object.MapObjectId, out Object))
                    {
                        switch (Object.Type)
                        {
                            case MapObjectType.Character:
                                Characters.Remove((Object as ZoneCharacter));
                                break;

                            case MapObjectType.Mob:
                                //MobSpawns.RemoveMob((Object as Mob));
                                break;
                        }

                        //remove from map sector
                        (Object.MapSector as MapSector).RemoveObject(Object);

                        //clear objects in range
                        Object.InRange.Clear(false);

                        if (FreeID)
                        {
                            AvailableObjectIDs.Enqueue(Object.MapObjectId);
                        }

                        FinalizeObjectRemove(Object, FreeID);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                GameLog.Write(ex, "Error removing object from map:");
                return false;
            }
        }

        protected virtual void FinalizeObjectRemove(IMapObject Object, bool IDFreed) => OnObjectRemoved?.Invoke(this, new MapObjectEventArgs(Object));

        public void Broadcast(FiestaPacket Packet, params ZoneCharacter[] Exclude)
        {
            CharacterAction((character) =>
            {
                character.Session.SendPacket(Packet);
            }, true, Exclude);
        }

        public void CharacterAction(Action<ZoneCharacter> Action, bool OnlyConnected = true, params ZoneCharacter[] Exclude)
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    var character = Characters[i];

                    if ((OnlyConnected
                        && !character.IsConnected)
                        || Exclude.Contains(character))
                        continue;

                    try
                    {
                        Action.Invoke(character);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();

            Objects.Dispose();

            OnObjectAdded = null;
            OnObjectRemoved = null;

            MapChat.Dispose();
            MapChat = null;

            MapShout.Dispose();
            MapShout = null;


            DisplayManager.Dispose();
            DisplayManager = null;

            ObjectsByID.Clear();
            ObjectsByID = null;


            Characters.Clear();
            Characters = null;

            AvailableObjectIDs = null;

            GroupSpawn.Dispose();
            GroupSpawn = null;

            NPCSpawn.Dispose();
            NPCSpawn = null;


        }
    }
}