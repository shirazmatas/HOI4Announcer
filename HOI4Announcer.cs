// See https://aka.ms/new-console-template for more information

using System.Reflection;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace hoi4announcer;

internal class Hoi4Announcer 
{
    public static DiscordClient client = new DiscordClient(new DiscordConfiguration {Token = "DUMMY_TOKEN", TokenType = TokenType.Bot, MinimumLogLevel = LogLevel.Debug, Intents = DiscordIntents.AllUnprivileged});
    private static SlashCommandsExtension commands = null; // The slash commands extension.
    private static void Main(string[] args)
    {
        // Create the slash commands extension.
        commands = client.UseSlashCommands();
	}	
    private static async Task MainAsync()
    {
        Logger.Log("Starting " + Assembly.GetEntryAssembly()?.GetName().Name + " version " + GetVersion() + "...");
        try
        {
	        Reload();

	        // Block this task until the program is closed.
	        await Task.Delay(-1);
        }
        catch (Exception e)
        {
	        Logger.Fatal("Fatal error:\n" + e);
	        Console.ReadLine();
        }
    }
    
    
    public static string GetVersion()
    {
        Version version = Assembly.GetEntryAssembly()?.GetName().Version;
        return version?.Major + "." + version?.Minor + "." + version?.Build + (version?.Revision == 0 ? "" : "-" + (char)(64 + version?.Revision ?? 0));
    }
    public static async void Reload()
	{
		if (client != null)
		{
			await client.DisconnectAsync();
			client.Dispose();
			Logger.Log("Discord client disconnected.");
		}

		Logger.Log("Loading config \"" + Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "config.yml\"");
		ConfigParser.LoadConfig();

		// Check if token is unset
		if (ConfigParser.config.bot.token is "<add-token-here>" or "")
		{
			Logger.Fatal("You need to set your bot token in the config and start the bot again.");
			throw new ArgumentException("Invalid Discord bot token");
		}

		Logger.Log("Setting up Discord client...");
		
		// Checking log level
		if (!Enum.TryParse(ConfigParser.config.bot.logLevel, true, out LogLevel logLevel))
		{
			Logger.Warn("Log level '" + ConfigParser.config.bot.logLevel + "' invalid, using 'Information' instead.", LogID.CONFIG);
			logLevel = LogLevel.Information;
		}
		
		// Setting up client configuration
		DiscordConfiguration cfg = new DiscordConfiguration
		{
			Token = ConfigParser.config.bot.token,
			TokenType = TokenType.Bot,
			MinimumLogLevel = logLevel,
			AutoReconnect = true,
			Intents = DiscordIntents.All
		};

		client = new DiscordClient(cfg);

		Logger.Log("Hooking events...");

		client.SessionCreated += EventHandler.OnReady;


		Logger.Log("Registering commands...");
		commands = client.UseSlashCommands();

		//commands.RegisterCommands<AddCategoryCommand>();
		//commands.RegisterCommands<AddCommand>();

		Logger.Log("Hooking command events...");
		//commands.SlashCommandErrored += EventHandler.OnCommandError;

		Logger.Log("Connecting to Discord...");
		await client.ConnectAsync();
		
	}
    

}