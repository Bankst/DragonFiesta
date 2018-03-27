using System;

namespace DragonFiesta.Messages.Zone.Note
{
    [Serializable]
    public class MapNote : IMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public ushort MapId { get; set; }

        public ushort InstanceId { get; set; }

        public string NoteText { get; set; }
    }
}