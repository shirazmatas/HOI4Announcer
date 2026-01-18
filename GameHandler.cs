using Newtonsoft.Json;

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

     private static Game currentGame = null;

     public static bool HasActiveGame()
     {
          return currentGame != null;
     }

     public static void LoadCurrentGame()
     {
         currentGame = LoadGame("currentGame.json");
     }

     public static Game LoadGame(string fileName)
     {
          if (!File.Exists($"{Directory.GetCurrentDirectory()}/games/{fileName}"))
          {
               return null;
          }

          try
          {
               return JsonConvert.DeserializeObject<Game>(File.ReadAllText($"{Directory.GetCurrentDirectory()}/games/{fileName}"));
          }
          catch (Exception e)
          {
               Logger.Error($"Unable to load game file \"{Directory.GetCurrentDirectory()}/games/{fileName}\"", e);
          }

          return null;
     }

     public static void SaveCurrentGame()
     {
          if (currentGame != null)
          {
               SaveGame(currentGame, "currentGame.json");
          }
     }

     public static void SaveGame(Game game, string fileName)
     {
          File.WriteAllText($"{Directory.GetCurrentDirectory()}/games/{fileName}", JsonConvert.SerializeObject(game));
     }

     public static void NewGame()
     {
          if (!HasActiveGame())
          {
               // create new game from template in the factions config
          }
          else
          {
               // tell user old game is still active.
          }
     }

     public void AddUser(ulong discordID, string nation)
     {
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

          SaveCurrentGame();
     }
}