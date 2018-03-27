using System;

namespace DragonFiesta.Messages.Command
{
    [Serializable]
    public class GameCommandToServer : IMessage
    {
        public int CharacterId { get; set; }

        public string[] Args { get; set; }

        public string Category { get; set; }

        public string Command { get; set; }
        public Guid Id { get; set; } = new Guid();
    }
}