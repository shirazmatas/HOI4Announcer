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
          }
     }
}