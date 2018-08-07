using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DragonFiesta.Utils.IO.SHN
{
    public class SHNManager
	{
		public const string DataPath = "Shine";
		private static readonly MethodInfo CryptoMethod = typeof(SHNCrypto).GetMethods().First(x => x.Name == "CryptoDefault");
		public static SHNResult Load(SHNType type)
		{			
			var fileName = $"{type.ToString()}.shn";
			var filePath = $"{DataPath}/{fileName}";
			var shnFile = new SHNFile(filePath, CryptoMethod);

			shnFile.SHNEncoding = shnFile.Type == SHNType.TextData
				? Encoding.ASCII
				: Encoding.GetEncoding("ISO-8859-1");

			shnFile.Read();
			shnFile.DisallowRowChanges();

			using (var shnData = shnFile.Table.CreateDataReader())
			{
				using (var retData = new SHNResult())
				{
					retData.Load(shnData);
					retData.Count = shnFile.Table.Rows.Count;

					return retData;
				}
			}
		}
	}
}
