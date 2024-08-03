using DSharpPlus.SlashCommands;

namespace HOI4Announcer.Commands;

public class AddNation
{
    [SlashCommand("addnation", "Add a nation to the roster")]
    public async Task OnExecute(InteractionContext command,
                                [Option("nation", "The nation to add")] Nations nation,
                                [Option("faction", "The faction of the nation")] Factions faction)
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