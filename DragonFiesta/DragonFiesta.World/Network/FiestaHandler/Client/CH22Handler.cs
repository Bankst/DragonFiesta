using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.World.Game.Transfer;
using DragonFiesta.World.Network.FiestaHandler.Server;

namespace DragonFiesta.World.Network.FiestaHandler.Client
{
    [PacketHandlerClass(Handler22Type._Header)]
    public class CH22Handler
    {
        [PacketHandler(Handler22Type.CMSG_KQ_LIST_REFRESH_REQ)]
        public static void CMSG_CLIENT_READY(WorldSession mSession, FiestaPacket packet)
        {
            if (mSession.Ingame)
            {
                mSession.Dispose();
                return;
            }

            if (!WorldServerTransferManager.FinishTransfer(mSession.Character.Info.CharacterID, out WorldMapTransfer Transfer))
            {
                mSession.Dispose();
                return;
            }

            //Update GameTimeSync
            SH02Handler.SendGameTimeUpdatePacket(mSession, GameTime.Now().Time);

            mSession.GameStates.IsTransferring = false;
            mSession.GameStates.IsReady = true;

            Game.Character.WorldCharacterManager.Instance.CharacterMapRefreshed(mSession.Character);
        }
    }
}