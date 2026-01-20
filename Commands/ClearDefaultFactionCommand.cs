using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class ClearDefaultFactionCommand
{
    [Command("cleardefaultfaction")]
    [Description("Clear all nations from a faction")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("faction")][Description("The faction to remove")] FactionID factionID)
    {
        if (FactionsHandler.GetFaction(factionID) == null)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: Faction {factionID.ToFriendlyString()} does not exist."
            }, true);
            return;
        }

        FactionsHandler.ClearFaction(factionID);

        await context.RespondAsync(new DiscordEmbedBuilder
        {
            Color = DiscordColor.Green,
            Description = $"Faction {factionID.ToFriendlyString()} and its nations have been removed."
        }, true);
    }
}
