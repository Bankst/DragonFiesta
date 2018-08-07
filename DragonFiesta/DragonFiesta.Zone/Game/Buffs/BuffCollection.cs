using System;
using System.Collections.Concurrent;
using System.Threading;
using DragonFiesta.Zone.Core;
using DragonFiesta.Zone.Data.Buffs;
using DragonFiesta.Zone.Game.Character;
using DragonFiesta.Zone.Game.Maps.Interface;

namespace DragonFiesta.Zone.Game.Buffs
{
    public class BuffCollection
    {
        public ILivingObject Owner { get; private set; }
        public int Count { get { return BuffList.Count; } }
        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;
        private SecureCollection<Buff> BuffList;
        private ConcurrentDictionary<uint, Buff> BuffsByAbStateIndex;
        private object ThreadLocker;

        protected BuffCollection(ILivingObject Owner)
        {
            this.Owner = Owner;
            BuffList = new SecureCollection<Buff>();
            BuffsByAbStateIndex = new ConcurrentDictionary<uint, Buff>();
            ThreadLocker = new object();
        }
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                Owner = null;
                BuffList.ForEach(b => b.Dispose());
                BuffList.Clear();
                BuffList = null;
                BuffsByAbStateIndex.Clear();
                BuffsByAbStateIndex = null;
                ThreadLocker = null;
            }
        }


        public Buff this[int Index]
        {
            get { return BuffList[Index]; }
        }
        public bool FindBuff(Predicate<Buff> Match, out Buff Buff)
        {
            return (Buff = BuffList.Find(Match)) != null;
        }
        public bool GetBuffByAbStateIndex(uint AbStateIndex, out Buff Buff)
        {
            return BuffsByAbStateIndex.TryGetValue(AbStateIndex, out Buff);
        }


        public void Clear()
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < BuffList.Count; i++)
                {
                    BuffList[i].Deactivate();
                    BuffList[i].Dispose();
                }
                BuffList.Clear();
                BuffsByAbStateIndex.Clear();
            }
        }

        public bool Refresh()
        {
            return true;
        }

        public bool Save()
        {
            return true;
        }

        public virtual bool Add(AbStateInfo AbState, uint Strength, out Buff Buff, TimeSpan? KeepTime = null)
        {
            try
            {
                lock (ThreadLocker)
                {
                    Buff = null;
                    //get subAbState for this strength
                    SubAbStateInfo subAbState;
                    if (!AbState.SubAbStates.TryGetValue(Strength, out subAbState))
                        return false;
                    //get times
                    var currentTime = ServerMain.InternalInstance.CurrentTime;
                    KeepTime = (KeepTime ?? subAbState.KeepTime);
                    //check if we already have a buff like that
                    if (BuffsByAbStateIndex.TryGetValue(AbState.AbStateIndex, out Buff))
                    {
                        //check if new strength is stronger
                        if (Buff.Strength > Strength)
                            return false;
                        //deactivate buff
                        Buff.Deactivate();
                        //update strength
                        Buff.UpdateStrength(Strength);
                        //activate buff
                        Buff.Activate();
                        //update start + expire time of the buff
                        Buff.StartTime = currentTime.Time;
                        Buff.ExpireTime = currentTime.Time.Add(KeepTime.Value);
                    }
                    else
                    {
                        if (!CreateBuff(AbState, Strength, out Buff))
                            return false;
                        //update start + expire time of the buff
                        Buff.StartTime = currentTime.Time;
                        Buff.ExpireTime = currentTime.Time.Add(KeepTime.Value);
                        //add to lists
                        BuffList.Add(Buff);
                        BuffsByAbStateIndex.TryAdd(AbState.AbStateIndex, Buff);
                        //add to database



                        //activate buff
                        Buff.Activate();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error adding buff to living object:");
                Buff = null;
                return false;
            }
        }
        public bool Remove(Buff Buff)
        {
            try
            {
                lock (ThreadLocker)
                {
                    if (BuffsByAbStateIndex.TryRemove(Buff.AbStateInfo.AbStateIndex, out Buff))
                    {
                        BuffList.Remove(Buff);
                        //deactivate buff
                        Buff.Deactivate();
                        //remove from db
                        if (Buff.AbStateInfo.IsSave)
                        {
                            var character = (Owner as ZoneCharacter);

                            if (character != null)
                            {

                            }
                        }

                        //clean up
                        Buff.Dispose();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error removing buff from living object:");

                return false;
            }
        }

        protected bool CreateBuff(AbStateInfo AbState, uint Strength, out Buff Buff)
        {
            Buff = null;
            return true;
        }

    }
}