using DSharpPlus.Commands;
using DSharpPlus.Commands.ContextChecks;
using System.ComponentModel;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HOI4Announcer.Commands;

public class AddNation
{
    [Command("addnation")]
    [Description("Add a nation to the roster of playable nations")]
    public async Task OnExecute(CommandContext context,
        [Parameter("nation")][Description("The nation to add")] NationID nation,
        [Parameter("faction")][Description("The faction of the nation")] Factions faction,
        [Parameter("maxplayers")] [Description("The maximum number of players allowed for this nation")] int maxPlayers = 1)
    {

        try
        {
            //if (nation in ActiveNations)
            //{
            //
            //}
            // Check if nation is listed in allowed nations (exist in HOI4)
            FileStream stream = File.OpenRead("nations.yml");
            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance).Build();
            var nationsData = deserializer.Deserialize<Dictionary<string, List<string>>>(new StreamReader(stream));

            bool nationExists = false;
            string nationName = nation.ToString();

            foreach (var factionList in nationsData.Values)
            {
                if (factionList.Contains(nationName))
                {
                    nationExists = true;
                    break;
                }
            }

            if (!nationExists)
            {
                // Check if game exists
                if (Games.HasActiveGame())
                {
                    // Add also to currentGame.json
                }

                // Add to nations.yml
                Games.AddNation(nation, faction);
                await context.RespondAsync($"Nation {nationName} has been added to the roster.");
                return;
            }

        }
        catch (Exception)
        {

        }
    }
}