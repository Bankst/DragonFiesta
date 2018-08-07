using System;
using System.Collections.Generic;
using System.Threading;
using DragonFiesta.Database.SQL;
using DragonFiesta.Game.Characters;
using DragonFiesta.Game.Stats;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Providers.Maps;
using DragonFiesta.Utils.Config;
using DragonFiesta.Utils.Logging;
using DragonFiesta.Zone.Game.Maps.Event;
using DragonFiesta.Zone.Game.Maps.Object;
using DragonFiesta.Zone.Game.Maps.Interface;
using DragonFiesta.Zone.Data.Character;
using DragonFiesta.Zone.Game.Chat;
using DragonFiesta.Zone.Game.Maps;
using DragonFiesta.Zone.Game.Maps.Types;
using DragonFiesta.Zone.Game.Stats;
using DragonFiesta.Zone.InternNetwork.InternHandler.Server.Character;
using DragonFiesta.Zone.Network;
using DragonFiesta.Zone.Network.FiestaHandler.Server;
using DragonFiesta.Zone.Network.Helpers;


namespace DragonFiesta.Zone.Game.Character
{
    public class ZoneCharacter : CharacterBase, ILivingObject
    {
        #region Property

        public ZoneSession Session { get; set; }

        public Question Question { get; set; }

        public ushort WorldSessionId { get; set; }

        public new ZoneClientLoginInfo LoginInfo
        {
            get => base.LoginInfo as ZoneClientLoginInfo;
            set => base.LoginInfo = value;
        }

        public new ZoneCharacterInfo Info => base.Info as ZoneCharacterInfo;

	    public bool IsOnThisZone => (IsConnected && Map is LocalMap);

        public byte Level
        {
            get => Info.Level;
            set => Info.Level = value;
        }


        public override bool IsGM => LoginInfo.RoleId > 0;

        public override bool IsConnected => (Session != null && Session.IsConnected);

        public LivingStats LivingStats { get; set; }

        public MapObjectType Type => MapObjectType.Character;

        public ushort MapObjectId { get; set; }

        public IMap Map { get => AreaInfo.Map; set => AreaInfo.Map = value; }

        private MapSector _mapSector;

        public MapSector MapSector
        {
            get => _mapSector;
	        set
            {
                var oldSector = _mapSector;

                _mapSector = value;

                InvokeOnMapSectorChanged(oldSector);
            }
        }

        public InRangeCollection InRange { get; set; }
        public Position Position { get => AreaInfo.Position; set => AreaInfo.Position = value; }

        public CharacterState State { get; set; } = CharacterState.Player;

        public DateTime NextUpdate { get; set; }

        public DateTime LastUpdate { get; set; }

        public bool IsAlive => (State != CharacterState.Dead);

        public StatsManager Stats => Info.Stats;

        public LivingObjectSelectionBase Selection { get; set; }

        private ushort _updateCounter;

        public ushort UpdateCounter
        {
            get
            {
                if (_updateCounter == 0xffff)
                {
                    _updateCounter = 0;
                }
                _updateCounter++;

                InvokeUpdateCounterChanged();

                return _updateCounter;
            }
        }


        private int _isDisposedInt;

        #endregion Property

        public ZoneCharacter() : base()
        {
            LoginInfo = new ZoneClientLoginInfo(this);
            base.Info = new ZoneCharacterInfo(this);
			
            LivingStats = new LivingStats(this);

            LivingStats.OnHPChanged += LivingStats_OnHPChanged;
            LivingStats.OnSPChanged += LivingStats_OnSPChanged;
            LivingStats.OnLPChanged += LivingStats_OnLPChanged;
            _onMapChangedHandlers = new List<EventHandler<MapChangedEventArgs>>();
            _onMapSectorChangedHandlers = new List<EventHandler<MapSectorChangedEventArgs>>();
            _onDisposeHandlers = new List<EventHandler<MapObjectEventArgs>>();
            _onUpdateCounterChangedHandler = new List<EventHandler<UpdateCounterChangeEventArgs>>();

			InRange = new InRangeCollection(this);
            Selection = new CharacterObjectSelection(this);

        }




        #region MapObject

        private List<EventHandler<MapChangedEventArgs>> _onMapChangedHandlers;

        public event EventHandler<MapChangedEventArgs> OnMapChanged { add => _onMapChangedHandlers.Add(value);
	        remove => _onMapChangedHandlers.Remove(value);
        }

        private readonly List<EventHandler<MapSectorChangedEventArgs>> _onMapSectorChangedHandlers;

        public event EventHandler<MapSectorChangedEventArgs> OnMapSectorChanged { add => _onMapSectorChangedHandlers.Add(value);
	        remove => _onMapSectorChangedHandlers.Remove(value);
        }

        public event EventHandler<LivingObjectMovementEventArgs> OnMove;

        private List<EventHandler<MapObjectEventArgs>> _onDisposeHandlers;

        public event EventHandler<MapObjectEventArgs> OnDispose { add => _onDisposeHandlers.Add(value);
	        remove => _onDisposeHandlers.Remove(value);
        }

