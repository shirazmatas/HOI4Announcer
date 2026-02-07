using System;
using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HOI4Announcer.Commands;

public class JoinNationCommand
{
    [Command("joinnation")]
    [Description("Add yourself to a nation in the current game")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("nation")][Description("The nation to join")] NationID nation)
    {
        try
        {
            if (!GameHandler.HasActiveGame())
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = "Error: There is no active game."
                }, true);
                return;
            }

            if (GameHandler.currentGame.locked)
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = "Error: The game is locked. You cannot join a nation."
                }, true);
                return;
            }

            if (ConfigParser.config.bot.blockedUsers != null && ConfigParser.config.bot.blockedUsers.Contains(context.User.Id))
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = "Error: You are blocked from participating in games."
                }, true);
                return;
            }

            // Add player to the nation in current_game.json
            string playerName = context.User.Username;
            ulong playerId = context.User.Id;
            
            bool success = await GameHandler.AddPlayerToNation(nation, playerName, playerId);

            if (success)
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Green,
                    Description = $"<@{playerId}> has joined {nation.ToFriendlyString()}."
                });
            }
            else
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = $"Error: Failed to join {nation.ToFriendlyString()}. Does the nation exist in the current game?"
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