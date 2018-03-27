using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Zone
{
    [Serializable]
    public class OnlineCharacterListRequest : ExpectAnswer
    {
        public List<OnlineCharacter> OnlineCharacters { get; set; }

        public OnlineCharacterListRequest() 
            : base(30000)
        {
        }
    }

    [Serializable]
    public class OnlineCharacter
    {
        public int CharacterId { get; set; }

        public ushort MapId { get; set; }

        public ushort InstanceId { get; set; }
    }
}
