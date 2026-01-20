using DSharpPlus.Commands;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class LeaveNationCommand
{
    [Command("leavenation")]
    [Description("Leave your current nation in the current game")]
    public async Task OnExecute(CommandContext context)
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync("There is no active game.");
            return;
        }

        if (GameHandler.currentGame.locked)
        {
            await context.RespondAsync("The game is locked. You cannot leave your nation.");
            return;
        }

        bool success = await GameHandler.RemovePlayer(context.User.Id);
        if (success)
        {
            await context.RespondAsync($"{context.User.Username} has left their nation.");
        }
        else
        {
            await context.RespondAsync($"{context.User.Username}, you were not assigned to any nation.");
        }
    }
}