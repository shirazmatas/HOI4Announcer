using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class RemoveNationCommand
{
    [Command("removenation")]
    [Description("Remove a nation from the roster of playable nations for the current game")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("nation")][Description("The nation to remove")] NationID nationID)
    {
        bool nationExists = FactionsHandler.config.factions.Any(f => f.nations.Any(n => n.id == nationID));

        if (!nationExists)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: Nation {nationID.ToFriendlyString()} does not exist in any faction."
            }, true);
            return;
        }

        bool success = false;
        if (GameHandler.HasActiveGame())
        {
            success = await GameHandler.RemoveNation(nationID);
        }

        if (success)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green,
                Description = $"Nation {nationID.ToFriendlyString()} has been removed from the roster."
            });
        }
        else
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: Failed to remove nation {nationID.ToFriendlyString()}. Does it exist in the current game?"
            }, true);
        }
    }
}