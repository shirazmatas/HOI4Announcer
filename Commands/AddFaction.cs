using DSharpPlus.Commands;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class AddFaction
{
    [Command("addfaction")]
    [Description("Add a faction to the list of factions")]
    public async Task OnExecute(CommandContext context,
        [Parameter("faction")][Description("The faction to add")] FactionID factionID)
    {
        if (FactionsHandler.GetFaction(factionID) != null)
        {
            await context.RespondAsync($"Error: Faction {factionID.ToFriendlyString()} already exists.");
            return;
        }

        FactionsHandler.AddFaction(factionID);

        await context.RespondAsync($"Faction {factionID.ToFriendlyString()} has been added.");
    }
}
