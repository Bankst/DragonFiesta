﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DFEngine.Server
{
	public enum CharDeleteError : ushort
	{
		FAILED_CREATE = 130, // 0x0082
		WRONG_CLASS = 131, // 0x0083
		NAME_TAKEN = 132, // 0x0084
		ERROR_MAX_SLOT = 133, // 0x0085
		LV60REQ = 134, // 0x0086
		NAME_IN_USE = 385, // 0x0181
	}
}