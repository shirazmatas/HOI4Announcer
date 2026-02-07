using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HOI4Announcer.Commands;

public class ClearFactionCommand
{
    [Command("clearfaction")]
    [Description("Clear all nations from a faction in the current game")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("faction")][Description("The faction to remove")] FactionID factionID)
    {
        if (FactionsHandler.GetFaction(factionID) == null)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: Faction {factionID.ToFriendlyString()} does not exist."
            }, true);
            return;
        }

        bool success = false;
        // Clear from current game
        if (GameHandler.HasActiveGame())
        {
            success = await GameHandler.ClearFaction(factionID);
        }

        if (success)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green,
                Description = $"Faction {factionID.ToFriendlyString()} and its nations have been removed from the current game."
            });
        }
        else
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: Failed to clear faction {factionID.ToFriendlyString()}. Does it exist in the current game?"
            }, true);
        }
    }
}
