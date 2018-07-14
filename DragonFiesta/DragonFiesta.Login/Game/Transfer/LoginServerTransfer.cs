using DragonFiesta.Game.Accounts;
using DragonFiesta.Game.Transfer;
using DragonFiesta.Utils.ServerTask;
using System;

namespace DragonFiesta.Login.Game.Transfer
{
    public class LoginServerTransfer : ServerTransferBase, IExpireGuidAble
    {
        public Guid Id { get; private set; }

        public Account pAccount { get; private set; }

        public string IP { get; private set; }

        public LoginServerTransfer(string IP, Guid Id, Account pAccount) : base()
        {
            this.IP = IP;
            this.Id = Id;
            this.pAccount = pAccount;
        }

        public override void OnExpire(GameTime gameTime)
        {
            if (LoginTransferManager.FinishTransfer(Id, out LoginServerTransfer mTransfer))
            {
                Dispose();
            }
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();

            IP = null;
            pAccount = null;
        }
    }
}