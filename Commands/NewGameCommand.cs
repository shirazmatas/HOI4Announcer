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

            // Get the game channel ID from config and check if channel exists
            DiscordChannel discordChannel = await context.Client.GetChannelAsync(ConfigParser.config.bot.gameChannel);

            DiscordEmbedBuilder gameInfo = new DiscordEmbedBuilder().WithAuthor("HOI4 Announcer", null,
                    "https://cdn2.steamgriddb.com/icon_thumb/e2d988c728d061b916697ba7f095f98c.png")
                .WithTitle("HOI4 Current Game")
                .WithColor(DiscordColor.Green)
                .WithDescription($"Game on {Utilities.DiscordRelativeTime(startTime)}!")
                .WithFooter("This action was done by KARL (Kaotic Artificial Rider LLM)");

            // For each Faction, create the fields with the nations and players.
            foreach (GameHandler.Faction faction in newGame.factions)
            {
                if (faction.nations == null || faction.nations.Count == 0)
                {
                    continue; // Skip empty factions
                }

                List<string> nationList = [];
                foreach (GameHandler.Nation nation in faction.nations)
                {
                    if (nation.players != null && nation.players.Count != 0)
                    {
                        string playerTags = string.Join(' ', nation.players.Select(player => player.Tag));
                        nationList.Add($"{nation.id.ToFriendlyString()}: {playerTags}");
                    }
                    else
                    {
                        nationList.Add($"{nation.id.ToFriendlyString()}: -");
                    }
                }

                gameInfo.AddField(faction.id.ToFriendlyString(), string.Join('\n', nationList));
            }

            // TODO: If message already exists it should be edited instead

            // Send the message and save it in the config
            DiscordMessage gameMessage = await discordChannel.SendMessageAsync(gameInfo.Build());
            GameHandler.SetMessageID(gameMessage.Id);

            await context.RespondAsync($"New game created successfully for {Utilities.DiscordRelativeTime(startTime)}!");
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"Error creating new game: {ex.Message}");
        }
    }
}