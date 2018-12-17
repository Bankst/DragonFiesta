using DFEngine.Content.Other;

namespace DFEngine.Content.GameObjects.Characters
{
	public class ParameterChange
	{
		public StatType Flag { get; set; }

		public int Value { get; set; }

		public ParameterChange(StatType type, int value)
		{
			Flag = type;
			Value = value;
		}
	}
}
