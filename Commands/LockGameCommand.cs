using System.ComponentModel;
using DSharpPlus.Commands;

namespace HOI4Announcer.Commands;
public class LockGameCommand
{
    [Command("lockgame")]
    [Description("Lock the game so that only moderators can make changes to the roster")]
    public async Task OnExecute(CommandContext context)
    {
        try
        {
            if (!GameHandler.HasActiveGame())
            {
                await context.RespondAsync("No active game to lock.");
                return;
            }

            if (GameHandler.currentGame.locked)
            {
                await context.RespondAsync("Game is already locked.");
                return;
            }

            bool success = await GameHandler.SetLocked(true);
            if (success)
            {
                await context.RespondAsync("✅ Game has been locked. Only moderators can make changes to the roster.");
            }
            else
            {
                await context.RespondAsync("❌ Failed to lock the game.");
            }
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"❌ Error locking game: {ex.Message}");
        }
    }
}