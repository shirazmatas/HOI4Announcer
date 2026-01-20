using DSharpPlus.Commands;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class AddNationCommand
{
    [Command("addnation")]
    [Description("Add a nation to the roster of playable nations for the current game")]
    public async Task OnExecute(CommandContext context,
        [Parameter("nation")][Description("The nation to add")] NationID nationID,
        [Parameter("faction")][Description("The faction of the nation")] FactionID factionID,
        [Parameter("maxplayers")] [Description("The maximum number of players allowed for this nation")] int maxPlayers = 1)
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync($"Error: No active game found. Please start a game first.");
            return;
        }

        bool success = await GameHandler.AddNation(nationID, factionID, maxPlayers);

        if (success)
        {
            await context.RespondAsync($"Nation {nationID.ToFriendlyString()} has been added to the {factionID.ToFriendlyString()} faction.");
        }
        else
        {
            await context.RespondAsync($"Failed to add nation {nationID.ToFriendlyString()}. It might already exist in the game.");
        }
    }
}