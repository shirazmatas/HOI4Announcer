using System;
using DSharpPlus.Commands;
using System.ComponentModel;
using System.Threading.Tasks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
namespace HOI4Announcer.Commands;

public class NewGameCommand
{
    [Command("newgame")]
    [Description("Start a new game")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("starttime")] [Description("YYYY-MM-DD HH:MM")] DateTimeOffset startTime)
    {
        try
        {
            GameHandler.Game newGame = GameHandler.NewGame(startTime);
            if (newGame == null)
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = "Error: Failed to create new game. Check logs for details."
                }, true);
                return;
            }

            // Create the Discord message
            await DiscordHandler.CreateGameMessage(newGame);

            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green,
                Description = $"New game created successfully for {Utilities.DiscordRelativeTime(startTime)}!"
            }, true);
        }
        catch (Exception ex)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error creating new game: {ex.Message}"
            }, true);
        }
    }
}