using System.ComponentModel;
using System.Threading.Tasks;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;

namespace HOI4Announcer.Commands;

// Unlock game should allow moderators to enable players to join nations again.
// Set GameHandler.Locked to false
public class UnlockGameCommand
{
    [Command("unlockgame")]
    [Description("Allow players to join nations again")]
    public async Task OnExecute(SlashCommandContext context)
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "Error: No active game to unlock."
            }, true);
            return;
        }

        if (!GameHandler.currentGame.locked)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "Error: Game is already unlocked."
            }, true);
            return;
        }

        bool success = await GameHandler.SetLocked(false);
        if (success)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green,
                Description = "✅ Game has been unlocked. Players can join nations again."
            });
        }
        else
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "Error: Failed to unlock the game."
            }, true);
        }
    }
}