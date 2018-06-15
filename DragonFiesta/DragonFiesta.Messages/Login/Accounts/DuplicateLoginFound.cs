using System;

namespace DragonFiesta.Messages.Accounts
{
    [Serializable]
    public class DuplicateLoginFound : IMessage
    {
        public int AccountID { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}