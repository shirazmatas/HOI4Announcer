using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HOI4Announcer.Commands;

public class SetDefaultMaxPlayersCommand
{
    [Command("setdefaultmaxplayers")]
    [Description("Set the default maximum amount of players for a nation in the faction roster")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("nation")][Description("The nation to change")] NationID nationID,
        [Parameter("maxplayers")][Description("The new maximum amount of players")] int maxPlayers)
    {
        bool found = false;
        foreach (var faction in FactionsHandler.config.factions)
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
                Description = $"Error: Nation {nationID.ToFriendlyString()} not found in faction roster."
            }, true);
            return;
        }

        FactionsHandler.SetMaxPlayers(nationID, maxPlayers);
        await context.RespondAsync(new DiscordEmbedBuilder
        {
            Color = DiscordColor.Green,
            Description = $"Default max players for {nationID.ToFriendlyString()} has been set to {maxPlayers}."
        }, true);
    }
}