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
        //GameHandler.RemoveUser(member.Id);
    }
}