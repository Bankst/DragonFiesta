using System.Linq;
using System.Reflection;
using System.Text;

namespace DragonFiesta.Utils.IO.SHN
{
    public class SHNManager
	{
		public const string DataPath = "Shine";
		private static readonly MethodInfo CryptoMethod = typeof(SHNCrypto).GetMethods().Where(x => x.Name == "CryptoDefault").First();
		public static SHNResult Load(SHNType type)
		{			
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
