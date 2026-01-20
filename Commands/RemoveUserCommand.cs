using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Entities;

namespace HOI4Announcer.Commands;

public class RemoveUserCommand
{
    [Command("removeuser")]
    [Description("Remove a user from the nation roster")]
    public async Task OnExecute(CommandContext context,
        [Parameter("player")][Description("Remove player from roster")] DiscordMember member)
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync("There is no active game.");
            return;
        }

        bool success = await GameHandler.RemovePlayer(member.Id);
        if (success)
        {
            await context.RespondAsync($"{member.DisplayName} has been removed from the game roster.");
        }
        else
        {
            await context.RespondAsync($"{member.DisplayName} was not assigned to any nation.");
        }
    }
}