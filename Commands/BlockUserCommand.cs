using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class BlockUserCommand
{
    [Command("blockuser")]
    [Description("Block a user from participating in games")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("user")] [Description("The user to block")] DiscordMember user)
    {
        // Simple implementation: Add to a blocked list in Config
        if (ConfigParser.config.bot.blockedUsers == null)
        {
            ConfigParser.config.bot.blockedUsers = new List<ulong>();
        }

        if (ConfigParser.config.bot.blockedUsers.Contains(user.Id))
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: {user.DisplayName} is already blocked."
            }, true);
            return;
        }

        ConfigParser.config.bot.blockedUsers.Add(user.Id);
        ConfigParser.SaveConfig();

        // Also remove them from current game if they are in it
        if (GameHandler.HasActiveGame())
        {
            await GameHandler.RemovePlayer(user.Id);
        }

        await context.RespondAsync(new DiscordEmbedBuilder
        {
            Color = DiscordColor.Green,
            Description = $"{user.DisplayName} has been blocked."
        }, true);
    }
}