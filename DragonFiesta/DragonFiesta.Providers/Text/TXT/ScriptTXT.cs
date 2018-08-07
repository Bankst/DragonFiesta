using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DragonFiesta.Utils.IO.TXT;

namespace DragonFiesta.Providers.Text.TXT
{
	public class ScriptTXT
	{
		public string ScrIndex { get; }
		public string ScrString { get; }

		public ScriptTXT(int row, TXTTable table)
		{
			ScrIndex = table.Read<string>(row, "ScrIndex");
			ScrString = table.Read<string>(row, "ScrString");
		}
	}
}
