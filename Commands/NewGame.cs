using DSharpPlus.SlashCommands;

namespace hoi4announcer.Commands;

public class NewGame
{
    [SlashCommand("newgame", "Start a new game")]
    public async Task OnExecute(InteractionContext command, 
        [Option("date","YYYY-MM-DD")] string date, 
        [Option("time","HH:MM")] string time)
    {
        // Look inside games folder for most recent game
        // Will be called "currentGame.json"
        // Load data from currentGame.json
        
        // Rename currentGame.json to date-time.json (of that game)
        
        // Create new game based on previous games nations and factions
        
        // Save new game as currentGame.json
        
    }
}