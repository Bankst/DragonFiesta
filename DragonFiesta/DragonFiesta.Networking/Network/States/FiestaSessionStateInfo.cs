﻿using DragonFiesta.Networking.Cryptography;
using System;

namespace DragonFiesta.Networking.Network
{
    public class FiestaSessionStateInfo
    {
        public bool Authenticatet { get; set; }

        public bool TimeOutet { get; set; }

        public bool IsTransfering { get; set; }

        public ClientRegion Region { get; set; }

        public bool IsReady { get; set; }

        public bool HasPong { get; set; }
        public DateTime LastPing { get; set; }
        public TimeSpan Lag { get; set; }

        public FiestaCryptoProvider Crypto { get; set; }
    }
}