using System;

namespace DragonFiesta.Messages.Login.IPBlock
{
    public class AddIPBlock : IMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Reason { get; set; }
        public string IP { get; set; }
    }
}