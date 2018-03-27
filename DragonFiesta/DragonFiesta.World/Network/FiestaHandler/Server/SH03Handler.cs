using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.Networking.Helpers;
using System;

namespace DragonFiesta.World.Network.FiestaHandler.Server
{
    public static class SH03Handler
    {
        public static void SendCharacterList(WorldSession pSession)
        {
            using (FiestaPacket mPacket = new FiestaPacket(Handler03Type._Header, Handler03Type.SMSG_USER_LOGINWORLD_ACK))
            {
                mPacket.Write<ushort>(pSession.BaseStateInfo.SessiondId);
                mPacket.Write<byte>((byte)pSession.CharacterList.Count);
                for (int i = 0; i < pSession.CharacterList.Count; i++)
                {
                    _SH04Helpers.WriteBasicInfo(pSession.CharacterList[i], mPacket);
                }
                pSession.SendPacket(mPacket);
            }
        }

        public static void SendLoginToWorldKey(WorldSession pSession, Guid Id, bool IsValid)
        {
            using (var mPacket = new FiestaPacket(Handler03Type._Header, Handler03Type.SMSG_USER_WILL_WORLD_SELECT_ACK))
            {
                mPacket.Write<ushort>(IsValid ? (ushort)7768 : (ushort)73);
                mPacket.WriteString(Id.ToString("N"), 32);
                pSession.SendPacket(mPacket);
            }
        }
    }
}