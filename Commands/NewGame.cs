using DSharpPlus.Commands;
using System.ComponentModel;
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
                // Load data from currentGame.json
                string currentGameData = await File.ReadAllTextAsync(currentGamePath);
                
                // Rename currentGame.json to date-time.json (of that game)
                string archivedGamePath = Path.Combine(gamesPath, $"{date}-{time.Replace(":", "-")}.json");
                File.Move(currentGamePath, archivedGamePath);
            }
            
            // Create a new game based on the previous games nations and factions
            // Load nations from nations.yml
            FileStream stream = File.OpenRead("nations.yml");
            IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).Build();
            var nationsData = deserializer.Deserialize<Dictionary<string, List<string>>>(new StreamReader(stream));
            
            // Create new game object with nations but no players
            var newGame = new
            {
                date = date,
                time = time,
                nations = nationsData,
                players = new List<object>()
            };
            
            // Save the new game as currentGame.json
            ISerializer serializer = new SerializerBuilder().JsonCompatible().Build();
            string jsonContent = serializer.Serialize(newGame);
            await File.WriteAllTextAsync(currentGamePath, jsonContent);
            
            // Create discord message for the game and post in channel "game-channel" TODO: Keep track of the message
            var channel = context.Guild.Channels.Values.FirstOrDefault(c => c.Name == "game-channel");
            if (channel != null)
            {
                await channel.SendMessageAsync($"New game started for {date} at {time}!");
            }
            
            await context.RespondAsync($"New game created successfully for {date} at {time}!");
            // TODO: CHANGE THIS logic
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"Error creating new game: {ex.Message}");
        }
    }
}