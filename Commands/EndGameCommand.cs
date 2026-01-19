using System.ComponentModel;
using DSharpPlus.Commands;

namespace HOI4Announcer.Commands;

public class EndGameCommand
{
    [Command("endgame")]
    [Description("End a game and declare a winning side")]
    public async Task OnExecute(CommandContext context, 
        [Parameter("winner")] [Description("Pick a faction'" +
                                           "or nation, " +
                                           "or a tie")] string winner) 
        // TODO: Change so winner is either a faction, a nation, or "tied"
    {
        // Active game set to no. Change text message within channel to say "Game ended" and display Trophy and who is the winner
        // Other context added later?
    }
}