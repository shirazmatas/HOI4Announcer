using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HOI4Announcer.Commands;

public class UnblockUserCommand
{
    [Command("unblockuser")]
    [Description("Unblock a user from participating in games")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("user")] [Description("The user to unblock")] DiscordMember user)
    {
        if (ConfigParser.config.bot.blockedUsers == null || !ConfigParser.config.bot.blockedUsers.Contains(user.Id))
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: {user.DisplayName} is not blocked."
            }, true);
            return;
        }

        ConfigParser.config.bot.blockedUsers.Remove(user.Id);
        ConfigParser.SaveConfig();

        await context.RespondAsync(new DiscordEmbedBuilder
        {
            Color = DiscordColor.Green,
            Description = $"{user.DisplayName} has been unblocked."
        }, true);
    }
}