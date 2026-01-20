using DSharpPlus.Commands;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class RemoveNationCommand
{
    [Command("removenation")]
    [Description("Remove a nation from the roster of playable nations for the current game")]
    public async Task OnExecute(CommandContext context,
        [Parameter("nation")][Description("The nation to remove")] NationID nationID)
    {
        bool nationExists = false;
        foreach (var f in FactionsHandler.config.factions)
        {
            if (f.nations.Any(n => n.id == nationID))
            {
                nationExists = true;
                break;
            }
        }

        if (!nationExists)
        {
            await context.RespondAsync($"Error: Nation {nationID.ToFriendlyString()} does not exist in any faction.");
            return;
        }

        if (GameHandler.HasActiveGame())
        {
            // Add also to currentGame.json
            GameHandler.RemoveNation(nationID);
        }

        await context.RespondAsync($"Nation {nationID.ToFriendlyString()} has been removed from the roster.");
    }
}