using DragonFiesta.Game.Characters.Event;
using System;
using System.Collections.Concurrent;
using System.Data.SqlClient;
using DragonFiesta.Providers.Characters;

namespace DragonFiesta.Game.Characters
{
    public abstract class CharacterManagerBase<CharacterType> where CharacterType : CharacterBase, new()
    {
        protected ConcurrentDictionary<int, CharacterType> CharactersByCharacterID { get; private set; }
        protected ConcurrentDictionary<string, CharacterType> CharactersByName { get; private set; }
        protected ConcurrentDictionary<long, CharacterType> LoggedInCharactersByAccountId { get; private set; }
        protected ConcurrentDictionary<int, CharacterType> LoggedInCharactersByCharacterID { get; private set; }

        protected ConcurrentDictionary<string, CharacterType> LoggedInCharactersByName;
        
     

        public event EventHandler<CharacterMapEventArgs<CharacterType, IMap>> OnCharacterMapChanged { add { lock (ThreadLocker) OnCharacterMapChangedHandler.Add(value); } remove { lock (ThreadLocker) OnCharacterMapChangedHandler.Remove(value); } }

        public event EventHandler<CharacterLevelChangedEventArgs<CharacterType>> OnCharacterLevelChanged { add { lock (ThreadLocker) OnCharacterLevelChangedHandler.Add(value); } remove { lock (ThreadLocker) OnCharacterLevelChangedHandler.Remove(value); } }

        public event EventHandler<CharacterDeleteEventArgs<CharacterType>> OnCharacterDelete { add { lock (ThreadLocker) OnCharacterDeleteHandler.Add(value); } remove { lock (ThreadLocker) OnCharacterDeleteHandler.Remove(value); } }

        public event EventHandler<CharacterClassChangeEventArgs<CharacterType>> OnCharacterClassChange { add { lock (ThreadLocker) OnCharacterClassChangeHandler.Add(value); } remove { lock (ThreadLocker) OnCharacterClassChangeHandler.Remove(value); } }

        public event EventHandler<CharacterEventArgs<CharacterType>> OnCharacterLogout { add { lock (ThreadLocker) OnCharacterLogoutHandler.Add(value); } remove { lock (ThreadLocker) OnCharacterLogoutHandler.Remove(value); } }

        public event EventHandler<CharacterEventArgs<CharacterType>> OnCharacterLogin { add { lock (ThreadLocker) OnCharacterLoginHandler.Add(value); } remove { lock (ThreadLocker) OnCharacterLoginHandler.Remove(value); } }

        public event EventHandler<CharacterMapRefreshEventArgs<CharacterType>> OnCharacterMapRefresh { add { lock (ThreadLocker) OnCharacterMapRefreshHandler.Add(value); } remove { lock (ThreadLocker) OnCharacterMapRefreshHandler.Remove(value); } }

        private SecureCollection<EventHandler<CharacterClassChangeEventArgs<CharacterType>>> OnCharacterClassChangeHandler;

        //need this handler because packet exis...^^ and more..
        private SecureCollection<EventHandler<CharacterDeleteEventArgs<CharacterType>>> OnCharacterDeleteHandler;

        private SecureCollection<EventHandler<CharacterMapEventArgs<CharacterType, IMap>>> OnCharacterMapChangedHandler;

        private SecureCollection<EventHandler<CharacterLevelChangedEventArgs<CharacterType>>> OnCharacterLevelChangedHandler;

        private SecureCollection<EventHandler<CharacterEventArgs<CharacterType>>> OnCharacterLoginHandler;

        private SecureCollection<EventHandler<CharacterEventArgs<CharacterType>>> OnCharacterLogoutHandler;

        private SecureCollection<EventHandler<CharacterMapRefreshEventArgs<CharacterType>>> OnCharacterMapRefreshHandler;


        protected object ThreadLocker { get; private set; }

