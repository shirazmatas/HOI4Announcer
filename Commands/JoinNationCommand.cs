using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using System.ComponentModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HOI4Announcer.Commands;

public class JoinNationCommand
{
    [Command("joinnation")]
    [Description("Add yourself to a nation in the current game")]
    public async Task OnExecute(CommandContext context,
        [Parameter("nation")][Description("The nation to join")] NationID nation)
    {
        try
        {
            if (!GameHandler.HasActiveGame())
            {
                await context.RespondAsync("There is no active game.");
                return;
            }

            if (GameHandler.currentGame.locked)
            {
                await context.RespondAsync("The game is locked. You cannot join a nation.");
                return;
            }

            if (ConfigParser.config.bot.blockedUsers != null && ConfigParser.config.bot.blockedUsers.Contains(context.User.Id))
            {
                await context.RespondAsync("You are blocked from participating in games.");
                return;
            }

            // Add player to the nation in current_game.json
            string playerName = context.User.Username;
            ulong playerId = context.User.Id;
            
            bool success = await GameHandler.AddPlayerToNation(nation, playerName, playerId);

            if (success)
            {
                await context.RespondAsync($"{playerName} has joined {nation.ToFriendlyString()}.");
            }
            else
            {
                await context.RespondAsync($"Failed to join {nation.ToFriendlyString()}. Does the nation exist in the current game?");
            }
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"❌ Error: {ex.Message}");
        }
    }
}