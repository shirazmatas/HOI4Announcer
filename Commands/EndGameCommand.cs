using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;

namespace HOI4Announcer.Commands;

public class EndGameCommand
{
    [Command("endgame")]
    [Description("End a game and declare a winning side")]
    public async Task OnExecute(SlashCommandContext context, 
        [Parameter("winner")] [Description("Pick a faction, nation, or a tie")] string winner) 
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "Error: No active game to end."
            }, true);
            return;
        }

        bool success = await GameHandler.EndGame();
        if (success)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green,
                Description = $"🏆 The game has ended! Winner: {winner}"
            });
        }
        else
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "Error: Failed to end the game properly, but the game state has been cleared."
            }, true);
        }
    }
}