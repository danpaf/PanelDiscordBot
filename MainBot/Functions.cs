using DSharpPlus;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;

namespace MainBot;

public static class Funcs
{
    public static async Task DeleteMessagesAsync(DiscordClient discord, DiscordChannel channel, int count)
    {
        var messages = await channel.GetMessagesAsync(count);
        await channel.DeleteMessagesAsync(messages);
    }
    
    public static async Task BanMemberAsync(DiscordClient discord, DiscordMember member, int daysOfMessagesToDelete = 0, string reason = "") {
        await member.BanAsync(daysOfMessagesToDelete, reason);
    }
    public static async Task SendEmbedMessage(CommandContext ctx, string title, string description, DateTime Timestamp )
    {
        var embed = new DiscordEmbedBuilder
        {
            Title = title,
            Description = description,
            Timestamp = DateTime.Now,
            Color = DiscordColor.Green
        };

        await ctx.RespondAsync(embed: embed);
    }
    
}
