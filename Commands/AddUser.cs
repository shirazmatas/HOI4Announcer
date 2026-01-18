using System.ComponentModel;
using DSharpPlus.Commands;
using DSharpPlus.Entities;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HOI4Announcer.Commands;

// Add user should allow a discord moderator :nerd_emoji: to assign a player to a nation.
public class AddUser
{
    [Command("adduser")]
    [Description("Add a user to the game roster")]
    public async Task OnExecute(CommandContext context,
        [Parameter("UserID")][Description("The user to add")] DiscordMember user,
        [Parameter("nation")][Description("The nation of the user")] NationID nation)
    {
        try
        {
            // Check if nation exists in examplegame.json
            if (GameHandler.HasActiveGame())
            {
                // Add User to currentGame.json
                GameHandler.AddUser(user.Id, nation.ToString());
            }
        }
        catch (Exception)
        {

        }
    }
}