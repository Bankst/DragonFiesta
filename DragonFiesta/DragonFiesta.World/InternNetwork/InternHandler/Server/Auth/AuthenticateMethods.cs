using DragonFiesta.Messages.Message.Auth;
using DragonFiesta.World.Config;
using DragonFiesta.World.InternNetwork.InternHandler.Response.Login;
using DragonFiesta.World.Network;
using System;

namespace DragonFiesta.World.InternNetwork.InternHandler.Server.Auth
{
    public class AuthenticateMethods
    {
        public static void SendAuthWorld(InternLoginConnector Session)
        {
            Session.SendMessage(new AuthenticatetWorld
            {
                Id = Guid.NewGuid(),
                WorldId = WorldConfiguration.Instance.WorldID,
                Password = WorldConfiguration.Instance.InternalServerInfo.Password,
                Port = WorldConfiguration.Instance.ServerInfo.ListeningPort,
                MaxConnection = WorldConfiguration.Instance.ServerInfo.MaxConnection,
                ActiveAccounts = WorldSessionManager.Instance.GetAccountList(),
                IP = WorldConfiguration.Instance.ServerInfo.ListeningIP,
                Callback = Auth_Response.HandleAuthtecicateWorld_Response,
                TimeOutCallBack = Auth_Response.AuthLoginTimeout,
            });
        }
    }
}