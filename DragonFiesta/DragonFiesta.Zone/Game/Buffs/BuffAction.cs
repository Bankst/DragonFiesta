using DragonFiesta.Zone.Data.Buffs;
using System.Threading;

namespace DragonFiesta.Zone.Game.Buffs
{
    public abstract class BuffAction
    {
        public Buff Buff { get; private set; }
        public SubAbStateAction SubAbStateAction { get; private set; }

        public bool IsDisposed { get { return (IsDisposedInt > 0); } }
        private int IsDisposedInt;

        protected BuffAction(Buff Buff, SubAbStateAction SubAbStateAction)
        {
            this.Buff = Buff;
            this.SubAbStateAction = SubAbStateAction;
        }

        ~BuffAction()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref IsDisposedInt, 1, 0) == 0)
            {
                DisposeInternal();
                Buff = null;
                SubAbStateAction = null;
            }
        }

        protected virtual void DisposeInternal() { }
        public abstract void Activate();
        public abstract void Deactivate();

    }
}