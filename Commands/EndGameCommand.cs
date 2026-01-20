using System.ComponentModel;
using DSharpPlus.Commands;

namespace HOI4Announcer.Commands;

public class EndGameCommand
{
    [Command("endgame")]
    [Description("End a game and declare a winning side")]
    public async Task OnExecute(CommandContext context, 
        [Parameter("winner")] [Description("Pick a faction, nation, or a tie")] string winner) 
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync("No active game to end.");
            return;
        }

        bool success = await GameHandler.EndGame();
        if (success)
        {
            await context.RespondAsync($"🏆 The game has ended! Winner: {winner}");
        }
        else
        {
            await context.RespondAsync("Failed to end the game properly, but the game state has been cleared.");
        }
    }
}