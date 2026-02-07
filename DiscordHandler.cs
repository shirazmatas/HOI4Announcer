using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public static async Task SendStartNotifications(GameHandler.Game game)
    {
        try
        {
            List<GameHandler.Player> players = game.factions
                .SelectMany(f => f.nations)
                .SelectMany(n => n.players)
                .GroupBy(p => p.discordID)
                .Select(g => g.First())
                .ToList();

            if (players.Count == 0)
            {
                Logger.Log("No players to notify for game start.");
                return;
            }

            DiscordEmbed embed = new DiscordEmbedBuilder()
                .WithTitle("HOI4 Game Starting Soon!")
                .WithDescription($"The game you joined is starting in {game.notificationMinutes} minutes!")
                .WithColor(DiscordColor.Blurple)
                .AddField("Start Time", Utilities.DiscordRelativeTime(game.startTime))
                .AddField("Server ID", string.IsNullOrEmpty(game.serverID) ? "Not set yet" : game.serverID)
                .AddField("Server Password", string.IsNullOrEmpty(game.serverPassword) ? "Not set yet" : game.serverPassword)
                .Build();

            foreach (GameHandler.Player player in players)
            {
                try
                {
                    DiscordMember member = null;
                    if (ConfigParser.config.bot.gameChannel != 0)
                    {
                        DiscordChannel channel = await HOI4Announcer.Client.GetChannelAsync(ConfigParser.config.bot.gameChannel);
                        member = await channel.Guild.GetMemberAsync(player.discordID);
                    }
                    
                    if (member != null)
                    {
                        await member.SendMessageAsync(embed);
                        Logger.Log($"Sent start notification to {player.name} ({player.discordID})");
                    }
                    else
                    {
                        // Fallback to searching all guilds if gameChannel is not set or member not found there
                        foreach (DiscordGuild guild in HOI4Announcer.Client.Guilds.Values)
                        {
                            try
                            {
                                member = await guild.GetMemberAsync(player.discordID);
                                if (member != null) break;
                            }
                            catch { /* Ignore */ }
                        }

                        if (member != null)
                        {
                            await member.SendMessageAsync(embed);
                            Logger.Log($"Sent start notification to {player.name} ({player.discordID}) via guild {member.Guild.Name}");
                        }
                        else
                        {
                            Logger.Error($"Could not find member {player.name} ({player.discordID}) in any guild to send DM.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Failed to send start notification to {player.name} ({player.discordID})", ex);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Error during sending start notifications", ex);
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

        if (game.notificationMinutes >= 0)
        {
            gameInfo.AddField("Notification", $"Scheduled for {game.notificationMinutes} minutes before start", false);
        }

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
