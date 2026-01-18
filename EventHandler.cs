using DSharpPlus;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.EventArgs;
using DSharpPlus.Commands.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;

namespace HOI4Announcer;

public static class EventHandler
{
    internal static bool hasLoggedGuilds = false;

    public static Task OnReady(DiscordClient client, GuildDownloadCompletedEventArgs e)
    {
        Logger.Log("Connected to Discord.");
        hasLoggedGuilds = true;
        return Task.CompletedTask;
    }

    public static async Task OnGuildAvailable(DiscordClient discordClient, GuildAvailableEventArgs e)
    {
        if (hasLoggedGuilds)
        {
            return;
        }

        Logger.Log("Found Discord server: " + e.Guild.Name + " (" + e.Guild.Id + ")");

        IReadOnlyDictionary<ulong, DiscordRole> roles = e.Guild.Roles;

        foreach ((ulong roleID, DiscordRole role) in roles)
        {
            Logger.Debug(role.Name.PadRight(40, '.') + roleID);
        }
    }
    public static async Task OnCommandError(CommandsExtension commandSystem, CommandErroredEventArgs e)
    {
        try
        {
            switch (e.Exception)
            {
                case ChecksFailedException checksFailedException:
                {
                    foreach (ContextCheckFailedData error in checksFailedException.Errors)
                    {
                        await e.Context.Channel.SendMessageAsync(new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Red,
                            Description = error.ErrorMessage
                        });
                    }
                    return;
                }

                case BadRequestException ex:
                    Logger.Error("Command exception occured.", e.Exception);
                    Logger.Error("JSON Message: " + ex.JsonMessage);
                    return;

                default:
                {
                    Logger.Error("Command exception occured.", e.Exception);
                    await e.Context.Channel.SendMessageAsync(new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Red,
                        Description = "Internal error occured, please report this to the developer."
                    });
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("An error occurred in command error handler.", ex);
            Logger.Error("Original exception:", e.Exception);
        }
    }
    internal class ErrorHandler : IClientErrorHandler
    {
        public ValueTask HandleEventHandlerError(string name,
            Exception exception,
            Delegate invokedDelegate,
            object sender,
            object args)
        {
            Logger.Error("Client exception occured:\n" + exception);
            if (exception is BadRequestException ex)
            {
                Logger.Error("JSON Message: " + ex.JsonMessage);
            }

            return ValueTask.FromException(exception);
        }

        public ValueTask HandleGatewayError(Exception exception)
        {
            Logger.Error("A gateway error occured:\n" + exception);
            return ValueTask.FromException(exception);
        }
    }
}