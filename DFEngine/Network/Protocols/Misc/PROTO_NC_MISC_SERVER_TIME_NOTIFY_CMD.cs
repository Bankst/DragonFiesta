using System;

namespace DFEngine.Network.Protocols
{
    public class PROTO_NC_MISC_SERVER_TIME_NOTIFY_CMD : NetworkMessage
    {
        public PROTO_NC_MISC_SERVER_TIME_NOTIFY_CMD() : base(NetworkCommand.NC_MISC_SERVER_TIME_NOTIFY_CMD)
        {
            Write((int)3);
            Write((int)DateTime.Today.Minute);
            Write((int)DateTime.Today.Hour);
            Write((int)DateTime.Today.Day);
            Write((int)DateTime.Today.Month - 1);
            Write((int)DateTime.Today.Year - 1900);
            Write((int)DateTime.Today.DayOfWeek);

            Write((int)105);
            Write((int)2);
            Write((int)1);
        }
    }
}
