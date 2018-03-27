using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Maps.Event;
using DragonFiesta.Zone.Network.FiestaHandler.Server;

namespace DragonFiesta.Zone.Game.Maps.Object
{
    public class MapSector
    {
        public SectorMap Map { get; private set; }

        public uint X { get; private set; }
        public uint Y { get; private set; }

        public SecureWriteCollection<MapSector> SurroundingSectors { get; private set; }

        /// <summary>
        /// MapObjects
        /// </summary>
        ///

        private ConcurrentDictionary<ushort, IMapObject> ObjectsByID;
        private List<IMapObject> ObjectList;
        private List<ZoneCharacter> CharacterList;

        private int IsDisposedInt;
        private object ThreadLocker;

        public MapSector(SectorMap Map, uint X, uint Y)
        {
            this.Map = Map;
            this.X = X;
            this.Y = Y;
        }

        public void Load()
        {
            SurroundingSectors = new SecureWriteCollection<MapSector>(out Func<MapSector, bool> addFunc, out Func<MapSector, bool> removeFunc, out Action clearFunc);

            for (int x = (int)(X - 1); x <= X + 1; x++)
            {
                for (int y = (int)(Y - 1); y <= Y + 1; y++)
                {
                    if (!x.IsBetween(0, (int)(Map.SectorCount - 1))
                        || !y.IsBetween(0, (int)(Map.SectorCount - 1))
                        || (x == X
                        && y == Y))
                    {
                        continue;
                    }

                    addFunc.Invoke(Map.Sectors[y, x]);
                }
            }

            CharacterList = new List<ZoneCharacter>();
            ObjectsByID = new ConcurrentDictionary<ushort, IMapObject>();
            ObjectList = new List<IMapObject>();
            ThreadLocker = new object();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                Map = null;

                SurroundingSectors.Dispose();
                SurroundingSectors = null;

                ObjectsByID.Clear();
                ObjectsByID = null;

                ObjectList.Clear();
                ObjectList = null;

                CharacterList.Clear();
                CharacterList = null;

            }
        }

        internal void AddObject(IMapObject Object)
        {
            lock (ThreadLocker)
            {
                if (ObjectsByID.TryAdd(Object.MapObjectId, Object))
                {
                    ObjectList.Add(Object);

                    switch (Object.Type)
                    {
                        case MapObjectType.Character:
                            CharacterList.Add((Object as ZoneCharacter));
                            break;
                    }

                    if (Object is ILivingObject)
                    {
                        (Object as ILivingObject).OnMove += On_LivingObject_Move;
                    }
                }
            }
        }

        internal void RemoveObject(IMapObject Object)
        {
            lock (ThreadLocker)
            {
                if (ObjectsByID.TryRemove(Object.MapObjectId, out Object))
                {
                    ObjectList.Remove(Object);

                    switch (Object.Type)
                    {
                        case MapObjectType.Character:
                            CharacterList.Remove((Object as ZoneCharacter));
                            break;
                    }

                    if (Object is ILivingObject)
                    {
                        (Object as ILivingObject).OnMove -= On_LivingObject_Move;
                    }
                }
            }
        }

        private void On_LivingObject_Move(object sender, LivingObjectMovementEventArgs args)
        {
            lock (ThreadLocker)
            {

                //check if object moved to new sector
                var sector = Map.GetSectorByPosition(args.NewPosition);

                if (sector != this)
                {
                    //remove from this sector
                    RemoveObject(args.Object);

                    //update object
                    args.Object.MapSector = sector;

                    //add to new sector
                    sector.AddObject(args.Object);
                }

                //broadcast movement packet

                SendMovePacket(args);
            }
        }

