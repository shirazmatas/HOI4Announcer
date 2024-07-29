using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DSharpPlus;
using DSharpPlus.Entities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace hoi4announcer
{
	public class Config
	{
		public class Bot
		{
			public string token = "";
			public string logLevel  = "Error";

			public ulong logChannel = 0;
			public ulong gameChannel = 0;
			public List<ulong> staffRoles = new List<ulong>();
			public List<ulong> userRoles = new List<ulong>();
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
			Logger.Log("Loading config \"" + Path.GetFullPath(configPath) + "\"", LogID.CONFIG);

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
		public static bool IsStaff(DiscordMember member)
		{
			foreach (DiscordRole role in member.Roles)
			{
				Logger.Debug("checking if " + member.Username + " is a staff member", LogID.CONFIG);
				if (config.bot.staffRoles.Contains(role.Id) || config.bot.staffRoles.Contains(0))
				{
					return true;
				}
			}
			Logger.Debug(member.Username + " is not staff.", LogID.CONFIG);
			return false;
		}
		
		public static bool IsUser(DiscordMember member)
		{
			foreach (DiscordRole role in member.Roles)
			{
				Logger.Debug("checking if " + member.Username + " is a user", LogID.CONFIG);
				if (config.bot.userRoles.Contains(role.Id) || config.bot.userRoles.Contains(0))
				{
					return true;
				}
			}
			Logger.Debug(member.Username + " is not a user.", LogID.CONFIG);
			return false;
		}
	}
}
