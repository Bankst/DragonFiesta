using System;

namespace DragonFiesta.Messages.Zone.Note
{
    [Serializable]
    public class ServerNote : IMessage
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
    }
}