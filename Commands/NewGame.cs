using DSharpPlus.Commands;
using System.ComponentModel;
using DSharpPlus.Entities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
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
            string gamesPath = Path.Combine(Directory.GetCurrentDirectory(), "games");
            string currentGamePath = Path.Combine(gamesPath, "currentGame.json");

            // Ensure games directory exists
            if (!Directory.Exists(gamesPath))
            {
                Directory.CreateDirectory(gamesPath);
            }

            // Check if there is a current game
            if (File.Exists(currentGamePath))
            {
                // Rename currentGame.json to date-time.json (of that game)
                string archivedGamePath = Path.Combine(gamesPath, $"{date}-{time.Replace(":", "-")}.json");
                File.Move(currentGamePath, archivedGamePath);
            }

            // Create a new game based on the previous games nations and factions
            // Load nations from nations.yml
            FileStream stream = File.OpenRead("nations.yml");
            IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).Build();
            var nationsData = deserializer.Deserialize<Dictionary<string, List<string>>>(new StreamReader(stream));

            // Create new game object with nations but no players, matching the new format
            var newGame = nationsData.Select(faction => new
            {
                faction = faction.Key,
                nations = faction.Value.Select(nation => new
                {
                    name = nation,
                    players = new List<object>()
                }).ToList()
            }).ToList();

            // Save the new game as currentGame.json
            ISerializer serializer = new SerializerBuilder().JsonCompatible().Build();
            string jsonContent = serializer.Serialize(newGame);
            await File.WriteAllTextAsync(currentGamePath, jsonContent);

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
                    foreach (var faction in newGame)
                    {
                       var fieldContent = string.Join("\n", faction.nations.Select(n =>
                        {
                            var nationStr = $"    {n.name}:";
                            if (n.players.Any())
                            {
                                var playersStr = string.Join("\n", n.players.Select(p => $"            - {p}"));
                                return $"{nationStr}\n{playersStr}";
                            }
                            return $"{nationStr}\n            -";
                        }));

                        gameInfo.AddField(faction.faction, string.IsNullOrEmpty(fieldContent) ? "No nations" : fieldContent);
                    }
                    DiscordMessage gameMessage = await discordChannel.SendMessageAsync(gameInfo.Build());
            }

            await context.RespondAsync($"New game created successfully for {date} at {time}!");
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"Error creating new game: {ex.Message}");
        }
    }
}