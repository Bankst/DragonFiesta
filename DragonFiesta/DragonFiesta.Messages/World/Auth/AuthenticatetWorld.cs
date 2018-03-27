using System;
using System.Collections.Generic;

namespace DragonFiesta.Messages.Message.Auth
{
    [Serializable]
    public class AuthenticatetWorld : ExpectAnswer
    {
        public string Password { get; set; }

        public byte WorldId { get; set; }

        public ushort Port { get; set; }

        public string IP { get; set; }

        public int MaxConnection { get; set; }

        public List<int> ActiveAccounts { get; set; }

        public AuthenticatetWorld()
            : base((int)ServerTaskTimes.SERVER_AUTH_WORLD)
        {
        }
    }
}