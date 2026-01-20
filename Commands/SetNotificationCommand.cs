using DSharpPlus.Commands;
using System.ComponentModel;
using DSharpPlus.Entities;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;

namespace HOI4Announcer.Commands;

public class SetNotificationCommand
{
    [Command("setnotification")]
    [Description("Set how many minutes before the game starts to notify participants via DM")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("minutes")] [Description("Minutes before game start (use -1 to disable)")] int minutes)
    {

        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "There is no active game."
            }, true);
            return;
        }

        GameHandler.currentGame.notificationMinutes = minutes;
        if (minutes >= 0)
        {
            // If we set a new notification time, we might want to reset the notified flag
            // so if they change it from 15 to 30 and it's 20 mins before, it notifies.
            GameHandler.currentGame.notified = false;
        }
        GameHandler.SaveCurrentGame();
        await GameHandler.UpdateDiscordMessage();

        string desc = minutes >= 0
            ? $"Notification set for {minutes} minutes before game start."
            : "Notifications disabled.";

        await context.RespondAsync(new DiscordEmbedBuilder
        {
            Color = DiscordColor.Green,
            Description = desc
        });
    }
}
