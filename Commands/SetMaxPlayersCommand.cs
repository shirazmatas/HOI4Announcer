using DSharpPlus.Commands;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class SetMaxPlayersCommand
{
    [Command("setmaxplayers")]
    [Description("Set the maximum amount of players for a nation in the current game")]
    public async Task OnExecute(CommandContext context,
        [Parameter("nation")][Description("The nation to change")] NationID nationID,
        [Parameter("maxplayers")][Description("The new maximum amount of players")] int maxPlayers)
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync("Error: No active game found.");
            return;
        }

        bool found = false;
        foreach (var faction in GameHandler.currentGame.factions)
        {
            if (faction.nations.Any(n => n.id == nationID))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            await context.RespondAsync($"Error: Nation {nationID.ToFriendlyString()} not found in current game.");
            return;
        }

        bool success = await GameHandler.SetMaxPlayers(nationID, maxPlayers);
        if (success)
        {
            await context.RespondAsync($"Max players for {nationID.ToFriendlyString()} has been set to {maxPlayers}.");
        }
        else
        {
            await context.RespondAsync($"Failed to set max players for {nationID.ToFriendlyString()}. Does it exist in the current game?");
        }
    }
}