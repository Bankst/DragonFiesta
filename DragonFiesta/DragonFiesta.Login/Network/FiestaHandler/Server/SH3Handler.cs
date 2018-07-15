using DragonFiesta.Game.Worlds;
using DragonFiesta.Login.Game.Worlds;
using DragonFiesta.Networking.HandlerTypes;

namespace DragonFiesta.Login.Network.FiestaHandler.Server
{
    public class SH3Handler
    {
        public static void WorldServerIP(LoginSession pClient, World pWorld, WorldStatus status = WorldStatus.LoginFailed)
        {
            using (var mIpPacket = new FiestaPacket(Handler03Type._Header, Handler03Type.SMSG_USER_WORLDSELECT_ACK))
            {
                mIpPacket.Write<byte>(pWorld != null && status != WorldStatus.OK ? pWorld.Status : status);//state?
                mIpPacket.WriteString(pWorld != null ? pWorld.ConnectionInfo.WorldExternalIP : "", 16);
                mIpPacket.Write<ushort>(pWorld?.ConnectionInfo.WorldPort ?? 0);
                mIpPacket.Write<int>(pWorld != null ? pClient.UserAccount.ID : 0);//acc id
                mIpPacket.Fill(60, 0x00);
                pClient.SendPacket(mIpPacket);
            }
        }

        public static void BinVersionAllowed(LoginSession pClient, bool allowed)
        {
            using (var pack = new FiestaPacket(Handler03Type._Header, allowed ? Handler03Type.SMSG_USER_CLIENT_RIGHTVERSION_CHECK_ACK : Handler03Type.SMSG_USER_CLIENT_WRONGVERSION_CHECK_ACK))
            {
                pack.Write<bool>(allowed);
                pack.Write<byte>(0);//unk
                pClient.SendPacket(pack);
            }
        }

        public static void SendWorldList(LoginSession pSession, bool ping)
        {
            using (var packet = new FiestaPacket(Handler03Type._Header, ping ? Handler03Type.SMSG_USER_WORLD_STATUS_ACK : Handler03Type.SMSG_USER_LOGIN_ACK))
            {
                packet.Write<byte>((byte)WorldManager.Instance.WorldList.Count);

                foreach (var pWorld in WorldManager.Instance.WorldList)
                {
                    pWorld.WriteInfo(packet);
                }

                pSession.SendPacket(packet);
            }
        }
    }
}