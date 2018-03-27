using DragonFiesta.Game.Characters.Event;

namespace DragonFiesta.World.Game.Character.Event
{
    public class ZoneTransferAddedEventArgs : CharacterEventArgs<WorldCharacter>
    {
        public ushort MapId { get; private set; }

        public ushort InstanceId { get; private set; }

        public Position SpawnPostion { get; private set; }


        public ZoneTransferAddedEventArgs(WorldCharacter Character,
            ushort MapId,
            ushort InstanceId,
            Position SpawnPosition) :
            base(Character)
        {
            this.MapId = MapId;
            this.InstanceId = InstanceId;
            this.SpawnPostion = SpawnPostion;
        }
    }
}
