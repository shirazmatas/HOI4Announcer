using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class LeaveNationCommand
{
    [Command("leavenation")]
    [Description("Leave your current nation in the current game")]
    public async Task OnExecute(SlashCommandContext context)
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "Error: There is no active game."
            }, true);
            return;
        }

        if (GameHandler.currentGame.locked)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "Error: The game is locked. You cannot leave your nation."
            }, true);
            return;
        }

        bool success = await GameHandler.RemovePlayer(context.User.Id);
        if (success)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green,
                Description = $"{context.User.Username} has left their nation."
            });
        }
        else
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: {context.User.Username}, you were not assigned to any nation."
            }, true);
        }
    }
}