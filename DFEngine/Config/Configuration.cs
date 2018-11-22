using System;
using System.IO;
using Newtonsoft.Json;

namespace DFEngine.Config
{
	public class Configuration<T>
	{
		private const string ConfigFolder = "Config";

		public void CreateDefaultFolder()
		{
			if (!Directory.Exists(ConfigFolder))
			{
				Directory.CreateDirectory(ConfigFolder);
			}
		}

		public void WriteJson(string configName = null)
		{
			CreateDefaultFolder();
			var path = Path.Combine(ConfigFolder, $"{configName ?? typeof(T).Name}.json");
			
			var writer = new JsonSerializer();
			var file = new StreamWriter(path);
			writer.Formatting = Formatting.Indented;
			writer.Serialize(file, this);
			file.Close();
		}

		public static T ReadJson(string configName = null)
		{
			var path = Path.Combine(ConfigFolder, $"{configName ?? typeof(T).Name}.json");
			if (!File.Exists(path)) return default(T);

			var reader = new JsonSerializer();
			var file = new StreamReader(path);
			var jsReader = new JsonTextReader(file);
			var ret = reader.Deserialize<T>(jsReader);
			file.Close();
			return ret;
		}
	}
}
