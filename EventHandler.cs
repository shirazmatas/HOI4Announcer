using DSharpPlus;
using DSharpPlus.EventArgs;

namespace HOI4Announcer;

public class EventHandler
{
    public static Task OnReady(DiscordClient client, SessionReadyEventArgs e)
    {
        Logger.Log("Connected to Discord.", LogID.DISCORD);
        return Task.CompletedTask;
    }
}