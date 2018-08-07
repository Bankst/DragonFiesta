using System;

namespace DragonFiesta.Messages.World.Character
{
    public class CharacterNameChanged : IMessage
    {
        public int CharacterId { get; set; }
        public string NewName { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}