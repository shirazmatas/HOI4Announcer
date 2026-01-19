using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;

namespace HOI4Announcer.Commands;

// Unlock game should allow moderators to enable players to join nations again.
// Set GameHandler.Locked to false
public class UnlockGameCommand
{
    [Command("unlockgame")]
    [Description("Allow players to join nations again")]
    public async Task OnExecute(CommandContext context)
    {
        GameHandler.SetLocked(false);
        // Edit existing discord message.
    }
}