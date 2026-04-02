using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
namespace HOI4Announcer.Commands;

public class SetGameID
{
     [Command("setGameID")]
     [Description("Set the Game ID for the current game")]
     public async Task OnExecute(SlashCommandContext context,
          [Parameter("gameid")] [Description("YYYY-MM-DD HH:MM")] string gameID)
     {
          if (!GameHandler.HasActiveGame())
          {
               await context.RespondAsync(new DiscordEmbedBuilder
               {
                    Color = DiscordColor.Red,
                    Description = "There is no active game."
               }, true);
               return;
          }

          GameHandler.currentGame.gameID = gameID;
        
          GameHandler.SaveCurrentGame();
          await GameHandler.UpdateDiscordMessage();

          await context.RespondAsync(new DiscordEmbedBuilder
          {
               Color = DiscordColor.Green,
               Description = $"Game ID set to {gameID}!"
          }, true);
     }
}