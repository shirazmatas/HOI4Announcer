using System.ComponentModel;
using System.Threading.Tasks;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Entities;

namespace HOI4Announcer.Commands;

public class RemoveUserCommand
{
    [Command("removeuser")]
    [Description("Remove a user from the nation roster")]
    public async Task OnExecute(SlashCommandContext context,
        [Parameter("player")][Description("Remove player from roster")] DiscordMember member)
    {
        if (!GameHandler.HasActiveGame())
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = "Error: There is no active game."
            }, true);
            return;
        }

        bool success = await GameHandler.RemovePlayer(member.Id);
        if (success)
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green,
                Description = $"{member.DisplayName} has been removed from the game roster."
            }, true);
        }
        else
        {
            await context.RespondAsync(new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Description = $"Error: {member.DisplayName} was not assigned to any nation."
            }, true);
        }
    }
}