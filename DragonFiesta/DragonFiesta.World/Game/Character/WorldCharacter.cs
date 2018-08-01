using System;

using DragonFiesta.Database.SQL;
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

namespace DragonFiesta.World.Game.Character
{
	public class WorldCharacter : CharacterBase
    {
        public Action ZoneTransferCallback { get; set; }

        public new WorldClientLoginInfo LoginInfo { get => base.LoginInfo as WorldClientLoginInfo; }

        public override bool IsGM => (LoginInfo != null && LoginInfo.GameRole != null && LoginInfo.GameRole.Id > 0);

        public new WorldCharacterInfo Info { get => base.Info as WorldCharacterInfo; }

        public WorldCharacter_Options Options { get; private set; }

        public bool IsOnInstance => Map is IInstanceMap;

	    public new WorldServerMap Map => (AreaInfo.Map as WorldServerMap);

	    public WorldSession Session { get; set; }

        public override bool IsConnected => (Session != null && Session.IsConnected);

	    // TODO: Rewrite FriendCollection
		//public new FriendCollection Friends { get; private set; }

        public WorldCharacter() : base()
        {
            base.LoginInfo = new WorldClientLoginInfo(this);
            base.Info = new WorldCharacterInfo(this);
            Options = new WorldCharacter_Options(this);

	        // TODO: Rewrite FriendCollection
			//Friends = new FriendCollection(this);
		}

		public void SetFriendPoint(ushort newFriendPoint)
            => CharacterMethods.SendSetFriendPoint(this, newFriendPoint);

        public bool ChangeMap(ushort mapId, ushort instanceId, uint spawnX = 0, uint spawnY = 0)
		{
            if (!MapDataProvider.GetFieldInfosByMapID(mapId, out FieldInfo info))
            {
                ZoneChat.CharacterNote(this, $"FieldInfo {mapId} not found!");
                return false;
            }

            if (!MapManager.GetMap(info, instanceId, out WorldServerMap newMap))
            {
                ZoneChat.CharacterNote(this, $"Map {mapId} Instance {instanceId} not found!");
                return false;
            }

            spawnX = (spawnX == 0 ? newMap.Info.RegenMap.Regen.X : spawnX);
            spawnY = (spawnY == 0 ? newMap.Info.RegenMap.Regen.Y : spawnY);

            WorldMapTransfer transfer = new WorldMapTransfer
            {
                Character = this,
                Map = newMap,
            };

            if (Map.Info.ZoneID != newMap.ZoneId)
            {
                ZoneTransferMessage transferMsg = new ZoneTransferMessage
                {
                    Id = Guid.NewGuid(),
                    CharacterId = this.Info.CharacterID,
                    InstanceId = instanceId,
                    MapId = newMap.MapId,
                    SpawnPosition = new Position(spawnX, spawnY),
                    WorldSessionId = Session.BaseStateInfo.SessionId,
                    Callback = ZoneTransfer_Response.HandleZoneTransfer_Response
                };
                ZoneTransferCallback = () =>
               {
                   //change Client 
                   if (!WorldServerTransferManager.AddTransfer(transfer))
                   {
                       Session.Dispose();
                       return;
                   }

                   CharacterMethods.BroadcastCharacterChangeMap(
                       this.Info.CharacterID,
                       info.MapInfo.ID,
                       new Position(spawnX, spawnY),
                       instanceId);

                   WorldCharacterManager.Instance.CharacterChangeMap(this, newMap);
               };

                //send Request to zone
                newMap.Zone.Send(transferMsg);
            }
            else
            {
                if (!WorldServerTransferManager.AddTransfer(transfer))
                {
                    Session.Dispose();
                    return false;
                }

                CharacterMethods.BroadcastCharacterChangeMap(
                    this.Info.CharacterID,
                    info.MapInfo.ID,
                    new Position(spawnX, spawnY),
                    instanceId);

                WorldCharacterManager.Instance.CharacterChangeMap(this, newMap);
            }

            return true;
        }

        public override bool FinalizeLoad(out CharacterErrors result)
        {
			/*
            if (!Friends.RefreshEntity())
            {
                result = CharacterErrors.ErrorInFriendInfo;
                return false;
            }
			*/

            if (!Options.Refresh())
            {
                result = CharacterErrors.ErrorInOptions;
                return false;
            }

            return base.FinalizeLoad(out result);
        }

        public override bool Save()
        {
            try
            {
                using (var cmd = DB.GetDatabaseClient(DatabaseType.World))
                {
                    cmd.CreateStoredProcedure("dbo.Character_Update_World");
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

        public override bool GiveEXP(uint amount, ushort mobId = ushort.MaxValue)
        {
            throw new NotImplementedException();
        }

        protected override void DisposeInternal()
        {
            //here invoke dispose
            base.DisposeInternal();

			// TODO: Rewrite FriendCollection
			/*
	        if (Friends != null)
	        {
		        Friends.Dispose();
		        Friends = null;
			}
			*/

			ZoneTransferCallback = null;
            Session = null;
        }
    }
}