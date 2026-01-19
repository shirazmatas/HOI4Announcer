using Newtonsoft.Json;

namespace HOI4Announcer;

// Controls the games folder and loads/saves games
public static class GameHandler
{
     public class Faction
     {
          public FactionID id;
          public List<Nation> nations;
     }

     public class Nation
     {
          public NationID id;
          public List<Player> players;
     }

     public class Player
     {
          public string name;
          public ulong discordID;
          public string Tag => $"<@{discordID}>";
     }

     public class Game
     {
          public DateTimeOffset startTime;
          public string serverID;
          public string serverPassword;
          public List<Faction> factions;
          public bool locked;
          public ulong messageID;
     }

     public static Game currentGame = null;
     private static readonly string gameDir = Directory.GetCurrentDirectory() + "/games";
     private static readonly string currentGameFile = "currentGame.json";

     public static bool HasActiveGame()
     {
          return currentGame != null;
     }

     public static void LoadCurrentGame()
     {
          currentGame = LoadGame(currentGameFile);
     }

     public static Game LoadGame(string fileName)
     {
          if (!File.Exists($"{gameDir}/{fileName}"))
          {
               return null;
          }

          try
          {
               return JsonConvert.DeserializeObject<Game>(File.ReadAllText($"{gameDir}/{fileName}"));
          }
          catch (Exception e)
          {
               Logger.Error($"Unable to load game file \"{gameDir}/{fileName}\"", e);
          }

          return null;
     }

     public static void SaveCurrentGame()
     {
          if (currentGame != null)
          {
               SaveGame(currentGame, currentGameFile);
          }
     }

     public static void SaveGame(Game game, string fileName)
     {
          if (!Directory.Exists(gameDir))
          {
               Directory.CreateDirectory(gameDir);
          }
          File.WriteAllText($"{Directory.GetCurrentDirectory()}/games/{fileName}", JsonConvert.SerializeObject(game, Formatting.Indented));
     }

     public static Game NewGame(DateTimeOffset startTime)
     {
          string currentGamePath = $"{gameDir}/{currentGameFile}";

          // Ensure games directory exists
          if (!Directory.Exists(gameDir))
          {
               Directory.CreateDirectory(gameDir);
          }

          // Check if there is a current game
          if (File.Exists(currentGamePath))
          {
               // Rename currentGame.json to date-time.json (of that game)
               string archivedGamePath = $"{gameDir}/{startTime.ToString("yyyy-MM-dd_HH:ss")}.json";
               if (File.Exists(archivedGamePath))
               {
                    // If it already exists, maybe add a timestamp or just overwrite. For now, let's just move/overwrite if needed.
                    File.Delete(archivedGamePath);
               }
               File.Move(currentGamePath, archivedGamePath);
          }



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

     public static void SetLocked(bool locked)
     {
          if (!HasActiveGame()) return;

          currentGame.locked = locked;
          SaveCurrentGame();

          // Change the discord message to display a locked emoji
          // TODO: Change discord message
     }

     public static void SetServerID(string serverID)
     {
          currentGame.serverID = serverID;
          SaveCurrentGame();
     }

     public static void SetServerPassword(string serverPassword)
     {
          currentGame.serverPassword = serverPassword;
          SaveCurrentGame();
     }

     public static void SetMessageID(ulong messageID)
     {
          currentGame.messageID = messageID;
          SaveCurrentGame();
     }

     public static void AddPlayerToNation(NationID nation, string playerName, ulong playerId)
     {
          throw new NotImplementedException();
     }
}