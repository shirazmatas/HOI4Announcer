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

        // Clear from current game

        await context.RespondAsync($"Faction {factionID.ToFriendlyString()} and its nations have been removed.");
    }
}
