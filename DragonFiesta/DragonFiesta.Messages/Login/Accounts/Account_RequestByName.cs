using DragonFiesta.Game.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Login.Accounts
{
    [Serializable]
    public class Account_RequestByName : ExpectAnswer
    {

        public string AccountName { get; set; }

        public Account_RequestByName() :
            base(5000)
        {
        }
    }
}
