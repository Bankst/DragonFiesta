using DragonFiesta.Zone.Data.Mob;
using DragonFiesta.Zone.Game.Maps.Types;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DragonFiesta.Zone.Game.Mobs
{
    public class MobGroup : IDisposable
    {
        public int ID { get; private set; }

        public ConcurrentDictionary<ushort, MobGroupMembers> Members { get; private set; }

        public MobGroupInfo Info { get; private set; }

        public LocalMap Map { get; private set; }

        private List<Position> WalkPositions { get; set; }

        public uint MinX { get; private set; }
        public uint MaxX { get; private set; }

        public uint MinY { get; private set; }
        public uint MaxY { get; private set; }

        private RandomMT Random;

        public MobGroup(MobGroupInfo Info, LocalMap Map)
        {
            Members = new ConcurrentDictionary<ushort, MobGroupMembers>();
            this.Info = Info;
            this.Map = Map;

            MinX = (uint)(Info.Center.X - Math.Max(Info.Width, Info.Range));
            MaxX = (uint)(Info.Center.X + Math.Max(Info.Width, Info.Range));

            MinY = (uint)(Info.Center.Y - Math.Max(Info.Width, Info.Range));
            MaxY = (uint)(Info.Center.Y + Math.Max(Info.Width, Info.Range));

            WalkPositions = Map.BlockInfos.WalkPositions.FindAll(p => p.X <= MaxX
                                                                 && p.X >= MinX

                                                                 && p.Y <= MaxY
                                                                 && p.Y >= MinY);

            Random = new RandomMT(true);
        }

        //Center x center y
        public void Update()
        {
            foreach (var Member in Members.Values)
            {
                Member.Update();
            }
        }

        public bool Spawn()
        {
            foreach (var Memb in Info.MemberInfo.Values)
            {
                if (Members.ContainsKey(Memb.MobInfo.ID))
                    return false;

                var Member = new MobGroupMembers(Memb, this);

                if (!Members.TryAdd(Memb.MobInfo.ID, Member)) //Need To Life?? or check by Data start?
                {
                    EngineLog.Write(EngineLogLevel.Warning, "Duplicate MobId  {0} in Group {1} found!!", Memb.MobInfo.ID, Memb.GroupId);
                    continue;
                }

                if (!Member.SpawnMemebers())
                    return false;
            }

            return true;
        }

        public void RemoveFromMap()
        {
            foreach (var Member in Members)
            {
                Member.Value.RemoveAllMember();
            }
            Members.Clear();
        }

        public Position GetRandomWalkPosition(Position BasePosition)
        {
            switch (WalkPositions.Count)
            {
                case 0:
                    return BasePosition;

                case 1:
                    return WalkPositions[0];

                default:
                    Position NewPosition = WalkPositions[Random.RandomRange(0, (WalkPositions.Count - 1))];

                    for (int i = 0; i < 1000; i++)
                    {
                        if (NewPosition.X != BasePosition.X
                            && NewPosition.Y != BasePosition.Y)
                            break;

                        NewPosition = WalkPositions[Random.RandomRange(0, (WalkPositions.Count - 1))];
                    }
                    NewPosition.Rotation = (byte)Random.RandomRange(byte.MinValue, byte.MaxValue);

                    return NewPosition;
            }
        }

        public void Dispose()
        {
            RemoveFromMap();
            Map = null;
            Info = null;
            Members = null;
        }
    }
}