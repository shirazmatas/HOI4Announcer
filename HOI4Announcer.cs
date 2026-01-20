using System.Reflection;
using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using HOI4Announcer.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HOI4Announcer;


internal class HOI4Announcer
{
    internal const string ApplicationName = "HOI4Announcer";
    public static DiscordClient Client { get; private set; }
    private static async Task<int> Main(string[] args)
    {
        Logger.Log("Starting " + Assembly.GetEntryAssembly()?.GetName().Name + " version " + GetVersion() + "...");
        try
        {
            Reload();
            await Connect();
            // Block this task until the program is closed.
            await Task.Delay(-1);
            return 0;
        }
        catch (Exception e)
        {
            Logger.Fatal("Fatal error:\n" + e);
            Console.ReadLine();
            return 1;
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
        if (Client != null)
        {
            await Client.DisconnectAsync();
            Client.Dispose();
            Logger.Log("Discord client disconnected.");
        }

        Logger.Log("Loading config \"" + Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "config.yml\"");
        ConfigParser.LoadConfig();

        // Check if the token is unset
        if (ConfigParser.config.bot.token is "<add-token-here>" or "")
        {
            Logger.Fatal("You need to set your bot token in the config and start the bot again.");
            throw new ArgumentException("Invalid Discord bot token");
        }

        Logger.Log("Setting up Discord client...");

        FactionsHandler.Load();
        GameHandler.LoadCurrentGame();
    }
    private static async Task<bool> Connect()
    {
        Logger.Log("Setting up Discord client.");
        DiscordClientBuilder clientBuilder = DiscordClientBuilder.CreateDefault(ConfigParser.config.bot.token,
                DiscordIntents.AllUnprivileged | DiscordIntents.GuildMembers)
                                                                 .SetReconnectOnFatalGatewayErrors();

        clientBuilder.ConfigureServices(configure =>
        {
            configure.AddSingleton<IClientErrorHandler>(new EventHandler.ErrorHandler());
        });

        clientBuilder.ConfigureEventHandlers(builder =>
        {
            builder.HandleGuildDownloadCompleted(EventHandler.OnReady);
            builder.HandleGuildAvailable(EventHandler.OnGuildAvailable);
        });

        clientBuilder.UseCommands((_, extension) =>
        {
            extension.AddCommands(
            [
                typeof(AddFactionCommand),
                typeof(AddNationCommand),
                //typeof(AddUserCommand),
                //typeof(BlockUserCommand),
                //typeof(EndGameCommand),
                //typeof(JoinNationCommand),
                //typeof(LeaveNationCommand),
                //typeof(LockGameCommand),
                typeof(NewGameCommand),
                typeof(RemoveFactionCommand),
                typeof(RemoveNationCommand),
                //typeof(RemoveUserCommand),
                //typeof(UnblockUserCommand),
                //typeof(UnlockGameCommand)
            ]);
            extension.AddProcessor(new SlashCommandProcessor());
            extension.CommandErrored += EventHandler.OnCommandError;
        }, new CommandsConfiguration
        {
            RegisterDefaultCommandProcessors = false,
            UseDefaultCommandErrorHandler = false
        });

        clientBuilder.ConfigureExtraFeatures(clientConfig =>
        {
            clientConfig.LogUnknownEvents = false;
            clientConfig.LogUnknownAuditlogs = false;
        });

        clientBuilder.ConfigureLogging(config =>
        {
            config.AddProvider(new LoggerProvider());
        });

        Client = clientBuilder.Build();

        Logger.Log("Connecting to Discord.");
        EventHandler.hasLoggedGuilds = false;

        try
        {
            await Client.ConnectAsync();
        }
        catch (Exception e)
        {
            Logger.Fatal("Error occured while connecting to Discord.", e);
            return false;
        }

        return true;
    }


}