using DragonFiesta.Messages.Zone.Maps;
using DragonFiesta.Messages.Zone.Zone;
using System;

namespace DragonFiesta.Zone.InternNetwork.InternHandler.Server.Zone
{
    public static class ZoneServerMethods
    {
        public static void SendMapListRequest(Action<IMessage> CallBack)
        {
            MapListRequest Request = new MapListRequest
            {
                Id = Guid.NewGuid(),
                Callback = CallBack,
            };
            InternWorldConnector.Instance.SendMessage(Request);
        }
        public static void SendOnlineCharacterRequest(Action<IMessage> Callback)
        {
            OnlineCharacterListRequest Request = new OnlineCharacterListRequest
            {
                Id = Guid.NewGuid(),
                Callback = Callback,
            };

            InternWorldConnector.Instance.SendMessage(Request);
        }
    }
}
