using DragonFiesta.Networking.HandlerTypes;
using DragonFiesta.World.Data.Settings;

namespace DragonFiesta.World.Network.FiestaHandler.Server
{
    public class SH28Handler
    {
        public static void SendDefaultKeyMap(WorldSession Session)
        {
            using (var Packet = new FiestaPacket(Handler28Type._Header, Handler28Type.SMSG_CHAR_OPTION_IMPROVE_INIT_KEYMAP_ACK))
            {
                Packet.Write<ushort>(8374); //Error Code hmm
                Packet.Write(GameSettingDataProvider.DefaultKeyMap.GetKeyMapData());
                Session.SendPacket(Packet);
            }
        }
        public static void SendKeyMap(WorldSession mSession)
        {
            using (var mPacket = new FiestaPacket(Handler28Type._Header, Handler28Type.SMSG_CHAR_OPTION_IMPROVE_GET_KEYMAP_CMD))
            {

                byte[] Data = mSession.Character.Options.KeyMap.GetKeyMapData();

                mPacket.Write(Data);
                mSession.SendPacket(mPacket);
            }
        }

        public static void SendQuickBarEntry(WorldSession mSession)
        {
            using (var mPacket = new FiestaPacket(Handler28Type._Header, Handler28Type.SMSG_CHAR_OPTION_IMPROVE_GET_SHORTCUTDATA_CMD))
            {
                mPacket.WriteHexAsBytes("00 00");
                /*
                mPacket.Write<ushort>(3);//count

                for (int i = 1; i < 4; i++)
                {
                    mPacket.Write<byte>(i);
                    mPacket.Write<ushort>(4);
                    mPacket.Write<ushort>(3505);
                    mPacket.Write<ushort>(0);
                }*/
                mSession.SendPacket(mPacket);
            }
        }
        public static void SendGameOptions(WorldSession mSession)
        {
            using (var mPacket = new FiestaPacket(Handler28Type._Header, Handler28Type.SMSG_CHAR_OPTION_IMPROVE_INIT_GAMEOPTION_ACK))
            {
                mPacket.Write<ushort>(8480);    //Success else error code
                mPacket.Write(mSession.Character.Options.GameSettings.GetGameSettingData());
                mSession.SendPacket(mPacket);
            }
        }
        public static void SendGameSettingsInital(WorldSession mSession)
        {
            using (var mPacket = new FiestaPacket(Handler28Type._Header, Handler28Type.SMSG_CHAR_OPTION_IMPROVE_GET_GAMEOPTION_CMD))
            {
                mPacket.Write(mSession.Character.Options.GameSettings.GetGameSettingData());
                mSession.SendPacket(mPacket);
            }
        }
    }
}