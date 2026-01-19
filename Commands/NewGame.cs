using DSharpPlus.Commands;
using System.ComponentModel;
using DSharpPlus.Entities;
namespace HOI4Announcer.Commands;

public class NewGame
{
    [Command("newgame")]
    [Description("Start a new game")]
    public async Task OnExecute(CommandContext context,
        [Parameter("date")] [Description("YYYY-MM-DD")] string date,
        [Parameter("time")][Description("HH:MM")] string time)
    {
        try
        {
            if (!DateTime.TryParse($"{date} {time}", out DateTime startTime))
            {
                await context.RespondAsync("Invalid date or time format. Please use YYYY-MM-DD and HH:MM.");
                return;
            }

            var newGame = GameHandler.NewGame(startTime, date, time);
            if (newGame == null)
            {
                await context.RespondAsync("Failed to create new game. Check logs for details.");
                return;
            }

            // Create discord message for the game and post in channel "game-channel" TODO: Keep track of the message

            // Get the game channel ID from config and check if channel exists
            DiscordChannel discordChannel = await context.Client.GetChannelAsync(ConfigParser.config.bot.gameChannel);
            if (discordChannel != null)
            {
                DiscordEmbedBuilder gameInfo = new DiscordEmbedBuilder().WithAuthor("HOI4 Announcer", null,
                        "https://cdn2.steamgriddb.com/icon_thumb/e2d988c728d061b916697ba7f095f98c.png")
                    .WithTitle("HOI4 Current Game")
                    .WithColor(DiscordColor.Green)
                    .WithDescription($"Game on {date} at {time}!") // TODO: Local time
                    .WithFooter("This action was done by KARL (Kaotic Artificial Rider LLM)");
                // For each Faction, create the fields with the nations and players.
                foreach (var faction in newGame.factions)
                {
                    var fieldContent = string.Join("\n", faction.nations.Select(n =>
                    {
                        var nationStr = $"    {n.name}:";
                        if (n.players != null && n.players.Any())
                        {
                            var playersStr = string.Join("\n", n.players.Select(p => $"            - {p.name}"));
                            return $"{nationStr}\n{playersStr}";
                        }
                        return $"{nationStr}\n            -";
                    }));

                    gameInfo.AddField(faction.name, string.IsNullOrEmpty(fieldContent) ? "No nations" : fieldContent);
                }
                DiscordMessage gameMessage = await discordChannel.SendMessageAsync(gameInfo.Build());
                newGame.messageID = gameMessage.Id.ToString();
                GameHandler.SaveCurrentGame();
            }

            await context.RespondAsync($"New game created successfully for {date} at {time}!");
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"Error creating new game: {ex.Message}");
        }
    }
}