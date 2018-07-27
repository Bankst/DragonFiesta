using DragonFiesta.Networking.HandlerTypes;
using System;

namespace DragonFiesta.World.Network.FiestaHandler.Server
{
    public static class SH02Handler
    {
        public static void SendGameTimeUpdatePacket(WorldSession Session,DateTime Time)
        {
            using (var packet = new FiestaPacket(Handler02Type._Header, Handler02Type.SMSG_SERVER_TIME_NOTIFY_CMD))
            {

                packet.Write<int>(3);

                packet.Write<int>(Time.Minute);
                packet.Write<int>(Time.Hour);
                packet.Write<int>(Time.Day);
                packet.Write<int>(Time.Month - 1);
                packet.Write<int>(Time.Year - 1900);
                packet.Write<int>((int)Time.DayOfWeek);

                packet.Write<int>(105);
                packet.Write<int>(2);
                packet.Write<int>(1);

                Session.SendPacket(packet);
            }
        }
    }
}