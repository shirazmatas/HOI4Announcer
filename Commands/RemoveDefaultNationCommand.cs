using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HOI4Announcer.Commands;

public class RemoveDefaultNationCommand
{
    [Command("removedefaultnation")]
    [Description("Remove a nation from the roster of playable nations")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("nation")][Description("The nation to remove")] NationID nationID)
    {
        bool nationExists = false;
        foreach (var f in FactionsHandler.config.factions)
        {
            if (f.nations.Any(n => n.id == nationID))
            {
                nationExists = true;
                break;
            }
        }

        if (!nationExists)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: Nation {nationID.ToFriendlyString()} does not exist in any faction."
            }, true);
            return;
        }

        FactionsHandler.RemoveNation(nationID);

        await context.RespondAsync(new DiscordEmbedBuilder
        {
            Color = DiscordColor.Green,
            Description = $"Nation {nationID.ToFriendlyString()} has been removed from the roster."
        }, true);
    }
}