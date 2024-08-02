// See https://aka.ms/new-console-template for more information

using System.Reflection;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace hoi4announcer;
public enum Nations {
	Afghanistan,
	Albania,
	Argentina,
	Australia,
	Austria,
	Belgium,
	Bhutan,
	Bolivian_Republic,
	British_Malaya,
	British_Raj,
	Bulgaria,
	Chile,
	China,
	Colombia,
	Communist_China,
	Costa_Rica,
	Cuba,
	Czechoslovakia,
	Denmark,
	Dominican_Republic,
	Dominion_of_Canada,
	Dutch_East_Indies,
	Ecuador,
	El_Salvador,
	Estonia,
	Ethiopia,
	Finland,
	France,
	German_Reich,
	Guangxi_Clique,
	Guatemala,
	Haiti,
	Honduras,
	Iceland,
	Iran,
	Iraq,
	Ireland,
	Italy,
	Japan,
	Greece,
	Hungary,
	Latvia,
	Liberia,
	Lithuania,
	Luxembourg,
	Manchukuo,
	Mengkukuo,
	Mexico,
	Mongolia,
	Nepal,
	Netherlands,
	New_Zealand,
	Nicaragua,
	Norway,
	Oman,
	Panama,
	Peru,
	Philippines,
	Poland,
	Portugal,
	Paraguay,
	Romania,
	Saudi_Arabia,
	Brazil,
	Shanxi,
	Siam,
	Sinkiang,
	South_Africa,
	Soviet_Union,
	Spain,
	Aussa,
	Sweden,
	Switzerland,
	Tannu_Tuva,
	Tibet,
	Turkey,
	United_Kingdom,
	United_States,
	Uruguay,
	Venezuela,
	Xibei_San_Ma,
	Yemen,
	Yugoslavia,
	Yunnan
}

public enum Factions {
	Allies,
	Axis,
	Comintern,
	Independent,
	Weeb,
	China_United_Front,
}


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
    
    /// <summary>
    /// Retrieves the version of the entry assembly as a string in the format "major.minor.build[-revision]".
    /// </summary>
    /// <returns>The version of the entry assembly as a string.</returns>
    public static string GetVersion()
    {
        Version version = Assembly.GetEntryAssembly()?.GetName().Version;
        return version?.Major + "." + version?.Minor + "." + version?.Build + (version?.Revision == 0 ? "" : "-" + (char)(64 + version?.Revision ?? 0));
    }
    
    /// <summary>
    /// Reloads the bot by disconnecting the Discord client, loading the config, setting up the Discord client,
    /// hooking events, registering commands, and connecting to Discord.
    /// </summary>
    /// <remarks>
    /// This method first checks if the Discord client is not null. If it is not null, it disconnects the client,
    /// disposes of it, and logs that the Discord client has been disconnected. It then loads the config.
    /// After that, it sets up the Discord client by creating a new `DiscordConfiguration` object and setting
    /// its properties based on the values in the config. It also checks the log level in the config and sets
    /// it to `LogLevel.Information` if it is invalid.
    /// 
    /// After setting up the Discord client, it hooks events by adding an event handler for the `SessionCreated`
    /// event of the client. It also registers commands by calling the `UseSlashCommands` method of the client and
    /// assigning the result to the `commands` field.
    /// 
    /// Finally, it connects to Discord by calling the `ConnectAsync` method of the client.
    /// </remarks>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown if the bot token in the config is unset.</exception>
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