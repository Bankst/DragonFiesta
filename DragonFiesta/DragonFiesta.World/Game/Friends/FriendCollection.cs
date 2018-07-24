using DragonFiesta.World.Game.Character;
using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Threading;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.World.Game.Friends
{
    public class FriendCollection : IDisposable
    {
        private WorldCharacter Owner { get; set; }

        public int Count { get { return List.Count; } }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;

        private SecureCollection<Friend> List;
        private ConcurrentDictionary<int, Friend> FriendsByCharacterID;

        private object ThreadLocker;

        public FriendCollection(WorldCharacter Owner)
        {
            this.Owner = Owner;
            List = new SecureCollection<Friend>();
            FriendsByCharacterID = new ConcurrentDictionary<int, Friend>();
            ThreadLocker = new object();
        }


        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                Owner = null;
                FriendAction((f) => f.Dispose(), false);
                List.Dispose();
                List = null;
                FriendsByCharacterID.Clear();
                FriendsByCharacterID = null;

            }
        }
        public void Clear()
        {
            lock (ThreadLocker)
            {
                FriendAction((f) => f.Dispose());
                List.Clear();
                FriendsByCharacterID.Clear();
            }
        }
        public bool Refresh()
        {
            Clear();

            lock (ThreadLocker)
            {
                try
                {
                    SQLResult Result = DB.Select(DatabaseType.World, "SELECT * FROM Friends WHERE OwnerID = @pOwnerID", new SqlParameter("@pOwnerID", Owner.Info.CharacterID));

                    //here todo fix stack overflow...
                    for (int i = 0; i < Result.Count; i++)
                    {
                        if (!WorldCharacterManager.Instance.GetCharacterByCharacterID(Result.Read<int>(i, "FriendId"), out WorldCharacter Character, false))
                        {
                            return false;
                        }

                        var Frend = new Friend(Owner, Character, Result.Read<DateTime>(i, "RegisterDate"));

                        if (!Add(Frend))
                            return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public Friend this[int Index]
        {
            get { return List[Index]; }
        }
        public bool Contains(int CharacterID)
        {
            return FriendsByCharacterID.ContainsKey(CharacterID);
        }

        public bool Contains(WorldCharacter Character)
        {
            return Contains(Character.Info.CharacterID);
        }

        public bool Add(Friend Friend)
        {
            lock (ThreadLocker)
            {
                if (FriendsByCharacterID.TryAdd(Friend.MyFriend.Info.CharacterID, Friend))
                {
                    List.Add(Friend);
                    return true;
                }
            }
            return false;
        }

        public bool Remove(Friend Friend)
        {
            lock (ThreadLocker)
            {
                if (FriendsByCharacterID.TryRemove(Friend.MyFriend.Info.CharacterID, out Friend))
                {
                    List.Remove(Friend);
                    Friend.Dispose();
                    return true;
                }
            }
            return false;
        }

        public bool RemoveByCharacterID(int CharacterID)
        {
            lock (ThreadLocker)
            {
                Friend friend;
                if (FriendsByCharacterID.TryGetValue(CharacterID, out friend))
                {
                    return Remove(friend);
                }
                return false;
            }
        }

        public void FriendAction(Action<Friend> Action, bool OnlyConnected = true)
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    try
                    {
                        var friend = List[i];

                        if (OnlyConnected && !friend.MyFriend.IsConnected)
                            continue;
                        Action.Invoke(friend);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        public void Broadcast(FiestaPacket Packet)
        {
            FriendAction((f) => f.MyFriend.Session.SendPacket(Packet), true);
        }

    }
}
