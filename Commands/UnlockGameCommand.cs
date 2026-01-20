using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;

namespace HOI4Announcer.Commands;

// Unlock game should allow moderators to enable players to join nations again.
// Set GameHandler.Locked to false
public class UnlockGameCommand
{
    [Command("unlockgame")]
    [Description("Allow players to join nations again")]
    public async Task OnExecute(CommandContext context)
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync("No active game to unlock.");
            return;
        }

        if (!GameHandler.currentGame.locked)
        {
            await context.RespondAsync("Game is already unlocked.");
            return;
        }

        bool success = await GameHandler.SetLocked(false);
        if (success)
        {
            await context.RespondAsync("✅ Game has been unlocked. Players can join nations again.");
        }
        else
        {
            await context.RespondAsync("❌ Failed to unlock the game.");
        }
    }
}