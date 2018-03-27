namespace DragonFiesta.World.Game.Friends
{
    public enum FriendDeleteResponse : ushort
    {
        Success = 0x0951,
        DatabaseError = 0x0952,
        CannotFindTarget = 0x0953,
    }
}
