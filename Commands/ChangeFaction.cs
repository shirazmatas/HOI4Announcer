using System.ComponentModel;
using DSharpPlus.Commands;

namespace HOI4Announcer.Commands;

public class ChangeFaction
{
    [Command("addnation")]
    [Description("Add a nation to the roster of playable nations")]
    public async Task OnExecute(CommandContext context,
        [Parameter("nation")][Description("The nation to add")] Nations nation,
        [Parameter("faction")][Description("The faction of the nation")] Factions faction)
    {
}