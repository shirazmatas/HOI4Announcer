using Newtonsoft.Json;

namespace HOI4Announcer;

// Controls the games folder and loads/saves games
public static class GameHandler
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
          public DateTime startTime;
          public string serverID;
          public string serverPassword;
          public List<Faction> factions;
          public bool locked;
          public string messageID;
     }

     public static Game currentGame = null;

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
          File.WriteAllText($"{Directory.GetCurrentDirectory()}/games/{fileName}", JsonConvert.SerializeObject(game, Formatting.Indented));
     }

     public static Game NewGame(DateTime startTime, string date, string time)
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
               if (File.Exists(archivedGamePath))
               {
                    // If it already exists, maybe add a timestamp or just overwrite. For now, let's just move/overwrite if needed.
                    File.Delete(archivedGamePath);
               }
               File.Move(currentGamePath, archivedGamePath);
          }

          // Create a new game based on factions.json
          if (!File.Exists("factions.json"))
          {
               Logger.Error("factions.json not found!");
               return null;
          }

          using var stream = File.OpenRead("factions.json");
/*
          currentGame = new Game
          {
               startTime = startTime,
               serverID = "",
               serverPassword = "",
               factions = nationsData.Select(f => new Faction
               {
                    name = f.Key,
                    nations = f.Value.Select(n => new Nation
                    {
                         name = n,
                         players = new List<Player>()
                    }).ToList()
               }).ToList(),
               locked = false,
               messageID = ""
          };
*/
          SaveCurrentGame();
          return currentGame;
     }

     /*public void AddUser(ulong discordID, string nation)
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
     }*/

     public static void LockGame() // TODO: Review Code
     {
          if (HasActiveGame())
          {
               // Write in game.lock
               currentGame.locked = true;
               SaveCurrentGame();
               // Change the discord message to display a locked emoji
               // TODO: Change discord message
          }
     }

     public static void UnlockGame() // TODO: Review Code
     {
          if (HasActiveGame())
          {
               string lockFilePath = $"{Directory.GetCurrentDirectory()}/games/game.lock";

               // Check if lock file exists
               if (File.Exists(lockFilePath))
               {
                    // Delete the lock file
                    File.Delete(lockFilePath);
               }

               currentGame.locked = false;
               SaveCurrentGame();

               // TODO: Change discord message to remove locked emoji
          }
     }

     public static void AddPlayerToNation(NationID nation, string playerName, ulong playerId)
     {
          throw new NotImplementedException();
     }
}