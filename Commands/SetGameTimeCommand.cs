using System;
using DSharpPlus.Commands;
using System.ComponentModel;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.Commands.Processors.SlashCommands;

namespace HOI4Announcer.Commands;

public class SetGameTimeCommand
{
    [Command("setgametime")]
    [Description("Change the start time of the current game")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("starttime")] [Description("YYYY-MM-DD HH:MM")] DateTimeOffset startTime)
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

        GameHandler.currentGame.startTime = startTime;
        // Reset notified flag so if the game is moved back, players get notified again
        GameHandler.currentGame.notified = false;
        
        GameHandler.SaveCurrentGame();
        await GameHandler.UpdateDiscordMessage();

        await context.RespondAsync(new DiscordEmbedBuilder
        {
            Color = DiscordColor.Green,
            Description = $"Game start time updated to {Utilities.DiscordRelativeTime(startTime)}!"
        }, true);
    }
}