        public CharacterManagerBase()
        {
            CharactersByCharacterID = new ConcurrentDictionary<int, CharacterType>();
            CharactersByName = new ConcurrentDictionary<string, CharacterType>();
            LoggedInCharactersByAccountId = new ConcurrentDictionary<long, CharacterType>();
            LoggedInCharactersByCharacterID = new ConcurrentDictionary<int, CharacterType>();
            LoggedInCharactersByName = new ConcurrentDictionary<string, CharacterType>();
            OnCharacterLoginHandler = new SecureCollection<EventHandler<CharacterEventArgs<CharacterType>>>();
            OnCharacterLogoutHandler = new SecureCollection<EventHandler<CharacterEventArgs<CharacterType>>>();
            OnCharacterMapChangedHandler = new SecureCollection<EventHandler<CharacterMapEventArgs<CharacterType, IMap>>>();
            OnCharacterLevelChangedHandler = new SecureCollection<EventHandler<CharacterLevelChangedEventArgs<CharacterType>>>();
            OnCharacterClassChangeHandler = new SecureCollection<EventHandler<CharacterClassChangeEventArgs<CharacterType>>>();
            OnCharacterDeleteHandler = new SecureCollection<EventHandler<CharacterDeleteEventArgs<CharacterType>>>();
            OnCharacterMapRefreshHandler = new SecureCollection<EventHandler<CharacterMapRefreshEventArgs<CharacterType>>>();

            ThreadLocker = new object();
        }

        private bool LoadCharacterFromDatabase(string Where, out CharacterType Character, out CharacterErrors Result, params SqlParameter[] Parameters)
        {
            Result = CharacterErrors.ErrorInCharacterInfo;
            Character = default(CharacterType);
            try
            {
                lock (ThreadLocker)
                {
                    SQLResult pResult = DB.Select(DatabaseType.World, "SELECT TOP 1 * FROM Characters WHERE " + Where, Parameters);

                    if (!pResult.HasValues)
                        return false;

                    return GetCharacterFromSQL(pResult, 0, out Character, out Result);
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error loading character from database: ");
                return false;
            }
        }

        public bool GetCharacterFromSQL(SQLResult pRes, int i, out CharacterType Character, out CharacterErrors Result)
        {
            Character = null;
            Result = CharacterErrors.ErrorInCharacterInfo;
            try
            {
                lock (ThreadLocker)
                {
                    if (!CharactersByCharacterID.TryGetValue(pRes.Read<int>(i, "ID"), out Character))
                    {
                        Character = new CharacterType();

                        if (!Character.RefreshCharacter(pRes, i, out Result))
                            return false;

                        return AddCharacter(Character,out Result);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error loading character from database: ");
                return false;
            }
        }

        public bool GetCharacterByCharacterID(int ID, out CharacterType Character, bool RefreshCharacter = false)
        {
            return GetCharacterByCharacterID(ID, out Character, out CharacterErrors er, RefreshCharacter);
        }

        public bool GetCharacterByCharacterID(int ID, out CharacterType Character, out CharacterErrors Result, bool RefreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (CharactersByCharacterID.TryGetValue(ID, out Character))
                {
                    if (RefreshCharacter)
                    {
                        return this.RefreshCharacter(Character, out Result);
                    }

                    Result = CharacterErrors.LoadOK;

                    return true;
                }
                return LoadCharacterFromDatabase("ID = @pID", out Character, out Result, new SqlParameter("@pID", ID));
            }
        }

        public bool GetCharacterByName(string Name, out CharacterType Character, bool RefreshCharacter = false)
        {
            return GetCharacterByName(Name, out Character, out CharacterErrors err, RefreshCharacter);
        }

        public bool GetCharacterByName(string Name, out CharacterType Character, out CharacterErrors Result, bool RefreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (CharactersByName.TryGetValue(Name, out Character))
                {
                    if (RefreshCharacter)
                    {
                        return this.RefreshCharacter(Character, out Result);
                    }

                    Result = CharacterErrors.LoadOK;

                    return true;
                }

                return LoadCharacterFromDatabase("Name = @pName", out Character, out Result, new SqlParameter("@pName", Name));
            }
        }

        public bool GetLoggedInCharacterByAccountId(long ID, out CharacterType Character, bool RefreshCharacter = false)
        {
            return GetLoggedInCharacterByAccountId(ID, out Character, out CharacterErrors er, RefreshCharacter);
        }

        public bool GetLoggedInCharacterByAccountId(long ID, out CharacterType Character, out CharacterErrors Result, bool RefreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (LoggedInCharactersByAccountId.TryGetValue(ID, out Character))
                {
                    if (RefreshCharacter)
                    {
                        return this.RefreshCharacter(Character, out Result);
                    }

                    Result = CharacterErrors.LoadOK;
                    return true;
                }

                Result = CharacterErrors.ErrorInCharacterInfo;
                return false;
            }
        }

