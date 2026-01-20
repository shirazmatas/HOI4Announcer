using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;

namespace HOI4Announcer.Commands;
public class LockGameCommand
{
    [Command("lockgame")]
    [Description("Lock the game so that only moderators can make changes to the roster")]
    public async Task OnExecute(SlashCommandContext context)
    {
        try
        {
            if (!GameHandler.HasActiveGame())
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = "Error: No active game to lock."
                }, true);
                return;
            }

            if (GameHandler.currentGame.locked)
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = "Error: Game is already locked."
                }, true);
                return;
            }

            bool success = await GameHandler.SetLocked(true);
            if (success)
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Green,
                    Description = "✅ Game has been locked. Only moderators can make changes to the roster."
                });
            }
            else
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = "Error: Failed to lock the game."
                }, true);
            }
        }
        catch (Exception ex)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error locking game: {ex.Message}"
            }, true);
        }
    }
}