        private static void SendMovePacket(LivingObjectMovementEventArgs args)
        {
            FiestaPacket movePacket;

            if (args.IsStop)
            {
                movePacket = SH08Handler.GetStopPacket(args.Object, args.NewPosition);
            }
            else
            {
                ushort speed = 115;

                switch (args.Object.Type)
                {
                    case MapObjectType.Character:

                        switch ((args.Object as ZoneCharacter).State)
                        {
                            case CharacterState.OnMount:
                                // speed = (args.IsRun ? (args.Object as ZoneCharacter).CurrentMount.Item.MountInfo.RunSpeed : (args.Object as ZoneCharacter).CurrentMount.Item.MountInfo.WalkSpeed);
                                /*
                                //check if food is used
                                if ((args.Object as ZoneCharacter).CurrentMount.Item.MountInfo.Feed != null)
                                {
                                    if ((args.Object as ZoneCharacter).CurrentMount.Item.Food < 1)
                                    {
                                        //decrease speed by 70%
                                        speed -= (ushort)((double)speed / 100d * 70d);
                                    }
                                }*/
                                break;

                            case CharacterState.Player:
                                speed = 115;//(args.IsRun ? args.Object.Stats.FullStats.RunSpeed : args.Object.Stats.FullStats.WalkSpeed);
                                break;

                            case CharacterState.Dead:
                            case CharacterState.Resting:
                            case CharacterState.Vendor:
                            default:
                                return;
                        }

                        break;

                    case MapObjectType.NPC:
                        speed = 10;//(args.IsRun ? (args.Object as NPC).Stats.FullStats.RunSpeed : (args.Object as NPC).Stats.FullStats.WalkSpeed);
                        break;

                    case MapObjectType.Mob:
                        speed = 30; //(args.IsRun ? (args.Object as iMob).Stats.FullStats.RunSpeed : (args.Object as iMob).Stats.FullStats.WalkSpeed);
                        break;

                    default:
                        speed = 115;//(args.IsRun ? CharacterDataProvider.ChrCommon.RunSpeed : CharacterDataProvider.ChrCommon.WalkSpeed);
                        break;
                }

                movePacket = SH08Handler.GetMovePacket(args.Object, args.OldPosition.X, args.OldPosition.Y, args.IsRun, speed);
            }

            using (movePacket)
            {
                args.Object.Broadcast(movePacket, false);
            }
        }

        public IMapObject[] GetObjects(bool FromSurroundingSectors, params IMapObject[] Exclude)
        {
            var list = new List<IMapObject>();

            lock (ThreadLocker)
            {
                list.AddRange(ObjectList);

                if (FromSurroundingSectors)
                {
                    for (int i = 0; i < SurroundingSectors.Count; i++)
                    {
                        list.AddRange(SurroundingSectors[i].ObjectList);
                    }
                }
            }

            for (int i = 0; i < Exclude.Length; i++)
            {
                list.Remove(Exclude[i]);
            }

            return list.ToArray();
        }

        public ZoneCharacter[] GetCharacters(bool FromSurroundingSectors, params ZoneCharacter[] Exclude)
        {
            var list = new List<ZoneCharacter>();

            lock (ThreadLocker)
            {
                list.AddRange(CharacterList);

                if (FromSurroundingSectors)
                {
                    for (int i = 0; i < SurroundingSectors.Count; i++)
                    {
                        list.AddRange(SurroundingSectors[i].CharacterList);
                    }
                }
            }

            for (int i = 0; i < Exclude.Length; i++)
            {
                list.Remove(Exclude[i]);
            }

            return list.ToArray();
        }

        public void Broadcast(FiestaPacket Packet, bool ToSurroundingSectors = true, params ZoneCharacter[] Exclude)
        {
            lock (ThreadLocker)
            {
                CharacterAction((character) =>
                {
                    character.Session.SendPacket(Packet);
                }, true, Exclude);

                if (ToSurroundingSectors)
                {
                    for (int i = 0; i < SurroundingSectors.Count; i++)
                    {
                        SurroundingSectors[i].Broadcast(Packet, false, Exclude);
                    }
                }
            }
        }

        public void CharacterAction(Action<ZoneCharacter> Action, bool OnlyConnected = true, params ZoneCharacter[] Exclude)
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < CharacterList.Count; i++)
                {
                    var character = CharacterList[i];

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

        public bool IsOutOfRange(MapSector OtherSector)
        {
            if (!(OtherSector is MapSector))
                return true;

            return (OtherSector != this && !SurroundingSectors.Contains((OtherSector as MapSector)));
        }
    }
}