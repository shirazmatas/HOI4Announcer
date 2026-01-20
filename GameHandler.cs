using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HOI4Announcer;

// Controls the games folder and loads/saves games
public static class GameHandler
{
     public class Faction
     {
          [JsonConverter(typeof(StringEnumConverter))]
          [JsonProperty("id")]
          public FactionID id;

          [JsonProperty("nations")]
          public List<Nation> nations;
     }

     public class Nation
     {
          [JsonConverter(typeof(StringEnumConverter))]
          [JsonProperty("id")]
          public NationID id;

          [JsonProperty("players")]
          public List<Player> players;

          [JsonProperty("max-players")]
          public int maxPlayers = 1;
     }

     public class Player
     {
          [JsonProperty("name")]
          public string name;

          [JsonProperty("discord-id")]
          public ulong discordID;

          [JsonIgnore]
          public string Tag => $"<@{discordID}>";
     }

     public class Game
     {
          [JsonProperty("start-time")]
          public DateTimeOffset startTime;

          [JsonProperty("server-id")]
          public string serverID;

          [JsonProperty("server-password")]
          public string serverPassword;

          [JsonProperty("factions")]
          public List<Faction> factions;

          [JsonProperty("locked")]
          public bool locked;

          [JsonProperty("message-id")]
          public ulong messageID;

          [JsonProperty("notification-minutes")]
          public int notificationMinutes = -1;

          [JsonProperty("notified")]
          public bool notified = false;
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
               string archivedGamePath = $"{gameDir}/{currentGame.startTime.ToString("yyyy-MM-dd_HH:mm")}.json";
               if (File.Exists(archivedGamePath))
               {
                    // If it already exists, maybe add a timestamp or just overwrite. For now, let's just move/overwrite if needed.
                    File.Delete(archivedGamePath);
               }
               File.Move(currentGamePath, archivedGamePath);
          }
          currentGame = new Game
          {
               startTime = startTime,
               serverID = "",
               serverPassword = "",
               factions = FactionsHandler.config.factions.Select(f => new Faction
               {
                    id = f.id,
                    nations = f.nations.Select(n => new Nation
                    {
                         id = n.id,
                         players = new List<Player>(),
                         maxPlayers = 1 // Default to 1, or maybe we should have maxPlayers in FactionsConfig too?
                    }).ToList()
               }).ToList(),
               locked = false,
               messageID = 0
          };
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

     public static async Task<bool> AddNation(NationID nationID, FactionID factionID, int maxPlayers = 1)
     {
          if (!HasActiveGame()) return false;

          Faction faction = currentGame.factions.FirstOrDefault(f => f.id == factionID);
          if (faction == null)
          {
               faction = new Faction { id = factionID, nations = new List<Nation>() };
               currentGame.factions.Add(faction);
          }

          if (faction.nations.Any(n => n.id == nationID)) return false;

          faction.nations.Add(new Nation { id = nationID, players = new List<Player>(), maxPlayers = maxPlayers });
          SaveCurrentGame();
          await UpdateDiscordMessage();
          return true;
     }

     public static async Task<bool> SetLocked(bool locked)
     {
          if (!HasActiveGame()) return false;

          currentGame.locked = locked;
          SaveCurrentGame();

          await UpdateDiscordMessage();
          return true;
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

     public static async Task<bool> AddPlayerToNation(NationID nationId, string playerName, ulong playerId)
     {
          if (!HasActiveGame()) return false;

          // Remove player from any other nation first (swap logic)
          foreach (var faction in currentGame.factions)
          {
               foreach (var nation in faction.nations)
               {
                    nation.players.RemoveAll(p => p.discordID == playerId);
               }
          }

          foreach (var faction in currentGame.factions)
          {
               var nation = faction.nations.FirstOrDefault(n => n.id == nationId);
               if (nation != null)
               {
                    if (nation.players == null)
                    {
                         nation.players = new List<Player>();
                    }

                    if (nation.players.Count >= nation.maxPlayers)
                    {
                         throw new Exception($"Nation {nationId.ToFriendlyString()} is full.");
                    }

                    nation.players.Add(new Player { name = playerName, discordID = playerId });
                    SaveCurrentGame();
                    await UpdateDiscordMessage();
                    return true;
               }
          }

          return false;
     }

     public static async Task UpdateDiscordMessage()
     {
          if (!HasActiveGame() || currentGame.messageID == 0)
          {
               return;
          }

          await DiscordHandler.UpdateGameMessage(currentGame.messageID, currentGame);
     }

     public static async Task<bool> RemoveNation(NationID nationID)
     {
          if (!HasActiveGame()) return false;

          foreach (var faction in currentGame.factions)
          {
               var nation = faction.nations.FirstOrDefault(n => n.id == nationID);
               if (nation != null)
               {
                    faction.nations.Remove(nation);
                    SaveCurrentGame();
                    await UpdateDiscordMessage();
                    return true;
               }
          }

          return false;
     }

     public static async Task<bool> RemovePlayer(ulong playerId)
     {
          if (!HasActiveGame()) return false;

          bool changed = false;
          foreach (var faction in currentGame.factions)
          {
               foreach (var nation in faction.nations)
               {
                    int removedCount = nation.players.RemoveAll(p => p.discordID == playerId);
                    if (removedCount > 0)
                    {
                         changed = true;
                    }
               }
          }

          if (changed)
          {
               SaveCurrentGame();
               await UpdateDiscordMessage();
               return true;
          }

          return false;
     }

     public static async Task<bool> ClearFaction(FactionID factionID)
     {
          if (!HasActiveGame()) return false;

          var faction = currentGame.factions.FirstOrDefault(f => f.id == factionID);
          if (faction != null)
          {
               faction.nations.Clear();
               SaveCurrentGame();
               await UpdateDiscordMessage();
               return true;
          }

          return false;
     }

     public static async Task<bool> EndGame()
     {
          if (!HasActiveGame()) return false;

          string currentGamePath = $"{gameDir}/{currentGameFile}";
          if (File.Exists(currentGamePath))
          {
               string archivedGamePath = $"{gameDir}/{currentGame.startTime.ToString("yyyy-MM-dd_HH-mm")}_ended.json";
               if (File.Exists(archivedGamePath))
               {
                    File.Delete(archivedGamePath);
               }
               File.Move(currentGamePath, archivedGamePath);
               currentGame = null;
               return true;
          }

          currentGame = null;
          return false;
          // Note: The Discord message update is handled by the command usually,
          // but we set currentGame to null so no more updates can happen to it.
     }

     public static async Task<bool> SetMaxPlayers(NationID nationID, int maxPlayers)
     {
          if (!HasActiveGame()) return false;

          foreach (var faction in currentGame.factions)
          {
               var nation = faction.nations.FirstOrDefault(n => n.id == nationID);
               if (nation != null)
               {
                    nation.maxPlayers = maxPlayers;
                    SaveCurrentGame();
                    await UpdateDiscordMessage();
                    return true;
               }
          }

          return false;
     }
}