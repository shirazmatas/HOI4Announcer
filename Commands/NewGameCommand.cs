using DSharpPlus.Commands;
using System.ComponentModel;
using DSharpPlus.Entities;
namespace HOI4Announcer.Commands;

public class NewGameCommand
{
    [Command("newgame")]
    [Description("Start a new game")]
    public async Task OnExecute(CommandContext context,
        [Parameter("starttime")] [Description("YYYY-MM-DD HH:MM")] DateTimeOffset startTime)
    {
        try
        {
            GameHandler.Game newGame = GameHandler.NewGame(startTime);
            if (newGame == null)
            {
                await context.RespondAsync("Failed to create new game. Check logs for details.");
                return;
            }

            // Create the Discord message
            await DiscordHandler.CreateGameMessage(newGame);

            await context.RespondAsync($"New game created successfully for {Utilities.DiscordRelativeTime(startTime)}!");
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"Error creating new game: {ex.Message}");
        }
    }
}