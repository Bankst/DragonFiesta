using System;
using System.IO;

namespace DragonFiesta.Utils.IO.TXT
{
	public class Headers
	{
		public static void Type1(StreamWriter Writer)
		{
			Writer.WriteLine("#Ignore\t\\o042");
			Writer.WriteLine("#Exchange\t#\t\\x20");
		}

		public static void Type2(StreamWriter Writer)
		{
			Writer.WriteLine("#Ignore\t\\o042");
			Writer.WriteLine("#Exchange\t#\t\\x220");
			Writer.WriteLine("#Delimiter\t\\x20");
		}

		public static void Type3(StreamWriter Writer)
		{
			Writer.WriteLine("#Ignore\t\\o042");
			Writer.WriteLine("#Exchange\t#\t\\x20");
			Writer.WriteLine("#Delimiter\t\\x20");
		}

		public static void Type4(StreamWriter Writer) { Writer.WriteLine("#Delimiter\t\\x20"); }

		public static void Type5(StreamWriter Writer)
		{
			Writer.WriteLine("#Ignore\t\\o042");
			Writer.WriteLine("#Exchange\t#\t\\x220");
		}

		public static void Type6(StreamWriter Writer) { Writer.WriteLine("#Ignore\t\\o042"); }

		public static void WriteHeaders(StreamWriter Writer, String TableName)
		{
			switch (TableName)
			{
				case "ItemGroup":
				case "FieldList":
				case "RandomOptionTable":
				case "ItemDropGroup":
				case "ItemUseFunction":
				case "Common":
				case "Param":
				case "Container":
				case "RecallPoint":
				case "Header":
				case "DamageByAngle_Chr":
				case "DamageBySoul":
				case "AttSeq":
				case "MobRegenGroup":
				case "Script":
				case "Roaming":
				case "Trigger":
				case "Tab00":
					{
						Type1(Writer);

						break;
					}
				case "ByPartyMem":
					{
						Type2(Writer);

						break;
					}
				case "SendMyBrief":
					{
						Type3(Writer);

						break;
					}
				case "PineScript":
				case "SkillBreedMob":
				case "NPCCondition":
					{
						Type4(Writer);

						break;
					}
				case "Options":
					{
						Type5(Writer);

						break;
					}
				case "PIECE":
					{
						Type6(Writer);

						break;
					}
			}
		}
	}
}
