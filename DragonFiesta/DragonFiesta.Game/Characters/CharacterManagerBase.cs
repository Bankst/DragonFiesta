#region

using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using DragonFiesta.Database.SQL;
using DragonFiesta.Game.Characters.Event;
using DragonFiesta.Providers.Characters;

#endregion

namespace DragonFiesta.Game.Characters
{
    public abstract class CharacterManagerBase<TCharacterType> where TCharacterType : CharacterBase, new()
    {
        protected ConcurrentDictionary<int, TCharacterType> CharactersByCharacterID { get; }
        protected ConcurrentDictionary<string, TCharacterType> CharactersByName { get; }
        protected ConcurrentDictionary<long, TCharacterType> LoggedInCharactersByAccountId { get; }
        protected ConcurrentDictionary<int, TCharacterType> LoggedInCharactersByCharacterID { get; }

        protected ConcurrentDictionary<string, TCharacterType> LoggedInCharactersByName;
        
     

        public event EventHandler<CharacterMapEventArgs<TCharacterType, IMap>> OnCharacterMapChanged { add { lock (ThreadLocker) _onCharacterMapChangedHandler.Add(value); } remove { lock (ThreadLocker) _onCharacterMapChangedHandler.Remove(value); } }

        public event EventHandler<CharacterLevelChangedEventArgs<TCharacterType>> OnCharacterLevelChanged { add { lock (ThreadLocker) _onCharacterLevelChangedHandler.Add(value); } remove { lock (ThreadLocker) _onCharacterLevelChangedHandler.Remove(value); } }

        public event EventHandler<CharacterDeleteEventArgs<TCharacterType>> OnCharacterDelete { add { lock (ThreadLocker) _onCharacterDeleteHandler.Add(value); } remove { lock (ThreadLocker) _onCharacterDeleteHandler.Remove(value); } }

        public event EventHandler<CharacterClassChangeEventArgs<TCharacterType>> OnCharacterClassChange { add { lock (ThreadLocker) _onCharacterClassChangeHandler.Add(value); } remove { lock (ThreadLocker) _onCharacterClassChangeHandler.Remove(value); } }

        public event EventHandler<CharacterEventArgs<TCharacterType>> OnCharacterLogout { add { lock (ThreadLocker) _onCharacterLogoutHandler.Add(value); } remove { lock (ThreadLocker) _onCharacterLogoutHandler.Remove(value); } }

        public event EventHandler<CharacterEventArgs<TCharacterType>> OnCharacterLogin { add { lock (ThreadLocker) _onCharacterLoginHandler.Add(value); } remove { lock (ThreadLocker) _onCharacterLoginHandler.Remove(value); } }

        public event EventHandler<CharacterMapRefreshEventArgs<TCharacterType>> OnCharacterMapRefresh { add { lock (ThreadLocker) _onCharacterMapRefreshHandler.Add(value); } remove { lock (ThreadLocker) _onCharacterMapRefreshHandler.Remove(value); } }

        private readonly SecureCollection<EventHandler<CharacterClassChangeEventArgs<TCharacterType>>> _onCharacterClassChangeHandler;

        //need this handler because packet exis...^^ and more..
        private readonly SecureCollection<EventHandler<CharacterDeleteEventArgs<TCharacterType>>> _onCharacterDeleteHandler;

        private readonly SecureCollection<EventHandler<CharacterMapEventArgs<TCharacterType, IMap>>> _onCharacterMapChangedHandler;

        private readonly SecureCollection<EventHandler<CharacterLevelChangedEventArgs<TCharacterType>>> _onCharacterLevelChangedHandler;

        private readonly SecureCollection<EventHandler<CharacterEventArgs<TCharacterType>>> _onCharacterLoginHandler;

        private readonly SecureCollection<EventHandler<CharacterEventArgs<TCharacterType>>> _onCharacterLogoutHandler;

        private readonly SecureCollection<EventHandler<CharacterMapRefreshEventArgs<TCharacterType>>> _onCharacterMapRefreshHandler;


