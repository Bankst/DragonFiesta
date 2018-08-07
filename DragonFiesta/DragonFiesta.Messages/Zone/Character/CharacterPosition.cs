using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Messages.Zone.Character
{
    [Serializable]
    public class CharacterPosition : ExpectAnswer
    {
        public int CharacterId { get; set; }
        public Position Position { get; set; }
        public ushort MapId { get; set; }
        public ushort InstanceId { get; set; }
        public byte DestZone { get; set; }
        public byte RequestZone { get; set; }

        [NonSerialized]
        private Action<IMessage> _RequestFailCallBack;

        public Action<IMessage> RequestFailCallBack
        {
            get => _RequestFailCallBack;
            set => _RequestFailCallBack = value;
        }


        public CharacterPosition()
            : base(5000)
        {
        }
    }
}
