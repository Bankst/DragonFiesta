using DragonFiesta.Messages.Zone;
using DragonFiesta.World.Config;
using DragonFiesta.World.Game.Zone;
using System;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Auth
{
    public static class ZoneAuthhandler
    {
        [InternMessageHandler(typeof(AuthenticatedZone))]
        public static void HandleAuthenticatetZone(AuthenticatedZone mAuthMessage, InternZoneSession pSession)
        {
            AuthenticatedZone_Response mResponse = new AuthenticatedZone_Response
            {
                Id = mAuthMessage.Id,
                Region = WorldConfiguration.Instance.ServerRegion,
                RemoteZoneList = ZoneManager.FindAllActiveZone(),
                Result = InternZoneAuthResult.InvalidZoneId,
            };

            if (!WorldConfiguration.Instance.ServerInfo.Password.Equals(mAuthMessage.NetInfo.Password))
            {
                mResponse.Result = InternZoneAuthResult.InvalidPassword;
            }
            else if (!ZoneManager.GetZoneByID(mAuthMessage.ZoneId, out ZoneServer mZone))
            {
                mResponse.Result = InternZoneAuthResult.InvalidZoneId;
            }
            else if (mZone.IsConnected || mZone.IsReady)
            {
                mResponse.Result = InternZoneAuthResult.IdAlreadyRegistered;
            }
            else
            {
                mResponse.Result = InternZoneAuthResult.OK;

                pSession.SessionStateInfo.Authenticated = true;

                mZone.SetClient(pSession);
                mZone.NetInfo = mAuthMessage.NetInfo;

                NewZoneStarted NewZoneStart = new NewZoneStarted //Tells another active zone
                {
                    Id = Guid.NewGuid(),
                    ZoneServer = mZone,
                };

                ZoneManager.Broadcast(NewZoneStart, mZone.ID);

                ServerMain.InternalInstance.Title.Update();
            }

            pSession.SendMessage(mResponse);
        }
    }
}