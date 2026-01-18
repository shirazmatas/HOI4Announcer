using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using System.ComponentModel;

namespace HOI4Announcer.Commands;

public class AddNation
{
    [Command("addnation")]
    [Description("Add a nation to the roster of playable nations")]
    public async Task OnExecute(CommandContext context,
        [Parameter("nation")][Description("The nation to add")] Nations nation,
        [Parameter("faction")][Description("The faction of the nation")] Factions faction)
    {

        try
        {
            //if (nation in ActiveNations)
            //{
            //
            //}
            // Check if nation is listed in allowed nations (exist in HOI4)
        }
        catch (Exception)
        {

        }
    }
}