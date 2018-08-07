using System;

namespace DragonFiesta.Messages.Zone.Note
{
    [Serializable]
    public class CharacterNote : IZoneToZoneMessage
    {
        public Guid Id { get; set; }

        public int CharacterId { get; set; }
        public string NotText { get; set; }

        public byte DestZone
        {
            get; set;
        }
    }
}