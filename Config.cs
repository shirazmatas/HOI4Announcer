using DSharpPlus.Entities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HOI4Announcer
{
	public class Config
	{
		public class Bot
		{
			public string token = "";

			public ulong logChannel = 0;
			public ulong gameChannel = 0;
			public List<ulong> blockedUsers = new List<ulong>();
		}
		public Bot bot = new Bot();
	}

	public static class ConfigParser
	{
		public static bool loaded  = false;

		public static Config config	= new Config();

		private static string configPath = "config.yml";

		public static void LoadConfig()
		{
			Logger.Log("Loading config \"" + Path.GetFullPath(configPath) + "\"");

			// Writes default config to file if it does not already exist
			if (!File.Exists(configPath))
			{
				File.WriteAllText(configPath, Utilities.ReadManifestData("default_config.yml"));
			}

			// Reads config contents into FileStream
			FileStream stream = File.OpenRead(configPath);

			// Converts the FileStream into a YAML object (Young Adult Male Losers)
			IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).Build();
			config = deserializer.Deserialize<Config>(new StreamReader(stream));

			loaded = true;
		}

		public static void SaveConfig()
		{
			ISerializer serializer = new SerializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).Build();
			string yaml = serializer.Serialize(config);
			File.WriteAllText(configPath, yaml);
		}
	}
}
