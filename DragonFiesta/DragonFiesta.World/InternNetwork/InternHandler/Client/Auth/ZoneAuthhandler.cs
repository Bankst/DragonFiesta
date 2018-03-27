using DragonFiesta.Messages.Zone;
using DragonFiesta.World.Config;
using DragonFiesta.World.Game.Zone;
using System;

namespace DragonFiesta.World.InternNetwork.InternHandler.Client.Auth
{
    public static class ZoneAuthhandler
    {
        [InternMessageHandler(typeof(AuthenticatetZone))]
        public static void HandleAuthenticatetZone(AuthenticatetZone mAuthMessage, InternZoneSession pSession)
        {
            AuthenticatetZone_Response mResponse = new AuthenticatetZone_Response
            {
                Id = mAuthMessage.Id,
                Region = WorldConfiguration.Instance.ServerRegion,
                RemoteZoneList = ZoneManager.FindAllActiveZone(),
                Result = InternZoneAuthesult.InvalidZoneId,
            };

            if (!WorldConfiguration.Instance.ServerInfo.Password.Equals(mAuthMessage.NetInfo.Password))
            {
                mResponse.Result = InternZoneAuthesult.InvalidPassword;
            }
            else if (!ZoneManager.GetZoneByID(mAuthMessage.ZoneId, out ZoneServer mZone))
            {
                mResponse.Result = InternZoneAuthesult.InvalidZoneId;
            }
            else if (mZone.IsConnected || mZone.IsReady)
            {
                mResponse.Result = InternZoneAuthesult.IdAlredyRegister;
            }
            else
            {
                mResponse.Result = InternZoneAuthesult.OK;

                pSession.SessionStateInfo.Authenticatet = true;

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