using DragonFiesta.Zone.Data.Mob;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps.Event;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using System.Collections.Concurrent;
using System.Linq;
using System;
using System.Threading;

namespace DragonFiesta.Zone.Game.Mobs
{
    public class MobGroupManager : IUpdateAbleServer
    {
        private LocalMap Map;

        private ConcurrentDictionary<int, MobGroup> SpawnnedGroupList;

        TimeSpan IUpdateAbleServer.UpdateInterval => TimeSpan.FromMilliseconds((int)ServerTaskTimes.MAP_GROUP_UPDATE_INTERVAL);

        public GameTime LastUpdate { get; private set; }


        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;


        public MobGroupManager(LocalMap Map)
        {
            SpawnnedGroupList = new ConcurrentDictionary<int, MobGroup>();

            this.Map = Map;
            Map.OnObjectAdded += Map_OnObjectAdded;
            Map.OnObjectRemoved += Map_OnObjectRemoved;

            LastUpdate = GameTime.Now();
        }

        private void Map_OnObjectRemoved(object sender, MapObjectEventArgs e)
        {
            // Its Possibel remove Multiobjects???? hmm
        }

        private void Map_OnObjectAdded(object sender, MapObjectEventArgs e)
        {
            if (e.MapObject.Type == MapObjectType.Character && e.MapObject is ZoneCharacter character)
            {
                var MobsList = e.MapObject.InRange.GetObjects().Where(o => o.Type == MapObjectType.Mob).ToArray();

                SH07Handler.SpawnMultiObject(MobsList, false, (packet) =>
                {
                    character.Session.SendPacket(packet);
                }, 255);
            }
        }

        public bool RemoveGroup(int GroupId)
        {
            if (!SpawnnedGroupList.TryGetValue(GroupId, out MobGroup Result))
                return false;

            Result.Dispose();

            return true;
        }

        public bool Start()
        {
            if (MobDataProvider.GetMobGroupsByMapId(Map.MapId, out ConcurrentDictionary<int, MobGroupInfo> GroupsList))
            {
                foreach (var Group in GroupsList)
                {
                    if (!AddMobGroup(Group.Value))
                        return false;
                }
            }

            return true;
        }

        public bool AddMobGroup(MobGroupInfo Info)
        {
            var MapGroup = new MobGroup(Info, Map);
            if (SpawnnedGroupList.TryAdd(Info.GroupId, MapGroup))
            {
                return MapGroup.Spawn();
            }
            return false;
        }




        bool IUpdateAbleServer.Update(GameTime gameTime)
        {
            if (IsDisposed) return false;

            foreach (var grp in SpawnnedGroupList.Values)
            {
                grp.Update();
            }

            LastUpdate = GameTime.Now();

            return true;
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                SpawnnedGroupList.Clear();
                SpawnnedGroupList = null;

                Map = null;
            }
        }
    }
}