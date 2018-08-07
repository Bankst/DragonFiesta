using System;
using System.Collections.Generic;

namespace DragonFiesta.Messages.Zone.Buffs
{
    [Serializable]
    public class RemoveBuff : IMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public ushort AbStateId { get; set; }
        public List<ushort> ReciverList { get; set; }
    }
}