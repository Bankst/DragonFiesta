using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Providers.Items
{
	class ItemInfoServer
	{
		public byte ID { get; private set; }
		public string InxName { get; private set; }
		public string MarketIndex { get; private set; }
		public byte City { get; private set; }
		public string DropGroupA { get; private set; }
		public string DropGroupB { get; private set; }
		public string RandomOptionDropGroup { get; private set; }
		public int Vanish { get; private set; }
		public int looting { get; private set; }
		public byte DropRateKilledByMob { get; private set; }
		public byte DropRateKilledByPlayer { get; private set; }
		public byte ISET_Index { get; private set; }
		public String ItemSort_Index { get; private set; }
		public byte KQItem { get; private set; }
		public byte PK_KQ_USE { get; private set; }
		public byte KQ_Item_Drop { get; private set; }
		public byte PreventAttack { get; private set; }
	}
}
