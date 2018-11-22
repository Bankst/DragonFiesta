using System;
using System.Runtime.Serialization;

namespace DFEngine
{
	public class StartupException : Exception
	{
		public StartupException(string message)
			: base(message)
		{ }

		public StartupException(string format, params object[] args)
			: base(string.Format(format, args))
		{ }

		protected StartupException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