        public bool GetLoggedInCharacterByCharacterID(int ID, out CharacterType Character, bool RefreshCharacter = false)
        {
            return GetLoggedInCharacterByCharacterID(ID, out Character, out CharacterErrors Err, RefreshCharacter);
        }

        public bool GetLoggedInCharacterByCharacterID(int ID, out CharacterType Character, out CharacterErrors Result, bool RefreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (LoggedInCharactersByCharacterID.TryGetValue(ID, out Character))
                {
                    if (RefreshCharacter)
                    {
                        return this.RefreshCharacter(Character, out Result);
                    }

                    Result = CharacterErrors.LoadOK;

                    return true;
                }
                Result = CharacterErrors.ErrorInCharacterInfo;
                return false;
            }
        }

        public bool GetLoggedInCharacterByName(string Name, out CharacterType Character, bool RefreshCharacter = false)
        {
            return GetLoggedInCharacterByName(Name, out Character, out CharacterErrors err, RefreshCharacter);
        }

        public bool GetLoggedInCharacterByName(string Name, out CharacterType Character, out CharacterErrors Result, bool RefreshCharacter = false)
        {
            lock (ThreadLocker)
            {
                if (LoggedInCharactersByName.TryGetValue(Name, out Character))
                {
                    if (RefreshCharacter)
                    {
                        return this.RefreshCharacter(Character, out Result);
                    }

                    Result = CharacterErrors.LoadOK;
                    return true;
                }

                Result = CharacterErrors.ErrorInCharacterInfo;
                return false;
            }
        }

        public bool RefreshCharacter(CharacterType Character, out CharacterErrors Result)
        {
            try
            {
                lock (ThreadLocker)
                {
                    SQLResult pResult = DB.Select(DatabaseType.World, "SELECT TOP 1 * FROM Characters WHERE ID = @pID",
                        new SqlParameter("@pID", Character.Info.CharacterID));

                    if (!pResult.HasValues)
                    {
                        Result = CharacterErrors.ErrorInCharacterInfo;
                        return false;
                    }

                    return Character.RefreshCharacter(pResult, 0, out Result);
                }
            }
            catch (Exception ex)
            {
                Result = CharacterErrors.ErrorInCharacterInfo;
                EngineLog.Write(ex, "Error refreshing character: ");

                return false;
            }
        }


        protected virtual bool FinalizeCharacterDeleted(CharacterType Character)
        {
            InvokeCharacterDeletet(Character);

            return true;
        }
        public bool DeleteCharacter(CharacterType Character)
        {

            if (!RemoveCharacter(Character))
                return false;

            return FinalizeCharacterDeleted(Character);
        }

        public virtual void CharacterChangeClass(CharacterType Character, ClassId NewClass)
        {
            if (Character.Info.Class == NewClass)
                return;

            InvokeCharacterClassChange(Character, NewClass);

            Character.Info.Class = NewClass;
        }
        public virtual void CharacterChangeMap(CharacterType Character, IMap NewMap)
        {
            InvokeCharacterMapChanged(Character, NewMap);
        }

        public virtual void CharacterMapRefreshed(CharacterType Character)
        {
            InvokeCharacterMapRefresh(Character);
        }
        protected virtual void FinalizeLogCharacterIn(CharacterType Character)
        {
            InvokeCharacterLogin(Character);
        }

        public void CharacterLevelChanged(CharacterType Character,byte NewLevel)
        {
            if (Character.Info.Level == NewLevel)
                return;

            FinalizeCharacterLevelChanged(Character, NewLevel);
        }

