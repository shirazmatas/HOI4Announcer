using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HOI4Announcer.Commands;

public class SetMaxPlayersCommand
{
    [Command("setmaxplayers")]
    [Description("Set the maximum amount of players for a nation in the current game")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("nation")][Description("The nation to change")] NationID nationID,
        [Parameter("maxplayers")][Description("The new maximum amount of players")] int maxPlayers)
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "Error: No active game found."
            }, true);
            return;
        }

        bool found = false;
        foreach (var faction in GameHandler.currentGame.factions)
        {
            if (faction.nations.Any(n => n.id == nationID))
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: Nation {nationID.ToFriendlyString()} not found in current game."
            }, true);
            return;
        }

        bool success = await GameHandler.SetMaxPlayers(nationID, maxPlayers);
        if (success)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green,
                Description = $"Max players for {nationID.ToFriendlyString()} has been set to {maxPlayers}."
            });
        }
        else
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: Failed to set max players for {nationID.ToFriendlyString()}. Does it exist in the current game?"
            }, true);
        }
    }
}