using DSharpPlus.Commands;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class AddNationCommand
{
    [Command("addnation")]
    [Description("Add a nation to the roster of playable nations")]
    public async Task OnExecute(CommandContext context,
        [Parameter("nation")][Description("The nation to add")] NationID nationID,
        [Parameter("faction")][Description("The faction of the nation")] FactionID factionID,
        [Parameter("maxplayers")] [Description("The maximum number of players allowed for this nation")] int maxPlayers = 1)
    {
        FactionsConfig.Faction faction = FactionsHandler.GetFaction(factionID);

        if (faction == null)
        {
            await context.RespondAsync($"Error: Faction {factionID} does not exist.");
            return;
        }

        if (faction.nations.Any(n => n.id == nationID))
        {
            await context.RespondAsync($"Error: Nation {nationID.ToFriendlyString()} already exists in faction {factionID.ToFriendlyString()}.");
            return;
        }

        FactionsHandler.AddNation(faction.id, nationID);

        if (GameHandler.HasActiveGame())
        {
            // Add also to currentGame.json
            GameHandler.AddNation(nationID, factionID);
        }

        await context.RespondAsync($"Nation {nationID.ToFriendlyString()} has been added to the {factionID.ToFriendlyString()} faction.");
    }
}