using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;

namespace HOI4Announcer.Commands;

// Unlock game should allow moderators to enable players to join nations again.
// Set GameHandler.Locked to false
public class UnlockGame
{
    [Command("unlockgame")]
    [Description("Allow players to join nations again")]
    public async Task OnExecute(CommandContext context)
    {
        GameHandler.UnlockGame();
        // Edit existing discord message.
    }
}