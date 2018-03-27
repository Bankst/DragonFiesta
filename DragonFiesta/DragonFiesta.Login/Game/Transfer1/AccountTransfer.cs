using DragonFiesta.Game;
using DragonFiesta.Game.Accounts;
using DragonFiesta.Game.Objects;
using DragonFiesta.Game.Transfer;
using DragonFiesta.Login.Game.Authentication;
using DragonFiesta.Login.Network;
using DragonFiesta.Networking.Packet.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Login.Game.Transfer
{
    public class AccountTransfer :  ServerTransferBase
    {
        public Guid TransferID { get; set; }

        public LoginSession pSession { get; set; }

        public byte WorldId { get; set; }

        public ClientRegion Region { get; set; }

        public Account mAccount { get; set; }

        public AccountTransfer() :
            base((int)ServerTaskTimes.ACCOUNT_TRANSFER_TIMEOUT)
        { 
        }

        protected override void DisposeInternal()
        {
    
        }

        protected override void OnExpire(GameTime Now)
        {
            AccountTransfer mTransfer;
            if (AccountTransferManager.Instance.FinishTransfer(TransferID,out mTransfer))
            {
                Dispose();
            }
        }


    }
}
