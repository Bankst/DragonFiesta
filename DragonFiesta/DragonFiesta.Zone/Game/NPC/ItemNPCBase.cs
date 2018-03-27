using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Zone.Game.Character;
using System.Data.SqlClient;

namespace DragonFiesta.Zone.Game.NPC
{
    public abstract class ItemNPCBase : NPCBase
    {
        public ItemNPCBase(NPCInfo Info) : base(Info)
        {
        }

        public override void OpenMenu(ZoneCharacter Character)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}