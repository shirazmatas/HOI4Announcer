using DSharpPlus.Entities;
using DSharpPlus;

namespace HOI4Announcer;

public static class DiscordHandler
{
    public static async Task CreateGameMessage(GameHandler.Game game)
    {
        try
        {
            DiscordChannel discordChannel =
                await HOI4Announcer.Client.GetChannelAsync(ConfigParser.config.bot.gameChannel);
            DiscordEmbed embed = BuildGameEmbed(game);
            DiscordMessage gameMessage = await discordChannel.SendMessageAsync(embed);
            GameHandler.SetMessageID(gameMessage.Id);
        }
        catch (Exception ex)
        {
            Logger.Error("Failed to create message for game information", ex);
        }
    }

    public static async Task UpdateGameMessage(ulong messageID, GameHandler.Game game)
    {
        try
        {
            DiscordChannel discordChannel = await HOI4Announcer.Client.GetChannelAsync(ConfigParser.config.bot.gameChannel);
            DiscordMessage message = await discordChannel.GetMessageAsync(messageID);
            DiscordEmbed embed = BuildGameEmbed(game);
            await message.ModifyAsync(new DiscordMessageBuilder().AddEmbed(embed));
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to update Discord message {messageID}", ex);
        }
    }

    private static DiscordEmbed BuildGameEmbed(GameHandler.Game game)
    {
        DiscordEmbedBuilder gameInfo = new DiscordEmbedBuilder().WithAuthor("HOI4 Announcer", null,
                "https://cdn2.steamgriddb.com/icon_thumb/e2d988c728d061b916697ba7f095f98c.png")
            .WithTitle("HOI4 Current Game")
            .WithColor(game.locked ? DiscordColor.Red : DiscordColor.Green)
            .WithDescription($"Game on {Utilities.DiscordRelativeTime(game.startTime)}!")
            .WithFooter("This action was done by KARL (Kaotic Artificial Rider LLM)");

        // For each Faction, create the fields with the nations and players.
        foreach (GameHandler.Faction faction in game.factions)
        {
            if (faction.nations == null || faction.nations.Count == 0)
            {
                continue; // Skip empty factions
            }

            List<string> nationList = [];
            foreach (GameHandler.Nation nation in faction.nations)
            {
                int playerCount = nation.players?.Count ?? 0;
                int maxPlayers = nation.maxPlayers;
                string playerInfo = (nation.players != null && nation.players.Count > 0)
                    ? string.Join(' ', nation.players.Select(player => player.Tag))
                    : "-";

                nationList.Add($"{nation.id.ToFriendlyString()} ({playerCount}/{maxPlayers}): {playerInfo}");
            }

            if (nationList.Count > 0)
            {
                gameInfo.AddField(faction.id.ToFriendlyString(), string.Join('\n', nationList), true);
            }
        }

        return gameInfo.Build();
    }
}