        private readonly List<EventHandler<UpdateCounterChangeEventArgs>> _onUpdateCounterChangedHandler;

        public event EventHandler<UpdateCounterChangeEventArgs> OnUpdateCounterChanged { add => _onUpdateCounterChangedHandler.Add(value);
	        remove => _onUpdateCounterChangedHandler.Remove(value);
        }
        
        #endregion MapObject

        public void SetQuestion(Question pQuestion)
        {
            lock (ThreadLocker)
            {
	            Question?.Dispose();

	            Question = pQuestion;
            }
        }

        public override bool RefreshCharacter(SQLResult pRes, int i, out CharacterErrors result)
        {
            if (!base.RefreshCharacter(pRes, i, out result))
                return false;

            LivingStats.Load(
                pRes.Read<uint>(i, "HP"),
                pRes.Read<uint>(i, "SP"),
                pRes.Read<uint>(i, "LP"));

            return true;
        }




        public void Broadcast(FiestaPacket packet) => InRange.Broadcast(packet);

        public void Broadcast(FiestaPacket packet, bool includeSelf)
        {
            Broadcast(packet);

            if (includeSelf
                && IsConnected)
            {
                Session.SendPacket(packet);
            }
        }

        public override void Dispose()
        {
            if (Interlocked.CompareExchange(ref _isDisposedInt, 1, 0) == 0)
            {
                base.Dispose();
            }
        }

