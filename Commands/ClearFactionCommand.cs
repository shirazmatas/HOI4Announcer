using DSharpPlus.Commands;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class ClearFactionCommand
{
    [Command("clearfaction")]
    [Description("Clear all nations from a faction in the current game")]
    public async Task OnExecute(CommandContext context,
        [Parameter("faction")][Description("The faction to remove")] FactionID factionID)
    {
        if (FactionsHandler.GetFaction(factionID) == null)
        {
            await context.RespondAsync($"Error: Faction {factionID.ToFriendlyString()} does not exist.");
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
            await context.RespondAsync($"Faction {factionID.ToFriendlyString()} and its nations have been removed from the current game.");
        }
        else
        {
            await context.RespondAsync($"Failed to clear faction {factionID.ToFriendlyString()}. Does it exist in the current game?");
        }
    }
}
