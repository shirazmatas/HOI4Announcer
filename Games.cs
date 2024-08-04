namespace HOI4Announcer;

public class Games
{
     public static bool HasActiveGame()
     {
          string path = Path.Combine(Directory.GetCurrentDirectory(), "games");
          string filepath = Path.Combine(path,"currentGame.json");
          if (File.Exists(filepath))
          {
               return true;
          }
          return false;
     }

     public static void LoadPreviousGame()
     {
          
     }

     public void NewGame()
     {
          if (!HasActiveGame())
          {
               // Define paths for the game folder and the current game
               string path = Path.Combine(Directory.GetCurrentDirectory(), "games");
               string filepath = Path.Combine(path,"currentGame.json");
               
               // Convert default_nations.yml to json
               FileStream stream = File.OpenRead("nations.yml");

               // Converts the FileStream into a YAML object (Young Adult Male Losers)
               IDeserializer deserializer = new DeserializerBuilder().WithNamingConvention(HyphenatedNamingConvention.Instance).Build();
               config = deserializer.Deserialize<Config>(new StreamReader(stream));
               
               // Create new json file
               File.WriteAllText(filepath, "{}");
          }
          else
          {
               // tell user old game is still active.
          }
     }
}