        protected object ThreadLocker { get; }

	    protected CharacterManagerBase()
        {
            CharactersByCharacterID = new ConcurrentDictionary<int, TCharacterType>();
            CharactersByName = new ConcurrentDictionary<string, TCharacterType>();
            LoggedInCharactersByAccountId = new ConcurrentDictionary<long, TCharacterType>();
            LoggedInCharactersByCharacterID = new ConcurrentDictionary<int, TCharacterType>();
            LoggedInCharactersByName = new ConcurrentDictionary<string, TCharacterType>();
            _onCharacterLoginHandler = new SecureCollection<EventHandler<CharacterEventArgs<TCharacterType>>>();
            _onCharacterLogoutHandler = new SecureCollection<EventHandler<CharacterEventArgs<TCharacterType>>>();
            _onCharacterMapChangedHandler = new SecureCollection<EventHandler<CharacterMapEventArgs<TCharacterType, IMap>>>();
            _onCharacterLevelChangedHandler = new SecureCollection<EventHandler<CharacterLevelChangedEventArgs<TCharacterType>>>();
            _onCharacterClassChangeHandler = new SecureCollection<EventHandler<CharacterClassChangeEventArgs<TCharacterType>>>();
            _onCharacterDeleteHandler = new SecureCollection<EventHandler<CharacterDeleteEventArgs<TCharacterType>>>();
            _onCharacterMapRefreshHandler = new SecureCollection<EventHandler<CharacterMapRefreshEventArgs<TCharacterType>>>();

            ThreadLocker = new object();
        }

        private bool LoadCharacterFromDatabase(string where, out TCharacterType character, out CharacterErrors result, params SqlParameter[] parameters)
        {
            result = CharacterErrors.ErrorInCharacterInfo;
            character = default(TCharacterType);
            try
            {
                lock (ThreadLocker)
                {
                    var pResult = DB.Select(DatabaseType.World, "SELECT TOP 1 * FROM Characters WHERE " + where, parameters);

                    return pResult.HasValues && GetCharacterFromSQL(pResult, 0, out character, out result);
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error loading character from database: ");
                return false;
            }
        }

        public bool GetCharacterFromSQL(SQLResult pRes, int i, out TCharacterType character, out CharacterErrors result)
        {
            character = null;
            result = CharacterErrors.ErrorInCharacterInfo;
            try
            {
                lock (ThreadLocker)
                {
	                if (CharactersByCharacterID.TryGetValue(pRes.Read<int>(i, "ID"), out character)) return true;
	                character = new TCharacterType();

	                return character.RefreshCharacter(pRes, i, out result) && AddCharacter(character,out result);
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error loading character from database: ");
                return false;
            }
        }

        public bool GetCharacterByCharacterID(int id, out TCharacterType character, bool refreshCharacter = false)
        {
            return GetCharacterByCharacterID(id, out character, out _, refreshCharacter);
        }

        public bool GetCharacterByCharacterID(int id, out TCharacterType character, out CharacterErrors result, bool refreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (CharactersByCharacterID.TryGetValue(id, out character))
                {
                    if (refreshCharacter)
                    {
                        return RefreshCharacter(character, out result);
                    }

                    result = CharacterErrors.LoadOK;

                    return true;
                }
                return LoadCharacterFromDatabase("ID = @pID", out character, out result, new SqlParameter("@pID", id));
            }
        }

        public bool GetCharacterByName(string name, out TCharacterType character, bool refreshCharacter = false)
        {
            return GetCharacterByName(name, out character, out _, refreshCharacter);
        }

        public bool GetCharacterByName(string name, out TCharacterType character, out CharacterErrors result, bool refreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (CharactersByName.TryGetValue(name, out character))
                {
                    if (refreshCharacter)
                    {
                        return RefreshCharacter(character, out result);
                    }

                    result = CharacterErrors.LoadOK;

                    return true;
                }

                return LoadCharacterFromDatabase("Name = @pName", out character, out result, new SqlParameter("@pName", name));
            }
        }