        protected virtual void FinalizeCharacterLevelChanged(CharacterType Character, byte NewLevel, ushort MobId = ushort.MaxValue)
        {
          
            InvokeCharacterLevelUP(Character, NewLevel);
        }
        public void LogCharacterIn(CharacterType Character)
        {
            try
            {
                lock (ThreadLocker)
                {
                    if (LoggedInCharactersByAccountId.TryAdd(Character.LoginInfo.AccountID, Character)
                        && LoggedInCharactersByCharacterID.TryAdd(Character.Info.CharacterID, Character)
                        && LoggedInCharactersByName.TryAdd(Character.Info.Name, Character))
                    {
                        FinalizeLogCharacterIn(Character);
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, "Error logging character '{0}' (ID: {1}) in:", Character.Info.Name, Character.Info.CharacterID);
            }
        }
        protected virtual void FinalizeCharacterLogOut(CharacterType Character)
        {
            InvokeCharacterLogOut(Character);
        }
        public void LogCharacterOut(CharacterType Character, bool SaveCharacter = true)
        {
            try
            {
                lock (ThreadLocker)
                {
                    if (LoggedInCharactersByAccountId.TryRemove(Character.LoginInfo.AccountID, out Character)
                        && LoggedInCharactersByCharacterID.TryRemove(Character.Info.CharacterID, out Character)
                        && LoggedInCharactersByName.TryRemove(Character.Info.Name, out Character))
                    {
                        if (SaveCharacter)
                        {
                            Character.Save();
                        }


                        FinalizeCharacterLogOut(Character);
                    }
                }
            }
            catch (Exception ex)
            {
                EngineLog.Write(ex, $"Error logging character '{Character.Info.Name}' (ID: {Character.Info.CharacterID}) out.");
            }
        }


        public bool AddCharacter(CharacterType Character, out CharacterErrors Error)
        {
            lock (ThreadLocker)
            {
                if (CharactersByCharacterID.TryAdd(Character.Info.CharacterID, Character)
                    && CharactersByName.TryAdd(Character.Info.Name, Character))
                {

                    return Character.FinalizeLoad(out Error);
                }
            }

            Error = CharacterErrors.ErrorInCharacterInfo;
            return false;
        }

        public bool RemoveCharacter(CharacterBase pCharacter)
        {
            lock (ThreadLocker)
            {
                if (CharactersByCharacterID.TryRemove(pCharacter.Info.CharacterID, out CharacterType pChar)
                 && CharactersByName.TryRemove(pCharacter.Info.Name, out pChar))
                {
                    return true;
                }
                return false;
            }
        }

        #region Events

        private void InvokeCharacterMapRefresh(CharacterType pCharacter) => InvokeEvents(OnCharacterMapRefreshHandler, new CharacterMapRefreshEventArgs<CharacterType>(pCharacter));
        private void InvokeCharacterDeletet(CharacterType pCharacter) => InvokeEvents(OnCharacterDeleteHandler, new CharacterDeleteEventArgs<CharacterType>(pCharacter));

        private void InvokeCharacterClassChange(CharacterType pCharacter, ClassId NewClass) => InvokeEvents(OnCharacterClassChangeHandler, new CharacterClassChangeEventArgs<CharacterType>(pCharacter, NewClass));
        private void InvokeCharacterMapChanged(CharacterType pCharacter, IMap NewMap) => InvokeEvents(OnCharacterMapChangedHandler, new CharacterMapEventArgs<CharacterType, IMap>(pCharacter, NewMap));

        private void InvokeCharacterLevelUP(CharacterType Character, byte NewLevel) => InvokeEvents(OnCharacterLevelChangedHandler, new CharacterLevelChangedEventArgs<CharacterType>(Character, NewLevel));

        private void InvokeCharacterLogin(CharacterType pCharacter) => InvokeEvents(OnCharacterLoginHandler, new CharacterEventArgs<CharacterType>(pCharacter));

        private void InvokeCharacterLogOut(CharacterType pCharacter) => InvokeEvents(OnCharacterLogoutHandler, new CharacterEventArgs<CharacterType>(pCharacter));

        private void InvokeEvents<TArgs>(SecureCollection<EventHandler<TArgs>> List, TArgs args)
        {
            lock (ThreadLocker)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    List[i].Invoke(this, args);
                }
            }
        }

        #endregion Events
    }
}