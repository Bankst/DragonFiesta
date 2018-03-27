using DragonFiesta.Game.Worlds;
using DragonFiesta.Login.Game.Worlds;
using DragonFiesta.Networking.HandlerTypes;

namespace DragonFiesta.Login.Network.FiestaHandler.Server
{
    public class SH3Handler
    {
        public static void WorldServerIP(LoginSession pClient, World pWorld, WorldStatus Status = WorldStatus.LoginFailed)
        {
            using (FiestaPacket mIPPacket = new FiestaPacket(Handler03Type._Header, Handler03Type.SMSG_USER_WORLDSELECT_ACK))
            {
                mIPPacket.Write<byte>(pWorld != null && Status != WorldStatus.OK ? pWorld.Status : Status);//state?
                mIPPacket.WriteString(pWorld != null ? pWorld.ConnectionInfo.WorldIP : "", 16);
                mIPPacket.Write<ushort>(pWorld != null ? pWorld.ConnectionInfo.WorldPort : (ushort)0);
                mIPPacket.Write<int>(pWorld != null ? pClient.UserAccount.ID : 0);//acc id
                mIPPacket.Fill(60, 0x00);
                pClient.SendPacket(mIPPacket);
            }
        }

        public static void BinVersionAllowed(LoginSession pClient, bool Allowed)
        {
            using (var pack = new FiestaPacket(Handler03Type._Header, Allowed ? Handler03Type.SMSG_USER_CLIENT_RIGHTVERSION_CHECK_ACK : Handler03Type.SMSG_USER_CLIENT_WRONGVERSION_CHECK_ACK))
            {
                pack.Write<bool>(Allowed);
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