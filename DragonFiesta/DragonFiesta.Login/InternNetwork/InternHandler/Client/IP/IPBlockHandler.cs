using DragonFiesta.Login.Network;
using DragonFiesta.Messages.Login.IPBlock;
using System;
using DragonFiesta.Utils.Logging;

namespace DragonFiesta.Login.InternNetwork.InternHandler.Client.IP
{
    public class IPBlockHandler
    {
        [InternMessageHandler(typeof(AddIPBlock))]
        public static void HandleAddIPBlock(AddIPBlock Request, InternWorldSession pSession)
        {
            if (IPBlockManager.BlockIP(Request.IP, DateTime.Now, Request.Reason))
                GameLog.Write(GameLogLevel.Internal, $"Add IPBlock IP {Request.IP} Reason {Request.Reason}");
        }

        [InternMessageHandler(typeof(RemoveIPBlock))]
        public static void HandleRemoveIPBlock(RemoveIPBlock Request, InternWorldSession pSession)
        {
            if (IPBlockManager.BlockIP(Request.IP))
                GameLog.Write(GameLogLevel.Internal, $"Remove IPBlock IP {Request.IP}");
        }
    }
}