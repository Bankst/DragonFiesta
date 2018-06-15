using DragonFiesta.Game.Characters;
using DragonFiesta.Messages.Zone.Transfer;
using DragonFiesta.Providers.Maps;
using DragonFiesta.World.Game.Chat;
using DragonFiesta.World.Game.Friends;
using DragonFiesta.World.Game.Maps;
using DragonFiesta.World.Game.Transfer;
using DragonFiesta.World.InternNetwork.InternHandler.Response.Transfer;
using DragonFiesta.World.InternNetwork.InternHandler.Server.Character;
using DragonFiesta.World.Network;
using System;

namespace DragonFiesta.World.Game.Character
{
    public class WorldCharacter : CharacterBase
    {



        public Action ZoneTransferCallback { get; set; }

        public new WorldClientLoginInfo LoginInfo { get => base.LoginInfo as WorldClientLoginInfo; }

        public override bool IsGM => (LoginInfo != null && LoginInfo.GameRole != null && LoginInfo.GameRole.Id > 0);

        public new WorldCharacterInfo Info { get => base.Info as WorldCharacterInfo; }

        public WorldCharacter_Options Options { get; private set; }

        public bool IsOnInstance { get => Map is IInstanceMap; }

        public WorldServerMap Map { get => (AreaInfo.Map as WorldServerMap); }

        public WorldSession Session { get; set; }

        public override bool IsConnected => (Session != null && Session.IsConnected);


        public FriendCollection Friends { get; private set; }

        public WorldCharacter() : base()
        {
            base.LoginInfo = new WorldClientLoginInfo(this);
            base.Info = new WorldCharacterInfo(this);
            Options = new WorldCharacter_Options(this);
            Friends = new FriendCollection(this);
        }


        public void SetFriendPoint(ushort NewFriendPoint)
            => CharacterMethods.SendSetFriendPoint(this, NewFriendPoint);

        public bool ChangeMap(ushort mapId, ushort InstanceId, uint SpawnX = 0, uint SpawnY = 0)
        {


            if (!MapDataProvider.GetFieldInfosByMapID(mapId, out FieldInfo Info))
            {
                ZoneChat.CharacterNote(this, $"FieldInfo {mapId} not found!");
                return false;
            }

            if (!MapManager.GetMap(Info, InstanceId, out WorldServerMap NewMap))
            {
                ZoneChat.CharacterNote(this, $"Map {mapId} Instance {InstanceId} not found!");
                return false;
            }

            SpawnX = (SpawnX == 0 ? NewMap.Info.RegenMap.Regen.X : SpawnX);
            SpawnY = (SpawnY == 0 ? NewMap.Info.RegenMap.Regen.Y : SpawnY);

            WorldMapTransfer Transfer = new WorldMapTransfer
            {
                Character = this,
                Map = NewMap,
            };



            if (Map.Info.ZoneID != NewMap.ZoneId)
            {
                ZoneTransferMessage TransferMsg = new ZoneTransferMessage
                {
                    Id = Guid.NewGuid(),
                    CharacterId = this.Info.CharacterID,
                    InstanceId = InstanceId,
                    MapId = NewMap.MapId,
                    SpawnPosition = new Position(SpawnX, SpawnY),
                    WorldSessionId = Session.BaseStateInfo.SessionId,
                    Callback = ZoneTransfer_Response.HandleZoneTransfer_Response
                };
                ZoneTransferCallback = () =>
               {
                   //change Client 
                   if (!WorldServerTransferManager.AddTransfer(Transfer))
                   {
                       Session.Dispose();
                       return;
                   }


                   CharacterMethods.BroadcastCharacterChangeMap(
                       this.Info.CharacterID,
                       Info.MapInfo.ID,
                       new Position(SpawnX, SpawnY),
                       InstanceId);

                   WorldCharacterManager.Instance.CharacterChangeMap(this, NewMap);
               };

                //send Request to zone
                NewMap.Zone.Send(TransferMsg);
            }
            else
            {
                if (!WorldServerTransferManager.AddTransfer(Transfer))
                {
                    Session.Dispose();
                    return false;
                }

                CharacterMethods.BroadcastCharacterChangeMap(
                    this.Info.CharacterID,
                    Info.MapInfo.ID,
                    new Position(SpawnX, SpawnY),
                    InstanceId);

                WorldCharacterManager.Instance.CharacterChangeMap(this, NewMap);
            }

            return true;
        }

        public override bool FinalizeLoad(out CharacterErrors Result)
        {
            if (!Friends.Refresh())
            {
                Result = CharacterErrors.ErrorInFriendInfo;
                return false;
            }

            if (!Options.Refresh())
            {
                Result = CharacterErrors.ErrorInOptions;
                return false;
            }

            return base.FinalizeLoad(out Result);
        }

       
        public override bool RefreshCharacter(SQLResult pRes, int i, out CharacterErrors Result)
        {
            if (!base.RefreshCharacter(pRes, i, out Result))
                return false;

           
            return true;
        }


        public override bool Save()
        {
            try
            {
                using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                {
                    cmd.CreateStoreProzedure("dbo.Character_Update_World");
                    cmd.SetParameter("@pID", Info.CharacterID);
                    cmd.SetParameter("@pIsMale", Info.IsMale);
                    cmd.SetParameter("@pHair", Style.Hair.ID);
                    cmd.SetParameter("@pHairColor", Style.HairColor.ID);
                    cmd.SetParameter("@pFace", Style.Face.ID);
                    cmd.ExecuteNonQuery();
                }

             


                return true;
            }
            catch (Exception ex)
            {
                DatabaseLog.Write(ex, "Failed to save WorldCharacter");
                return false;
            }
        }

        public override bool GiveEXP(uint Amount, ushort MobId = ushort.MaxValue)
        {
            throw new NotImplementedException();
        }

        protected override void DisposeInternal()
        {
            //here invoke dispose
            base.DisposeInternal();


            Friends.Dispose();
            Friends = null;

            ZoneTransferCallback = null;
            Session = null;


        }

    }
}