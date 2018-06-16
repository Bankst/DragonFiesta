using System;

namespace DragonFiesta.Messages.Message.Auth
{
    [Serializable]
    public class AuthenticatedWorld_Response : IMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public InternWorldAuthResult Result { get; set; }

        public ClientRegion Region { get; set; } = ClientRegion.None;
    }
}