        public bool GetLoggedInCharacterByAccountId(long id, out TCharacterType character, bool refreshCharacter = false)
        {
            return GetLoggedInCharacterByAccountId(id, out character, out _, refreshCharacter);
        }

        public bool GetLoggedInCharacterByAccountId(long id, out TCharacterType character, out CharacterErrors result, bool refreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (LoggedInCharactersByAccountId.TryGetValue(id, out character))
                {
                    if (refreshCharacter)
                    {
                        return RefreshCharacter(character, out result);
                    }

                    result = CharacterErrors.LoadOK;
                    return true;
                }

                result = CharacterErrors.ErrorInCharacterInfo;
                return false;
            }
        }

        public bool GetLoggedInCharacterByCharacterID(int id, out TCharacterType character, bool refreshCharacter = false)
        {
            return GetLoggedInCharacterByCharacterID(id, out character, out _, refreshCharacter);
        }

        public bool GetLoggedInCharacterByCharacterID(int id, out TCharacterType character, out CharacterErrors result, bool refreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (LoggedInCharactersByCharacterID.TryGetValue(id, out character))
                {
                    if (refreshCharacter)
                    {
                        return RefreshCharacter(character, out result);
                    }

                    result = CharacterErrors.LoadOK;

                    return true;
                }
                result = CharacterErrors.ErrorInCharacterInfo;
                return false;
            }
        }

        public bool GetLoggedInCharacterByName(string name, out TCharacterType character, bool refreshCharacter = false)
        {
            return GetLoggedInCharacterByName(name, out character, out _, refreshCharacter);
        }

        public bool GetLoggedInCharacterByName(string name, out TCharacterType character, out CharacterErrors result, bool refreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (LoggedInCharactersByName.TryGetValue(name, out character))
                {
                    if (refreshCharacter)
                    {
                        return RefreshCharacter(character, out result);
                    }

                    result = CharacterErrors.LoadOK;
                    return true;
                }

                result = CharacterErrors.ErrorInCharacterInfo;
                return false;
            }
        }

        public bool RefreshCharacter(TCharacterType character, out CharacterErrors result)
        {
            try
            {
                lock (ThreadLocker)
                {
                    var pResult = DB.Select(DatabaseType.World, "SELECT TOP 1 * FROM Characters WHERE ID = @pID",
                        new SqlParameter("@pID", character.Info.CharacterID));

	                if (pResult.HasValues) return character.RefreshCharacter(pResult, 0, out result);

	                result = CharacterErrors.ErrorInCharacterInfo;
	                return false;
                }
            }
            catch (Exception ex)
            {
                result = CharacterErrors.ErrorInCharacterInfo;
                EngineLog.Write(ex, "Error refreshing character: ");

                return false;
            }
        }

        protected virtual bool FinalizeCharacterDeleted(TCharacterType character)
        {
            InvokeCharacterDeleted(character);

            return true;
        }

        public bool DeleteCharacter(TCharacterType character)
        {
	        return RemoveCharacter(character) && FinalizeCharacterDeleted(character);
        }

        public virtual void CharacterChangeClass(TCharacterType character, ClassId newClass)
        {
            if (character.Info.Class == newClass)
                return;

            InvokeCharacterClassChange(character, newClass);

            character.Info.Class = newClass;
        }

        public virtual void CharacterChangeMap(TCharacterType character, IMap newMap)
        {
            InvokeCharacterMapChanged(character, newMap);
        }

        public virtual void CharacterMapRefreshed(TCharacterType character)
        {
            InvokeCharacterMapRefresh(character);
        }

        protected virtual void FinalizeLogCharacterIn(TCharacterType character)
        {
            InvokeCharacterLogin(character);
        }

        public void CharacterLevelChanged(TCharacterType character,byte newLevel)
        {
            if (character.Info.Level == newLevel)
                return;

            FinalizeCharacterLevelChanged(character, newLevel);
        }

        protected virtual void FinalizeCharacterLevelChanged(TCharacterType character, byte newLevel, ushort mobId = ushort.MaxValue)
        {
            InvokeCharacterLevelUp(character, newLevel);
        }

        public void LogCharacterIn(TCharacterType character)
        {
            try
            {
                lock (ThreadLocker)
                {
                    if (LoggedInCharactersByAccountId.TryAdd(character.LoginInfo.AccountID, character)
                        && LoggedInCharactersByCharacterID.TryAdd(character.Info.CharacterID, character)
                        && LoggedInCharactersByName.TryAdd(character.Info.Name, character))
                    {
                        FinalizeLogCharacterIn(character);
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error logging character '{0}' (ID: {1}) in:", character.Info.Name, character.Info.CharacterID);
            }
        }

        protected virtual void FinalizeCharacterLogOut(TCharacterType character)
        {
            InvokeCharacterLogOut(character);
        }

        public void LogCharacterOut(TCharacterType character, bool saveCharacter = true)
        {
            try
            {
                lock (ThreadLocker)
                {
                    if (LoggedInCharactersByAccountId.TryRemove(character.LoginInfo.AccountID, out character)
                        && LoggedInCharactersByCharacterID.TryRemove(character.Info.CharacterID, out character)
                        && LoggedInCharactersByName.TryRemove(character.Info.Name, out character))
                    {
                        if (saveCharacter)
                        {
                            character.Save();
                        }


                        FinalizeCharacterLogOut(character);
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, $"Error logging character '{character.Info.Name}' (ID: {character.Info.CharacterID}) out.");
            }
        }


        public bool AddCharacter(TCharacterType character, out CharacterErrors error)
        {
            lock (ThreadLocker)
            {
                if (CharactersByCharacterID.TryAdd(character.Info.CharacterID, character)
                    && CharactersByName.TryAdd(character.Info.Name, character))
                {

                    return character.FinalizeLoad(out error);
                }
            }

            error = CharacterErrors.ErrorInCharacterInfo;
            return false;
        }

        public bool RemoveCharacter(CharacterBase pCharacter)
        {
            lock (ThreadLocker)
            {
	            return CharactersByCharacterID.TryRemove(pCharacter.Info.CharacterID, out _)
	                   && CharactersByName.TryRemove(pCharacter.Info.Name, out _);
            }
        }

        #region Events

        private void InvokeCharacterMapRefresh(TCharacterType pCharacter) => InvokeEvents(_onCharacterMapRefreshHandler, new CharacterMapRefreshEventArgs<TCharacterType>(pCharacter));
        private void InvokeCharacterDeleted(TCharacterType pCharacter) => InvokeEvents(_onCharacterDeleteHandler, new CharacterDeleteEventArgs<TCharacterType>(pCharacter));

        private void InvokeCharacterClassChange(TCharacterType pCharacter, ClassId newClass) => InvokeEvents(_onCharacterClassChangeHandler, new CharacterClassChangeEventArgs<TCharacterType>(pCharacter, newClass));
        private void InvokeCharacterMapChanged(TCharacterType pCharacter, IMap newMap) => InvokeEvents(_onCharacterMapChangedHandler, new CharacterMapEventArgs<TCharacterType, IMap>(pCharacter, newMap));

        private void InvokeCharacterLevelUp(TCharacterType character, byte newLevel) => InvokeEvents(_onCharacterLevelChangedHandler, new CharacterLevelChangedEventArgs<TCharacterType>(character, newLevel));

        private void InvokeCharacterLogin(TCharacterType pCharacter) => InvokeEvents(_onCharacterLoginHandler, new CharacterEventArgs<TCharacterType>(pCharacter));

        private void InvokeCharacterLogOut(TCharacterType pCharacter) => InvokeEvents(_onCharacterLogoutHandler, new CharacterEventArgs<TCharacterType>(pCharacter));

        private void InvokeEvents<TArgs>(SecureCollection<EventHandler<TArgs>> list, TArgs args)
        {
            lock (ThreadLocker)
            {
	            foreach (var eventHandler in list)
	            {
		            eventHandler.Invoke(this, args);
	            }
            }
        }

        #endregion Events
    }
}