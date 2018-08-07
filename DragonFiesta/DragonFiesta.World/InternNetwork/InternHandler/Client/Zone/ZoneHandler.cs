using DragonFiesta.Messages.Zone.Zone;
using DragonFiesta.World.Game.Character;
using DragonFiesta.World.Game.Zone;
using System;
using System.Collections.Generic;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Zone
{
    public class ZoneHandler
    {
        [InternMessageHandler(typeof(OnlineCharacterListRequest))]
        public static void HandleCharacterListRequest(OnlineCharacterListRequest ZoneMsg,  InternZoneSession pSession)
        {
            ZoneMsg.OnlineCharacters = new List<OnlineCharacter>();

            WorldCharacterManager.Instance.OnlineCharacterList.ForEach(m => ZoneMsg.OnlineCharacters.Add(new OnlineCharacter
            {
                CharacterId = m.Info.CharacterID,
                InstanceId = m.AreaInfo.InstanceId,
                MapId = m.AreaInfo.Map.MapId,
            }));

            GameLog.Write(GameLogLevel.Internal, "OnlineCharacterList Requesting from Zone {0}", pSession.Zone.ID);

            pSession.SendMessage(ZoneMsg, false);
        }

        [InternMessageHandler(typeof(UpdateZoneServer))]
        public static void HandleUpdateZoneServer(UpdateZoneServer mZoneUpdateMsg, InternZoneSession pSession)
        {
            if (!ZoneManager.GetZoneByID(mZoneUpdateMsg.ZoneId, out ZoneServer mZone))
                return;

            mZone.CurrentConnection = mZoneUpdateMsg.CurrentConnection;

            ZoneManager.Broadcast(new UpdateZoneServer
            {
                Id = Guid.NewGuid(),
                CurrentConnection = mZone.CurrentConnection,
                ZoneId = mZone.ID,
            }, mZone.ID); //Tels Anothers Zone Update Connection...
        }
    }
}