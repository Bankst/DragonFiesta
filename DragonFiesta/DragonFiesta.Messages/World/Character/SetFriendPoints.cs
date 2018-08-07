using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.World.Character
{
    [Serializable]
    public class SetFriendPoints : ExpectAnswer
    {
        public SetFriendPoints() : base(20000)
        {
        }

        public int CharacterId { get; set; }

        public ushort FriendPoint { get; set; }
    }
}
