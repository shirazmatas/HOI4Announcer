using DSharpPlus.Commands;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;

namespace HOI4Announcer.Commands;

public class AddDefaultNationCommand
{
    [Command("adddefaultnation")]
    [Description("Add a nation to the roster of playable nations")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("nation")][Description("The nation to add")] NationID nationID,
        [Parameter("faction")][Description("The faction of the nation")] FactionID factionID,
        [Parameter("maxplayers")][Description("The maximum number of players allowed for this nation")] int maxPlayers = 1)
    {
        // Check if nation already exists in ANY faction
        foreach (var f in FactionsHandler.config.factions)
        {
            if (f.nations.Any(n => n.id == nationID))
            {
                await context.RespondAsync(new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Description = $"Error: Nation {nationID.ToFriendlyString()} already exists in faction {f.id.ToFriendlyString()}."
                }, true);
                return;
            }
        }

        FactionsHandler.AddNation(factionID, nationID, maxPlayers);
        
        await context.RespondAsync(new DiscordEmbedBuilder
        {
            Color = DiscordColor.Green,
            Description = $"Nation {nationID.ToFriendlyString()} has been added to the {factionID.ToFriendlyString()} faction roster with {maxPlayers} max players."
        }, true);
    }
}