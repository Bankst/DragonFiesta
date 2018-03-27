using System;

namespace DragonFiesta.Messages.World.Transfer
{
    [Serializable]
    public class AddLoginServerTransfer : ExpectAnswer
    {
        public string IP { get; set; }

        public int AccountId { get; set; }

        public bool Added { get; set; }

        public AddLoginServerTransfer() : base(2000)
        {
        }
    }
}