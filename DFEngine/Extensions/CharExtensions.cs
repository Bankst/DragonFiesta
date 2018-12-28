namespace DFEngine
{
	public static class CharExtensions
	{
		public static bool IsHexDigit(this char input)
		{
			if ('0' <= input && input <= '9' || 'A' <= input && input <= 'F')
				return true;
			if ('a' <= input)
				return input <= 'f';
			return false;
		}
	}
}
