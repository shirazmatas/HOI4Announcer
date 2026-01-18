namespace HOI4Announcer.Commands;

/// <summary>
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using System.ComponentModel;

namespace HOI4Announcer;

/// <summary>
///  Should lock the game so that no changes to roster can be done by others than those with the moderator role
/// </summary>
public class LockGame
{
    [Command("lockgame")]
    [Description("Lock the game so that only moderators can make changes to the roster")]
    [RequirePermissions(DSharpPlus.Permissions.ManageGuild)] // Change this
    public async Task OnExecute(CommandContext context)
    {
        try
        {
            if (!Games.HasActiveGame())
            {
                await context.RespondAsync("No active game to lock.");
                return;
            }

            string path = Path.Combine(Directory.GetCurrentDirectory(), "games");
            string filepath = Path.Combine(path, "currentGame.json");
            string lockfilepath = Path.Combine(path, "game.lock");

            if (File.Exists(lockfilepath))
            {
                await context.RespondAsync("Game is already locked.");
                return;
            }

            File.WriteAllText(lockfilepath, "locked");
            await context.RespondAsync("Game has been locked. Only moderators can make changes to the roster.");
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"Error locking game: {ex.Message}");
        }
    }
}
