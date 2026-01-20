using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HOI4Announcer.Commands;

// Add user should allow a discord moderator :nerd_emoji: to assign a player to a nation.
public class AddUserCommand
{
    [Command("adduser")]
    [Description("Add a user to the game roster")]
    public async Task OnExecute(CommandContext context,
        [Parameter("UserID")][Description("The user to add")] DiscordMember user,
        [Parameter("nation")][Description("The nation of the user")] NationID nation)
    {
        try
        {
            if (GameHandler.HasActiveGame())
            {
                if (ConfigParser.config.bot.blockedUsers != null && ConfigParser.config.bot.blockedUsers.Contains(user.Id))
                {
                    await context.RespondAsync($"{user.DisplayName} is blocked from participating in games.");
                    return;
                }

                bool success = await GameHandler.AddPlayerToNation(nation, user.DisplayName, user.Id);
                if (success)
                {
                    await context.RespondAsync($"{user.Mention} has been assigned to {nation.ToFriendlyString()}.");
                }
                else
                {
                    await context.RespondAsync($"Failed to assign {user.DisplayName} to {nation.ToFriendlyString()}. Does the nation exist in the current game?");
                }
            }
            else
            {
                await context.RespondAsync("There is no active game.");
            }
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"❌ Error: {ex.Message}");
        }
    }
}