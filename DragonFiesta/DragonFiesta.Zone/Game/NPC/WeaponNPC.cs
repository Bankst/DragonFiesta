using DragonFiesta.Zone.Data.NPC;
using DragonFiesta.Networking.HandlerTypes;
using System.Data.SqlClient;

namespace DragonFiesta.Zone.Game.NPC
{
    public sealed class WeaponNPC : ItemNPCBase
    {
        public WeaponNPC(NPCInfo info) : base(info)
        {
        }

        protected override void DisposeInternal()
        {
            base.DisposeInternal();
        }    
    }
}