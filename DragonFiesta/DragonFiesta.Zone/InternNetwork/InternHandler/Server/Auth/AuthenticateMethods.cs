using DragonFiesta.Messages.Zone;
using DragonFiesta.Utils.Config.Section.Network;
using DragonFiesta.Zone.Config;
using DragonFiesta.Zone.InternNetwork.InternHandler.Response.Auth;
using System;

namespace DragonFiesta.Zone.InternNetwork.InternHandler.Server.Auth
{
    public static class AuthenticateMethods
    {
        public static void SendAuthZone(InternWorldConnector Session)
        {
            Session.SendMessage(new AuthenticatedZone
            {
                Id = Guid.NewGuid(),
                ZoneId = ZoneConfiguration.Instance.ZoneID,
                NetInfo = new ExternServerInfo
                {
                    ListeningIP = ZoneConfiguration.Instance.ZoneServerInfo.ListeningIP,
                    ListeningPort = ZoneConfiguration.Instance.ZoneServerInfo.ListeningPort,
                    MaxConnection = ZoneConfiguration.Instance.ZoneServerInfo.MaxConnection,
                    Password = ZoneConfiguration.Instance.ZoneServerInfo.Password,
                },
                Callback = Auth_Response.HandleAuthenticatetZone_Response,
                TimeOutCallBack = Auth_Response.AuthWorldTimeout,
            });
        }
    }
}