using DSharpPlus.Commands;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class NewGame
{
    [Command("newgame")]
    [Description("Start a new game")]
    public async Task OnExecute(CommandContext context, 
        [Parameter("date")] [Description("YYYY-MM-DD")] string date, 
        [Parameter("time")][Description("HH:MM")] string time)
    {
        // Look inside games folder for most recent game
        // Will be called "currentGame.json"
        // Load data from currentGame.json
        
        // Rename currentGame.json to date-time.json (of that game)
        
        // Create new game based on previous games nations and factions
        
        // Save new game as currentGame.json
        
    }
}