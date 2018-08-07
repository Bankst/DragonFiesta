namespace DragonFiesta.Game.Accounts
{
    public enum AccountDeleteResponse
    {
        NameTaken = -1,
        SQLError = -3,
        InternalError = -4,
        Success = 0,
    }
}