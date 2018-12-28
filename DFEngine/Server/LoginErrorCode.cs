using System;
using System.Collections.Generic;
using System.Text;

namespace DFEngine.Server
{
	public enum LoginErrorCode : ushort
	{
		Exception = 66, // 0x0042
		DbError = 67, // 0x0043
		WrongCredentials = 69, // 0x0045
		Banned = 71, // 0x0047
		Maintenance = 72, // 0x0048
		Timeout = 73, // 0x0049
	}
}
