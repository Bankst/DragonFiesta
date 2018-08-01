using DragonFiesta.World.Game.Character;
using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using DragonFiesta.Database.Models;
using DragonFiesta.Database.SQL;

namespace DragonFiesta.World.Game.Friends
{
    public class FriendCollection : IDisposable
    {
        private WorldCharacter Owner { get; set; }

        public int Count { get { return _list.Count; } }

        public bool IsDisposed { get { return (_isDisposedInt > 0); } }
        private int _isDisposedInt;

        private SecureCollection<Friend> _list;
        private ConcurrentDictionary<int, Friend> _friendsByCharacterID;

        private object _threadLocker;

        public FriendCollection(WorldCharacter owner)
        {
            this.Owner = owner;
            _list = new SecureCollection<Friend>();
            _friendsByCharacterID = new ConcurrentDictionary<int, Friend>();
            _threadLocker = new object();
        }


        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) == 0)
            {
                Owner = null;
                FriendAction(f => f.Dispose(), false);
                _list.Dispose();
                _list = null;
                _friendsByCharacterID.Clear();
                _friendsByCharacterID = null;

            }
        }

        public void Clear()
        {
            lock (_threadLocker)
            {
                FriendAction((f) => f.Dispose());
                _list.Clear();
                _friendsByCharacterID.Clear();
            }
        }

	    public bool RefreshEntity()
	    {
		    Clear();

		    lock (_threadLocker)
		    {
			    try
			    {
				    using (var worldEntity = EDM.GetWorldEntity())
				    {
					    var result = worldEntity.DBFriends.Where(fr => fr.OwnerID == Owner.Info.CharacterID);

					    foreach (var dbFriend in result)
					    {
						    if (!WorldCharacterManager.Instance.GetCharacterByCharacterID(dbFriend.FriendID,
							    out var character, false))
						    {
							    return false;
						    }

						    var friend = new Friend(Owner, character, dbFriend.RegisterDate);

						    if (!Add(friend)) return false;
					    }
				    }
			    }
			    catch (Exception)
			    {
				    return false;
			    }
		    }

		    return true;
	    }

        public bool Refresh()
        {
            Clear();

            lock (_threadLocker)
            {
                try
                {
                    SQLResult result = DB.Select(DatabaseType.World, "SELECT * FROM Friends WHERE OwnerID = @pOwnerID", new SqlParameter("@pOwnerID", Owner.Info.CharacterID));

                    //here todo fix stack overflow...
                    for (var i = 0; i < result.Count; i++)
                    {
                        if (!WorldCharacterManager.Instance.GetCharacterByCharacterID(result.Read<int>(i, "FriendId"), out var character, false))
                        {
                            return false;
                        }

                        var frend = new Friend(Owner, character, result.Read<DateTime>(i, "RegisterDate"));

                        if (!Add(frend))
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

        public Friend this[int index] => _list[index];

	    public bool Contains(int characterID)
        {
            return _friendsByCharacterID.ContainsKey(characterID);
        }

        public bool Contains(WorldCharacter character)
        {
            return Contains(character.Info.CharacterID);
        }

        public bool Add(Friend friend)
        {
            lock (_threadLocker)
            {
	            if (!_friendsByCharacterID.TryAdd(friend.MyFriend.Info.CharacterID, friend)) return false;
	            _list.Add(friend);
	            return true;
            }
        }

        public bool Remove(Friend friend)
        {
            lock (_threadLocker)
            {
	            if (!_friendsByCharacterID.TryRemove(friend.MyFriend.Info.CharacterID, out friend)) return false;
	            _list.Remove(friend);
	            friend.Dispose();
	            return true;
            }
        }

        public bool RemoveByCharacterID(int characterID)
        {
            lock (_threadLocker)
            {
	            return _friendsByCharacterID.TryGetValue(characterID, out var friend) && Remove(friend);
            }
        }

        public void FriendAction(Action<Friend> action, bool onlyConnected = true)
        {
            lock (_threadLocker)
            {
                for (var i = 0; i < _list.Count; i++)
                {
                    try
                    {
                        var friend = _list[i];

                        if (onlyConnected && !friend.MyFriend.IsConnected)
                            continue;
                        action.Invoke(friend);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        public void Broadcast(FiestaPacket packet)
        {
            FriendAction((f) => f.MyFriend.Session.SendPacket(packet), true);
        }

    }
}
