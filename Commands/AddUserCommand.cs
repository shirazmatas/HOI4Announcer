using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HOI4Announcer.Commands;

// Add user should allow a discord moderator :nerd_emoji: to assign a player to a nation.
public class AddUserCommand
{
    [Command("adduser")]
    [Description("Add a user to the game roster")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("UserID")][Description("The user to add")] DiscordMember user,
        [Parameter("nation")][Description("The nation of the user")] NationID nation)
    {
        try
        {
            if (GameHandler.HasActiveGame())
            {
                if (ConfigParser.config.bot.blockedUsers != null && ConfigParser.config.bot.blockedUsers.Contains(user.Id))
                {
                    await context.RespondAsync(new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Red,
                        Description = $"Error: {user.DisplayName} is blocked from participating in games."
                    }, true);
                    return;
                }

                bool success = await GameHandler.AddPlayerToNation(nation, user.DisplayName, user.Id);
                if (success)
                {
                    await context.RespondAsync(new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Green,
                        Description = $"{user.Mention} has been assigned to {nation.ToFriendlyString()}."
                    });
                }
                else
                {
                    await context.RespondAsync(new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Red,
                        Description = $"Error: Failed to assign {user.DisplayName} to {nation.ToFriendlyString()}. Does the nation exist in the current game?"
                    }, true);
                }
            }
            else
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = "Error: There is no active game."
                }, true);
            }
        }
        catch (Exception ex)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: {ex.Message}"
            }, true);
        }
    }
}