using DragonFiesta.Zone.Data.NPC;
using System.Data.SqlClient;

namespace DragonFiesta.Zone.Game.NPC
{
    public class ClientMenu : ItemNPCBase
    {
        public ClientMenu(NPCInfo info) : base(info)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }
    }
}