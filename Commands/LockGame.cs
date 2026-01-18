namespace HOI4Announcer.Commands;

/// <summary>
    [Command("lockgame")]
    [Description("Lock the game so that only moderators can make changes to the roster")]
    [RequirePermissions(DSharpPlus.Permissions.ManageGuild)]
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
            string lockfilepath = Path.Combine(path, "game.lock");

            if (File.Exists(lockfilepath))
            {
                await context.RespondAsync("Game is already locked.");
                return;
            }

            // Ensure the games directory exists
            Directory.CreateDirectory(path);
            
            File.WriteAllText(lockfilepath, "locked");
            await context.RespondAsync("✅ Game has been locked. Only moderators can make changes to the roster.");
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"❌ Error locking game: {ex.Message}");
        }
    }
}
/// </summary>
public class LockGame
{
    
}