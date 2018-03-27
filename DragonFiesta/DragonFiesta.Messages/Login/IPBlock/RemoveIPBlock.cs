using System;

namespace DragonFiesta.Messages.Login.IPBlock
{
    [Serializable]
    public class RemoveIPBlock : IMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int RequestCharAccountId { get; set; }

        public string IP { get; set; }
    }
}