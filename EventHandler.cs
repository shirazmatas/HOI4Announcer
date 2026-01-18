using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

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
}