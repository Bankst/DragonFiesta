using DragonFiesta.Zone.Data.Mob;
using System;
using System.Collections.Generic;

namespace DragonFiesta.Zone.Game.Mobs
{
    public class MobGroupMembers
    {
        public List<Mob> MemberList;

        private MobGroupMemberInfo Info { get; set; }

        private MobGroup Group { get; set; }

        public MobGroupMembers(MobGroupMemberInfo Info, MobGroup Group)
        {
            MemberList = new List<Mob>();
            this.Info = Info;
            this.Group = Group;
        }

        public void Update()
        {
            if (MemberList.Count < this.Info.MobCount)
            {
                AddMember(new Mob(Info.MobInfo));
            }

            foreach (var mob in MemberList)
            {
                if (!mob.IsAlive && DateTime.Now.Subtract(mob.LastMovment).Seconds >= 4)//Die Anim
                {
                    Group.Map.RemoveObject(mob, true);
                    MemberList.Remove(mob);
                }
                else if (DateTime.Now.Subtract(mob.LastMovment).TotalMilliseconds >= Info.MobInfo.Stats.WalkSpeed)//Move Mobs
                {
                    mob.LastMovment = DateTime.Now;
                    mob.Chat.Peace();

                    if (Info.HasWayPoint)
                        mob.MoveToNextPoint();
                    else
                        mob.Move(Group.GetRandomWalkPosition(Group.Info.Center), false, false);
                }

                mob.LastUpdate = DateTime.Now;

            }
        }
        public bool SpawnMemebers()
        {
            while (MemberList.Count < Info.MobCount)
            {
                if (!AddMember(new Mob(Info.MobInfo)))
                    return false;
            }

            return true;
        }

        public bool AddMember(Mob Mob)
        {
            Mob.Position = Group.GetRandomWalkPosition(Group.Info.Center);

            if (Info.HasWayPoint)
            {
                Mob.WalkPosition = Info.WayPointInfo.WalkPosition;
                Mob.MaxMovement = Info.WayPointInfo.MaxMoveIndex;
            }

            if (!Group.Map.AddObject(Mob, true))
            {
                return false;
            }

            MemberList.Add(Mob);
            return true;
        }

        public void RemoveAllMember()
        {
            foreach (var Member in MemberList)
            {
                Group.Map.RemoveObject(Member, true);
                MemberList.Remove(Member);
            }
        }

        ~MobGroupMembers()
        {
            MemberList.Clear();
            MemberList = null;
            Info = null;
            Group = null;
        }
    }
}