using System;
using System.IO;
using DFEngine.Logging;
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
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new PrivateSetterContractResolver()
			};

			var path = Path.Combine(ConfigFolder, $"{configName ?? typeof(T).Name}.json");
			if (!File.Exists(path)) return default(T);

			using (var file = File.OpenText(path))
			{
				var text = file.ReadToEnd();
				var obj = JsonConvert.DeserializeObject<T>(text, settings);
				return obj;
			}
		}

		public static T Initialize(out string message)
		{
			var fullTypeName = typeof(T).FullName.Replace("DFEngine.Config.", "");
			var shortTypeName = fullTypeName.Replace("Configuration", "");

			try
			{
				var instance = ReadJson();
				if (instance != null)
				{
					EngineLog.Write(EngineLogLevel.Startup, $"Successfully read {shortTypeName} config.");
					message = "";
					return instance;
				}

				if (!Write(out var pConfig))
				{
					message = $"Failed to create default {fullTypeName}.";
					return default(T);
				}
				pConfig.WriteJson();

				EngineLog.Write(EngineLogLevel.Startup, $"Successfully generated {shortTypeName} config.");
				message = $"No {fullTypeName} found! Please edit generated config.";
				return default(T);
			}
			catch(Exception ex)
			{
				EngineLog.Write(EngineLogLevel.Exception, $"Failed to load {shortTypeName} config:\n {0}", ex);
				message = $"Failed to load {fullTypeName}:\n {ex.StackTrace}";
				return default(T);
			}
		}

		public static bool Write(out dynamic pConfig)
		{
			pConfig = default(T);
			try
			{
				pConfig = (T)Activator.CreateInstance(typeof(T));
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
