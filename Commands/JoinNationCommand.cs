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
            // Add player to the nation in current_game.json
            string playerName = context.User.Username;
            ulong playerId = context.User.Id;

            GameHandler.AddPlayerToNation(nation, playerName, playerId);

            await context.RespondAsync($"{playerName} has been added to {nation}.");
        }
        catch (Exception ex)
        {
            await context.RespondAsync($"An error occurred: {ex.Message}");
        }
    }
}