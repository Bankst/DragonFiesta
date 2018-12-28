using System;
using System.Collections.Generic;
using System.Text;

namespace DFEngine.IO.Definitions.SHN
{
	[Definition]
	public class ClassName
	{
		[Identity]
		public byte ClassID { get; set; }
		public string acPrefix { get; set; }
		public string acEngName { get; set; }
		public string acLocalName { get; set; }
	}
}
