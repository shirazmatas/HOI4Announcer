using DSharpPlus.Commands;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class ClearDefaultFactionCommand
{
    [Command("cleardefaultfaction")]
    [Description("Clear all nations from a faction")]
    public async Task OnExecute(CommandContext context,
        [Parameter("faction")][Description("The faction to remove")] FactionID factionID)
    {
        if (FactionsHandler.GetFaction(factionID) == null)
        {
            await context.RespondAsync($"Error: Faction {factionID.ToFriendlyString()} does not exist.");
            return;
        }

        FactionsHandler.ClearFaction(factionID);

        await context.RespondAsync($"Faction {factionID.ToFriendlyString()} and its nations have been removed.");
    }
}
