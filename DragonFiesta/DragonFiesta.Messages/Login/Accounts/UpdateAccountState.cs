using DragonFiesta.Game.Accounts;
using System;

namespace DragonFiesta.Messages.Accounts
{
    [Serializable]
    public class UpdateAccountState : IMessage
    {
        public Account pAccount { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}