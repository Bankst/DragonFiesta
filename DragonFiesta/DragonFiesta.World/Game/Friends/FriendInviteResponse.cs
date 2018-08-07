
namespace DragonFiesta.World.Game.Friends
{
    public enum FriendInviteResponse : ushort
    {
        Success = 0x0941,
        CannotAddSelf = 0x0942,
        HasMaxFriends = 0x0943,
        TargetIsAlreadyFriend = 0x0945,
        CannotFindTarget = 0x0946,
        TargetHasMaxFriends = 0x0947,
        DatabaseError = 0x0949,
    }
}
