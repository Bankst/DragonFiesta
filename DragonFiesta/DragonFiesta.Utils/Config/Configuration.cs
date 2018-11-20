using System.IO;
using System.Xml.Serialization;

namespace DragonFiesta.Utils.Config
{
	public class Configuration<T>
	{
		public void CreateDefaultFolder()
		{
			const string folder = "Configuration";
			if (!Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}
		}

		public void WriteXml(string configName = null)
		{
			CreateDefaultFolder();

			var path = Path.Combine("Configuration", $"{configName ?? typeof(T).Name}.xml");

			var writer = new XmlSerializer(GetType());
			var file = new StreamWriter(path);
			writer.Serialize(file, this);
			file.Close();
		}

		public static T ReadXml(string configName = null)
		{
			var path = Path.Combine("Configuration", $"{configName ?? typeof(T).Name}.xml");

			if (File.Exists(path))
			{
				var reader = new XmlSerializer(typeof(T));
				var file = new StreamReader(path);

				var xml = (T)reader.Deserialize(file);

				file.Close();
				return xml;
			}
			else return default(T);
		}
	}
}