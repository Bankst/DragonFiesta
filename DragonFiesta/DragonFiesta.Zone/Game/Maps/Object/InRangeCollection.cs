using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps.Event;
using System;
using System.Collections.Generic;

namespace DragonFiesta.Zone.Game.Maps.Object
{
    public sealed class InRangeCollection
    {
        public IMapObject Owner { get; private set; }

        public event EventHandler<MapObjectEventArgs> OnObjectAdded;

        public event EventHandler<MapObjectEventArgs> OnObjectRemoved;

        private SecureCollection<IMapObject> Objects;
        private SecureCollection<ZoneCharacter> Characters;
        private object ThreadLocker;

        public InRangeCollection(ILivingObject Owner)
        {
            this.Owner = Owner;
            Objects = new SecureCollection<IMapObject>();
            Characters = new SecureCollection<ZoneCharacter>();
            ThreadLocker = new object();
            Owner.OnMapChanged += On_Owner_MapChanged;
            Owner.OnMapSectorChanged += On_Owner_MapSectorChanged;
        }

        public void Dispose()
        {
            Owner = null;
            OnObjectAdded = null;
            OnObjectRemoved = null;
            Objects.Dispose();
            Objects = null;
            Characters.Dispose();
            Characters = null;
            ThreadLocker = null;
        }

        public void Refresh()
        {
            lock (ThreadLocker)
            {
                //remove all objects
                Clear(false);
                //get new objects which are in range of owner
                if (Owner.MapSector != null)
                {
                    var objects = Owner.MapSector.GetObjects(true, Owner);
                    for (int i = 0; i < objects.Length; i++)
                    {
                        var obj = objects[i];
                        if (obj.IsDisposed)
                            continue;
                        AddObject(obj, true);
                    }
                    objects = null;
                }
            }
        }

        public void Broadcast(FiestaPacket Packet)
        {
            CharacterAction((character) =>
            {
                character.Session.SendPacket(Packet);
            }, true);
        }

        public void CharacterAction(Action<ZoneCharacter> Action, bool OnlyConnected = true)
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < Characters.Count; i++)
                {
                    var character = Characters[i];
                    try
                    {
                        if (OnlyConnected
                            && !character.IsConnected)
                            continue;
                        Action.Invoke(character);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        public IMapObject[] GetObjects(params IMapObject[] Exclude)
        {
            var list = new List<IMapObject>();

            lock (ThreadLocker)
            {
                list.AddRange(Objects);
            }

            for (int i = 0; i < Exclude.Length; i++)
            {
                list.Remove(Exclude[i]);
            }
            return list.ToArray();
        }

        public ZoneCharacter[] GetCharacters(params ZoneCharacter[] Exclude)
        {
            var list = new List<ZoneCharacter>();

            lock (ThreadLocker)
            {
                list.AddRange(Characters);
            }

            for (int i = 0; i < Exclude.Length; i++)
            {
                list.Remove(Exclude[i]);
            }
            return list.ToArray();
        }

        public ILivingObject[] GetLivingObjectsByDistance(double MaxDistance, int MaxObjects = int.MaxValue, params ILivingObject[] Exclude)
        {
            var list = new List<ILivingObject>();

            lock (ThreadLocker)
            {
                for (int i = 0; i < Objects.Count && list.Count < MaxObjects; i++)
                {
                    var obj = Objects[i];

                    if (!(obj is ILivingObject)
                        || Position.GetDistance(obj.Position, Owner.Position) > MaxDistance)
                        continue;
                    list.Add((obj as ILivingObject));
                }
            }

            for (int i = 0; i < Exclude.Length; i++)
            {
                list.Remove(Exclude[i]);
            }
            return list.ToArray();
        }

        public void Clear(bool InvokeEvent = true)
        {
            lock (ThreadLocker)
            {
                for (int i = (Objects.Count - 1); i >= 0; i--)
                {
                    RemoveObject(Objects[0], InvokeEvent);
                }
            }
        }

        private void On_Owner_MapChanged(object sender, MapChangedEventArgs args)
        {
            Refresh();
        }

        private void On_Owner_MapSectorChanged(object sender, MapSectorChangedEventArgs args)
        {
            lock (ThreadLocker)
            {
                //remove all objects which are out of range
                if (Objects.Count > 0)
                {
                    var toRemove = new List<IMapObject>();
                    for (int i = 0; i < Objects.Count; i++)
                    {
                        var obj = Objects[i];

                        if (obj.IsDisposed
                            || obj.MapSector == null
                            || args.NewSector.IsOutOfRange(obj.MapSector))
                        {
                            toRemove.Add(obj);
                        }
                    }
                    for (int i = 0; i < toRemove.Count; i++)
                    {
                        var obj = toRemove[i];

                        RemoveObject(obj, true);
                    }
                    toRemove.Clear();
                    toRemove = null;
                }
                //get new objects
                var newObjects = args.NewSector.GetObjects(true);

                for (int i = 0; i < newObjects.Length; i++)
                {
                    var obj = newObjects[i];

                    if (obj == Owner)
                        continue;
                    AddObject(obj, true);
                }
            }
        }

        private void On_MapObject_Dispose(object sender, MapObjectEventArgs args)
        {
            RemoveObject(args.MapObject, true);
        }

        private void AddObject(IMapObject Object, bool InvokeEvent)
        {
            if (Object.IsDisposed)
                return;
            lock (ThreadLocker)
            {
                if (Objects.Add(Object))
                {
                    if (Object is ZoneCharacter)
                    {
                        Characters.Add((Object as ZoneCharacter));
                    }
                    //bind event to object
                    Object.OnDispose += On_MapObject_Dispose;
                    //add owner to Object
                    Object.InRange.AddObject(Owner, true);

                    if (InvokeEvent
                        && OnObjectAdded != null)
                    {
                        var args = new MapObjectEventArgs(Object);

                        OnObjectAdded.Invoke(this, args);
                    }
                }
            }
        }

        private void RemoveObject(IMapObject Object, bool InvokeEvent)
        {
            lock (ThreadLocker)
            {
                Object.OnDispose -= On_MapObject_Dispose;
                if (Objects.Remove(Object))
                {
                    if (Object is ZoneCharacter)
                    {
                        Characters.Remove((Object as ZoneCharacter));
                    }
                    //remove owner from object
                    if (!Object.IsDisposed)
                    {
                        Object.InRange.RemoveObject(Owner, true);
                    }

                    if (InvokeEvent
                        && OnObjectRemoved != null)
                    {
                        var args = new MapObjectEventArgs(Object);
                        OnObjectRemoved.Invoke(this, args);
                    }
                }
            }
        }
    }
}