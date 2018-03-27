using DragonFiesta.World.Game.Character;

public class WorldCharacterNameChangedEventArgs : CharacterNameChangedEventArgs<WorldCharacter>
{
    public WorldCharacterNameChangedEventArgs(WorldCharacter Character, string OldName) :
        base(Character, OldName)
    {
    }
}