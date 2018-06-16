using System;
using System.Collections.Generic;
using System.Threading;
using DragonFiesta.Game.Characters;
using DragonFiesta.Game.Stats;
using DragonFiesta.Providers.Characters;
using DragonFiesta.Providers.Maps;
using DragonFiesta.Utils.Config;
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

        public new ZoneCharacterInfo Info { get => base.Info as ZoneCharacterInfo; }

        public bool IsOnThisZone => (IsConnected && Map != null && Map is LocalMap);

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

        private MapSector _MapSector;

        public MapSector MapSector
        {
            get { return _MapSector; }
            set
            {
                var oldSector = _MapSector;

                _MapSector = value;

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

        private ushort _UpdateCounter;

        public ushort UpdateCounter
        {
            get
            {
                if (_UpdateCounter == 0xffff)
                {
                    _UpdateCounter = 0;
                }
                _UpdateCounter++;

                InvokeUpdateCounterChanged();

                return _UpdateCounter;
            }
        }


        private int IsDisposedInt;

        #endregion Property

        public ZoneCharacter() : base()
        {
            LoginInfo = new ZoneClientLoginInfo(this);
            base.Info = new ZoneCharacterInfo(this);


            LivingStats = new LivingStats(this);

            LivingStats.OnHPChanged += LivingStats_OnHPChanged;
            LivingStats.OnSPChanged += LivingStats_OnSPChanged;
            LivingStats.OnLPChanged += LivingStats_OnLPChanged;
            OnMapChangedHandlers = new List<EventHandler<MapChangedEventArgs>>();
            OnMapSectorChangedHandlers = new List<EventHandler<MapSectorChangedEventArgs>>();
            OnDisposeHandlers = new List<EventHandler<MapObjectEventArgs>>();
            OnUpdateCounterChangedHandler = new List<EventHandler<UpdateCounterChangeEventArgs>>();




            InRange = new InRangeCollection(this);
            Selection = new CharacterObjectSelection(this);

        }




        #region MapObject

        private List<EventHandler<MapChangedEventArgs>> OnMapChangedHandlers;

        public event EventHandler<MapChangedEventArgs> OnMapChanged { add { OnMapChangedHandlers.Add(value); } remove { OnMapChangedHandlers.Remove(value); } }

        private List<EventHandler<MapSectorChangedEventArgs>> OnMapSectorChangedHandlers;

        public event EventHandler<MapSectorChangedEventArgs> OnMapSectorChanged { add { OnMapSectorChangedHandlers.Add(value); } remove { OnMapSectorChangedHandlers.Remove(value); } }

        public event EventHandler<LivingObjectMovementEventArgs> OnMove;

        private List<EventHandler<MapObjectEventArgs>> OnDisposeHandlers;

        public event EventHandler<MapObjectEventArgs> OnDispose { add { OnDisposeHandlers.Add(value); } remove { OnDisposeHandlers.Remove(value); } }

        private List<EventHandler<UpdateCounterChangeEventArgs>> OnUpdateCounterChangedHandler;

        public event EventHandler<UpdateCounterChangeEventArgs> OnUpdateCounterChanged { add { OnUpdateCounterChangedHandler.Add(value); } remove { OnUpdateCounterChangedHandler.Remove(value); } }
        
        #endregion MapObject

        public void SetQuestion(Question pQuestion)
        {
            lock (ThreadLocker)
            {
                if (Question != null)
                    Question.Dispose();

                Question = pQuestion;
            }
        }





        public override bool RefreshCharacter(SQLResult pRes, int i, out CharacterErrors Result)
        {
            if (!base.RefreshCharacter(pRes, i, out Result))
                return false;

            LivingStats.Load(
                pRes.Read<uint>(i, "HP"),
                pRes.Read<uint>(i, "SP"),
                pRes.Read<uint>(i, "LP"));


            return true;
        }


        public void Broadcast(FiestaPacket Packet) => InRange.Broadcast(Packet);

        public void Broadcast(FiestaPacket Packet, bool IncludeSelf)
        {
            Broadcast(Packet);

            if (IncludeSelf
                && IsConnected)
            {
                Session.SendPacket(Packet);
            }
        }



        public override void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
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

        public void OnUpdate(GameTime Now)
        {
        }

        public void WriteSelectionUpdate(FiestaPacket Packet)
        {
            Packet.Write<uint>(LivingStats.HP);
            Packet.Write<int>(Stats.FullStats.MaxHP);
            Packet.Write<uint>(LivingStats.SP);
            Packet.Write<int>(Stats.FullStats.MaxSP);
            Packet.Write<uint>(LivingStats.LP);
            Packet.Write<int>(Stats.FullStats.MaxLP);
            Packet.Write<byte>(Level);
            Packet.Write<ushort>(UpdateCounter);
        }


        public void ChangeMap(ushort NewMapId, ushort InstanceId, uint SpawnX = 0, uint SpawnY = 0)
        {

            if (!MapDataProvider.GetMapInfoByID(NewMapId, out MapInfo NewMap))
            {
                ZoneChat.CharacterNote(this, $"Map not Found in Database");
                return;
            }

            if (!MapManager.GetMap(NewMapId, InstanceId, out ZoneServerMap Map))
            {
                ZoneChat.CharacterNote(this, $"Map {NewMapId}:{InstanceId} is Offline");
                return;
            }

            SpawnX = (SpawnX == 0 ? NewMap.Regen.X : SpawnX);
            SpawnY = (SpawnY == 0 ? NewMap.Regen.Y : SpawnY);





            CharacterMethods.SendCharacterChangeMap(Info.CharacterID,
                NewMap.ID,
                new Position(SpawnX, SpawnY),
                InstanceId);
        }

        protected override void DisposeInternal()
        {
            InvokeOnDispose();

            InRange.Dispose();
            InRange = null;
            OnMapChangedHandlers.Clear();
            OnMapChangedHandlers = null;

            OnDisposeHandlers.Clear();
            OnDisposeHandlers = null;


            _MapSector = null;

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


        public void Move(Position NewPosition, bool IsRun, bool IsStop)
        {
            lock (ThreadLocker)
            {
                var OldPosition = Position;
                Position = NewPosition;
                if (OnMove != null)
                {
                    var args = new LivingObjectMovementEventArgs(this, OldPosition, NewPosition, IsRun, IsStop);

                    OnMove.Invoke(this, args);
                }
            }
        }
        private void InvokeUpdateCounterChanged()
        {
            var args = new UpdateCounterChangeEventArgs(this, _UpdateCounter);
            for (int i = 0; i < OnUpdateCounterChangedHandler.Count; i++)
            {
                OnUpdateCounterChangedHandler[i].Invoke(this, args);
            }
        }
        private void InvokeOnDispose()
        {
            if (OnDisposeHandlers.Count > 0)
            {
                var args = new MapObjectEventArgs(this);
                for (int i = 0; i < OnDisposeHandlers.Count; i++)
                {
                    OnDisposeHandlers[i].Invoke(this, args);
                }
            }
        }

        private void InvokeOnMapSectorChanged(MapSector OldSector)
        {
            var args = new MapSectorChangedEventArgs(this, OldSector, _MapSector);

            for (int i = 0; i < OnMapSectorChangedHandlers.Count; i++)
            {
                OnMapSectorChangedHandlers[i].Invoke(this, args);
            }
        }

        public void InvokeOnMapChanged(IMap OldMap)
        {
            var args = new MapChangedEventArgs(this, OldMap, Map);

            for (int i = 0; i < OnMapChangedHandlers.Count; i++)
            {
                OnMapChangedHandlers[i].Invoke(this, args);
            }
        }
        public override bool LevelUP(ushort MobID = ushort.MaxValue, bool FinalizeLevelUp = true)
        {
            if (!base.LevelUP(MobID, false))
                return false;


            if (!CharacterDataProvider.GetLevelParameters(Info.Class, Info.Level, out CharacterLevelParameter Parameters))
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

            Info.LevelParameter = Parameters;

            //give skill point
            if (Level % 2 != 0)
            {
                Info.SkillPoints++;
            }

            SH09Handler.SendLevelUpAnimation(this, MobID);
            SH09Handler.SendLevelUpData(this, MobID);
            //stat points
            SH04Handler.SendRemainingStatPoints(this.Session);
            //skill points
            if (Info.SkillPoints > 0)
            {
                SH18Handler.SendRemainingSkillPoints(this);
            }

            if (FinalizeLevelUp)
                CharacterMethods.SendCharacterLevelChanged(Info.CharacterID, Level);



            return true;
        }

        public void ChangeClass(ClassId NewClass)
        {
            CharacterMethods.SendCharacterChangeClass(this, NewClass);
        }

        public override bool GiveEXP(uint Amount, ushort MobId = ushort.MaxValue)
        {

            if (Level >= GameConfiguration.Instance.LimitSetting.LevelLimit)
                return false;


            if (!base.GiveEXP(Amount, MobId))
            {
                return false;  
            }

            SH09Handler.SendGainEXP(this, Amount, MobId);

            return true;
        }
    }

    #endregion

    
}