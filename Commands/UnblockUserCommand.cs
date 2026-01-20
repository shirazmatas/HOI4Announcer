using DSharpPlus.Commands;
using DSharpPlus.Entities;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class UnblockUserCommand
{
    [Command("unblockuser")]
    [Description("Unblock a user from participating in games")]
    public async Task OnExecute(CommandContext context,
        [Parameter("user")] [Description("The user to unblock")] DiscordMember user)
    {
        if (ConfigParser.config.bot.blockedUsers == null || !ConfigParser.config.bot.blockedUsers.Contains(user.Id))
        {
            await context.RespondAsync($"{user.DisplayName} is not blocked.");
            return;
        }

        ConfigParser.config.bot.blockedUsers.Remove(user.Id);
        ConfigParser.SaveConfig();

        await context.RespondAsync($"{user.DisplayName} has been unblocked.");
    }
}