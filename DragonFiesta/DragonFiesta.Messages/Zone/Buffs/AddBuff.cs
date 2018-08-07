using System;
using System.Collections.Generic;

namespace DragonFiesta.Messages.Zone.Buffs
{
    [Serializable]
    public class AddBuff : ExpectAnswer
    {
        public AddBuff(int TimeToAnswerExpire): base((int)MessageRequestTimeOuts.ZONE_SET_BUFF)
        {
        }

        public ushort AbStateID { get; set; }

        public int Strength { get; set; }

        public int KeepTimeMS { get; set; }

        public List<int> ReciverList { get; set; }
    }
}