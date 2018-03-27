using DragonFiesta.Game.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Login.Accounts
{
    [Serializable]
    public class Account_Response : IMessage
    {
        public Guid Id { get; set; }

        public Account Account { get; set; }
    }
}
