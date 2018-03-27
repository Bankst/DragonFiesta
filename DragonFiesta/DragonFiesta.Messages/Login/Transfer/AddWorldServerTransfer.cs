using DragonFiesta.Game.Accounts;
using System;

namespace DragonFiesta.Messages.Login.Transfer
{
    [Serializable]
    public class AddWorldServerTransfer : ExpectAnswer
    {
        public Account Account { get; set; }

        public byte WorldId { get; set; }

        public string IP { get; set; }

        public bool Added { get; set; }

        public AddWorldServerTransfer() : base(2000)
        {
        }
    }
}