using DragonFiesta.Game.Accounts;
using System;

namespace DragonFiesta.Messages.Accounts
{
    [Serializable]
    public class AccountUpdate : ExpectAnswer
    {

        public Account Account { get; set; }
 
        public AccountUpdate() :
            base(5000)
        {
        }
    }
}