        public override bool Save()
        {
            try
            {
                using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                {
                    cmd.CreateStoredProcedure("dbo.Character_Update_Zone");
                    cmd.SetParameter("@pID", Info.CharacterID);
                    cmd.SetParameter("@pName", Info.Name);
                    cmd.SetParameter("@pMap", (short)AreaInfo.MapInfo.ID);
                    cmd.SetParameter("@pPositionX", (int)Position.X);
                    cmd.SetParameter("@pPositionY", (int)Position.Y);
                    cmd.SetParameter("@pRotation", Position.Rotation);
                    cmd.SetParameter("@pClass", (byte)Info.Class);
                    cmd.SetParameter("@pLevel", Info.Level);
                    cmd.SetParameter("@pHP", (int)LivingStats.HP);
                    cmd.SetParameter("@pSP", (int)LivingStats.SP);
                    cmd.SetParameter("@pHPStones", (short)Info.HPStones);
                    cmd.SetParameter("@pSPStones", (short)Info.SPStones);
                    cmd.SetParameter("@pEXP", (long)Info.EXP);
                    cmd.SetParameter("@pMoney", (long)Info.Money);
                    cmd.SetParameter("@pFame", (int)Info.Fame);
                    cmd.SetParameter("@pKillPoints", (int)Info.KillPoints);
                    cmd.SetParameter("@pFriendPoints", (int)Info.FriendPoints);
                    cmd.SetParameter("@pFreeStat_Points", Info.FreeStats.FreeStat_Points);
                    cmd.SetParameter("@pFreeStat_Str", Info.FreeStats.StatPoints_STR);
                    cmd.SetParameter("@pFreeStat_End", Info.FreeStats.StatPoints_END);
                    cmd.SetParameter("@pFreeStat_Dex", Info.FreeStats.StatPoints_DEX);
                    cmd.SetParameter("@pFreeStat_Int", Info.FreeStats.StatPoints_INT);
                    cmd.SetParameter("@pFreeStat_Spr", Info.FreeStats.StatPoints_SPR);
                    cmd.SetParameter("@pSkillPoints", Info.SkillPoints);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                DatabaseLog.Write(ex, "Failed to Save ZoneCharacter");
                return false;
            }
        }

		public void WriteDisplay(FiestaPacket packet)
        {
            SH04Helpers.WriteCharacterDisplay(this, packet);
        }

        public void Die()
        {
        }

        public void OnUpdate(GameTime gameTime)
        {
        }

        public void WriteSelectionUpdate(FiestaPacket packet)
        {
            packet.Write<uint>(LivingStats.HP);
            packet.Write<int>(Stats.FullStats.MaxHP);
            packet.Write<uint>(LivingStats.SP);
            packet.Write<int>(Stats.FullStats.MaxSP);
            packet.Write<uint>(LivingStats.LP);
            packet.Write<int>(Stats.FullStats.MaxLP);
            packet.Write<byte>(Level);
            packet.Write<ushort>(UpdateCounter);
        }


        public void ChangeMap(ushort newMapId, ushort instanceId, uint spawnX = 0, uint spawnY = 0)
        {

            if (!MapDataProvider.GetMapInfoByID(newMapId, out var newMap))
            {
                ZoneChat.CharacterNote(this, $"Map not Found in Database");
                return;
            }

            if (!MapManager.GetMap(newMapId, instanceId, out var map))
            {
                ZoneChat.CharacterNote(this, $"Map {newMapId}:{instanceId} is Offline");
                return;
            }

            spawnX = (spawnX == 0 ? newMap.Regen.X : spawnX);
            spawnY = (spawnY == 0 ? newMap.Regen.Y : spawnY);

            CharacterMethods.SendCharacterChangeMap(Info.CharacterID,
                newMap.ID,
                new Position(spawnX, spawnY),
                instanceId);
        }

        protected override void DisposeInternal()
        {
            InvokeOnDispose();

            InRange.Dispose();
            InRange = null;
            _onMapChangedHandlers.Clear();
            _onMapChangedHandlers = null;

            _onDisposeHandlers.Clear();
            _onDisposeHandlers = null;


            _mapSector = null;

            Question?.Dispose();
            Question = null;

        

            base.DisposeInternal();

        }
        #region Event


        private void LivingStats_OnLPChanged(object sender, LivingObjectInterActiveStatsChangedEventArgs e)
        {
            if (IsConnected
                && Session.Ingame
                && CharacterClass.ClassUsedLP(Info.Class))
            {
                SH09Handler.SendLPUpdate(this);
            }
        }
        private void LivingStats_OnSPChanged(object sender, LivingObjectInterActiveStatsChangedEventArgs e)
        {
            if (IsConnected && Session.Ingame)
                SH09Handler.SendSPUpdate(this);
        }

        private void LivingStats_OnHPChanged(object sender, LivingObjectInterActiveStatsChangedEventArgs e)
        {
            if (IsConnected && Session.Ingame)
                SH09Handler.SendHPUpdate(this);
        }


        public void Move(Position newPosition, bool isRun, bool isStop)
        {
            lock (ThreadLocker)
            {
                var oldPosition = Position;
                Position = newPosition;
	            if (OnMove == null) return;
	            var args = new LivingObjectMovementEventArgs(this, oldPosition, newPosition, isRun, isStop);

	            OnMove.Invoke(this, args);
            }
        }
        private void InvokeUpdateCounterChanged()
        {
	        var args = new UpdateCounterChangeEventArgs(this, _updateCounter);
	        foreach (var t in _onUpdateCounterChangedHandler)
	        {
		        t.Invoke(this, args);
	        }
        }
        private void InvokeOnDispose()
        {
	        if (_onDisposeHandlers.Count <= 0) return;
	        var args = new MapObjectEventArgs(this);
	        foreach (var t in _onDisposeHandlers)
	        {
		        t.Invoke(this, args);
	        }
        }

        private void InvokeOnMapSectorChanged(MapSector oldSector)
        {
	        var args = new MapSectorChangedEventArgs(this, oldSector, _mapSector);

	        foreach (var t in _onMapSectorChangedHandlers)
	        {
		        t.Invoke(this, args);
	        }
        }

        public void InvokeOnMapChanged(IMap oldMap)
        {
	        var args = new MapChangedEventArgs(this, oldMap, Map);

	        foreach (var t in _onMapChangedHandlers)
	        {
		        t.Invoke(this, args);
	        }
        }
        public override bool LevelUP(ushort mobID = ushort.MaxValue, bool finalizeLevelUp = true)
        {
            if (!base.LevelUP(mobID, false))
                return false;


            if (!CharacterDataProvider.GetLevelParameters(Info.Class, Info.Level, out var parameters))
            {
                GameLog.Write(GameLogLevel.Warning, "Failed to Get Level Parameters Class {0} Level {1}", Info.Class,Info.Level);
                return false;
            }

           
      
           
            Stats.UpdateAll();
            //update hp + sp to 100%
            LivingStats.HP = (uint)Stats.FullStats.MaxHP;
            LivingStats.SP = (uint)Stats.FullStats.MaxSP;
            //update max hp and sp stones
            Info.MaxHPStones = Info.LevelParameter.MaxHPStones;
            Info.MaxSPStones = Info.LevelParameter.MaxSPStones;
            //give stat point
            Info.FreeStats.FreeStat_Points++;

            Info.LevelParameter = parameters;

            //give skill point
            if (Level % 2 != 0)
            {
                Info.SkillPoints++;
            }

            SH09Handler.SendLevelUpAnimation(this, mobID);
            SH09Handler.SendLevelUpData(this, mobID);
            //stat points
            SH04Handler.SendRemainingStatPoints(this.Session);
            //skill points
            if (Info.SkillPoints > 0)
            {
                SH18Handler.SendRemainingSkillPoints(this);
            }

            if (finalizeLevelUp)
                CharacterMethods.SendCharacterLevelChanged(Info.CharacterID, Level);



            return true;
        }

        public void ChangeClass(ClassId newClass)
        {
            CharacterMethods.SendCharacterChangeClass(this, newClass);
        }

        public override bool GiveEXP(uint amount, ushort mobId = ushort.MaxValue)
        {

            if (Level >= GameConfiguration.Instance.LimitSetting.LevelLimit)
                return false;


            if (!base.GiveEXP(amount, mobId))
            {
                return false;  
            }

            SH09Handler.SendGainEXP(this, amount, mobId);

            return true;
        }
    }

    #endregion

    
}