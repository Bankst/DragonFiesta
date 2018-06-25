using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DragonFiesta.Utils.IO.SHN
{
	public class SHNManager
	{
		public const string DataPath = "Shine";
		public static SHNResult Load(SHNType type)
		{
			MethodInfo CryptoMethod = typeof(SHNCrypto).GetMethods().Where(x => x.Name == "CryptoDefault").First();
			var fileName = $"{type.ToString()}.shn";
			var filePath = $"Shine/{fileName}";
			SHNFile shnFile = new SHNFile(filePath, CryptoMethod);

			if (shnFile.Type == SHNType.TextData) { shnFile.SHNEncoding = Encoding.ASCII; }
			else { shnFile.SHNEncoding = Encoding.GetEncoding("ISO-8859-1"); }

			shnFile.Read();
			shnFile.DisallowRowChanges();

			using (var ShnData = shnFile.Table.CreateDataReader())
			{
				using (var retData = new SHNResult())
				{
					retData.Load(ShnData);					
					retData.Count = shnFile.Table.Rows.Count;

					return retData;
				}
			}
		}
	}
}
