using DragonFiesta.Game.Accounts;
using DragonFiesta.Login.Game.Worlds;
using DragonFiesta.Messages.Login.Transfer;
using System;

namespace DragonFiesta.Login.InternNetwork.InternHandler.Server
{
    public static class ServerTransferMethods
    {
        public static void SendAddWorldTransfer(
            World World,
            Account UseAccount,
            string IP,
            Action<IMessage> CallBack)
        {
            AddWorldServerTransfer Msg = new AddWorldServerTransfer
            {
                Id = Guid.NewGuid(),
                Account = UseAccount,
                IP = IP,
                WorldId = World.Info.WorldID,
                Callback = CallBack,
            };

            World.SendMessage(Msg);
        }
    }
}
