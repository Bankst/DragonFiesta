namespace DragonFiesta.Game.Accounts
{
    public enum AccountCreateResponse : int
    {
        NameTaken = -1,
        IDOverflow = -2,
        SQLError = -3,
        InternalError = -4,
        Success = 0,
    }
}