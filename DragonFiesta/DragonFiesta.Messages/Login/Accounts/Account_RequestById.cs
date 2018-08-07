using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Login.Accounts
{
    [Serializable]
    public class Account_RequestById : ExpectAnswer
    {
        public int AccountId { get; set; }
        public Account_RequestById()
            : base(6000)
        {
        }
    }
}
