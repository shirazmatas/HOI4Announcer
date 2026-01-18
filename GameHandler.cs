using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HOI4Announcer;

public class GameHandler
{
     public class Faction
     {
          public string name;
          public List<Nation> nations;
     }

     public class Nation
     {
          public string name;
          public List<Player> players;
     }

     public class Player
     {
          public string name;
          public ulong discordID;
     }

     public class Game
     {
          internal DateTime startTime;
          internal string serverID;
          internal string serverPassword;
          internal List<Faction> factions;
     }

     public static bool HasActiveGame()
     {
          return File.Exists(Directory.GetCurrentDirectory() + "/games/currentGame.json");
     }

     public Game LoadGame(string fileName = "currentGame.json")
     {
          FileStream stream = File.OpenRead($"{Directory.GetCurrentDirectory()}/games/{fileName}");

          IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).Build();
          return deserializer.Deserialize<Game>(new StreamReader(stream));
     }

     public void NewGame()
     {
          if (!HasActiveGame())
          {


               // Create new json file
               File.WriteAllText(filepath, "{}");
          }
          else
          {
               // tell user old game is still active.
          }
     }

     public void AddUser(ulong discordID, string nation)
     {
          // Load the current game
          Game currentGame = LoadGame();

          // Search for the nation in all factions
          if (!NationExists(nation, currentGame))
          {
               return; // Nation doesn't exist
          }

          // Create new player and add to nation
          Player newPlayer = new Player { discordID = discordID, name = "" };
          if (nat.players == null)
          {
               nat.players = new List<Player>();
          }
          nat.players.Add(newPlayer);
          nationFound = true;

          // Save the updated game back to file
          string filepath = $"{Directory.GetCurrentDirectory()}/games/currentGame.json";
          ISerializer serializer = new SerializerBuilder()
               .WithNamingConvention(HyphenatedNamingConvention.Instance)
               .Build();
          File.WriteAllText(filepath, serializer.Serialize(currentGame));
     }
}